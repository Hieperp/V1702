//BEGIN Grid data source helper =>  to render kendoGridDetails
var rowNumber = 0;
function RowNumber(data) {
    return ++rowNumber;
}

function index(dataItem) {
    var data = $("#kendoGridDetails").data("kendoGrid").dataSource.data();
    return data.indexOf(dataItem);
}
//END Grid data source helper
