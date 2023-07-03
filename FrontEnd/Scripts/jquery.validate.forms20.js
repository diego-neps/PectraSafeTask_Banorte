$().ready(function () {
    // agrega metodo de validacion para números (utilizando formato regional de España).
    jQuery.validator.addMethod("numero", function (value, element) {
        var bResult;

        // PARA VALIDAR CON "," (coma) en sep. de miles y "." (punto) para decimales.
        //bResult = this.optional(element) || /^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/.test(value);
        // Utilizar la siguiente línea de código (en reemplazo de la anterior), para utilizar "." en sep. miles y "," en decimales.
        bResult = this.optional(element) || /^-?(?:\d+|\d{1,3}(?:.\d{3})+)(?:\,\d+)?$/.test(value);

        // si existe un error de validacion personaliza el mensaje a mostrar
        if (bResult == false)
            jQuery.validator.messages.numero = jQuery.validator.messages.numero; //"debe ser numerico"

        return bResult;

    });

    // agrega metodo de validacion para expresiones regulares
    jQuery.validator.addMethod("regex", function(value, element, regexp) {
        var re = new RegExp(regexp);

        if (this.optional(element))
            return true;

        // en el caso del control SELECT valida contra el texto a mostrar y no por el value del item seleccionado
        if (element.type.toUpperCase() == "SELECT-ONE") {
            value = element.options[element.selectedIndex].text;
        }

        return re.test(value);
    });

    // agrega metodo de validacion para comparar valores
    jQuery.validator.addMethod("compare", function(value, element) {
        // verifica si el campo es requerido
        if (this.optional(element))
            return true;

        if (element.getAttribute("comparetype") != null && element.getAttribute("compareoperator") != null &&
                    element.getAttribute("comparevalue") != null) {

            // en el caso del control SELECT valida contra el texto a mostrar y no por el value del item seleccionado
            if (element.type.toUpperCase() == "SELECT-ONE") {
                value = element.options[element.selectedIndex].text;
            }

            switch (element.getAttribute("comparetype").toUpperCase()) {
                case "NUMERICO":
                    if (isNaN(parseFloat(value)) == true) {
                        jQuery.validator.messages.compare = jQuery.validator.messages.number; //"debe ser numerico"
                        return false;
                    }
                    else
                        return CompareValue(parseFloat(value), parseFloat(element.getAttribute("comparevalue")), element.getAttribute("compareoperator"));
                    break;
                case "FECHA":
                    if (value.indexOf(" ") > 0)
                    { value = value.substr(0, value.indexOf(" ")); }                
                    
                    if (isNaN(Date.parse(value)) == true) {
                        jQuery.validator.messages.compare = jQuery.validator.messages.date; //"debe ser una fecha"
                        return false;
                    }
                    else {
                        // asume que las fechas vienen en formato "dd/mm/yyyy" para pasar el string a Date
                        // en javascript Enero es el mes 0 (cero) por eso resta uno a cada mes
                        var valueDate = new Date(value.split("/")[2], value.split("/")[1] - 1, value.split("/")[0]);
                        var comparevalueDate = new Date(element.getAttribute("comparevalue").split("/")[2], element.getAttribute("comparevalue").split("/")[1] - 1, element.getAttribute("comparevalue").split("/")[0]);

                        return CompareValue(valueDate, comparevalueDate, element.getAttribute("compareoperator"), element.getAttribute("comparevalue"));
                    }
                    break;
                case "ALFANUMERICO":
                default:
                    return CompareValue(value.toLowerCase(), element.getAttribute("comparevalue").toLowerCase(), element.getAttribute("compareoperator"));
                    break;
            }
        }

    });

    // agrega metodo de validacion para comparar un valor dentro de un rango
    jQuery.validator.addMethod("rango", function(value, element) {
        // verifica si el campo es requerido
        if (this.optional(element))
            return true;

        if (element.getAttribute("comparetype") != null && element.getAttribute("minvalue") != null &&
                    element.getAttribute("maxvalue") != null) {

            // personaliza el mensaje de error a mostrar
            jQuery.validator.messages.rango = jQuery.validator.format(jQuery.validator.messages.range, element.getAttribute("minvalue"), element.getAttribute("maxvalue")); //"entre tal y tal valor"

            // en el caso del control SELECT valida contra el texto a mostrar y no por el value del item seleccionado
            if (element.type.toUpperCase() == "SELECT-ONE") {
                value = element.options[element.selectedIndex].text;
            }

            switch (element.getAttribute("comparetype").toUpperCase()) {
                case "NUMERICO":
                    if (isNaN(parseFloat(value)) == true)
                        return false;
                    else
                        return CompareRange(parseFloat(value), parseFloat(element.getAttribute("minvalue")), parseFloat(element.getAttribute("maxvalue")));
                    break;
                case "FECHA":
                    if (value.indexOf(" ") > 0)
                    { value = value.substr(0, value.indexOf(" ")); }

                    if (isNaN(Date.parse(value)) == true)
                        return false;
                    else {
                        // asume que las fechas vienen en formato "dd/mm/yyyy" para pasar el string a Date
                        // en javascript Enero es el mes 0 (cero) por eso resta uno a cada mes
                        var valueDate = new Date(value.split("/")[2], value.split("/")[1] - 1, value.split("/")[0]);
                        var minvalueDate = new Date(element.getAttribute("minvalue").split("/")[2], element.getAttribute("minvalue").split("/")[1] - 1, element.getAttribute("minvalue").split("/")[0]);
                        var maxvalueDate = new Date(element.getAttribute("maxvalue").split("/")[2], element.getAttribute("maxvalue").split("/")[1] - 1, element.getAttribute("maxvalue").split("/")[0]);

                        return CompareRange(valueDate, minvalueDate, maxvalueDate);
                    }
                    break;
                case "ALFANUMERICO":
                default:
                    return CompareRange(value.toLowerCase(), element.getAttribute("minvalue").toLowerCase(), element.getAttribute("maxvalue").toLowerCase());
                    break;
            }
        }

    });

    // compara 2 valores a partir de un operador
    function CompareValue(valueInput, valueToCompare, compareOperator, date) {
        var bResult;

        switch (compareOperator.toUpperCase()) {
            case "=":
                // verifica si la validacion resulta correcta
                bResult = (valueInput == valueToCompare);

                // arma el mensaje de error a mostrar en caso de fallo de la validacion
                if (bResult == false) {
                    if (date != null)
                        valueToCompare = date;

                    szMessage = jQuery.validator.format(jQuery.validator.messages.compareIgual, valueToCompare);
                }

                break;
            case ">":
                bResult = (valueInput > valueToCompare);

                if (bResult == false) {
                    if (date != null)
                        valueToCompare = date;

                    szMessage = jQuery.validator.format(jQuery.validator.messages.compareMayor, valueToCompare);
                }

                break;
            case ">=":
                bResult = (valueInput >= valueToCompare);

                if (bResult == false) {
                    if (date != null)
                        valueToCompare = date;

                    szMessage = jQuery.validator.format(jQuery.validator.messages.compareMayorIgual, valueToCompare);
                }

                break;
            case "<":
                bResult = (valueInput < valueToCompare);

                if (bResult == false) {
                    if (date != null)
                        valueToCompare = date;

                    szMessage = jQuery.validator.format(jQuery.validator.messages.compareMenor, valueToCompare);
                }

                break;
            case "<=":
                bResult = (valueInput <= valueToCompare);

                if (bResult == false) {
                    if (date != null)
                        valueToCompare = date;

                    szMessage = jQuery.validator.format(jQuery.validator.messages.compareMenorIgual, valueToCompare);
                }

                break;
            case "<>":
                bResult = (valueInput != valueToCompare);

                if (bResult == false) {
                    if (date != null)
                        valueToCompare = date;

                    szMessage = jQuery.validator.format(jQuery.validator.messages.compareDistinto, valueToCompare);
                }

                break;
            case "CONTENGA":
                bResult = (valueInput.indexOf(valueToCompare) != -1);

                if (bResult == false)
                    szMessage = jQuery.validator.format(jQuery.validator.messages.compareContenga, valueToCompare);

                break;
            case "COMIENZA CON":
                bResult = (valueInput.indexOf(valueToCompare) === 0);

                if (bResult == false)
                    szMessage = jQuery.validator.format(jQuery.validator.messages.compareComienzaCon, valueToCompare);

                break;
            case "TERMINA CON":
                bResult = (valueInput.match(valueToCompare + "$") == valueToCompare);

                if (bResult == false)
                    szMessage = jQuery.validator.format(jQuery.validator.messages.compareTerminaCon, valueToCompare);

                break;
        }

        // si existe un error de validacion personaliza el mensaje a mostrar
        if (bResult == false)
            jQuery.validator.messages.compare = szMessage;

        return bResult;
    }

    // compara un valor dentro de un rango
    function CompareRange(valueInput, minValue, maxValue) {
        if ((valueInput >= minValue) && (valueInput <= maxValue))
            return true;
        else
            return false;
    }
});