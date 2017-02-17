define(["gridEditorCommodity"], (function (gridEditorCommodity) {

    gridEditorCommoditySelect = function (e) {
        var gridEditorCommodityInstance = new gridEditorCommodity("kendoGridDetails");
        gridEditorCommodityInstance.handleSelect(e);
    }

    gridEditorCommodityChange = function (e) {
        var gridEditorCommodityInstance = new gridEditorCommodity("kendoGridDetails");
        gridEditorCommodityInstance.handleChange(e);
    }

    gridEditorCommodityDataBound = function (e) {
        $(".k-animation-container:has(#CommodityName-list)").css("width", "382");
        $("#CommodityName-list").css("width", "382");        
        $("#CommodityName-list").css("height", $("#CommodityName-list").height() + 1);
    }

}));