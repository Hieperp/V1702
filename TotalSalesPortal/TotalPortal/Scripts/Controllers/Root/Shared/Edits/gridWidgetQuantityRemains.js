define(["superBase", "gridWidget"], (function (superBase, gridWidget) {

    var definedExemplar = function () {
        definedExemplar._super.constructor.call(this);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridWidget);



    definedExemplar.prototype._setCellValue = function (dataItem, editingDataRow, editingCellFieldName) {
        if (editingCellFieldName === "Quantity")
            dataItem.set(editingCellFieldName, dataItem["QuantityRemains"]);//Set all selected rows: the Quantity by QuantityRemains
        else
            if (editingCellFieldName === "FreeQuantity")
                dataItem.set(editingCellFieldName, dataItem["FreeQuantityRemains"]);//Set all selected rows: the Quantity by QuantityRemains
            else
                definedExemplar._super._setCellValue.call(this, dataItem, editingDataRow, editingCellFieldName);
    }

    return definedExemplar;
}));