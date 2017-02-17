var czDLGResult = "Cancel";
var czDLGWindow;
var czDLGCallBack;

function czDLGClose(BtnResult) {
    czDLGResult = BtnResult;
    czDLGWindow.close();
};

function czDLGCloseCallBack(e) {
    czDLGWindow.unbind("close", czDLGCloseCallBack);
    if (czDLGCallBack !== null) {
        czDLGCallBack(czDLGResult);
    }
}

function czDLG(Title, Message, Type, Buttons, theFunction) {
    var DLGData = '<table cellpadding="0" cellspacing="0"><tr><td><div class="czDLGIcon ' + Type + '"></div></td>' +
        '<td><div class="czDLGText">' + Message + '</div></td></tr></table><div style="text-align:right;">';
    for (var i in Buttons) {
        var s = Buttons[i];
        DLGData += '<input class="k-button" style="margin:2px;" type="button" onclick="czDLGClose(\'' + s + '\')" value="' + s + '">';
    }
    DLGData += '</div>';
    czDLGResult = "Cancel";
    if (theFunction !== undefined) {
        czDLGCallBack = theFunction;
    } else {
        czDLGCallBack = null;
    }

    czDLGWindow = $("#czDLGWindow").kendoWindow({
        actions: ["Close"],
        draggable: true,
        modal: true,
        resizable: false,
        visible: false,
        title: "Confirm action",
    }).data("kendoWindow");

    czDLGWindow.bind("close", czDLGCloseCallBack);
    czDLGWindow.title(Title);
    czDLGWindow.center();
    czDLGWindow.content(DLGData);
    czDLGWindow.open();
}


function tableToExcel(table, sheetname) {
    var uri = 'data:application/vnd.ms-excel;base64,'
      , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><meta http-equiv="content-type" content="text/plain; charset=UTF-8"/></head><body><table>{table}</table></body></html>'
      , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
      , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
    if (!table.nodeType) table = document.getElementById(table)
    var ctx = { worksheet: sheetname || 'Worksheet', table: table.innerHTML }
    window.location.href = uri + base64(format(template, ctx))
}

