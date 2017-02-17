define(["gridDatasourceAmount"], (function (gridDatasourceAmount) {
    $(document).ready(function () {

        $("#kendoGridDetails").data("kendoGrid").dataSource.bind("change", function (e) {
            var gridDatasourceAmountInstance = new gridDatasourceAmount("kendoGridDetails");
            gridDatasourceAmountInstance.handleDataSourceChange(e);
        });

    });
}));
