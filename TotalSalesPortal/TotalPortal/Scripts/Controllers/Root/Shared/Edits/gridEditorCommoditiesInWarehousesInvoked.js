define(["gridEditorCommoditiesInWarehouses"], (function (gridEditorCommoditiesInWarehouses) {
    
    commoditiesInWarehousesSelect = function (e) {
        var gridEditorCommoditiesInWarehousesInstance = new gridEditorCommoditiesInWarehouses("kendoGridDetails");
        gridEditorCommoditiesInWarehousesInstance.handleSelect(e);
    }

    commoditiesInWarehousesChange = function (e) {
        var gridEditorCommoditiesInWarehousesInstance = new gridEditorCommoditiesInWarehouses("kendoGridDetails");
        gridEditorCommoditiesInWarehousesInstance.handleChange(e);
    }

    commoditiesInWarehousesDataBound = function (e) {
        $(".k-animation-container:has(#CommodityCode-list)").css("width", "382");
        $("#CommodityCode-list").css("width", "382");
        //$("#CommodityCode-list").css("height", $(".k-animation-container:has(#CommodityCode-list)").height());
        //$("#CommodityCode-list").css("height", $("#CommodityCode-list").height() + 1);
    }

}));