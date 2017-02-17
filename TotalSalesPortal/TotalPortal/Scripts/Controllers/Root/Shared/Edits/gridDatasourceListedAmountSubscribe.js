define(["gridDatasourceListedAmount"], (function (gridDatasourceListedAmount) {
    $(document).ready(function () {

        $("#kendoGridDetails").data("kendoGrid").dataSource.bind("change", function (e) {
            var gridDatasourceListedAmountInstance = new gridDatasourceListedAmount("kendoGridDetails");
            gridDatasourceListedAmountInstance.handleDataSourceChange(e);
        });

    });
}));


