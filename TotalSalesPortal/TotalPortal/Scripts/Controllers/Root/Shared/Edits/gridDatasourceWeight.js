define(["superBase", "gridDatasourceQuantity"], (function (superBase, gridDatasourceQuantity) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridDatasourceQuantity);






    definedExemplar.prototype._removeTotalToModelProperty = function (dataRow) {
        this._updateTotalToModelProperty("TotalWeight", "Weight", "sum", requireConfig.websiteOptions.rndWeight, false);

        definedExemplar._super._removeTotalToModelProperty.call(this, dataRow);
    }








    definedExemplar.prototype._changeQuantity = function (dataRow) {
        this._updateRowWeight(dataRow);

        definedExemplar._super._changeQuantity.call(this, dataRow);
    }

    definedExemplar.prototype._changeUnitWeight = function (dataRow) {
        this._updateRowWeight(dataRow);
    }
    
    definedExemplar.prototype._changeWeight = function (dataRow) {        
        this._updateTotalToModelProperty("TotalWeight", "Weight", "sum", requireConfig.websiteOptions.rndWeight);
    }

    

    

    definedExemplar.prototype._updateRowWeight = function (dataRow) {
        dataRow.set("Weight", this._round(dataRow.Quantity * dataRow.UnitWeight /1000, requireConfig.websiteOptions.rndWeight));
    }


    return definedExemplar;
}));