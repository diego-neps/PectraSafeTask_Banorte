(function () {
    // Register Service Worker
    if ('serviceWorker' in navigator) {
        navigator.serviceWorker.ready.then(function (sw) {
            var parameter = new Object();
            parameter.apiUrl = CONST_ApiUrl;
            sw.active.postMessage(parameter);
        });

        navigator.serviceWorker.register('service-worker.js').then(function () {
            console.log('[Service Worker] Registered');
        });
    }
})();

// Login
function loginAsync(requestData) {
    var bIsBackgroundCall = false;

    // set request
    if (requestData == null) {
        // set as background call
        bIsBackgroundCall = true;

        // get user data
        var userData = localStorage.getItem(CONST_UserData);
        if (userData != null) {
            var decryptData = CryptoJS.AES.decrypt(userData, CONST_AppKey);
            requestData = JSON.parse(decryptData.toString(CryptoJS.enc.Utf8));
        }
    }

    return new Promise(resolve => {
        $.ajax({
            url: CONST_ApiUrl + '/login',
            type: 'POST',
            contentType: "application/x-www-form-urlencoded",
            data: 'grant_type=password&username=' + requestData.user + '&password=' + requestData.password,
            success: function (data) {
                // almacena el nuevo token de usuario que se utilizara en las llamadas siguientes a la api
                localStorage.setItem(CONST_AccessTokenKey, data.access_token);

                // almacena los datos del usuario
                var encryptData = CryptoJS.AES.encrypt(JSON.stringify(requestData), CONST_AppKey).toString();
                localStorage.setItem(CONST_UserData, encryptData);
                localStorage.setItem(CONST_UserName, requestData.user);

                // return
                resolve(200);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                // verifica que si se trata de un cambio de contraseña en el server el usuario debe iniciar sesion nuevamente
                if (bIsBackgroundCall == true && jqXHR.status == 401) {
                    logout();
                }

                // return
                resolve(jqXHR.status);
            }
        });
    });
}

// Logout
function logout() {
    // remove items
    sessionStorage.clear();
    localStorage.clear();

    // clear data cache
    if ('serviceWorker' in navigator) {
        navigator.serviceWorker.controller.postMessage('CLEARDATACACHE');
    }

    // redirect (no back history)
    window.location.replace('Default');
}

// Post Form (activity end)
function postFormAsync(jsonToSend) {
    var szPostFormUrl = '';
    var objectToSend = JSON.parse(jsonToSend);

    if (objectToSend != null) {
        if (objectToSend.Origin != null && objectToSend.ExecuteActivityEnd != null) {
            szPostFormUrl += '/' + objectToSend.Origin;

            if (objectToSend.ExecuteActivityEnd == true) {
                if (objectToSend.TrxId != null) {
                    szPostFormUrl += '/PostWithActivityEnd';
                }
                else {
                    szPostFormUrl += '/PostExtWithActivityEnd';
                }
            }
            else {
                if (objectToSend.TrxId != null) {
                    szPostFormUrl += '/PostWithoutActivityEnd';
                }
                else {
                    szPostFormUrl += '/PostExtWithoutActivityEnd';
                }
            }
        }
    }

    return new Promise(resolve => {
        $.ajax({
            url: CONST_ApiUrl + szPostFormUrl,
            type: 'POST',
            contentType: "application/json",
            data: jsonToSend,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + getAccessToken());
            },
            success: function (data) {
                // return
                resolve(200);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                // return
                resolve(jqXHR.status);
            }
        });
    });
}

// Get Access Token
function getAccessToken() {
    return localStorage.getItem(CONST_AccessTokenKey);
}

// Try to get Function Definition and TrxId
function redirectToFormAsync(instanceData) {
    // First, get the function definition
    $.ajax({
        url: CONST_ApiUrl + '/Pectra/GetFunctionDef?FuncId=' + instanceData.FunctionId,
        type: 'GET',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + getAccessToken());
        },
        success: function (data) {
            // Second, get TrxId and redirect to form
            if (data != null && data.Definition != null) {
                getTrxIdAsync(data.Definition.Url, instanceData);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            switch (jqXHR.status) {
                case 0:
                    // hide waiting
                    waitingDialog.hide();

                    // indica que debe tener conexion la primera vez para ejecutar esta accion
                    toastr.error(CONST_AlertRequiredNetworkFirstTime, '', CONST_NotificationDefaultOptions);

                    break;
                case 401:
                    // significa que el token actual ha caducado. Obtiene un nuevo token valido y rehace la llamada a la api
                    loginAsync(null).then(function (statusCode) {
                        if (statusCode == 200) {
                            redirectToFormAsync(instanceData);
                        }
                        else {
                            // hide waiting
                            waitingDialog.hide();

                            // show error
                            toastr.error(CONST_AlertErrorBody, '', CONST_NotificationDefaultOptions);
                        }
                    });

                    break;
                default:
                    // hide waiting
                    waitingDialog.hide();

                    // show error
                    toastr.error(CONST_AlertErrorBody, '', CONST_NotificationDefaultOptions);
                    break;
            }
        }
    });
}

// Get new TrxId
function getTrxIdAsync(formToRedirect, instanceData) {
    $.ajax({
        url: CONST_ApiUrl + '/Pectra/GetNewTrxId?OrgId=' + instanceData.OrgId + '&UoId=' + instanceData.UoId + '&ProfId=' + instanceData.ProfId + '&PngId=' + instanceData.PngId + '&VersionId=' + instanceData.VersionId + '&SubProId=' + instanceData.SubProId +
                            '&ActId=' + instanceData.ActId + '&FunctionId=' + instanceData.FunctionId + '&InsId=' + instanceData.InsId,
        type: 'GET',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + getAccessToken());
        },
        success: function (data) {
            // save the new trxid
            instanceData.TrxId = data;

            // save the instance with the new trxid (temporarily)
            sessionStorage.setItem(CONST_InstanceData, JSON.stringify(instanceData));

            // redirect
            window.location.href = formToRedirect;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            switch (jqXHR.status) {
                case 0: /* se trata de un caso offline */

                    if (instanceData.InsId == 0) {
                        /* en caso de una actividad inicial (no es critica la falta del trxid) */

                        // save the instance with trxid in NULL (temporarily)
                        instanceData.TrxId = null;

                        // save the instance with the new trxid (temporarily)
                        sessionStorage.setItem(CONST_InstanceData, JSON.stringify(instanceData));

                        // redirect
                        window.location.href = formToRedirect;
                    }
                    else {
                        /* en caso de una actividad normal (es muy critica la falta del trxid) */

                        // hide waiting
                        waitingDialog.hide();

                        // show error
                        toastr.error(CONST_AlertWarningNotNetworkForm, '', CONST_NotificationDefaultOptions);
                        break;
                    }

                    break;
                case 401:
                    // significa que el token actual ha caducado. Obtiene un nuevo token valido y rehace la llamada a la api
                    loginAsync(null).then(function (statusCode) {
                        if (statusCode == 200) {
                            getTrxIdAsync(formToRedirect, instanceData);
                        }
                        else {
                            // hide waiting
                            waitingDialog.hide();

                            // show error
                            toastr.error(CONST_AlertErrorBody, '', CONST_NotificationDefaultOptions);
                        }
                    });

                    break;
                default:
                    // hide waiting
                    waitingDialog.hide();

                    // show error
                    toastr.error(CONST_AlertErrorBody, '', CONST_NotificationDefaultOptions);
                    break;
            }
        }
    });
}

function compareValues(key, order) {
    return function (a, b) {
        if (!a.hasOwnProperty(key) || !b.hasOwnProperty(key)) {
            // property doesn't exist on either object
            return 0;
        }

        const varA = (typeof a[key] === 'string') ?
          a[key].toUpperCase() : a[key];
        const varB = (typeof b[key] === 'string') ?
          b[key].toUpperCase() : b[key];

        let comparison = 0;
        if (varA > varB) {
            comparison = 1;
        } else if (varA < varB) {
            comparison = -1;
        }

        return ((order == 'desc') ? (comparison * -1) : comparison);
    };
}

function isValidDate(d) {
    return d instanceof Date && !isNaN(d);
}

function toDate(dateStr, format) {
    let day;
    let month;
    let year;
    switch (format) {
        case "DD/MM/YYYY":
            [day, month, year] = dateStr.split('/');
            return new Date(year, month - 1, day);
            break;
        case "DD-MM-YYYY":
            [day, month, year] = dateStr.split('-');
            return new Date(year, month - 1, day);
            break;
        case "MM/DD/YYYY":
            [month, day, year] = dateStr.split('/');
            return new Date(year, month - 1, day);
            break;
        case "MM-DD-YYYY":
            [month, day, year] = dateStr.split('-');
            return new Date(year, month - 1, day);
            break;
        default:
            return new Date(dateStr);
    }
}