define(["superBase", "commonEditBasic", "kenGridValidator"], (function (superBase, commonEditBasic, kenGridValidator) {

    var definedExemplar = function () {
        definedExemplar._super.constructor.call(this);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, commonEditBasic);



    definedExemplar.prototype._validateDetail = function () {
        if (definedExemplar._super._validateDetail()) {
            if ($("#kendoGridDetails").data("kendoGrid") != undefined) {
                var kenGridValidatorInstance = new kenGridValidator("kendoGridDetails");
                return kenGridValidatorInstance.validate();
            }
            else
                return true;
        }
        else
            return false;
    }


    return definedExemplar;

}));
