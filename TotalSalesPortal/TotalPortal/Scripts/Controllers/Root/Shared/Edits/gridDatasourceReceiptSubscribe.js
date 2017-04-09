define(["gridDatasourceReceipt"], (function (gridDatasourceReceipt) {
    $(document).ready(function () {

        $("#kendoGridDetails").data("kendoGrid").dataSource.bind("change", function (e) {
            var gridDatasourceReceiptInstance = new gridDatasourceReceipt("kendoGridDetails");
            gridDatasourceReceiptInstance.handleDataSourceChange(e);
        });

    });
}));
