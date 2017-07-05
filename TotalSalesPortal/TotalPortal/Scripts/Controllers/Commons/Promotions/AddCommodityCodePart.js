////function cancelButton_Click() {
////    window.parent.$("#myWindow").data("kendoWindow").close();
////}

function handleOKEvent(deliveryAdviceGridDataSource) {
    
    if (deliveryAdviceGridDataSource != undefined && ($("#CodePartA_CodePart").val() != "" || $("#CodePartB_CodePart").val() != "" || $("#CodePartC_CodePart").val() != "")) {
        
        var deliveryAdviceJSON = deliveryAdviceGridDataSource.data().toJSON();
        _setParentInput(deliveryAdviceJSON, $("#CodePartA_CodePart").val(), $("#CodePartB_CodePart").val(), $("#CodePartC_CodePart").val());


        deliveryAdviceJSON.push(new Object()); //Add a temporary empty row

        deliveryAdviceGridDataSource.data(deliveryAdviceJSON);

        var rawData = deliveryAdviceGridDataSource.data()
        deliveryAdviceGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row

        //cancelButton_Click();
    }


    //http://www.telerik.com/forums/adding-multiple-rows-performance
    //By design the dataSource does not provide an opportunity for inserting multiple records via one operation. The performance is low, because each time when you add row through the addRow method, the DataSource throws its change event which forces the Grid to refresh and re-paint the content.
    //To avoid the problem you may try to modify the data of the DataSource manually.
    //var grid = $("#grid").data("kendoGrid");
    //var data = gr.dataSource.data().toJSON(); //the data of the DataSource

    ////change the data array
    ////any changes in the data array will not automatically reflect in the Grid

    //grid.dataSource.data(data); //set changed data as data of the Grid


    function _setParentInput(deliveryAdviceJSON, codePartA, codePartB, codePartC) {
        var dataRow = new Object();

        dataRow.PromotionCommodityCodePartID = 0;
        dataRow.PromotionID = window.parent.$("#PromotionID").val();

        dataRow.CommodityBrandID = null;

        dataRow.CodePartA = codePartA;
        dataRow.CodePartB = codePartB;
        dataRow.CodePartC = codePartC;

        dataRow.Remarks = null;
        dataRow.InActive = false;

        deliveryAdviceJSON.push(dataRow);
    }
}

