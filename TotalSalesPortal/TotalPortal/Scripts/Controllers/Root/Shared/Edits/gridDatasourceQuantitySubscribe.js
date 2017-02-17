define(["gridDatasourceQuantity"], (function (gridDatasourceQuantity) {
    $(document).ready(function () {

        $("#kendoGridDetails").data("kendoGrid").dataSource.bind("change", function (e) {
            var gridDatasourceQuantityInstance = new gridDatasourceQuantity("kendoGridDetails");
            gridDatasourceQuantityInstance.handleDataSourceChange(e);
        });

    });
}));
