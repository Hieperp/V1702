define(["gridEditorWarehouse"], (function (gridEditorWarehouse) {

    gridEditorWarehouseSelect = function (e) {
        var gridEditorWarehouseInstance = new gridEditorWarehouse("kendoGridDetails");
        gridEditorWarehouseInstance.handleSelect(e);
    }
    
    gridEditorWarehouseChange = function (e) {
        var gridEditorWarehouseInstance = new gridEditorWarehouse("kendoGridDetails");
        gridEditorWarehouseInstance.handleChange(e);
    }
}));