define([], (function () {

    var definedExemplar = function (kenGrid) {
        this._kenGrid = kenGrid;
    };


    definedExemplar.prototype.ToggleBonusWarrantyClaim = function (e) {
        
        if (e.keyCode == 119 || e.keyCode == 120) { //F8 || F9
            var kenGrid = this._kenGrid;
            var selectedItems = [];

            this._kenGrid.select().each(function (index, eachItem) { //Step 1) Add all selected dataItem to an array selectedItems
                var selectedItem = kenGrid.dataItem(eachItem);
                if (selectedItem != undefined) { //Note: We can not handle each selectedItem here, because: the this._kenGrid render selectedItem right after call selectedItem.set("xyz", 123) => so we lost the select() collection
                    selectedItems.push(selectedItem); //To overcome this: we use this workaround: Using step 1) and then step 2) as comment above
                }
            });

            selectedItems.forEach(function (eachItem) { //Step 2) And then handle eachItem in array
                if (e.keyCode == 119) { eachItem.set("IsBonus", !eachItem.IsBonus); eachItem.set("InActivePartial", null); }
                if (e.keyCode == 120) { eachItem.set("InActivePartial", !eachItem.InActivePartial); eachItem.set("IsBonus", null); }
                eachItem.set("DiscountPercent", (eachItem.IsBonus) ? 100 : 0);
            });
        }

    };

    return definedExemplar;

}));