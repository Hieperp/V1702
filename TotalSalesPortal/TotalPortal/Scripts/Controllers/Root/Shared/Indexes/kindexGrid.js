define([], (function () {

    var definedExemplar = function (kenGridName) {
        this._kenGrid = $("#" + kenGridName).data("kendoGrid");
        //this.pageIndex = 0;
    };


    definedExemplar.prototype.setSelectedRow = function (idName, idValue) {
        var that = this; var elemFound = false
        $.each(this._kenGrid.dataSource.data(), function (idx, elem) {
            if (idValue > 0 && elem[idName] == idValue) {
                $('[data-uid=' + elem.uid + ']').addClass('k-state-selected');

                //var pageIndex = Math.floor(idx / that._kenGrid.dataSource.pageSize() + 1);
                //if (pageIndex >= 0) {
                //    that._kenGrid.dataSource.fetch(function () {
                //        that._kenGrid.dataSource.page(pageIndex);
                //    });
                //}

                elemFound = true //return false;
            }

            if (typeof decoratingKindex === "function")
                decoratingKindex(idx, elem); // check whether decoratingKindex is defined, then safe to call the decoratingKindex function
            else
                if (elemFound === true)
                    return false;
        });
    };

    definedExemplar.prototype.showName = function (idName, idValue) { alert(this._kenGrid); }

    return definedExemplar;
}));