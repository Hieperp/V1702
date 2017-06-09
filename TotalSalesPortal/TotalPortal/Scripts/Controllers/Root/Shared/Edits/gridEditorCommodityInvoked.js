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
        $(".k-animation-container:has(#CommodityCode-list)").css("width", "382");
        $("#CommodityCode-list").css("width", "382");        
        //$("#CommodityCode-list").css("height", $("#CommodityCode-list").height() + 1);
    }

}));