define(["superBase", "gridDatasource"], (function (superBase, gridDatasource) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridDatasource);






    definedExemplar.prototype._removeTotalToModelProperty = function () {
        this._updateTotalToModelProperty("TotalQuantity", "Quantity", "sum", requireConfig.websiteOptions.rndQuantity, false);

        definedExemplar._super._removeTotalToModelProperty.call(this);
    }








    definedExemplar.prototype._changeQuantity = function (dataRow) {
        this._updateTotalToModelProperty("TotalQuantity", "Quantity", "sum", requireConfig.websiteOptions.rndQuantity);
    }
    

    return definedExemplar;
}));