define(["superBase", "gridDatasourceQuantity"], (function (superBase, gridDatasourceQuantity) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridDatasourceQuantity);






    definedExemplar.prototype._removeTotalToModelProperty = function () {
        this._updateTotalToModelProperty("TotalAmount", "Amount", "sum", requireConfig.websiteOptions.rndAmount, false);
        this._updateTotalToModelProperty("TotalVATAmount", "VATAmount", "sum", requireConfig.websiteOptions.rndAmount, false);
        this._updateTotalToModelProperty("TotalGrossAmount", "GrossAmount", "sum", requireConfig.websiteOptions.rndAmount, false);

        definedExemplar._super._removeTotalToModelProperty.call(this);
    }








    definedExemplar.prototype._changeQuantity = function (dataRow) {
        this._updateRowAmount(dataRow);

        if (dataRow.CalculatingTypeID != 0)
            this._updateRowGrossAmount(dataRow);

        definedExemplar._super._changeQuantity.call(this, dataRow);
    }

    definedExemplar.prototype._changeUnitPrice = function (dataRow) {
        this._updateRowGrossPrice(dataRow);
        this._updateRowAmount(dataRow);
    }

    definedExemplar.prototype._changeVATPercent = function (dataRow) {
        this._updateRowGrossPrice(dataRow);

        if (dataRow.CalculatingTypeID == 0)
            this._updateRowVATAmount(dataRow);
    }

    definedExemplar.prototype._changeGrossPrice = function (dataRow) {
        this._updateRowUnitPrice(dataRow);

        if (dataRow.CalculatingTypeID != 0)
            this._updateRowGrossAmount(dataRow);
    }

    definedExemplar.prototype._changeAmount = function (dataRow) {
        this._updateRowVATAmount(dataRow);

        if (dataRow.CalculatingTypeID == 0)
            this._updateRowGrossAmount(dataRow);

        this._updateTotalToModelProperty("TotalAmount", "Amount", "sum", requireConfig.websiteOptions.rndAmount);
    }

    definedExemplar.prototype._changeVATAmount = function (dataRow) {
        if (dataRow.CalculatingTypeID == 0)
            this._updateRowGrossAmount(dataRow);

        this._updateTotalToModelProperty("TotalVATAmount", "VATAmount", "sum", requireConfig.websiteOptions.rndAmount);
    }

    definedExemplar.prototype._changeGrossAmount = function (dataRow) {
        if (dataRow.CalculatingTypeID != 0)
            this._updateRowVATAmount(dataRow);

        this._updateTotalToModelProperty("TotalGrossAmount", "GrossAmount", "sum", requireConfig.websiteOptions.rndAmount);
    }





    definedExemplar.prototype._updateRowUnitPrice = function (dataRow) {
        var newUnitPrice = dataRow.GrossPrice * 100 / (100 + dataRow.VATPercent);
        if (dataRow.UnitPrice - newUnitPrice > 0.8 || newUnitPrice - dataRow.UnitPrice > 0.8)
            dataRow.set("UnitPrice", this._round(newUnitPrice, requireConfig.websiteOptions.rndAmount));
    }

    definedExemplar.prototype._updateRowGrossPrice = function (dataRow) {
        var newGrossPrice = dataRow.UnitPrice * (1 + dataRow.VATPercent / 100);
        if (dataRow.GrossPrice - newGrossPrice > 0.8 || newGrossPrice - dataRow.GrossPrice > 0.8)
            dataRow.set("GrossPrice", this._round(newGrossPrice, requireConfig.websiteOptions.rndAmount));
    }

    definedExemplar.prototype._updateRowAmount = function (dataRow) {
        dataRow.set("Amount", this._round(dataRow.Quantity * dataRow.UnitPrice, requireConfig.websiteOptions.rndAmount));
    }

    definedExemplar.prototype._updateRowVATAmount = function (dataRow) {
        dataRow.set("VATAmount", this._round((dataRow.CalculatingTypeID == 0 ? dataRow.Amount * dataRow.VATPercent / 100 : dataRow.GrossAmount - dataRow.Amount), requireConfig.websiteOptions.rndAmount));
    }

    definedExemplar.prototype._updateRowGrossAmount = function (dataRow) {
        dataRow.set("GrossAmount", this._round((dataRow.CalculatingTypeID == 0 ? dataRow.Amount + dataRow.VATAmount : dataRow.Quantity * dataRow.GrossPrice), requireConfig.websiteOptions.rndAmount));
    }


    return definedExemplar;
}));