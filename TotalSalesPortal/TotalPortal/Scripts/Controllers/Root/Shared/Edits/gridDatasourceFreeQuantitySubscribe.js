define(["gridDatasourceFreeQuantity"], (function (gridDatasourceFreeQuantity) {
    $(document).ready(function () {

        $("#kendoGridDetails").data("kendoGrid").dataSource.bind("change", function (e) {
            var gridDatasourceFreeQuantityInstance = new gridDatasourceFreeQuantity("kendoGridDetails");
            gridDatasourceFreeQuantityInstance.handleDataSourceChange(e);
        });

    });
}));


