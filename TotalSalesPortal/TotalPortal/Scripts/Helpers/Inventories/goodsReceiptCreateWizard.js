//This javascript is used in GoodsReceipt.CreateWizard.cshtml

function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}


function handleOKEvent(tabIndex) {

    var selectedGrid = $(tabIndex === 0 ? "#getPurchaseInvoicesGrid" : "#getStockTransfersGrid").data("kendoGrid");
    var selectedDataItem = selectedGrid.dataItem(selectedGrid.select());
    if (selectedDataItem === undefined)
        return false;

    _setParentInput(selectedDataItem);

    window.parent.$("#submitCreateWizard").trigger("click");


    function _setParentInput(selectedDataItem) {
        window.parent.$("#GoodsReceiptTypeID").val(selectedDataItem.GoodsReceiptTypeID);
        window.parent.$("#VoucherID").val(selectedDataItem.VoucherID);
        
        window.parent.$("#Description").val(selectedDataItem.Description);
        window.parent.$("#Remarks").val(selectedDataItem.Remarks);
    }
}