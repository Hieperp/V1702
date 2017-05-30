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

            currentDataSourceRow.set("GoodsReceiptDetailID", dataItem.GoodsReceiptDetailID);

            if (currentDataSourceRow.SupplierID != undefined) currentDataSourceRow.set("SupplierID", dataItem.SupplierID); //THERE IS A COLUMN SupplierID IN MODEL: StockTransferDetails.SupplierID, BUT: There is NO SupplierID IN SalesInvoiceDetails

            currentDataSourceRow.set("CommodityID", dataItem.CommodityID);
            currentDataSourceRow.set("CommodityCode", dataItem.CommodityCode);
            currentDataSourceRow.set("CommodityName", dataItem.CommodityName);
            currentDataSourceRow.set("CommodityTypeID", dataItem.CommodityTypeID);
            currentDataSourceRow.set("WarehouseID", dataItem.WarehouseID);
            currentDataSourceRow.set("WarehouseCode", dataItem.WarehouseCode);
            currentDataSourceRow.set("ChassisCode", dataItem.ChassisCode);
            currentDataSourceRow.set("EngineCode", dataItem.EngineCode);
            currentDataSourceRow.set("ColorCode", dataItem.ColorCode);
            currentDataSourceRow.set("QuantityAvailable", dataItem.QuantityAvailable);
            currentDataSourceRow.set("Quantity", 1);

            currentDataSourceRow.set("TradeDiscountRate", currentDataSourceRow.VATbyRow === true ? dataItem.TradeDiscountRate : $("#TradeDiscountRate").val());
            currentDataSourceRow.set("VATPercent", currentDataSourceRow.VATbyRow === true ? dataItem.VATPercent : $("#VATPercent").val());

            currentDataSourceRow.set("GrossPrice", dataItem.GrossPrice);

            if (currentDataSourceRow.ListedPrice != undefined)
                currentDataSourceRow.set("ListedPrice", currentDataSourceRow.UnitPrice);
            if (currentDataSourceRow.ListedGrossPrice != undefined)
                currentDataSourceRow.set("ListedGrossPrice", currentDataSourceRow.GrossPrice);

            currentDataSourceRow.set("DiscountPercent", dataItem.DiscountPercent);
        }

        window.commodityNameBeforeChange = dataItem.CommodityName;
    };


    definedExemplar.prototype.handleChange = function (e) {
        this._setEditorValue("CommodityName", window.commodityNameBeforeChange);
    };



    return definedExemplar;
}));