define(["superBase", "commonEditBasic", "kenGridValidator"], (function (superBase, commonEditBasic, kenGridValidator) {

    var definedExemplar = function () {
        definedExemplar._super.constructor.call(this);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, commonEditBasic);



    definedExemplar.prototype._validateDetail = function () {        
        if (definedExemplar._super._validateDetail()) {
            var kenGridValidatorInstance = new kenGridValidator(this, "kendoGridDetails");
            return kenGridValidatorInstance.validate();
        }
        else
            return false;
    }


    return definedExemplar;

}));
