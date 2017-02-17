define([], (function () {

    var definedExemplar = function (validatedDocument, kenGridName) {
        this._validatedDocument = validatedDocument;
        this._kenGrid = $("#" + kenGridName).data("kendoGrid");
        this._kenGrid._name = kenGridName;
    };


    definedExemplar.prototype.validate = function () {
        if ($("#SubmitTypeOption").val() == SubmitTypeOption.Save || $("#SubmitTypeOption").val() == SubmitTypeOption.Closed) {
            if (!$(this._validatedDocument).valid()) {
                $("#div-alert").css("display", "block");
            }
            else {
                $("#div-alert").css("display", "none");

                if (this._kenGrid.dataSource.hasChanges()) {

                    var validatable = $("#" + this._kenGrid._name).kendoValidator().data("kendoValidator");
                    if (validatable.validate()) {

                        __enforceKendoValidator(this._kenGrid);
                        return validatable.validate();
                    }
                    else {
                        return false;
                    }

                }
                else {
                    return true;
                }
            }
        }


        //inner local function.begin
        function __enforceKendoValidator(kendoGrid) {

            var rows = kendoGrid.tbody.find("tr");    // See if there are any insert rows
            var listrows = kendoGrid.dataSource.data();
            for (var i = 0; i < listrows.length; i++) {
                if ((listrows[i].isNew() || listrows[i].dirty)) {//Check only IsNew or IsDirty rows
                    var cols = $(rows[i]).find("td");
                    for (var j = 0; j < cols.length; j++) {
                        kendoGrid.editCell($(cols[j]));
                        if (kendoGrid.editable != null) {
                            if (!kendoGrid.editable.end()) {// By calling editable end we will make validation fire
                                return; //Return when first invalid cell found
                            }
                            else {
                                kendoGrid.closeCell();// Take cell out of edit mode
                            }
                        }
                    }
                }
            }
        }//inner local function.end



    };




    return definedExemplar;
}));