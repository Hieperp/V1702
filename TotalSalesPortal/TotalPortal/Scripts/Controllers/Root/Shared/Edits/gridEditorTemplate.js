define([], (function () {

    var definedExemplar = function (kenGridName) {
        if (arguments.length === 1)
            this._kenGrid = $("#" + kenGridName).data("kendoGrid");
    };


    
    definedExemplar.prototype._getCurrentDataSourceRow = function () {
        var dataSourceCollection = this._kenGrid.dataSource.data();
        return dataSourceCollection[$(".k-edit-cell").closest("tr").index()];
    };


    //set EditorTemplate by the backup value (the value of the EditorTemplate (field) before change event raise)
    definedExemplar.prototype._setEditorValue = function (fieldName, fieldValue) {
        var currentDataSourceRow = this._getCurrentDataSourceRow();

        if (currentDataSourceRow != undefined)
            currentDataSourceRow.set(fieldName, fieldValue);
    };


    return definedExemplar;

}));