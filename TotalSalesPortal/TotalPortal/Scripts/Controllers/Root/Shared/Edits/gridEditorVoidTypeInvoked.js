define(["gridEditorVoidType"], (function (gridEditorVoidType) {

    gridEditorVoidTypeSelect = function (e) {
        var gridEditorVoidTypeInstance = new gridEditorVoidType("kendoGridDetails");
        gridEditorVoidTypeInstance.handleSelect(e);
    }

    gridEditorVoidTypeChange = function (e) {
        var gridEditorVoidTypeInstance = new gridEditorVoidType("kendoGridDetails");
        gridEditorVoidTypeInstance.handleChange(e);
    }
}));