define(["gridWidgetGoodsReceipt"], (function (gridWidgetGoodsReceipt) {
    $(document).ready(function () {

        $("#kendoGridDetails").data("kendoGrid").bind("change", function (e) {
            var gridWidgetGoodsReceiptInstance = new gridWidgetGoodsReceipt();
            gridWidgetGoodsReceiptInstance.copyEditingToSelectedCell(e);
        });

    });
}));
