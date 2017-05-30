function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(deliveryAdviceGridDataSource, pendingSalesOrderDetailGridDataSource) {
    if (deliveryAdviceGridDataSource != undefined && pendingSalesOrderDetailGridDataSource != undefined) {
        var pendingSalesOrderDetailGridDataItems = pendingSalesOrderDetailGridDataSource.view();
        var deliveryAdviceJSON = deliveryAdviceGridDataSource.data().toJSON();
        for (var i = 0; i < pendingSalesOrderDetailGridDataItems.length; i++) {
            if (pendingSalesOrderDetailGridDataItems[i].IsSelected === true)
                _setParentInput(deliveryAdviceJSON, pendingSalesOrderDetailGridDataItems[i]);
        }

        deliveryAdviceJSON.push(new Object()); //Add a temporary empty row

        deliveryAdviceGridDataSource.data(deliveryAdviceJSON);

        var rawData = deliveryAdviceGridDataSource.data()
        deliveryAdviceGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row

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


    function _setParentInput(deliveryAdviceJSON, salesOrderGridDataItem) {

        //var dataRow = deliveryAdviceJSON.add({});

        var dataRow = new Object();

        dataRow.LocationID = null;
        dataRow.EntryDate = null;

        dataRow.DeliveryAdviceDetailID = 0;
        dataRow.DeliveryAdviceID = window.parent.$("#DeliveryAdviceID").val();
        
        dataRow.SalesOrderID = salesOrderGridDataItem.SalesOrderID;
        dataRow.SalesOrderDetailID = salesOrderGridDataItem.SalesOrderDetailID;
        dataRow.SalesOrderCode = salesOrderGridDataItem.SalesOrderCode;
        dataRow.SalesOrderReference = salesOrderGridDataItem.SalesOrderReference;
        dataRow.SalesOrderEntryDate = salesOrderGridDataItem.SalesOrderEntryDate;

        dataRow.CommodityID = salesOrderGridDataItem.CommodityID;
        dataRow.CommodityName = salesOrderGridDataItem.CommodityName;
        dataRow.CommodityCode = salesOrderGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = salesOrderGridDataItem.CommodityTypeID;

        dataRow.WarehouseID = salesOrderGridDataItem.WarehouseID;
        dataRow.WarehouseCode = salesOrderGridDataItem.WarehouseCode;

        dataRow.CalculatingTypeID = salesOrderGridDataItem.CalculatingTypeID;
        dataRow.VATbyRow = salesOrderGridDataItem.VATbyRow;

        dataRow.QuantityAvailable = salesOrderGridDataItem.QuantityAvailable;
        dataRow.ControlFreeQuantity = salesOrderGridDataItem.ControlFreeQuantity;
        dataRow.QuantityRemains = salesOrderGridDataItem.QuantityRemains;
        dataRow.Quantity = salesOrderGridDataItem.Quantity;
        dataRow.FreeQuantityRemains = salesOrderGridDataItem.FreeQuantityRemains;
        dataRow.FreeQuantity = salesOrderGridDataItem.FreeQuantity;
        dataRow.ListedPrice = salesOrderGridDataItem.ListedPrice;
        dataRow.DiscountPercent = salesOrderGridDataItem.DiscountPercent;
        dataRow.UnitPrice = salesOrderGridDataItem.UnitPrice;
        dataRow.ListedAmount = salesOrderGridDataItem.ListedAmount;
        dataRow.Amount = salesOrderGridDataItem.Amount;
        dataRow.TradeDiscountRate = salesOrderGridDataItem.TradeDiscountRate;
        dataRow.VATPercent = salesOrderGridDataItem.VATPercent;
        dataRow.ListedVATAmount = salesOrderGridDataItem.ListedVATAmount;
        dataRow.VATAmount = salesOrderGridDataItem.VATAmount;
        dataRow.ListedGrossPrice = salesOrderGridDataItem.ListedGrossPrice;
        dataRow.GrossPrice = salesOrderGridDataItem.GrossPrice;
        dataRow.ListedGrossAmount = salesOrderGridDataItem.ListedGrossAmount;
        dataRow.GrossAmount = salesOrderGridDataItem.GrossAmount;

        dataRow.IsBonus = salesOrderGridDataItem.IsBonus;


        dataRow.Remarks = null;
        dataRow.VoidTypeID = null;
        dataRow.VoidTypeName = null;
        dataRow.InActive = false;
        dataRow.InActivePartial = false;
        dataRow.InActiveIssue = false;


        deliveryAdviceJSON.push(dataRow);
    }
}

