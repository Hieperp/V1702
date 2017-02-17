function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(accountInvoiceGridDataSource, pendingSalesInvoiceGridDataSource) {
    if (accountInvoiceGridDataSource != undefined && pendingSalesInvoiceGridDataSource != undefined) {
        var pendingSalesInvoiceGridDataItems = pendingSalesInvoiceGridDataSource.view();
        var accountInvoiceJSON = accountInvoiceGridDataSource.data().toJSON();
        for (var i = 0; i < pendingSalesInvoiceGridDataItems.length; i++) {
            if (pendingSalesInvoiceGridDataItems[i].IsSelected === true)
                _setParentInput(accountInvoiceJSON, pendingSalesInvoiceGridDataItems[i]);
        }
        accountInvoiceGridDataSource.data(accountInvoiceJSON);

        var dataRowTest = accountInvoiceGridDataSource.add({}); //To calculate total
        //accountInvoiceGridDataSource.trigger("change");

        cancelButton_Click();
    }


    //http://www.telerik.com/forums/adding-multiple-rows-performance
    //By design the dataSource does not provide an opportunity for inserting multiple records via one operation. The performance is low, because each time when you add row through the addRow method, the DataSource throws its change event which forces the Grid to refresh and re-paint the content.
    //To avoid the problem you may try to modify the data of the DataSource manually.
    //var grid = $("#grid").data("kendoGrid");
    //var data = gr.dataSource.data().toJSON(); //the data of the DataSource
 
    ////change the data array
    ////any changes in the data array will not automatically reflect in the Grid
 
    //grid.dataSource.data(data); //set changed data as data of the Grid


    function _setParentInput(accountInvoiceJSON, transferOrderGridDataItem) {

        //var dataRow = accountInvoiceJSON.add({});

        var dataRow = new Object();
        
        dataRow.AccountInvoiceDetailID = 0;
        dataRow.AccountInvoiceID = window.parent.$("#AccountInvoiceID").val();
        dataRow.EntryDate = null;
        dataRow.LocationID = null;
        dataRow.Remarks = null;

        dataRow.SalesInvoiceDetailID = transferOrderGridDataItem.SalesInvoiceDetailID;

        dataRow.CustomerID = transferOrderGridDataItem.CustomerID;
        dataRow.CommodityID = transferOrderGridDataItem.CommodityID;
        dataRow.CommodityName = transferOrderGridDataItem.CommodityName;
        dataRow.CommodityCode = transferOrderGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = transferOrderGridDataItem.CommodityTypeID;

        

        dataRow.Quantity = transferOrderGridDataItem.Quantity;
        dataRow.ListedPrice = transferOrderGridDataItem.ListedPrice;
        dataRow.DiscountPercent = transferOrderGridDataItem.DiscountPercent;
        dataRow.UnitPrice = transferOrderGridDataItem.UnitPrice;
        dataRow.VATPercent = transferOrderGridDataItem.VATPercent;
        dataRow.GrossPrice = transferOrderGridDataItem.GrossPrice;
        dataRow.Amount = transferOrderGridDataItem.Amount;
        dataRow.VATAmount = transferOrderGridDataItem.VATAmount;
        dataRow.GrossAmount = transferOrderGridDataItem.GrossAmount;

        dataRow.IsBonus = transferOrderGridDataItem.IsBonus;
        dataRow.IsWarrantyClaim = transferOrderGridDataItem.IsWarrantyClaim;

        dataRow.ChassisCode = transferOrderGridDataItem.ChassisCode;
        dataRow.EngineCode = transferOrderGridDataItem.EngineCode;
        dataRow.ColorCode = transferOrderGridDataItem.ColorCode;

        accountInvoiceJSON.push(dataRow);
    }
}

