function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(salesInvoiceGridDataSource, quotationGridDataSource) {
    if (salesInvoiceGridDataSource != undefined && quotationGridDataSource != undefined) {
        var quotationGridDataItems = quotationGridDataSource.data();
        for (var i = 0; i < quotationGridDataItems.length; i++) {
            if (quotationGridDataItems[i].IsSelected === true)
                _setParentInput(salesInvoiceGridDataSource, quotationGridDataItems[i]);
        }
        cancelButton_Click();
    }




    function _setParentInput(salesInvoiceGridDataSource, quotationGridDataItem) {

        var dataRow = salesInvoiceGridDataSource.add({});

        dataRow.set("QuotationDetailID", quotationGridDataItem.QuotationDetailID);

        dataRow.set("CommodityID", quotationGridDataItem.CommodityID);
        dataRow.set("CommodityName", quotationGridDataItem.CommodityName);
        dataRow.set("CommodityCode", quotationGridDataItem.CommodityCode);
        dataRow.set("CommodityTypeID", quotationGridDataItem.CommodityTypeID);


        if (dataRow.WarehouseID != undefined) dataRow.set("WarehouseID", quotationGridDataItem.WarehouseID);
        if (dataRow.WarehouseCode != undefined) dataRow.set("WarehouseCode", quotationGridDataItem.WarehouseCode);
        if (dataRow.QuantityAvailable != undefined) dataRow.set("QuantityAvailable", quotationGridDataItem.QuantityAvailable);


        dataRow.set("Quantity", quotationGridDataItem.Quantity);
        dataRow.set("ListedPrice", quotationGridDataItem.ListedPrice);
        dataRow.set("DiscountPercent", quotationGridDataItem.DiscountPercent);
        dataRow.set("UnitPrice", quotationGridDataItem.UnitPrice);
        dataRow.set("VATPercent", quotationGridDataItem.VATPercent);
        dataRow.set("GrossPrice", quotationGridDataItem.GrossPrice);

        dataRow.set("Remarks", quotationGridDataItem.Remarks);
        dataRow.set("IsBonus", quotationGridDataItem.IsBonus);
        dataRow.set("IsWarrantyClaim", quotationGridDataItem.IsWarrantyClaim);
    }
}

