define(["superBase", "gridEditorTemplate"], (function (superBase, gridEditorTemplate) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridEditorTemplate);



    //The commodity here is AutoComplete Widget
    definedExemplar.prototype.handleSelect = function (e) {
        var currentDataSourceRow = this._getCurrentDataSourceRow();

        if (currentDataSourceRow != undefined) {
            var dataItem = e.sender.dataItem(e.item.index());

            currentDataSourceRow.set("CommodityID", dataItem.CommodityID);
            currentDataSourceRow.set("CommodityCode", dataItem.Code);
            currentDataSourceRow.set("CommodityName", dataItem.Name);
            currentDataSourceRow.set("CommodityTypeID", dataItem.CommodityTypeID);
            currentDataSourceRow.set("Quantity", 1);

            currentDataSourceRow.set("VATPercent", currentDataSourceRow.VATbyRow === true ? dataItem.VATPercent : $("#VATPercent").val());

            currentDataSourceRow.set("GrossPrice", dataItem.GrossPrice);

            if (currentDataSourceRow.ListedPrice != undefined)
                currentDataSourceRow.set("ListedPrice", currentDataSourceRow.UnitPrice);
            if (currentDataSourceRow.ListedGrossPrice != undefined)
                currentDataSourceRow.set("ListedGrossPrice", currentDataSourceRow.GrossPrice);
        }


        window.commodityNameBeforeChange = dataItem.Name;
    };


    definedExemplar.prototype.handleChange = function (e) {
        this._setEditorValue("CommodityName", window.commodityNameBeforeChange);
    };




    return definedExemplar;
}));