define(["superBase", "gridEditorTemplate"], (function (superBase, gridEditorTemplate) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridEditorTemplate);


    //The VoidType here is AutoComplete Widget
    definedExemplar.prototype.handleSelect = function (e) {
        var currentDataSourceRow = this._getCurrentDataSourceRow();

        if (currentDataSourceRow != undefined) {
            var dataItem = e.sender.dataItem(e.item.index());

            currentDataSourceRow.set("VoidTypeID", dataItem.VoidTypeID);
            currentDataSourceRow.set("VoidTypeCode", dataItem.Code);
            currentDataSourceRow.set("VoidTypeName", dataItem.Name);
            currentDataSourceRow.set("VoidClassID", dataItem.VoidClassID);
        }
    };

    definedExemplar.prototype.handleChange = function (e) {
        this._setEditorValue("VoidTypeName", window.VoidTypeNameBeforeChange);
    };

    return definedExemplar;
}));