//This javascript is used in _SearchTransferOrder.cshtml (PartTransfers.CreateWizard.cshtml + VehicleTransfers.CreateWizard.cshtml)

function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(selectedGrid) {
    if (selectedGrid === undefined)
        _setParentInput();
    else {
        var selectedDataItem = selectedGrid.dataItem(selectedGrid.select());
        if (selectedDataItem === undefined)
            return false;

        _setParentInput(selectedDataItem);
    }
    window.parent.$("#submitCreateWizard").trigger("click");


    function _setParentInput(selectedDataItem) {
        window.parent.$("#TransferOrderID").val(selectedDataItem === undefined ? null : selectedDataItem.TransferOrderID);
        window.parent.$("#TransferOrderReference").val(selectedDataItem === undefined ? null : selectedDataItem.TransferOrderReference);
        window.parent.$("#TransferOrderEntryDate").val(selectedDataItem === undefined ? null : kendo.toString(selectedDataItem.TransferOrderEntryDate, window.parent.SettingsManager.DateTimeFormat));
        window.parent.$("#TransferOrderRequestedDate").val(selectedDataItem === undefined ? null : kendo.toString(selectedDataItem.TransferOrderRequestedDate, window.parent.SettingsManager.DateTimeFormat));

        window.parent.$("#WarehouseID").val(selectedDataItem === undefined ? null : selectedDataItem.WarehouseID);
        window.parent.$("#WarehouseName").val(selectedDataItem === undefined ? null : selectedDataItem.WarehouseName);
        window.parent.$("#WarehouseLocationName").val(selectedDataItem === undefined ? null : selectedDataItem.WarehouseLocationName);
        window.parent.$("#WarehouseLocationTelephone").val(selectedDataItem === undefined ? null : selectedDataItem.WarehouseLocationTelephone);
        window.parent.$("#WarehouseLocationFacsimile").val(selectedDataItem === undefined ? null : selectedDataItem.WarehouseLocationFacsimile);
        window.parent.$("#WarehouseLocationAddress").val(selectedDataItem === undefined ? null : selectedDataItem.WarehouseLocationAddress);
    }
}
