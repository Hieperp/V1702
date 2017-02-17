function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(stockTransferGridDataSource, transferOrderGridDataSource) {
    if (stockTransferGridDataSource != undefined && transferOrderGridDataSource != undefined) {
        var transferOrderGridDataItems = transferOrderGridDataSource.data();
        for (var i = 0; i < transferOrderGridDataItems.length; i++) {
            if (transferOrderGridDataItems[i].IsSelected === true)
                _setParentInput(stockTransferGridDataSource, transferOrderGridDataItems[i]);
        }
        cancelButton_Click();
    }




    function _setParentInput(stockTransferGridDataSource, transferOrderGridDataItem) {

        var dataRow = stockTransferGridDataSource.add({});

        dataRow.set("TransferOrderDetailID", transferOrderGridDataItem.TransferOrderDetailID);

        dataRow.set("SupplierID", transferOrderGridDataItem.SupplierID);
        dataRow.set("CommodityID", transferOrderGridDataItem.CommodityID);
        dataRow.set("CommodityName", transferOrderGridDataItem.CommodityName);
        dataRow.set("CommodityCode", transferOrderGridDataItem.CommodityCode);
        dataRow.set("CommodityTypeID", transferOrderGridDataItem.CommodityTypeID);

        dataRow.set("WarehouseID", transferOrderGridDataItem.WarehouseID);
        dataRow.set("WarehouseCode", transferOrderGridDataItem.WarehouseCode);
        dataRow.set("QuantityAvailable", transferOrderGridDataItem.QuantityAvailable);

        dataRow.set("Quantity", (transferOrderGridDataItem.QuantityOrderPending <= transferOrderGridDataItem.QuantityAvailable? transferOrderGridDataItem.QuantityOrderPending: transferOrderGridDataItem.QuantityAvailable));

        if (dataRow.GoodsReceiptDetailID != undefined) dataRow.set("GoodsReceiptDetailID", transferOrderGridDataItem.GoodsReceiptDetailID);

        if (dataRow.ChassisCode != undefined) dataRow.set("ChassisCode", transferOrderGridDataItem.ChassisCode);
        if (dataRow.EngineCode != undefined) dataRow.set("EngineCode", transferOrderGridDataItem.EngineCode);
        if (dataRow.ColorCode != undefined) dataRow.set("ColorCode", transferOrderGridDataItem.ColorCode);

        dataRow.set("Remarks", transferOrderGridDataItem.Remarks);
    }
}

