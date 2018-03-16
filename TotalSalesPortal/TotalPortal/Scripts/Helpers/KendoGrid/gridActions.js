function doSelectAll(kenGrid) {

    var allDataRows = kenGrid.dataSource.data();

    if (allDataRows.length > 0) {
        kenGrid.bind("dataBinding", function (e) { e.preventDefault(); }); //You can prevent the data bind action at the dataBinding event, when updating multiple model properties

        for (var i = 0; i < allDataRows.length; i++) {
            var dataItem = allDataRows[i];
            if (dataItem.IsSelected != undefined)
                dataItem.set('IsSelected', true);
        }

        kenGrid.unbind("dataBinding"); //When finish modifying the data, unbind dataBinding event
        kenGrid.refresh(); //Then, finally, refresh grid
    }
}
