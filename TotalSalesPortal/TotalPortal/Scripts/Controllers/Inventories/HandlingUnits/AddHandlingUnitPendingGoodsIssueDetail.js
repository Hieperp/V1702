function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(handlingUnitGridDataSource, pendingGoodsIssueDetailGridDataSource) {
    if (handlingUnitGridDataSource != undefined && pendingGoodsIssueDetailGridDataSource != undefined) {
        var pendingGoodsIssueDetailGridDataItems = pendingGoodsIssueDetailGridDataSource.view();
        var handlingUnitJSON = handlingUnitGridDataSource.data().toJSON();
        for (var i = 0; i < pendingGoodsIssueDetailGridDataItems.length; i++) {
            if (pendingGoodsIssueDetailGridDataItems[i].IsSelected === true)
                _setParentInput(handlingUnitJSON, pendingGoodsIssueDetailGridDataItems[i]);
        }

        handlingUnitJSON.push(new Object()); //Add a temporary empty row

        handlingUnitGridDataSource.data(handlingUnitJSON);

        var rawData = handlingUnitGridDataSource.data()
        handlingUnitGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row

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


    function _setParentInput(handlingUnitJSON, goodsIssueGridDataItem) {

        //var dataRow = handlingUnitJSON.add({});

        var dataRow = new Object();

        dataRow.HandlingUnitDetailID = 0;
        dataRow.HandlingUnitID = window.parent.$("#HandlingUnitID").val();
        dataRow.EntryDate = null;
        dataRow.LocationID = null;
        dataRow.Remarks = null;

        dataRow.GoodsIssueID = goodsIssueGridDataItem.GoodsIssueID;
        dataRow.GoodsIssueDetailID = goodsIssueGridDataItem.GoodsIssueDetailID;
        dataRow.GoodsIssueReference = goodsIssueGridDataItem.GoodsIssueReference;
        dataRow.GoodsIssueEntryDate = goodsIssueGridDataItem.GoodsIssueEntryDate;
        dataRow.GoodsIssueCode = goodsIssueGridDataItem.GoodsIssueCode;

        dataRow.CommodityID = goodsIssueGridDataItem.CommodityID;
        dataRow.CommodityName = goodsIssueGridDataItem.CommodityName;
        dataRow.CommodityCode = goodsIssueGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = goodsIssueGridDataItem.CommodityTypeID;

        dataRow.QuantityRemains = goodsIssueGridDataItem.QuantityRemains;
        dataRow.Quantity = goodsIssueGridDataItem.Quantity;
        dataRow.UnitWeight = goodsIssueGridDataItem.UnitWeight;
        dataRow.Weight = goodsIssueGridDataItem.Weight;

        handlingUnitJSON.push(dataRow);
    }
}

