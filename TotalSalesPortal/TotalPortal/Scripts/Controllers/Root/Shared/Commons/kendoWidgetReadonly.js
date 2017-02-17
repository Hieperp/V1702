define([], (function () {

    $(document).ready(function () {
        if (!requireConfig.pageOptions.Editable)
            $("input").each(function (index, element) {
                if (!$(element).hasClass("SpecialEditable")) {
                    var widgetObject = kendo.widgetInstance($(element), kendo.ui);
                    if (typeof widgetObject != 'undefined') widgetObject.readonly(true);
                }
            });

    });

}));