define([], (function () {

    var definedExemplar = function () {
        
    };


    definedExemplar.prototype._validate = function () {        
        var validator = $("#MainForm").kendoValidator().data("kendoValidator");
        if (validator != undefined)
            if (validator.validate())
                return this._validateDetail();
            else {
                $("#div-alert").css("display", "block");
                return false;
            }
        else
            return true;
    }


    definedExemplar.prototype._validateDetail = function () {        
        return true;
    }

    return definedExemplar;

}));
