function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(goodsDeliveryGridDataSource, pendingHandlingUnitDetailGridDataSource) {
    if (goodsDeliveryGridDataSource != undefined && pendingHandlingUnitDetailGridDataSource != undefined) {
        var pendingHandlingUnitDetailGridDataItems = pendingHandlingUnitDetailGridDataSource.view();
        var goodsDeliveryJSON = goodsDeliveryGridDataSource.data().toJSON();
        for (var i = 0; i < pendingHandlingUnitDetailGridDataItems.length; i++) {
            if (pendingHandlingUnitDetailGridDataItems[i].IsSelected === true)
                _setParentInput(goodsDeliveryJSON, pendingHandlingUnitDetailGridDataItems[i]);
        }

        goodsDeliveryJSON.push(new Object()); //Add a temporary empty row

        goodsDeliveryGridDataSource.data(goodsDeliveryJSON);

        var rawData = goodsDeliveryGridDataSource.data()
        goodsDeliveryGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row

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


    function _setParentInput(goodsDeliveryJSON, handlingUnitGridDataItem) {

        //var dataRow = goodsDeliveryJSON.add({});

        var dataRow = new Object();

        dataRow.GoodsDeliveryDetailID = 0;
        dataRow.GoodsDeliveryID = window.parent.$("#GoodsDeliveryID").val();
        dataRow.EntryDate = null;
        dataRow.LocationID = null;
        dataRow.Remarks = null;

        dataRow.HandlingUnitID = handlingUnitGridDataItem.HandlingUnitID;

        dataRow.CustomerID = handlingUnitGridDataItem.CustomerID;
        dataRow.CustomerName = handlingUnitGridDataItem.CustomerName;
        dataRow.CustomerCode = handlingUnitGridDataItem.CustomerCode;

        dataRow.ReceiverID = handlingUnitGridDataItem.ReceiverID;
        dataRow.ReceiverName = handlingUnitGridDataItem.ReceiverName;
        dataRow.ReceiverCode = handlingUnitGridDataItem.ReceiverCode;
        dataRow.ShippingAddress = handlingUnitGridDataItem.ShippingAddress;

        dataRow.ReceiverDescription = (handlingUnitGridDataItem.CustomerID == handlingUnitGridDataItem.ReceiverID ? "" : handlingUnitGridDataItem.ReceiverName + ", ") + handlingUnitGridDataItem.ShippingAddress;

        dataRow.GoodsIssueReferences = handlingUnitGridDataItem.GoodsIssueReferences;
        dataRow.HandlingUnitIdentification = handlingUnitGridDataItem.HandlingUnitIdentification;
        dataRow.PrintedLabel = handlingUnitGridDataItem.PrintedLabel;

        dataRow.Quantity = handlingUnitGridDataItem.Quantity;
        dataRow.Weight = handlingUnitGridDataItem.Weight;
        dataRow.RealWeight = handlingUnitGridDataItem.RealWeight;

        goodsDeliveryJSON.push(dataRow);
    }
}

