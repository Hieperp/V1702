define(["superBase", "gridEditorTemplate"], (function (superBase, gridEditorTemplate) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridEditorTemplate);


    //The warehouse here is DropDown Widget
    definedExemplar.prototype.handleSelect = function (e) {
        var currentDataSourceRow = this._getCurrentDataSourceRow();

        if (currentDataSourceRow != undefined) {
            var dataItem = e.sender.dataItem(e.item.index());

            currentDataSourceRow.set("WarehouseID", dataItem.WarehouseID);
            currentDataSourceRow.set("WarehouseCode", dataItem.Code);
        }
    };

    definedExemplar.prototype.handleChange = function (e) {
        this._setEditorValue("WarehouseCode", window.WarehouseCodeBeforeChange);
    };

    return definedExemplar;
}));