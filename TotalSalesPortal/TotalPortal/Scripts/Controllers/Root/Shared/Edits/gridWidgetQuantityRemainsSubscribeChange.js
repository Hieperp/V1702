define(["gridWidget", "gridWidgetQuantityRemains"], (function (gridWidget, gridWidgetQuantityRemains) {
    $(document).ready(function () {

        $("#kendoGridDetails").data("kendoGrid").bind("change", function (e) {
            var gridWidgetQuantityRemainsInstance = new gridWidgetQuantityRemains();
            gridWidgetQuantityRemainsInstance.copyEditingToSelectedCell(e);
        });

    });

    copyRemainsToQuantity = function () { //Define function to be called by action some button
        var kenGrid = $("#kendoGridDetails").data("kendoGrid");
        kenGrid.bind("dataBinding", function (e) { e.preventDefault(); }); //You can prevent the data bind action at the dataBinding event, when updating multiple model properties

        var gridWidgetInstance = new gridWidget();
        gridWidgetInstance.copySourceToDestinationField(kenGrid, "QuantityRemains", "Quantity", "FreeQuantityRemains", "FreeQuantity");

        kenGrid.unbind("dataBinding"); //When finish modifying the data, unbind dataBinding event
        kenGrid.refresh(); //Then, finally, refresh grid
    }

}));
