define(["gridDatasourceWeight"], (function (gridDatasourceWeight) {
    $(document).ready(function () {

        $("#kendoGridDetails").data("kendoGrid").dataSource.bind("change", function (e) {
            var gridDatasourceWeightInstance = new gridDatasourceWeight("kendoGridDetails");
            gridDatasourceWeightInstance.handleDataSourceChange(e);
        });

    });
}));
