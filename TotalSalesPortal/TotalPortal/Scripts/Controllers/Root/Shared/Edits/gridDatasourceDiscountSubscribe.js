define(["gridDatasourceDiscount"], (function (gridDatasourceDiscount) {
    $(document).ready(function () {

        $("#kendoGridDetails").data("kendoGrid").dataSource.bind("change", function (e) {
            var gridDatasourceDiscountInstance = new gridDatasourceDiscount("kendoGridDetails");
            gridDatasourceDiscountInstance.handleDataSourceChange(e);
        });

    });
}));


