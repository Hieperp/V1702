define(["commonEditCoreInvoked", "commonEditWithDetail"], (function (commonEditCoreInvoked, commonEditWithDetail) {

    $(document).ready(function () {

        $("form").submit(function (event) {            
            var commonEditWithDetailInstance = new commonEditWithDetail();
            return commonEditWithDetailInstance._validate();
        });



        if ($("#kendoGridDetails").data("kendoGrid") != undefined)
            $("#kendoGridDetails").data("kendoGrid").bind("edit", function (e) {
                var input = e.container.find("input");
                setTimeout(function () { input.select(); });
            });

        $(document.body).keydown(function (e) {
            if (e.keyCode === 45) {
                keydown_insert("CommodityName");
            }
        });

    });
    

}));