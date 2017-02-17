define(["superBase", "gridWidgetQuantityRemains"], (function (superBase, gridWidgetQuantityRemains) {

    var definedExemplar = function () {
        definedExemplar._super.constructor.call(this);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridWidgetQuantityRemains);



    definedExemplar.prototype._setCellValue = function (dataItem, editingDataRow, editingCellFieldName) {
        if (editingCellFieldName === "WarehouseCode")
        {
            dataItem.set("WarehouseID", editingDataRow.get("WarehouseID"));
            dataItem.set("WarehouseCode", editingDataRow.get("WarehouseCode"));
        }
        else
            definedExemplar._super._setCellValue.call(this, dataItem, editingDataRow, editingCellFieldName);
    }


    return definedExemplar;
}));