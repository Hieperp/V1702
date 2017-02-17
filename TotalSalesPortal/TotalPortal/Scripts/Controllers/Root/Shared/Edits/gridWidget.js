define([], (function () {

    var definedExemplar = function () {

    };




    definedExemplar.prototype.copySourceToDestinationField = function (kenGrid, sourceField1, destinationField1, sourceField2, destinationField2, sourceField3, destinationField3, sourceField4, destinationField4, sourceField5, destinationField5) {
        var allDataRows = kenGrid.dataSource.data();
        for (var i = 0; i < allDataRows.length; i++) {
            var dataItem = allDataRows[i];
            if (sourceField1 != undefined && destinationField1 != undefined)
                dataItem.set(destinationField1, dataItem[sourceField1]);
            if (sourceField2 != undefined && destinationField2 != undefined)
                dataItem.set(destinationField2, dataItem[sourceField2]);
            if (sourceField3 != undefined && destinationField3 != undefined)
                dataItem.set(destinationField3, dataItem[sourceField3]);
            if (sourceField4 != undefined && destinationField4 != undefined)
                dataItem.set(destinationField4, dataItem[sourceField4]);
            if (sourceField5 != undefined && destinationField5 != undefined)
                dataItem.set(destinationField5, dataItem[sourceField5]);
        }
    };





    //Important: This function run ok only when one cell in edit mode
    //This avoid redrawing the Grid rows when use the 'dataItem.set()' method
    //Important Note: When use the 'dataItem.set()' method: right after call 'set' the grid is repainted and the rows are receiving different uid attributes i.e. the previously selected row does not exist anymore. This will raise error when handle for the next selected row.
    definedExemplar.prototype.copyEditingToSelectedCell = function (e) {
        var editingCellRowIndex = $(".k-edit-cell").closest("tr").index(); //Check current editing row index

        if (editingCellRowIndex >= 0) {//Current editing cell found

            var editingCellColIndex = $(".k-edit-cell").first().index(); //Check current editing col index
            var selectedRows = e.sender.select();

            if (editingCellColIndex > 0 && selectedRows.length > 1) { //Current editing cell found && user drag to select more than 1 row.

                //Get the current editing field name
                var gridHeader = e.sender.thead;
                var thGridHeader = $(gridHeader).find("th").eq(editingCellColIndex);
                var editingCellFieldName = $(thGridHeader).data("field");

                //Get the current editing cell data
                var gridData = e.sender.dataSource.data();
                var editingDataRow = gridData[editingCellRowIndex];

                for (var i = 0; i < selectedRows.length; i++) {

                    var dataItem = e.sender.dataItem(selectedRows[i]);
                    this._setCellValue(dataItem, editingDataRow, editingCellFieldName);

                }

                e.sender.refresh();
            }
        }
    };


    definedExemplar.prototype._setCellValue = function (dataItem, editingDataRow, editingCellFieldName) {
        dataItem.set(editingCellFieldName, editingDataRow.get(editingCellFieldName));//Set all selected rows the same value of the current editing cell
    }


    return definedExemplar;

}));