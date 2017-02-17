//This javascript is used in PartsInvoice.CreateWizard.cshtml + ServicesInvoice.CreateWizard.cshtml + Quotation.CreateWizard.cshtml

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
        if (window.parent.$("#ServiceInvoiceID") != undefined) window.parent.$("#ServiceInvoiceID").val(selectedDataItem === undefined || selectedDataItem.SalesInvoiceID === undefined ? null : selectedDataItem.SalesInvoiceID);
        if (window.parent.$("#ServiceInvoiceReference") != undefined) window.parent.$("#ServiceInvoiceReference").val(selectedDataItem === undefined || selectedDataItem.Reference === undefined ? null : selectedDataItem.Reference);
        if (window.parent.$("#ServiceInvoiceEntryDate") != undefined) window.parent.$("#ServiceInvoiceEntryDate").val(selectedDataItem === undefined || selectedDataItem.EntryDate === undefined ? null : kendo.toString(selectedDataItem.EntryDate, window.parent.SettingsManager.DateTimeFormat));

        //CHECK if (window.parent.$("#QuotationEntryDate") != undefined): TO EX-CLUSIVE Quotation.CreateWizard.cshtml (BECAUSE IN Quotation.CreateWizard.cshtml: THERE IS NO FIELD NAME: QuotationEntryDate
        if (window.parent.$("#QuotationEntryDate") != undefined) window.parent.$("#QuotationID").val(selectedDataItem === undefined || selectedDataItem.QuotationID === undefined ? null : selectedDataItem.QuotationID);
        if (window.parent.$("#QuotationEntryDate") != undefined) window.parent.$("#QuotationReference").val(selectedDataItem === undefined || selectedDataItem.QuotationReference === undefined ? null : selectedDataItem.QuotationReference);
        if (window.parent.$("#QuotationEntryDate") != undefined) window.parent.$("#QuotationEntryDate").val(selectedDataItem === undefined || selectedDataItem.QuotationEntryDate === undefined ? null : kendo.toString(selectedDataItem.QuotationEntryDate, window.parent.SettingsManager.DateTimeFormat));


        window.parent.$("#CustomerID").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerID);
        window.parent.$("#CustomerName").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerName);
        window.parent.$("#CustomerBirthday").val(selectedDataItem === undefined ? null : kendo.toString(selectedDataItem.CustomerBirthday, window.parent.SettingsManager.DateTimeFormat));
        window.parent.$("#CustomerTelephone").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerTelephone);
        window.parent.$("#CustomerBillingAddress").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerBillingAddress);
        window.parent.$("#CustomerEntireTerritoryEntireName").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerEntireTerritoryEntireName);


        window.parent.$("#ServiceContractID").val(selectedDataItem === undefined ? null : selectedDataItem.ServiceContractID);
        window.parent.$("#ServiceContractReference").val(selectedDataItem === undefined ? null : selectedDataItem.ServiceContractReference);
        window.parent.$("#ServiceContractCommodityCode").val(selectedDataItem === undefined ? null : selectedDataItem.ServiceContractCommodityCode);
        window.parent.$("#ServiceContractCommodityName").val(selectedDataItem === undefined ? null : selectedDataItem.ServiceContractCommodityName);
        window.parent.$("#ServiceContractLicensePlate").val(selectedDataItem === undefined ? null : selectedDataItem.ServiceContractLicensePlate);
        window.parent.$("#ServiceContractColorCode").val(selectedDataItem === undefined ? null : selectedDataItem.ServiceContractColorCode);
        window.parent.$("#ServiceContractChassisCode").val(selectedDataItem === undefined ? null : selectedDataItem.ServiceContractChassisCode);
        window.parent.$("#ServiceContractEngineCode").val(selectedDataItem === undefined ? null : selectedDataItem.ServiceContractEngineCode);
        window.parent.$("#ServiceContractPurchaseDate").val(selectedDataItem === undefined ? null : kendo.toString(selectedDataItem.ServiceContractPurchaseDate, window.parent.SettingsManager.DateTimeFormat));
        window.parent.$("#ServiceContractAgentName").val(selectedDataItem === undefined ? null : selectedDataItem.ServiceContractAgentName);
    }
}