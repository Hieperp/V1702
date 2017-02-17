define(["superBase", "gridDatasourceDiscount"], (function (superBase, gridDatasourceDiscount) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridDatasourceDiscount);






    definedExemplar.prototype._removeTotalToModelProperty = function () {
        this._updateTotalToModelProperty("TotalListedAmount", "ListedAmount", "sum", requireConfig.websiteOptions.rndAmount, false);
        this._updateTotalToModelProperty("TotalListedVATAmount", "ListedVATAmount", "sum", requireConfig.websiteOptions.rndAmount, false);
        this._updateTotalToModelProperty("TotalListedGrossAmount", "ListedGrossAmount", "sum", requireConfig.websiteOptions.rndAmount, false);

        definedExemplar._super._removeTotalToModelProperty.call(this);
    }








    definedExemplar.prototype._changeQuantity = function (dataRow) {
        this._updateRowListedAmount(dataRow);

        if (dataRow.CalculatingTypeID != 0)
            this._updateRowListedGrossAmount(dataRow);

        definedExemplar._super._changeQuantity.call(this, dataRow);
    }

    definedExemplar.prototype._changeListedPrice = function (dataRow) {
        this._updateRowListedGrossPrice(dataRow);
        this._updateRowListedAmount(dataRow);

        definedExemplar._super._changeListedPrice.call(this, dataRow);
    }

    definedExemplar.prototype._changeVATPercent = function (dataRow) {
        this._updateRowListedGrossPrice(dataRow);

        if (dataRow.CalculatingTypeID == 0)
            this._updateRowListedVATAmount(dataRow);

        definedExemplar._super._changeVATPercent.call(this, dataRow);
    }

    definedExemplar.prototype._changeListedGrossPrice = function (dataRow) {
        this._updateRowListedPrice(dataRow);

        if (dataRow.CalculatingTypeID != 0)
            this._updateRowListedGrossAmount(dataRow);
    }

    definedExemplar.prototype._changeListedAmount = function (dataRow) {
        this._updateRowListedVATAmount(dataRow);

        if (dataRow.CalculatingTypeID == 0)
            this._updateRowListedGrossAmount(dataRow);

        this._updateTotalToModelProperty("TotalListedAmount", "ListedAmount", "sum", requireConfig.websiteOptions.rndAmount);
    }

    definedExemplar.prototype._changeListedVATAmount = function (dataRow) {
        if (dataRow.CalculatingTypeID == 0)
            this._updateRowListedGrossAmount(dataRow);

        this._updateTotalToModelProperty("TotalListedVATAmount", "ListedVATAmount", "sum", requireConfig.websiteOptions.rndAmount);
    }

    definedExemplar.prototype._changeListedGrossAmount = function (dataRow) {
        if (dataRow.CalculatingTypeID != 0)
            this._updateRowListedVATAmount(dataRow);

        this._updateTotalToModelProperty("TotalListedGrossAmount", "ListedGrossAmount", "sum", requireConfig.websiteOptions.rndAmount);
    }





    definedExemplar.prototype._updateRowListedPrice = function (dataRow) {
        var newListedPrice = dataRow.ListedGrossPrice * 100 / (100 + dataRow.VATPercent);
        if (dataRow.ListedPrice - newListedPrice > 0.8 || newListedPrice - dataRow.ListedPrice > 0.8)
            dataRow.set("ListedPrice", this._round(newListedPrice, requireConfig.websiteOptions.rndAmount));
    }

    definedExemplar.prototype._updateRowListedGrossPrice = function (dataRow) {
        var newListedGrossPrice = dataRow.ListedPrice * (1 + dataRow.VATPercent / 100);
        if (dataRow.ListedGrossPrice - newListedGrossPrice > 0.8 || newListedGrossPrice - dataRow.ListedGrossPrice > 0.8)
            dataRow.set("ListedGrossPrice", this._round(newListedGrossPrice, requireConfig.websiteOptions.rndAmount));
    }

    definedExemplar.prototype._updateRowListedAmount = function (dataRow) {
        dataRow.set("ListedAmount", this._round(dataRow.Quantity * dataRow.ListedPrice, requireConfig.websiteOptions.rndAmount));
    }

    definedExemplar.prototype._updateRowListedVATAmount = function (dataRow) {
        dataRow.set("ListedVATAmount", this._round((dataRow.CalculatingTypeID == 0 ? dataRow.ListedAmount * dataRow.VATPercent / 100 : dataRow.ListedGrossAmount - dataRow.ListedAmount), requireConfig.websiteOptions.rndAmount));
    }

    definedExemplar.prototype._updateRowListedGrossAmount = function (dataRow) {
        dataRow.set("ListedGrossAmount", this._round((dataRow.CalculatingTypeID == 0 ? dataRow.ListedAmount + dataRow.ListedVATAmount : dataRow.Quantity * dataRow.ListedGrossPrice), requireConfig.websiteOptions.rndAmount));
    }


    return definedExemplar;
}));