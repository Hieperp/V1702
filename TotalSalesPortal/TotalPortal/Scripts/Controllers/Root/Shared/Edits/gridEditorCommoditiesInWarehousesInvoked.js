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
        $(".k-animation-container:has(#CommodityName-list)").css("width", "382");
        $("#CommodityName-list").css("width", "382");
        //$("#CommodityName-list").css("height", $(".k-animation-container:has(#CommodityName-list)").height());
        //$("#CommodityName-list").css("height", $("#CommodityName-list").height() + 1);
    }

}));