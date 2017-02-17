define([], (function () {

    var definedExemplar = function (requiredColumn) {
        this.requiredColumn = requiredColumn;
    };

    definedExemplar.prototype.checkValidColumn = function (workSheet) {
        if (workSheet.length > 0) {
            var isValid = true;

            $.each(this.requiredColumn, function (index, eachColumn) {
                if (workSheet[0][eachColumn] === undefined) {
                    isValid = false;
                    return false;
                }
            })

            return isValid;
        }
        else
            return false;
    };





    return definedExemplar;

}));