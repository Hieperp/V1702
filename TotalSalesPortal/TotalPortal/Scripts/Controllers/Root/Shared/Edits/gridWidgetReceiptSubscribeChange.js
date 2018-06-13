define([], (function () {

    copyAmountDueToReceiptAmount = function (totalReceiptAmount) { //Define function to be called by action some button
        var kenGrid = $("#kendoGridDetails").data("kendoGrid");
        kenGrid.bind("dataBinding", function (e) { e.preventDefault(); }); //You can prevent the data bind action at the dataBinding event, when updating multiple model properties

        doCopy(kenGrid, totalReceiptAmount);

        kenGrid.unbind("dataBinding"); //When finish modifying the data, unbind dataBinding event
        kenGrid.refresh(); //Then, finally, refresh grid
    }

    doCopy = function (kenGrid, totalReceiptAmount) {
        var allDataRows = kenGrid.dataSource.data(); var receiptAmount = 0;
        for (var i = 0; i < allDataRows.length; i++) {
            var dataItem = allDataRows[i];

            if (totalReceiptAmount != undefined) {
                receiptAmount = DoRound(totalReceiptAmount > (dataItem["AmountDue"] - dataItem["CashDiscount"] - dataItem["OtherDiscount"] + dataItem["FluctuationAmount"]) ? dataItem["AmountDue"] - dataItem["CashDiscount"] - dataItem["OtherDiscount"] + dataItem["FluctuationAmount"]: totalReceiptAmount, requireConfig.websiteOptions.rndAmount);
                totalReceiptAmount = DoRound(totalReceiptAmount - receiptAmount, requireConfig.websiteOptions.rndAmount);
            }
            else
                receiptAmount = DoRound(dataItem["AmountDue"] - dataItem["CashDiscount"] - dataItem["OtherDiscount"] + dataItem["FluctuationAmount"], requireConfig.websiteOptions.rndAmount);

            dataItem.set("ReceiptAmount", receiptAmount);
        }
    };

}));
