define(["superBase", "gridDatasource"], (function (superBase, gridDatasource) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridDatasource);






    definedExemplar.prototype._removeTotalToModelProperty = function (dataRow) {
        this._updateTotalToModelProperty("TotalReceiptAmount", "ReceiptAmount", "sum", requireConfig.websiteOptions.rndAmount, false);
        this._updateTotalToModelProperty("TotalCashDiscount", "CashDiscount", "sum", requireConfig.websiteOptions.rndAmount, false);
        this._updateTotalToModelProperty("TotalFluctuationAmount", "FluctuationAmount", "sum", requireConfig.websiteOptions.rndAmount, false);

        definedExemplar._super._removeTotalToModelProperty.call(this, dataRow);
    }








    definedExemplar.prototype._changeReceiptAmount = function (dataRow) {
        this._updateTotalToModelProperty("TotalReceiptAmount", "ReceiptAmount", "sum", requireConfig.websiteOptions.rndAmount);
    }


    definedExemplar.prototype._changeCashDiscount = function (dataRow) {
        this._updateTotalToModelProperty("TotalCashDiscount", "CashDiscount", "sum", requireConfig.websiteOptions.rndAmount);
    }


    definedExemplar.prototype._changeFluctuationAmount = function (dataRow) {
        this._updateTotalToModelProperty("TotalFluctuationAmount", "FluctuationAmount", "sum", requireConfig.websiteOptions.rndAmount);
    }

    return definedExemplar;
}));