define(["gridEditorCommoditiesInGoodsReceipts"], (function (gridEditorCommoditiesInGoodsReceipts) {

    commoditiesInGoodsReceiptsSelect = function (e) {
        var gridEditorCommoditiesInGoodsReceiptsInstance = new gridEditorCommoditiesInGoodsReceipts("kendoGridDetails");
        gridEditorCommoditiesInGoodsReceiptsInstance.handleSelect(e);
    }

    commoditiesInGoodsReceiptsChange = function (e) {
        var gridEditorCommoditiesInGoodsReceiptsInstance = new gridEditorCommoditiesInGoodsReceipts("kendoGridDetails");
        gridEditorCommoditiesInGoodsReceiptsInstance.handleChange(e);
    }
    
    commoditiesInGoodsReceiptsDataBound = function (e) {
        $(".k-animation-container:has(#CommodityName-list)").css("width", "720");
        $("#CommodityName-list").css("width", "720");
        //$("#CommodityName-list").css("height", $(".k-animation-container:has(#CommodityName-list)").height());
        $("#CommodityName-list").css("height", $("#CommodityName-list").height() + 1);
    }
}));