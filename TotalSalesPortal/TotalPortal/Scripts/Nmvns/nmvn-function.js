function kendGrid_ErrorHandler(e) {
    if (e.errors) {
        var message = "Errors:\n";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });
        alert(message);
    }
}


function CreatePopUp(popuptitle, widthsize, heightsize, popupWindow) {    
    popupWindow.append("<div id='window'></div>");
    var mywindow = $("#window")
    .kendoWindow({
        width: widthsize,
        height: heightsize,
        actions: ["Pin", "Close"],
        title: popuptitle,
        //draggable: false,
        resizable: false,
        modal: true,
        iframe: true,
        pinned: true,
        //open: function(){
        //    this.wrapper.css({ top: 20 });
        //},
        deactivate: function () {
            this.destroy();
            this.close();
        }
    }).data("kendoWindow");
    return mywindow;
}



function keydown_insert(columnEdit) {
    $("#kendoGridDetails").children(".k-grid-toolbar").children(".k-grid-add").trigger("click");
    var grid = $("#kendoGridDetails").data("kendoGrid");
    var row = grid.tbody.find('tr').last();
    grid.select(row);

    var cells = $(row).find("td");

    var columns = grid.columns;
    for (var i = 0; i < columns.length; i++) {
        if (columns[i].title == columnEdit) {
            grid.editCell(cells[i]);
        }
    }
}




function DoRound(value, decimals) {
    if (arguments.length < 2 || decimals === undefined || decimals === 0)
        return Math.round(value);
    else
        return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
}