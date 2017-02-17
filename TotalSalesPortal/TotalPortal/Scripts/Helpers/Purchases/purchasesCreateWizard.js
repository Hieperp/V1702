//This javascript is used in PurchasesInvoice.CreateWizard.cshtml

function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}


function handleOKEvent(tabIndex) {
    if (tabIndex === undefined)
        _setParentInput();
    else {
        var selectedGrid = $(tabIndex === 0 ? "#getPurchaseOrdersGrid" : "#getSuppliersGrid").data("kendoGrid");
        var selectedDataItem = selectedGrid.dataItem(selectedGrid.select());
        if (selectedDataItem === undefined)
            return false;

        _setParentInput(selectedDataItem);
    }
    window.parent.$("#submitCreateWizard").trigger("click");


    function _setParentInput(selectedDataItem) {
        window.parent.$("#PurchaseOrderID").val(selectedDataItem === undefined || selectedDataItem.PurchaseOrderID === undefined ? null : selectedDataItem.PurchaseOrderID);
        window.parent.$("#PurchaseOrderReference").val(selectedDataItem === undefined || selectedDataItem.PurchaseOrderReference === undefined ? null : selectedDataItem.PurchaseOrderReference);
        window.parent.$("#PurchaseOrderEntryDate").val(selectedDataItem === undefined || selectedDataItem.PurchaseOrderEntryDate === undefined ? null : kendo.toString(selectedDataItem.PurchaseOrderEntryDate, window.parent.SettingsManager.DateFormat));
        window.parent.$("#PurchaseOrderAttentionName").val(selectedDataItem === undefined || selectedDataItem.AttentionName === undefined ? null : selectedDataItem.AttentionName);
        window.parent.$("#PurchaseOrderDescription").val(selectedDataItem === undefined || selectedDataItem.Description === undefined ? null : selectedDataItem.Description);
        window.parent.$("#PurchaseOrderRemarks").val(selectedDataItem === undefined || selectedDataItem.Remarks === undefined ? null : selectedDataItem.Remarks);

        window.parent.$("#SupplierID").val(selectedDataItem === undefined ? null : selectedDataItem.SupplierID);
        window.parent.$("#CustomerName").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerName);
        window.parent.$("#CustomerAttentionName").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerAttentionName);
        window.parent.$("#CustomerTelephone").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerTelephone);
        window.parent.$("#CustomerBillingAddress").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerBillingAddress);
        window.parent.$("#CustomerEntireTerritoryEntireName").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerEntireTerritoryEntireName);     
    }
}