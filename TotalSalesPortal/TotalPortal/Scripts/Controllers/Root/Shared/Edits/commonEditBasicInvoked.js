define(["commonEditCoreInvoked", "commonEditBasic"], (function (commonEditCoreInvoked, commonEditBasic) {

    $(document).ready(function () {

        $("form").submit(function (event) {
            alert("basic");
            var commonEditBasicInstance = new commonEditBasic();
            return commonEditBasicInstance._validate();
        });

    });

}));