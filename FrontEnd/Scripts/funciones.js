/**
* Funciones globales a la aplicación.
*/


/**
* Obtiene un QueryString partiendo de la clave.
*/
function getQueryString(key) {
    var queries = window.location.href.split('?')[1];
    if (queries) {
        for (var i = 0; i < queries.split('&').length; i++) {
            if (queries.split('&')[i].split('=')[0] == key) {
                return queries.split('&')[i].split('=')[1];
            }
        }
    }
    return '';
}

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}

// function for dynamic sorting
function compareValues(key, order) {
    return function(a, b) {
        if (!order) {
            return 0;
        }
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
        return (
          (order == 'desc') ? (comparison * -1) : comparison
        );
    };
}

