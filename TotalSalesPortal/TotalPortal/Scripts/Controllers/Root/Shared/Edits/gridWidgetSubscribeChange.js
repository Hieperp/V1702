define(["gridWidget"], (function (gridWidget) {
    $(document).ready(function () {

        $("#kendoGridDetails").data("kendoGrid").bind("change", function (e) {
            var gridWidgetInstance = new gridWidget();
            gridWidgetInstance.copyEditingToSelectedCell(e);
        });

    });
}));
