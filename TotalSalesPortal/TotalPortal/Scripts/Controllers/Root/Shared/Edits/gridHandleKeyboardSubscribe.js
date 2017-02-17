define(["gridHandleKeyboard"], (function (gridHandleKeyboard) {
    $(document).ready(function () {

        $("#kendoGridDetails").bind("keydown", function (e) {
            var gridHandleKeyboardInstance = new gridHandleKeyboard($("#kendoGridDetails").data("kendoGrid"));
            gridHandleKeyboardInstance.ToggleBonusWarrantyClaim(e);
        });

    });
}));


