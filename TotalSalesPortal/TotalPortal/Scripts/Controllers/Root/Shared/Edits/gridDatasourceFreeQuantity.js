define(["superBase", "gridDatasourceListedAmount"], (function (superBase, gridDatasourceListedAmount) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridDatasourceListedAmount);






    definedExemplar.prototype._removeTotalToModelProperty = function (dataRow) {
        this._updateTotalToModelProperty("TotalFreeQuantity", "FreeQuantity", "sum", requireConfig.websiteOptions.rndQuantity, false);

        definedExemplar._super._removeTotalToModelProperty.call(this, dataRow);
    }










    definedExemplar.prototype._changeQuantity = function (dataRow) {
        this._updateRowFreeQuantity(dataRow);

        definedExemplar._super._changeQuantity.call(this, dataRow);
    }


    definedExemplar.prototype._changeFreeQuantity = function (dataRow) {
        this._updateTotalToModelProperty("TotalFreeQuantity", "FreeQuantity", "sum", requireConfig.websiteOptions.rndQuantity);
    }





    definedExemplar.prototype._updateRowFreeQuantity = function (dataRow) {
        dataRow.set("FreeQuantity", this._round((dataRow.ControlFreeQuantity === 0 ? 0 : Math.floor(dataRow.Quantity / dataRow.ControlFreeQuantity)), requireConfig.websiteOptions.rndQuantity));
    }


    return definedExemplar;
}));