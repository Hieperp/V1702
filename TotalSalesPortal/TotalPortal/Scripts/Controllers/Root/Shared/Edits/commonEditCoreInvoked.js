define([], (function () {

    $(document).ready(function () {

        var submitTypeOptionSaveCollection = document.getElementsByClassName("SubmitTypeOption-Save");
        for (var i = 0; i < submitTypeOptionSaveCollection.length; i++) {
            submitTypeOptionSaveCollection[i].addEventListener('click', function () {
                $("#SubmitTypeOption").val(SubmitTypeOption.Save);
            });
        }

        var submitTypeOptionClosedCollection = document.getElementsByClassName("SubmitTypeOption-Closed");
        for (var i = 0; i < submitTypeOptionClosedCollection.length; i++) {
            submitTypeOptionClosedCollection[i].addEventListener('click', function () {
                $("#SubmitTypeOption").val(SubmitTypeOption.Closed);
            });
        }


        var submitTypeOptionPopupCollection = document.getElementsByClassName("SubmitTypeOption-Popup");
        for (var i = 0; i < submitTypeOptionPopupCollection.length; i++) {
            submitTypeOptionPopupCollection[i].addEventListener('click', function () {
                $("#SubmitTypeOption").val(SubmitTypeOption.Popup);
            });
        }

        var submitTypeOptionCreateCollection = document.getElementsByClassName("SubmitTypeOption-Create");
        for (var i = 0; i < submitTypeOptionCreateCollection.length; i++) {
            submitTypeOptionCreateCollection[i].addEventListener('click', function () {
                $("#SubmitTypeOption").val(SubmitTypeOption.Create);
            });
        }


    });

}));