//This javascript is used in ServicesContract.CreateWizard.cshtml

function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}


function handleOKEvent(tabIndex) {
    if (tabIndex === undefined)
        _setParentInput();
    else {
        var selectedGrid = $("#gridVehiclesInvoices").data("kendoGrid");
        var selectedDataItem = selectedGrid.dataItem(selectedGrid.select());
        if (selectedDataItem === undefined || selectedDataItem === null)
            return false;

        _setParentInput(selectedDataItem);
    }
    window.parent.$("#submitCreateWizard").trigger("click");


    function _setParentInput(selectedDataItem) {
        window.parent.$("#SalesInvoiceDetailID").val(selectedDataItem === undefined ? null : selectedDataItem.SalesInvoiceDetailID);
        window.parent.$("#PurchaseDate").val(selectedDataItem === undefined ? null : kendo.toString(selectedDataItem.EntryDate, window.parent.SettingsManager.DateFormat));


        window.parent.$("#CustomerID").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerID);
        window.parent.$("#CustomerName").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerName);
        window.parent.$("#CustomerBirthday").val(selectedDataItem === undefined ? null : kendo.toString(selectedDataItem.CustomerBirthday, window.parent.SettingsManager.DateFormat));
        window.parent.$("#CustomerTelephone").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerTelephone);
        window.parent.$("#CustomerBillingAddress").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerBillingAddress);
        window.parent.$("#CustomerEntireTerritoryEntireName").val(selectedDataItem === undefined ? null : selectedDataItem.CustomerEntireTerritoryEntireName);


        window.parent.$("#CommodityID").val(selectedDataItem === undefined ? null : selectedDataItem.CommodityID);
        window.parent.$("#CommodityCode").val(selectedDataItem === undefined ? null : selectedDataItem.CommodityCode);
        window.parent.$("#CommodityName").val(selectedDataItem === undefined ? null : selectedDataItem.CommodityName);
        window.parent.$("#ChassisCode").val(selectedDataItem === undefined ? null : selectedDataItem.ChassisCode);
        window.parent.$("#EngineCode").val(selectedDataItem === undefined ? null : selectedDataItem.EngineCode);
        window.parent.$("#ColorCode").val(selectedDataItem === undefined ? null : selectedDataItem.ColorCode);

        window.parent.$("#BeginningDate").val(selectedDataItem === undefined ? null : kendo.toString(selectedDataItem.BeginningDate, window.parent.SettingsManager.DateFormat));
        window.parent.$("#EndingDate").val(selectedDataItem === undefined ? null : kendo.toString(selectedDataItem.EndingDate, window.parent.SettingsManager.DateFormat));
        window.parent.$("#BeginningMeters").val(selectedDataItem === undefined ? null : 0);
        window.parent.$("#EndingMeters").val(selectedDataItem === undefined ? null : selectedDataItem.LimitedKilometreWarranty);
    }
}