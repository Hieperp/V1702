function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(salesReturnGridDataSource, pendingGoodsIssueDetailGridDataSource) {
    if (salesReturnGridDataSource != undefined && pendingGoodsIssueDetailGridDataSource != undefined) {
        var pendingGoodsIssueDetailGridDataItems = pendingGoodsIssueDetailGridDataSource.view();
        var salesReturnJSON = salesReturnGridDataSource.data().toJSON();
        for (var i = 0; i < pendingGoodsIssueDetailGridDataItems.length; i++) {
            if (pendingGoodsIssueDetailGridDataItems[i].IsSelected === true)
                _setParentInput(salesReturnJSON, pendingGoodsIssueDetailGridDataItems[i]);
        }

        salesReturnJSON.push(new Object()); //Add a temporary empty row

        salesReturnGridDataSource.data(salesReturnJSON);

        var rawData = salesReturnGridDataSource.data()
        salesReturnGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row

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


    function _setParentInput(salesReturnJSON, goodsIssueGridDataItem) {

        //var dataRow = salesReturnJSON.add({});

        var dataRow = new Object();

        dataRow.LocationID = null;
        dataRow.EntryDate = null;

        dataRow.SalesReturnDetailID = 0;
        dataRow.SalesReturnID = window.parent.$("#SalesReturnID").val();
        
        dataRow.GoodsIssueID = goodsIssueGridDataItem.GoodsIssueID;
        dataRow.GoodsIssueDetailID = goodsIssueGridDataItem.GoodsIssueDetailID;
        dataRow.GoodsIssueReference = goodsIssueGridDataItem.GoodsIssueReference;
        dataRow.GoodsIssueEntryDate = goodsIssueGridDataItem.GoodsIssueEntryDate;

        dataRow.CommodityID = goodsIssueGridDataItem.CommodityID;
        dataRow.CommodityName = goodsIssueGridDataItem.CommodityName;
        dataRow.CommodityCode = goodsIssueGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = goodsIssueGridDataItem.CommodityTypeID;

        dataRow.WarehouseID = goodsIssueGridDataItem.WarehouseID;
        dataRow.WarehouseCode = goodsIssueGridDataItem.WarehouseCode;

        dataRow.CalculatingTypeID = goodsIssueGridDataItem.CalculatingTypeID;
        dataRow.VATbyRow = goodsIssueGridDataItem.VATbyRow;

        dataRow.QuantityAvailable = goodsIssueGridDataItem.QuantityAvailable;
        dataRow.ControlFreeQuantity = goodsIssueGridDataItem.ControlFreeQuantity;
        dataRow.QuantityRemains = goodsIssueGridDataItem.QuantityRemains;
        dataRow.Quantity = goodsIssueGridDataItem.Quantity;
        dataRow.FreeQuantityRemains = goodsIssueGridDataItem.FreeQuantityRemains;
        dataRow.FreeQuantity = goodsIssueGridDataItem.FreeQuantity;
        dataRow.ListedPrice = goodsIssueGridDataItem.ListedPrice;
        dataRow.DiscountPercent = goodsIssueGridDataItem.DiscountPercent;
        dataRow.UnitPrice = goodsIssueGridDataItem.UnitPrice;
        dataRow.ListedAmount = goodsIssueGridDataItem.ListedAmount;
        dataRow.Amount = goodsIssueGridDataItem.Amount;
        dataRow.TradeDiscountRate = goodsIssueGridDataItem.TradeDiscountRate;
        dataRow.VATPercent = goodsIssueGridDataItem.VATPercent;
        dataRow.ListedVATAmount = goodsIssueGridDataItem.ListedVATAmount;
        dataRow.VATAmount = goodsIssueGridDataItem.VATAmount;
        dataRow.ListedGrossPrice = goodsIssueGridDataItem.ListedGrossPrice;
        dataRow.GrossPrice = goodsIssueGridDataItem.GrossPrice;
        dataRow.ListedGrossAmount = goodsIssueGridDataItem.ListedGrossAmount;
        dataRow.GrossAmount = goodsIssueGridDataItem.GrossAmount;

        dataRow.IsBonus = goodsIssueGridDataItem.IsBonus;


        dataRow.Remarks = null;
        dataRow.VoidTypeID = null;
        dataRow.VoidTypeName = null;
        dataRow.InActive = false;
        dataRow.InActivePartial = false;        


        salesReturnJSON.push(dataRow);
    }
}

