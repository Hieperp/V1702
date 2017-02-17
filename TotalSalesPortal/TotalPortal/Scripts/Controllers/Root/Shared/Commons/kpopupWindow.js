define([], (function () {

    var definedExemplar = function () {
    }

    definedExemplar.prototype.openWindow = function (popupWindow, url, popupTitle, popupWidth, popupHeight) {

        popupWindow.append("<div id='myWindow'></div>");

        var myWindow = $("#myWindow").kendoWindow({
            title: popupTitle,
            width: popupWidth,
            height: popupHeight,
            actions: ["Pin", "Close"],
            resizable: false,//draggable: false,
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

        myWindow.center().open();
        myWindow.refresh(url);
    }

    return definedExemplar;
}));