define([], (function () {

    var definedExemplar = function (kenGridName) {
        this._kenGrid = $("#" + kenGridName).data("kendoGrid");
    };


    definedExemplar.prototype.handleDataSourceChange = function (e) {
        if (e.action != undefined) {
            if (e.action == "remove") {
                this._removeTotalToModelProperty();
            }
            else {
                if (this["_change" + e.field] != undefined) {
                    for (var i = 0; i < e.items.length; i++)
                        this["_change" + e.field](e.items[i]); //each item is a row. Normally, items.length = 1
                }
            }
        }
    };






    definedExemplar.prototype._removeTotalToModelProperty = function () {

    }


    definedExemplar.prototype._updateTotalToModelProperty = function (totalFieldName, fieldName, aggregateFunction, decimals, isUpdateFooterTemplate) {//Update model property for post data
        if (this._kenGrid.dataSource.view().length == 0)
            $("#" + totalFieldName).val(0);
        else {
            if (this._kenGrid.dataSource.aggregates()[fieldName][aggregateFunction] != undefined && this._kenGrid.dataSource.aggregates()[fieldName][aggregateFunction] != null)
                $("#" + totalFieldName).val(this._round(this._kenGrid.dataSource.aggregates()[fieldName][aggregateFunction], decimals));
        }

        if (arguments.length < 5 || isUpdateFooterTemplate === true) //Missing isUpdateFooterTemplate => Default isUpdateFooterTemplate === true
            this._updateTotalToFooterTemplate(fieldName, aggregateFunction);

        $("#" + totalFieldName).trigger("change"); //Raise Change event for the BOM because sometime we need to Listening for Change event of model total property. Ex: We listening for Change Events on TotalWeight to update WeightDifference, ...
    }


    definedExemplar.prototype._updateTotalToFooterTemplate = function (fieldName, aggregateFunction) {//Refresh FooterTemplate for display

        var footerRow = $(".k-footer-template").children("td");

        if (footerRow != undefined) {//Footer found
            var foundItem = function (that) { //Found column + footerTemplate (search by fieldName)
                for (var i = 0; i < that._kenGrid.columns.length; i++) {
                    if (that._kenGrid.columns[i].field === fieldName) {
                        if (that._kenGrid.columns[i].footerTemplate != undefined)
                            return { footerCell: footerRow.eq(i), format: that._kenGrid.columns[i].format != undefined ? that._kenGrid.columns[i].format : "{0:n}" };
                        else
                            return;
                    }
                }
            }(this)

            if (foundItem != undefined)
                foundItem.footerCell.html(kendo.format(foundItem.format, this._kenGrid.dataSource.aggregates()[fieldName][aggregateFunction]));
        }
    }


    definedExemplar.prototype._round = function (value, decimals) {
        if (arguments.length < 2 || decimals === undefined || decimals === 0)
            return Math.round(value);
        else
            return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
    }


    return definedExemplar;

}));