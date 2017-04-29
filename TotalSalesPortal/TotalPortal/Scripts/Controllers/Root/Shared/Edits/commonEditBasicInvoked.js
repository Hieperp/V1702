define(["commonEditCoreInvoked", "commonEditBasic"], (function (commonEditCoreInvoked, commonEditBasic) {

    $(document).ready(function () {

        $("form").submit(function (event) {            
            var commonEditBasicInstance = new commonEditBasic();
            return commonEditBasicInstance._validate();
        });

    });

}));