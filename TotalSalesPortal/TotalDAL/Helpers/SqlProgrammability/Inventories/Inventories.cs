﻿using System.Text;
using TotalModel.Models;
using TotalBase.Enums;
using System;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class Inventories
    {

        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public Inventories(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            //this.VWCommodityCategories();
            //this.UpdateWarehouseBalance();
            //this.GetOverStockItems();
            //this.WarehouseJournal();
            //this.WarehouseJournalTESTSummary();
            //this.VehicleJournal();
            //this.VehicleCard();



            //this.SalesInvoiceJournal(); THAY THE BOI SalesInvoiceJournal MOI!
        }


        private void UpdateWarehouseBalance()
        {
            //@UpdateWarehouseBalanceOption: 1 ADD, -1-MINUS
            string queryString = " @UpdateWarehouseBalanceOption Int, @GoodsReceiptID Int, @SalesInvoiceID Int, @StockTransferID Int, @InventoryAdjustmentID Int " + "\r\n";

            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";


            #region INIT DATA TO BE INPUT OR OUTPUT
            //INIT DATA TO BE INPUT OR OUTPUT.BEGIN
            queryString = queryString + "       DECLARE @ActionTable TABLE (" + "\r\n";
            queryString = queryString + "           ActionID int NOT NULL ," + "\r\n";
            queryString = queryString + "           CommodityID int NOT NULL ," + "\r\n";
            queryString = queryString + "           WarehouseID int NOT NULL ," + "\r\n";
            queryString = queryString + "           GoodsReceiptTypeID int NOT NULL ," + "\r\n";
            queryString = queryString + "           AdjustmentWithAddedQuantity int NOT NULL ," + "\r\n";
            queryString = queryString + "           EntryDate datetime NOT NULL ," + "\r\n";
            queryString = queryString + "           Quantity float NOT NULL ," + "\r\n";
            queryString = queryString + "           AmountCost float NOT NULL ," + "\r\n";
            queryString = queryString + "           Remarks nvarchar (100))" + "\r\n";

            queryString = queryString + "       IF @GoodsReceiptID > 0 " + "\r\n";
            queryString = queryString + "           INSERT      @ActionTable " + "\r\n";
            queryString = queryString + "           SELECT      MIN(GoodsReceiptID), CommodityID, WarehouseID, MIN(GoodsReceiptTypeID) AS GoodsReceiptTypeID, 0 AS AdjustmentWithAddedQuantity, MIN(EntryDate), SUM(@UpdateWarehouseBalanceOption * Quantity), 0 AS AmountCost, '' AS Remarks " + "\r\n";
            queryString = queryString + "           FROM        GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "           WHERE       GoodsReceiptID = @GoodsReceiptID AND CommodityID IN (SELECT CommodityID FROM Commodities WHERE CommodityTypeID IN (" + (int)GlobalEnums.CommodityTypeID.Parts + ", " + (int)GlobalEnums.CommodityTypeID.Consumables + "))" + "\r\n";
            queryString = queryString + "           GROUP BY    WarehouseID, CommodityID" + "\r\n";

            queryString = queryString + "       IF @SalesInvoiceID > 0 " + "\r\n";
            queryString = queryString + "           INSERT      @ActionTable " + "\r\n";
            queryString = queryString + "           SELECT      MIN(SalesInvoiceID), CommodityID, WarehouseID, 0 AS GoodsReceiptTypeID, 0 AS AdjustmentWithAddedQuantity, MIN(EntryDate), SUM(@UpdateWarehouseBalanceOption * Quantity), 0 AS AmountCost, '' AS Remarks " + "\r\n";
            queryString = queryString + "           FROM        SalesInvoiceDetails " + "\r\n";
            queryString = queryString + "           WHERE       SalesInvoiceID = @SalesInvoiceID " + "\r\n";
            queryString = queryString + "           GROUP BY    WarehouseID, CommodityID" + "\r\n";

            queryString = queryString + "       IF @StockTransferID > 0 " + "\r\n";
            queryString = queryString + "           INSERT      @ActionTable " + "\r\n";
            queryString = queryString + "           SELECT      MIN(StockTransferID), CommodityID, WarehouseID, 0 AS GoodsReceiptTypeID, 0 AS AdjustmentWithAddedQuantity, MIN(EntryDate), SUM(@UpdateWarehouseBalanceOption * Quantity), 0 AS AmountCost, '' AS Remarks " + "\r\n";
            queryString = queryString + "           FROM        StockTransferDetails " + "\r\n";
            queryString = queryString + "           WHERE       StockTransferID = @StockTransferID " + "\r\n";
            queryString = queryString + "           GROUP BY    WarehouseID, CommodityID" + "\r\n";

            queryString = queryString + "       IF @InventoryAdjustmentID > 0 " + "\r\n";
            queryString = queryString + "           INSERT      @ActionTable " + "\r\n";
            queryString = queryString + "           SELECT      MIN(InventoryAdjustmentID), CommodityID, WarehouseID, 0 AS GoodsReceiptTypeID, MAX(Quantity) AS AdjustmentWithAddedQuantity, MIN(EntryDate), SUM(@UpdateWarehouseBalanceOption * Quantity), 0 AS AmountCost, '' AS Remarks " + "\r\n";
            queryString = queryString + "           FROM        InventoryAdjustmentDetails " + "\r\n";
            queryString = queryString + "           WHERE       InventoryAdjustmentID = @InventoryAdjustmentID " + "\r\n";
            queryString = queryString + "           GROUP BY    WarehouseID, CommodityID" + "\r\n";

            //INIT DATA TO BE INPUT OR OUTPUT.END



            queryString = queryString + "       DECLARE         @EntryDate DateTime " + "\r\n";
            queryString = queryString + "       DECLARE         CursorEntryDate CURSOR LOCAL FOR SELECT MAX(EntryDate) AS EntryDate FROM @ActionTable" + "\r\n";
            queryString = queryString + "       OPEN            CursorEntryDate" + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorEntryDate INTO @EntryDate" + "\r\n";
            queryString = queryString + "       CLOSE           CursorEntryDate DEALLOCATE CursorEntryDate " + "\r\n";
            queryString = queryString + "       IF @EntryDate = NULL   RETURN " + "\r\n";//Nothing to update -> Exit immediately
            #endregion


            #region FIRST CALCULATE FOR QUANTITY
            queryString = queryString + "       DECLARE @EntryDateEveryMonth DateTime, @EntryDateMAX DateTime" + "\r\n";

            queryString = queryString + "       DECLARE         CursorWarehouseBalance CURSOR LOCAL FOR SELECT MAX(EntryDate) AS EntryDate FROM WarehouseBalanceDetail" + "\r\n";
            queryString = queryString + "       OPEN            CursorWarehouseBalance" + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorWarehouseBalance INTO @EntryDateMAX" + "\r\n";
            queryString = queryString + "       CLOSE           CursorWarehouseBalance DEALLOCATE CursorWarehouseBalance " + "\r\n";


            queryString = queryString + "       IF @EntryDateMAX IS NULL SET @EntryDateMAX = CONVERT(Datetime, '2015-04-30 23:59:59', 120) " + "\r\n"; //--END OF APR/ 2015: FIRT MONTH

            queryString = queryString + "       SET @EntryDateEveryMonth = @EntryDateMAX " + "\r\n";//--GET THE MAXIMUM OF EntryDate


            //                                  STEP 1: COPY THE SAME BALANCE/ PRICE FOR EVERY WEEKEND UP TO THE MONTH CONTAIN @EntryDate
            queryString = queryString + "       IF @EntryDate > @EntryDateMAX" + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               WHILE dbo.EOMONTHTIME(@EntryDate, 9999) >= dbo.EOMONTHTIME(@EntryDateEveryMonth, 1)" + "\r\n";
            queryString = queryString + "                   BEGIN" + "\r\n";
            queryString = queryString + "                       SET @EntryDateEveryMonth = dbo.EOMONTHTIME(@EntryDateEveryMonth, 1)" + "\r\n";

            queryString = queryString + "                       INSERT INTO WarehouseBalanceDetail (EntryDate, WarehouseID, CommodityID, Quantity, AmountCost, Remarks)" + "\r\n";
            queryString = queryString + "                       SELECT      @EntryDateEveryMonth, WarehouseID, CommodityID, Quantity, AmountCost, Remarks " + "\r\n";
            queryString = queryString + "                       FROM        WarehouseBalanceDetail " + "\r\n";
            queryString = queryString + "                       WHERE       EntryDate = @EntryDateMAX" + "\r\n";

            //-----------04/12/2015: VAN DE MAU CHOT LA O CHO NAY!!!!COPY WarehouseBalancePrice CHO THANG SAU TU THANG TRUOC: CAN PHAI TINH TOAN LAI AVERAGE PRICE FOR NEXT MONTH (UnitPrice) BY: TONG TRI GIA/ TONG SO LUONG OF LAST MONTH ===> CACH TINH DON GIAN AP DUNG TU NGAY 09/03/2016 LA: LAY GIA TRI/ SO LUONG CUOI THANG TRUOC => DON GIA TRUNG BINH CHO THANG SAU
            //DOI VOI HOTEL: AVERAGE PRICE FOR NEXT MONTH DUOC TINH TOAN THEO TUNG HOTEL, TRONG KHI DO: TotalBikePortals: AVERAGE PRICE FOR NEXT MONTH BY ALL WarehouseID => CAN PHAI SUM BY CommodityID FOR ALL WarehouseID
            //LUU Y: TABLE WarehouseBalanceDetail KHONG NEN CO COLUMN UnitPrice VI SE GAY HIEU NHAM (CONFUSE), BOI VI UnitPrice PHAI DUOC TINH BINH QUAN FOR ALL WarehouseID, TRONG KHI DO: TABLE WarehouseBalanceDetail LUU TRU GIA TRI AmountCost VA Quantity FOR EVERY WarehouseID
            queryString = queryString + "                       INSERT INTO WarehouseBalancePrice (EntryDate, CommodityID, UnitPrice) " + "\r\n";
            queryString = queryString + "                       SELECT      @EntryDateEveryMonth, CommodityID, SUM(AmountCost)/ SUM(Quantity) AS UnitPrice " + "\r\n";
            queryString = queryString + "                       FROM        WarehouseBalanceDetail " + "\r\n";
            queryString = queryString + "                       WHERE       EntryDate = @EntryDateMAX GROUP BY CommodityID " + "\r\n";

            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               SET @EntryDateMAX = @EntryDateEveryMonth " + "\r\n";//--SET THE MAXIMUM OF EntryDate
            queryString = queryString + "           END " + "\r\n";


            //                                  STEP 2: UPDATE NEW QUANTITY FOR THESE ITEMS CURRENTLY EXIST IN WarehouseBalanceDetail
            queryString = queryString + "       UPDATE  WarehouseBalanceDetail" + "\r\n";//NO NEED TO UPDATE AmountCost. It will be calculated later in the process
            queryString = queryString + "       SET     WarehouseBalanceDetail.Quantity = WarehouseBalanceDetail.Quantity + ActionTable.Quantity" + "\r\n";
            queryString = queryString + "       FROM    WarehouseBalanceDetail INNER JOIN" + "\r\n";
            queryString = queryString + "               @ActionTable ActionTable ON WarehouseBalanceDetail.WarehouseID = ActionTable.WarehouseID AND WarehouseBalanceDetail.CommodityID = ActionTable.CommodityID AND WarehouseBalanceDetail.EntryDate >= @EntryDate" + "\r\n";



            //                                  STEP 3: INSERT INTO WarehouseBalanceDetail FOR THESE ITEMS NOT CURRENTLY EXIST IN WarehouseBalanceDetail
            queryString = queryString + "       SET     @EntryDateEveryMonth = dbo.EOMONTHTIME(@EntryDate, 9999)" + "\r\n";//--FIND THE FIRST @EntryDateEveryMonth WHICH IS GREATER OR EQUAL TO @EntryDate

            queryString = queryString + "       WHILE @EntryDateEveryMonth <= @EntryDateMAX" + "\r\n";
            queryString = queryString + "           BEGIN" + "\r\n";

            queryString = queryString + "               INSERT INTO     WarehouseBalanceDetail (EntryDate, WarehouseID, CommodityID, Quantity, AmountCost, Remarks)" + "\r\n";
            queryString = queryString + "               SELECT          @EntryDateEveryMonth, ActionTable.WarehouseID, ActionTable.CommodityID, ActionTable.Quantity, ActionTable.AmountCost, ActionTable.Remarks" + "\r\n";
            queryString = queryString + "               FROM            @ActionTable ActionTable LEFT JOIN" + "\r\n";
            queryString = queryString + "                               WarehouseBalanceDetail ON ActionTable.WarehouseID = WarehouseBalanceDetail.WarehouseID AND ActionTable.CommodityID = WarehouseBalanceDetail.CommodityID AND WarehouseBalanceDetail.EntryDate = @EntryDateEveryMonth" + "\r\n";
            queryString = queryString + "               WHERE           WarehouseBalanceDetail.CommodityID IS NULL " + "\r\n"; //--ADD NOT-IN-LIST ITEM"

            queryString = queryString + "               SET     @EntryDateEveryMonth = dbo.EOMONTHTIME(@EntryDateEveryMonth, 1)" + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       DELETE FROM WarehouseBalanceDetail WHERE Quantity = 0 " + "\r\n"; //????WHAT WRONG HERE?

            #endregion

            #region Update Warehouse balance average price + ending amount

            queryString = queryString + "       DECLARE     @LastDayOfPreviousMonth DateTime" + "\r\n";
            queryString = queryString + "       DECLARE     @WarehouseInputCollection TABLE (WarehouseID int NOT NULL, CommodityID int NOT NULL, Quantity float NOT NULL, PurchaseInvoiceQuantity float NOT NULL, AmountCost float NOT NULL, UnitPrice float NOT NULL) " + "\r\n";
            queryString = queryString + "       DECLARE     @WarehouseInputAveragePrice TABLE (CommodityID int NOT NULL, UnitPrice float NOT NULL) " + "\r\n";

            queryString = queryString + "       SET         @EntryDateEveryMonth = dbo.EOMONTHTIME(@EntryDate, 9999)" + "\r\n";//--FIND THE FIRST @EntryDateEveryMonth WHICH IS GREATER OR EQUAL TO @EntryDate



            queryString = queryString + "       DECLARE     @NeedToGenerateAverageUnitPrice bit " + "\r\n"; //DELETE and RE-UPDATE WarehouseBalancePrice OF THESE ITEMS INCLUDE IN @ActionTable WHEN (@GoodsReceiptID > 0 AND GoodsReceiptTypeID = GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice) (MEAN WHEN WAREHOUSE INPUT BY PurchaseInvoice)   OR   (@SalesInvoiceID AND @EntryDateEveryMonth < @EntryDateMAX) (MEAN WAREHOUSE OUTPUT OCCUR ON THE MONTH BEFORE THE LAST MONTH) IN ORDER TO RECALCULATE WarehouseBalancePrice       OR (WHEN @InventoryAdjustmentID > 0: @EntryDateEveryMonth < @EntryDateMAX OR AdjustmentWithAddedQuantity XEM CHI TIET NGAY BEN DUOI DE HIEU THEM)
            queryString = queryString + "       SET         @NeedToGenerateAverageUnitPrice = IIF (@SalesInvoiceID > 0 AND @EntryDateEveryMonth < @EntryDateMAX, 1, 0) " + "\r\n";

            queryString = queryString + "       IF          @NeedToGenerateAverageUnitPrice = 0 AND @GoodsReceiptID > 0 " + "\r\n";
            queryString = queryString + "                   IF      EXISTS (SELECT * FROM @ActionTable WHERE GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + ") " + "\r\n";
            queryString = queryString + "                           SET     @NeedToGenerateAverageUnitPrice = 1 " + "\r\n";

            queryString = queryString + "       IF          @NeedToGenerateAverageUnitPrice = 0 AND @InventoryAdjustmentID > 0 " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                           IF @EntryDateEveryMonth < @EntryDateMAX " + "\r\n";
            queryString = queryString + "                                   SET     @NeedToGenerateAverageUnitPrice = 1 " + "\r\n";
            queryString = queryString + "                           ELSE " + "\r\n";
            queryString = queryString + "                                   IF      EXISTS (SELECT * FROM @ActionTable WHERE AdjustmentWithAddedQuantity > 0) " + "\r\n";
            queryString = queryString + "                                           SET     @NeedToGenerateAverageUnitPrice = 1 " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";





            queryString = queryString + "       IF @NeedToGenerateAverageUnitPrice = 1 " + "\r\n";
            queryString = queryString + "                   DELETE FROM WarehouseBalancePrice WHERE EntryDate >= @EntryDateEveryMonth AND CommodityID IN (SELECT CommodityID FROM @ActionTable) " + "\r\n"; //(USING @ActionTable AS A FILTER)




            queryString = queryString + "       WHILE @EntryDateEveryMonth <= @EntryDateMAX" + "\r\n";
            queryString = queryString + "           BEGIN" + "\r\n";

            queryString = queryString + "               SET     @LastDayOfPreviousMonth = dbo.EOMONTHTIME(@EntryDateEveryMonth, -1) " + "\r\n";

            //                                          STEP 1: GET COLLECTION OF BEGIN BALANCE + INPUT (Quantity, AmountCost). Note: PurchaseInvoiceQuantity AND AmountCost effected by BEGINING + INPUT FOR GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice ONLY
            queryString = queryString + "               INSERT INTO     @WarehouseInputCollection (WarehouseID, CommodityID, Quantity, PurchaseInvoiceQuantity, AmountCost, UnitPrice) " + "\r\n";
            queryString = queryString + "               SELECT          WarehouseID, CommodityID, SUM(Quantity), SUM(PurchaseInvoiceQuantity), SUM(AmountCost), 0 AS UnitPrice " + "\r\n";

            queryString = queryString + "               FROM            (" + "\r\n";

            queryString = queryString + "                               SELECT      WarehouseBalanceDetail.WarehouseID, WarehouseBalanceDetail.CommodityID, WarehouseBalanceDetail.Quantity, WarehouseBalanceDetail.Quantity AS PurchaseInvoiceQuantity, WarehouseBalanceDetail.AmountCost AS AmountCost" + "\r\n";
            queryString = queryString + "                               FROM        WarehouseBalanceDetail " + "\r\n"; //BEGIN BALANCE (USING @ActionTable AS A FILTER)
            queryString = queryString + "                               WHERE       EntryDate = @LastDayOfPreviousMonth AND CommodityID IN (SELECT CommodityID FROM @ActionTable) " + "\r\n";

            queryString = queryString + "                               UNION ALL " + "\r\n";

            queryString = queryString + "                               SELECT      GoodsReceiptDetails.WarehouseID, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.Quantity, CASE WHEN GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " THEN GoodsReceiptDetails.Quantity ELSE 0 END AS PurchaseInvoiceQuantity, ISNULL(ROUND(GoodsReceiptDetails.Quantity * PurchaseInvoiceDetails.UnitPrice, " + (int)GlobalEnums.rndAmount + "), 0) AS AmountCost" + "\r\n";
            queryString = queryString + "                               FROM        GoodsReceiptDetails LEFT JOIN " + "\r\n";  //INPUT (USING @ActionTable AS A FILTER) + AND PLEASE SPECIAL CONSIDER TO THIS CONDITION CLAUSE: (@UpdateWarehouseBalanceOption = " + (int)GlobalEnums.UpdateWarehouseBalanceOption.Add + " OR GoodsReceiptDetails.GoodsReceiptID <> @GoodsReceiptID)      (MEANS: @GoodsReceiptID <> 0 AND WHEN GlobalEnums.UpdateWarehouseBalanceOption.Minus: DON'T INCLUDE THIS GoodsReceipt TO THE COLLECTION FOR CALCULATE THE PRICE + ENDING AMOUNT)
            queryString = queryString + "                                           PurchaseInvoiceDetails ON GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " AND GoodsReceiptDetails.VoucherDetailID = PurchaseInvoiceDetails.PurchaseInvoiceDetailID " + "\r\n";

            queryString = queryString + "                               WHERE      (@UpdateWarehouseBalanceOption = " + (int)GlobalEnums.UpdateWarehouseBalanceOption.Add + " OR GoodsReceiptDetails.GoodsReceiptID <> @GoodsReceiptID) AND GoodsReceiptDetails.EntryDate > @LastDayOfPreviousMonth AND GoodsReceiptDetails.EntryDate <= @EntryDateEveryMonth AND GoodsReceiptDetails.CommodityID IN (SELECT CommodityID FROM @ActionTable) " + "\r\n";

            queryString = queryString + "                               UNION ALL " + "\r\n";

            queryString = queryString + "                               SELECT      InventoryAdjustmentDetails.WarehouseID, InventoryAdjustmentDetails.CommodityID, InventoryAdjustmentDetails.Quantity, InventoryAdjustmentDetails.Quantity AS PurchaseInvoiceQuantity, InventoryAdjustmentDetails.Amount AS AmountCost" + "\r\n";
            queryString = queryString + "                               FROM        InventoryAdjustmentDetails " + "\r\n";  //INPUT (USING @ActionTable AS A FILTER) + AND PLEASE SPECIAL CONSIDER TO THIS CONDITION CLAUSE: (@UpdateWarehouseBalanceOption = " + (int)GlobalEnums.UpdateWarehouseBalanceOption.Add + " OR InventoryAdjustmentDetails.InventoryAdjustmentID <> @InventoryAdjustmentID)      (MEANS: @InventoryAdjustmentID <> 0 AND WHEN GlobalEnums.UpdateWarehouseBalanceOption.Minus: DON'T INCLUDE THIS InventoryAdjustment TO THE COLLECTION FOR CALCULATE THE PRICE + ENDING AMOUNT)

            queryString = queryString + "                               WHERE      (@UpdateWarehouseBalanceOption = " + (int)GlobalEnums.UpdateWarehouseBalanceOption.Add + " OR InventoryAdjustmentDetails.InventoryAdjustmentID <> @InventoryAdjustmentID) AND InventoryAdjustmentDetails.EntryDate > @LastDayOfPreviousMonth AND InventoryAdjustmentDetails.EntryDate <= @EntryDateEveryMonth AND InventoryAdjustmentDetails.Quantity > 0 AND InventoryAdjustmentDetails.CommodityID IN (SELECT CommodityID FROM @ActionTable) " + "\r\n";


            queryString = queryString + "                               )WarehouseJournalUnion" + "\r\n";

            queryString = queryString + "               GROUP BY        WarehouseID, CommodityID " + "\r\n";
            queryString = queryString + "               HAVING          SUM(Quantity) <> 0 " + "\r\n";






            //                                          STEP 2: GENERATE AVERAGE UNIT PRICE FOR THE MONTH (IF NEEDED)--- THE PRICE HERE IS THE SAME PRICE AT: STEP 2.1: UPDATE AVERAGE PRICE (THE SAME UnitPrice ACROSS WarehousID)
            queryString = queryString + "               IF @NeedToGenerateAverageUnitPrice = 1 " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";

            //!!!!!!!!!!!!!Very important: can bo sung t/h chuyen kho cuoi thang chua nhap vo kho, bao gom chuyen kho thang truoc: chua nhap kho + t/h nhap kho vao thang sau
            //                                                  A: CALCULATE THE NEW AveragePrice OF THE MONTH
            queryString = queryString + "                       INSERT INTO     @WarehouseInputAveragePrice (CommodityID, UnitPrice) " + "\r\n";
            queryString = queryString + "                       SELECT          CommodityID, SUM(AmountCost)/ SUM(Quantity) " + "\r\n";
            queryString = queryString + "                       FROM            (" + "\r\n";

            queryString = queryString + "                                       SELECT      CommodityID, PurchaseInvoiceQuantity AS Quantity, AmountCost " + "\r\n";
            queryString = queryString + "                                       FROM        @WarehouseInputCollection" + "\r\n"; //IN STOCK COLLECTION (BEGIN + GOODSRECEIPT + ADJUSTMENT)

            // --OPENNING: PENDING STOCKTRANSFER (STOCKTRANSFER BUT NOT GOODSRECEIPT YET)  //BEGIN
            queryString = queryString + "                                       UNION ALL" + "\r\n";
            queryString = queryString + "                                       SELECT      StockTransferDetails.CommodityID, ROUND(StockTransferDetails.Quantity - StockTransferDetails.QuantityReceipt, " + (int)GlobalEnums.rndAmount + ") AS Quantity, ROUND((StockTransferDetails.Quantity - StockTransferDetails.QuantityReceipt) * WarehouseBalancePrice.UnitPrice, " + (int)GlobalEnums.rndAmount + ") AS AmountCost " + "\r\n";
            queryString = queryString + "                                       FROM        StockTransferDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                                                   WarehouseBalancePrice ON StockTransferDetails.EntryDate <= @LastDayOfPreviousMonth AND ROUND(StockTransferDetails.Quantity - StockTransferDetails.QuantityReceipt, " + (int)GlobalEnums.rndAmount + ") > 0 AND StockTransferDetails.CommodityID IN (SELECT CommodityID FROM @ActionTable) AND WarehouseBalancePrice.EntryDate = @LastDayOfPreviousMonth AND StockTransferDetails.CommodityID = WarehouseBalancePrice.CommodityID " + "\r\n";
            queryString = queryString + "                                       WHERE       @UpdateWarehouseBalanceOption = " + (int)GlobalEnums.UpdateWarehouseBalanceOption.Minus + " OR StockTransferDetails.StockTransferID <> @StockTransferID " + "\r\n";

            queryString = queryString + "                                       UNION ALL" + "\r\n";
            queryString = queryString + "                                       SELECT      GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.Quantity, ROUND(GoodsReceiptDetails.Quantity * WarehouseBalancePrice.UnitPrice, " + (int)GlobalEnums.rndAmount + ") AS AmountCost " + "\r\n";
            queryString = queryString + "                                       FROM        StockTransfers INNER JOIN " + "\r\n";
            queryString = queryString + "                                                   GoodsReceiptDetails ON StockTransfers.EntryDate <= @LastDayOfPreviousMonth AND GoodsReceiptDetails.EntryDate > @LastDayOfPreviousMonth AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " AND GoodsReceiptDetails.CommodityID IN (SELECT CommodityID FROM @ActionTable) AND StockTransfers.StockTransferID = GoodsReceiptDetails.VoucherID INNER JOIN " + "\r\n";
            queryString = queryString + "                                                   WarehouseBalancePrice ON WarehouseBalancePrice.EntryDate = @LastDayOfPreviousMonth AND GoodsReceiptDetails.CommodityID = WarehouseBalancePrice.CommodityID " + "\r\n";
            //queryString = queryString + "                                     WHERE       @UpdateWarehouseBalanceOption = " + (int)GlobalEnums.UpdateWarehouseBalanceOption.Minus + " OR StockTransfers.StockTransferID <> @StockTransferID " + "\r\n"; THAT RA, DIEU KIEN NAY LA KHONG CAN THIET, VI NEU StockTransfers.StockTransferID = @StockTransferID, TUC LA StockTransfers EDITABLE -> KHI DO: INNER JOIN GoodsReceiptDetails: SE RETURN NOTHING
            // --OPENNING: PENDING STOCKTRANSFER (STOCKTRANSFER BUT NOT GOODSRECEIPT YET)  //END
            queryString = queryString + "                                       )WarehouseInputAveragePriceUnion" + "\r\n";
            queryString = queryString + "                       GROUP BY        CommodityID " + "\r\n";




            //                                                  B: SAVE THE NEW AveragePrice
            queryString = queryString + "                       INSERT INTO     WarehouseBalancePrice (EntryDate, CommodityID, UnitPrice) " + "\r\n";
            queryString = queryString + "                       SELECT          @EntryDateEveryMonth, CommodityID, UnitPrice " + "\r\n";
            queryString = queryString + "                       FROM            @WarehouseInputAveragePrice" + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //@NeedToGenerateAverageUnitPrice = 0 
            queryString = queryString + "                   BEGIN " + "\r\n";

            //                                                  A: GET THE CURRENT SAVED AveragePrice OF THE MONTH
            queryString = queryString + "                       INSERT INTO     @WarehouseInputAveragePrice (CommodityID, UnitPrice) " + "\r\n";
            queryString = queryString + "                       SELECT          CommodityID, UnitPrice " + "\r\n";
            queryString = queryString + "                       FROM            WarehouseBalancePrice " + "\r\n";
            queryString = queryString + "                       WHERE           EntryDate = @EntryDateEveryMonth AND CommodityID IN (SELECT CommodityID FROM @ActionTable) " + "\r\n";


            //-------------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!CHO NAY: CHAC LA PHAI BO BOT VIEC REMOVE, BOI VI ENDING AMOUNT: KHONG PHAI LA LAY SL * DON GIA NUA, THAY VAO DO: PHAI TINH TOAN LAI CHO TAT CA CAC KHO!!!!!
            //                                                  B: REMOVE ROW WITH (WarehouseID, CommodityID) NO NEED TO UPDATE ENDING CODE (USING @ActionTable AS FILTER)  --- BOI VI: AveragePrice KHONG THAY DOI -> NHUNG WarehouseID NAO KHONG CAN THIET (TUC WAREHOUSE KHONG CO TRONG @ActionTable) SE KHONG CAN PHAI UPDATE ENDING
            queryString = queryString + "                       DELETE          WarehouseInputCollection " + "\r\n";
            queryString = queryString + "                       FROM            @WarehouseInputCollection WarehouseInputCollection LEFT JOIN " + "\r\n";
            queryString = queryString + "                                       @ActionTable ActionTable ON WarehouseInputCollection.WarehouseID = ActionTable.WarehouseID AND WarehouseInputCollection.CommodityID = ActionTable.CommodityID " + "\r\n";
            queryString = queryString + "                       WHERE           ActionTable.WarehouseID IS NULL " + "\r\n"; //--ADD NOT-IN-LIST ITEM"
            queryString = queryString + "                   END " + "\r\n";


            //                                          STEP 2.1: UPDATE AVERAGE PRICE FOR THESE CommodityID IN @WarehouseInputCollection
            queryString = queryString + "               UPDATE          WarehouseInputCollection " + "\r\n";
            queryString = queryString + "               SET             WarehouseInputCollection.UnitPrice = WarehouseInputAveragePrice.UnitPrice " + "\r\n";
            queryString = queryString + "               FROM            @WarehouseInputCollection WarehouseInputCollection INNER JOIN " + "\r\n";
            queryString = queryString + "                               @WarehouseInputAveragePrice WarehouseInputAveragePrice ON WarehouseInputCollection.CommodityID = WarehouseInputAveragePrice.CommodityID " + "\r\n";




            //                                          STEP 3: RECALCULATE END BALANCE (AmountCost)
            queryString = queryString + "               UPDATE          WarehouseBalanceDetail " + "\r\n";
            queryString = queryString + "               SET             WarehouseBalanceDetail.AmountCost = ROUND(WarehouseBalanceAmount.AmountCost, 0) " + "\r\n";
            queryString = queryString + "               FROM            WarehouseBalanceDetail INNER JOIN " + "\r\n";
            queryString = queryString + "                              (SELECT          WarehouseID, CommodityID, SUM(Quantity) AS Quantity, SUM(AmountCost) AS AmountCost " + "\r\n";

            queryString = queryString + "                               FROM           (" + "\r\n";

            queryString = queryString + "                                               SELECT      WarehouseInputCollection.WarehouseID, WarehouseInputCollection.CommodityID, WarehouseInputCollection.Quantity, WarehouseInputCollection.Quantity * WarehouseInputCollection.UnitPrice AS AmountCost" + "\r\n";
            queryString = queryString + "                                               FROM        @WarehouseInputCollection WarehouseInputCollection " + "\r\n";

            queryString = queryString + "                                               UNION ALL " + "\r\n";

            queryString = queryString + "                                               SELECT      WarehouseInputCollection.WarehouseID, WarehouseInputCollection.CommodityID, -SalesInvoiceDetails.Quantity AS Quantity, -SalesInvoiceDetails.Quantity * WarehouseInputCollection.UnitPrice AS AmountCost" + "\r\n";
            queryString = queryString + "                                               FROM        @WarehouseInputCollection WarehouseInputCollection INNER JOIN " + "\r\n";
            queryString = queryString + "                                                           SalesInvoiceDetails ON WarehouseInputCollection.WarehouseID = SalesInvoiceDetails.WarehouseID AND WarehouseInputCollection.CommodityID = SalesInvoiceDetails.CommodityID AND SalesInvoiceDetails.EntryDate > @LastDayOfPreviousMonth AND SalesInvoiceDetails.EntryDate <= @EntryDateEveryMonth " + "\r\n";
            queryString = queryString + "                                               WHERE       @UpdateWarehouseBalanceOption = " + (int)GlobalEnums.UpdateWarehouseBalanceOption.Minus + " OR SalesInvoiceDetails.SalesInvoiceID <> @SalesInvoiceID " + "\r\n";

            queryString = queryString + "                                               UNION ALL " + "\r\n";

            queryString = queryString + "                                               SELECT      WarehouseInputCollection.WarehouseID, WarehouseInputCollection.CommodityID, -StockTransferDetails.Quantity AS Quantity, -StockTransferDetails.Quantity * WarehouseInputCollection.UnitPrice AS AmountCost" + "\r\n";
            queryString = queryString + "                                               FROM        @WarehouseInputCollection WarehouseInputCollection INNER JOIN " + "\r\n";
            queryString = queryString + "                                                           StockTransferDetails ON WarehouseInputCollection.WarehouseID = StockTransferDetails.WarehouseID AND WarehouseInputCollection.CommodityID = StockTransferDetails.CommodityID AND StockTransferDetails.EntryDate > @LastDayOfPreviousMonth AND StockTransferDetails.EntryDate <= @EntryDateEveryMonth " + "\r\n";
            queryString = queryString + "                                               WHERE       @UpdateWarehouseBalanceOption = " + (int)GlobalEnums.UpdateWarehouseBalanceOption.Minus + " OR StockTransferDetails.StockTransferID <> @StockTransferID " + "\r\n";

            queryString = queryString + "                                               UNION ALL " + "\r\n";

            queryString = queryString + "                                               SELECT      WarehouseInputCollection.WarehouseID, WarehouseInputCollection.CommodityID, InventoryAdjustmentDetails.Quantity AS Quantity, InventoryAdjustmentDetails.Quantity * WarehouseInputCollection.UnitPrice AS AmountCost" + "\r\n";
            queryString = queryString + "                                               FROM        @WarehouseInputCollection WarehouseInputCollection INNER JOIN " + "\r\n";
            queryString = queryString + "                                                           InventoryAdjustmentDetails ON WarehouseInputCollection.WarehouseID = InventoryAdjustmentDetails.WarehouseID AND WarehouseInputCollection.CommodityID = InventoryAdjustmentDetails.CommodityID AND InventoryAdjustmentDetails.EntryDate > @LastDayOfPreviousMonth AND InventoryAdjustmentDetails.EntryDate <= @EntryDateEveryMonth AND InventoryAdjustmentDetails.Quantity < 0 " + "\r\n";
            queryString = queryString + "                                               WHERE       @UpdateWarehouseBalanceOption = " + (int)GlobalEnums.UpdateWarehouseBalanceOption.Add + " OR InventoryAdjustmentDetails.InventoryAdjustmentID <> @InventoryAdjustmentID " + "\r\n";

            queryString = queryString + "                                              )WarehouseBalanceUnion" + "\r\n";

            queryString = queryString + "                               GROUP BY        WarehouseID, CommodityID " + "\r\n";

            queryString = queryString + "                              )WarehouseBalanceAmount ON WarehouseBalanceDetail.EntryDate = @EntryDateEveryMonth AND WarehouseBalanceDetail.WarehouseID = WarehouseBalanceAmount.WarehouseID AND WarehouseBalanceDetail.CommodityID = WarehouseBalanceAmount.CommodityID " + "\r\n";


            //                                          STEP 4: INIT VARIBLE FOR NEW LOOP
            queryString = queryString + "               DELETE FROM     @WarehouseInputCollection " + "\r\n";
            queryString = queryString + "               DELETE FROM     @WarehouseInputAveragePrice " + "\r\n";
            queryString = queryString + "               SET     @EntryDateEveryMonth = dbo.EOMONTHTIME(@EntryDateEveryMonth, 1)" + "\r\n";

            queryString = queryString + "           END " + "\r\n";

            #endregion Update Warehouse balance average price + ending amount


            queryString = queryString + "   END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("UpdateWarehouseBalance", queryString);
        }


        private void GetOverStockItems()
        {
            //CAN PHAI XEM LAI CAC GHI CHU TRONG ERmgrVCP DE BIET NHUNG VAN DE CAN PHAI LUU Y, TRONG DO: VE THOI DIEM KIEM TRA, VE MA HANG/ KHO HANG CAN PHAI KIEM TRA
            //LUU Y VAN DE BACKLOG COLLECTION!!! HIEN CHUA QUAN TAM DEN BACKLOG
            string queryWhere = " WarehouseID IN (SELECT WarehouseID FROM @WarehouseFilter) AND CommodityID IN (SELECT CommodityID FROM @CommodityFilter) ";

            string queryString = " (@CheckedDate DateTime, @WarehouseIDList varchar(35), @CommodityIDList varchar(3999)) " + "\r\n";
            queryString = queryString + " RETURNS @OverStockTable TABLE (OverStockDate DateTime NOT NULL, WarehouseID int NOT NULL, WarehouseCode nvarchar(100) NOT NULL, CommodityID int NOT NULL, CommodityCode nvarchar(100) NOT NULL, CommodityName nvarchar(100) NOT NULL, Quantity float NOT NULL) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@WarehouseIDList = '' OR @CommodityIDList = '') RETURN " + "\r\n";

            queryString = queryString + "       DECLARE     @WarehouseFilter TABLE (WarehouseID int NOT NULL) " + "\r\n";
            queryString = queryString + "       INSERT INTO @WarehouseFilter SELECT Id FROM dbo.SplitToIntList (@WarehouseIDList) " + "\r\n";


            queryString = queryString + "       DECLARE     @CommodityFilter TABLE (CommodityID int NOT NULL) " + "\r\n";
            queryString = queryString + "       INSERT INTO @CommodityFilter SELECT Id FROM dbo.SplitToIntList (@CommodityIDList) " + "\r\n";


            queryString = queryString + "       DECLARE @TempDate DateTime " + "\r\n";
            queryString = queryString + "       DECLARE @BackLogDateMax DateTime " + "\r\n";


            // --GET THE BEGIN BALANCE IF AVAILABLE.BEGIN
            queryString = queryString + "       DECLARE     @WarehouseBalanceDetail TABLE (WarehouseID int NOT NULL, CommodityID int NOT NULL, Quantity float NOT NULL)" + "\r\n";

            queryString = queryString + "       DECLARE     @EntryDateBEGIN DateTime" + "\r\n";
            queryString = queryString + "       SELECT      @EntryDateBEGIN = MAX(EntryDate) FROM WarehouseBalanceDetail WHERE EntryDate <= @CheckedDate" + "\r\n";

            queryString = queryString + "       IF NOT @EntryDateBEGIN IS NULL" + "\r\n";
            queryString = queryString + "           INSERT  @WarehouseBalanceDetail SELECT WarehouseID, CommodityID, Quantity FROM WarehouseBalanceDetail WHERE EntryDate = @EntryDateBEGIN AND " + queryWhere + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           SET     @EntryDateBEGIN = CONVERT(Datetime, '2015-05-31 23:59:59', 120) " + "\r\n";
            // --GET THE BEGIN BALANCE IF AVAILABLE.END


            // --GET THE DATE RANGE NEED TO BE CHECKED.BEGIN
            queryString = queryString + "       DECLARE     @EntryDateEND DateTime" + "\r\n";
            queryString = queryString + "       SELECT      @EntryDateEND = MAX(EntryDate) FROM WarehouseBalanceDetail " + "\r\n";

            queryString = queryString + "       IF          @EntryDateEND IS NULL OR @EntryDateEND < @CheckedDate SET @EntryDateEND = @CheckedDate " + "\r\n";  //--CHECK UNTIL THE LAST BALANCE
            queryString = queryString + "       IF          @EntryDateEND < @BackLogDateMax SET @EntryDateEND = @BackLogDateMax " + "\r\n"; //--OR CHECK UNTIL THE LAST DATE OF BACKLOG

            // --GET THE DATE RANGE NEED TO BE CHECKED.END



            queryString = queryString + "       SET         @TempDate = @CheckedDate " + "\r\n";
            queryString = queryString + "       WHILE       @TempDate <= @EntryDateEND" + "\r\n";
            queryString = queryString + "           BEGIN" + "\r\n";

            // --BALANCE AT: @EntryDateBEGIN: LOOK ON WarehouseBalanceDetail ONLY
            // --BALANCE AT: @TempDate > @EntryDateBEGIN: WarehouseBalanceDetail + SUM(INPUT) - SUM(Output)
            queryString = queryString + "               INSERT INTO @OverStockTable" + "\r\n";
            queryString = queryString + "               SELECT      @TempDate, WarehouseID, N'', CommodityID, N'', N'', ROUND(SUM(Quantity), 0) AS Quantity" + "\r\n";
            queryString = queryString + "               FROM        (" + "\r\n";
            // --OPENNING
            queryString = queryString + "                           SELECT      WarehouseID, CommodityID, Quantity" + "\r\n";
            queryString = queryString + "                           FROM        @WarehouseBalanceDetail WarehouseBalanceDetail" + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";
            // --INPUT: GoodsReceiptDetails
            queryString = queryString + "                           SELECT      WarehouseID, CommodityID, Quantity" + "\r\n";
            queryString = queryString + "                           FROM        GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                           WHERE       EntryDate > @EntryDateBEGIN AND EntryDate <= @TempDate AND " + queryWhere + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";
            // --OUTPUT: SalesInvoiceDetails
            queryString = queryString + "                           SELECT      WarehouseID, CommodityID, -Quantity" + "\r\n";
            queryString = queryString + "                           FROM        SalesInvoiceDetails " + "\r\n";
            queryString = queryString + "                           WHERE       EntryDate > @EntryDateBEGIN AND EntryDate <= @TempDate AND " + queryWhere + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";
            // --OUTPUT: StockTransferDetails
            queryString = queryString + "                           SELECT      WarehouseID, CommodityID, -Quantity" + "\r\n";
            queryString = queryString + "                           FROM        StockTransferDetails " + "\r\n";
            queryString = queryString + "                           WHERE       EntryDate > @EntryDateBEGIN AND EntryDate <= @TempDate AND " + queryWhere + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";
            // --INPUT: InventoryAdjustmentDetails
            queryString = queryString + "                           SELECT      WarehouseID, CommodityID, Quantity" + "\r\n"; //THIS INCLUDED BOTH + AND - Quantity
            queryString = queryString + "                           FROM        InventoryAdjustmentDetails " + "\r\n";
            queryString = queryString + "                           WHERE       EntryDate > @EntryDateBEGIN AND EntryDate <= @TempDate AND " + queryWhere + "\r\n";

            queryString = queryString + "                           )OverStockTable" + "\r\n";
            queryString = queryString + "               GROUP BY    WarehouseID, CommodityID " + "\r\n";
            queryString = queryString + "               HAVING      ROUND(SUM(Quantity), 0) < 0 " + "\r\n";

            queryString = queryString + "               DECLARE     @COUNTOverStock Int SET @COUNTOverStock = 0" + "\r\n";
            queryString = queryString + "               SELECT      @COUNTOverStock = COUNT(*) FROM @OverStockTable" + "\r\n";

            queryString = queryString + "               IF @COUNTOverStock > 0 " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   UPDATE OverStockTable SET OverStockTable.CommodityCode = Commodities.Code, OverStockTable.CommodityName = Commodities.Name FROM @OverStockTable OverStockTable INNER JOIN Commodities ON OverStockTable.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   UPDATE OverStockTable SET OverStockTable.WarehouseCode = Warehouses.Code FROM @OverStockTable OverStockTable INNER JOIN Warehouses ON OverStockTable.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                   BREAK" + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "               SET @TempDate = DATEADD(Day, 1, @TempDate)" + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       RETURN " + "\r\n";
            queryString = queryString + "   END " + "\r\n";


            this.totalSalesPortalEntities.CreateUserDefinedFunction("GetOverStockItems", queryString);
        }



        public string GET_WarehouseJournal_BUILD_SQL_VINHLONG(string warehouseJournalTable, string fromDate, string toDate, string warehouseIDList, string commodityIDList, string isFullJournal, string isAmountIncluded)
        {//Note: AmountInputINVByAveragePrice la gia tri hang mua trong thang nhap kho theo don gia trung binh thang (Tuc la: So luong mua hang * don gia binh quan cua thang (Tuc la don gia dau ky binh quan voi don gia mua hang trong thang -> don gia binh quan cua thang))
            string queryString = "              DECLARE     " + warehouseJournalTable + " TABLE \r\n";

            queryString = queryString + "                  (GroupName nvarchar(70) NOT NULL, NMVNTaskID int NOT NULL, JournalPrimaryID int NOT NULL, JournalDate DateTime NOT NULL, JournalReference nvarchar(50) NOT NULL, JournalDescription nvarchar(210) NULL, " + "\r\n";
            queryString = queryString + "                   WarehouseID int NOT NULL, WarehouseCode nvarchar(200) NOT NULL, WarehouseName nvarchar(200) NOT NULL, CommodityID int NOT NULL, CommodityCode nvarchar(50) NOT NULL, CommodityName nvarchar(200) NOT NULL, SalesUnit nvarchar(50) NULL, " + "\r\n";
            queryString = queryString + "                   QuantityBeginREC decimal(18, 2) NOT NULL, QuantityBeginTRA decimal(18, 2) NOT NULL, AmountBegin float NOT NULL, QuantityInputINV decimal(18, 2) NOT NULL, UnitPriceInputINV float NOT NULL, AmountInputINV float NOT NULL, VATAmountInputINV float NOT NULL, GrossAmountInputINV float NOT NULL, AmountInputINVByAveragePrice float NOT NULL, QuantityInputTRA decimal(18, 2) NOT NULL, AmountInputTRA float NOT NULL, QuantityInputADJ decimal(18, 2) NOT NULL, AmountInputADJ float NOT NULL, QuantityOutputINV decimal(18, 2) NOT NULL, AmountOutputINV float NOT NULL, QuantityOutputTRA decimal(18, 2) NOT NULL, AmountOutputTRA float NOT NULL, QuantityOutputADJ decimal(18, 2) NOT NULL, AmountOutputADJ float NOT NULL, QuantityEndREC decimal(18, 2) NOT NULL, QuantityEndTRA decimal(18, 2) NOT NULL, QuantityEnd decimal(18, 2) NOT NULL, AmountEnd float NOT NULL, " + "\r\n";
            queryString = queryString + "                   CommodityCategoryID int NOT NULL, CommodityCategory1 nvarchar(100) NOT NULL, CommodityCategory2 nvarchar(100) NOT NULL, CommodityCategory3 nvarchar(100) NOT NULL) " + "\r\n";

            //--12-Sep-2016: tam thoi bo qua  queryString = queryString + "       INSERT INTO " + warehouseJournalTable + " EXEC WarehouseJournal " + fromDate + ", " + toDate + ", " + warehouseIDList + ", " + commodityIDList + ", " + isFullJournal + ", " + isAmountIncluded;

            return queryString;
        }

        //Public Function BUILD_SQL_SKUInventoryJournal(ByVal ltmpSKUInventoryJournal As String, ByVal lDateFrom As String, ByVal lDateTo As String, ByVal lCommodityIDList As String, ByVal lWarehouseGroupIDList As String, ByVal lWHLocationIDList As String, ByVal lWHCategoryIDList As String, ByVal lWarehouseClassIDList As String, ByVal lWarehouseIDList As String) As String
        public string GET_WarehouseJournal_BUILD_SQL(string warehouseJournalTable, string fromDate, string toDate, string warehouseIDList, string commodityIDList, string isFullJournal, string isAmountIncluded)
        {
            string queryString = "              DECLARE     " + warehouseJournalTable + " TABLE \r\n";

            queryString = queryString + "                  (NMVNTaskID int NULL, JournalPrimaryID int NULL, JournalDate datetime NULL, JournalReference nvarchar(30) NULL, JournalDescription nvarchar(202) NULL, " + "\r\n";
            queryString = queryString + "                   CommodityID int NULL, Description nvarchar(50) NULL, DescriptionOfficial nvarchar(200) NULL, DescriptionPartA nvarchar(20) NULL, DescriptionPartB nvarchar(20) NULL, DescriptionPartC nvarchar(20) NULL, DescriptionPartD nvarchar(20) NULL, UnitSales nvarchar(50) NULL, Weight float NULL, LeadTime float NULL, SellLife int NULL,  " + "\r\n";
            queryString = queryString + "                   WarehouseGroupID int NULL, WHLocationID int NULL, WHCategoryID int NULL, WarehouseClassID int NULL, WarehouseID int NULL, WarehouseName nvarchar(60) NULL, WarehouseOutID int NULL, WarehouseOutName nvarchar(60) NULL, " + "\r\n";
            queryString = queryString + "                   QuantityBegin float NULL, QuantityInputPRO float NULL, QuantityInputINV float NULL, QuantityInputRTN float NULL, QuantityInputTRF float NULL, QuantityInputADJ float NULL, QuantityInputBLD float NULL, QuantityInputUBL float NULL, QuantityInput float NULL, " + "\r\n";
            queryString = queryString + "                   QuantityOutputINV float NULL, QuantityOutputTRF float NULL, QuantityOutputADJ float NULL, QuantityOutputBLD float NULL, QuantityOutputUBL float NULL, QuantityOutput float NULL, QuantityOnTransfer float NULL, QuantityOnTransferAdviceOut float NULL, QuantityOnTransferAdviceIn float NULL, QuantityOnProduction float NULL, UPriceNMDInventory float NULL," + "\r\n";
            queryString = queryString + "                   ItemCategoryID int NULL, Description1 nvarchar(100) NULL, Description2 nvarchar(100) NULL, Description3 nvarchar(100) NULL, Description4 nvarchar(100) NULL, Description5 nvarchar(100) NULL, Description6 nvarchar(100) NULL, Description7 nvarchar(100) NULL, Description8 nvarchar(100) NULL, Description9 nvarchar(100) NULL, MaxTransferOutputDate  datetime NULL)" + "\r\n";

            //SQL = SQL & "   INSERT " & ltmpSKUInventoryJournal & " EXEC SPSKUInventoryJournal " & lDateFrom & ", " & lDateTo & ", " & lCommodityIDList & ", " & lWarehouseGroupIDList & ", " & lWHLocationIDList & ", " & lWHCategoryIDList & ", " & lWarehouseClassIDList & ", " & lWarehouseIDList
            queryString = queryString + "                   INSERT INTO " + warehouseJournalTable + " EXEC SPSKUInventoryJournal " + fromDate + ", " + toDate + ", " + commodityIDList + ", N'', N'', N'', N'', " + warehouseIDList + "\r\n";//+ ", " + isFullJournal + ", " + isAmountIncluded;










            //COMMENT ON 01-JAN-2017: NO USE WAREHOUSE INVENTORY: 

            queryString = "                     DECLARE @My01JAN2017Commodities TABLE (CommodityID int NOT NULL) INSERT INTO @My01JAN2017Commodities SELECT Id FROM dbo.SplitToIntList (" + commodityIDList + ") " + "\r\n";
            queryString = queryString + "       DECLARE     " + warehouseJournalTable + " TABLE \r\n";
            queryString = queryString + "                  (CommodityID int NULL, WarehouseID int NULL, QuantityBegin float NULL) " + "\r\n";
            queryString = queryString + "       INSERT INTO " + warehouseJournalTable + " SELECT Commodities.CommodityID, Warehouses.WarehouseID, 9999 AS QuantityBegin FROM @My01JAN2017Commodities Commodities CROSS JOIN Warehouses WHERE Warehouses.WarehouseID IN (SELECT WarehouseID FROM CustomerWarehouses WHERE CustomerID = @CustomerID AND InActive = 0) " + "\r\n";

            return queryString;
        }



        public void WarehouseJournalTESTSummary()
        {
            string queryString = " @FromDate DateTime, @ToDate DateTime " + "\r\n";

            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       " + this.GET_WarehouseJournal_BUILD_SQL("@WarehouseJournalTable", "@FromDate", "@ToDate", "''", "''", "1", "1") + "\r\n";
            queryString = queryString + "       SELECT SUM(QuantityBeginREC) AS QuantityBeginREC, SUM(QuantityBeginTRA) AS QuantityBeginTRA, SUM(AmountBegin) AS AmountBegin, SUM(QuantityInputINV) AS QuantityInputINV, SUM(AmountInputINV) AS AmountInputINV, SUM(QuantityInputTRA) AS QuantityInputTRA, SUM(AmountInputTRA) AS AmountInputTRA, SUM(QuantityInputADJ) AS QuantityInputADJ, SUM(AmountInputADJ) AS AmountInputADJ, SUM(QuantityOutputINV) AS QuantityOutputINV, SUM(AmountOutputINV) AS AmountOutputINV, SUM(QuantityOutputTRA) AS QuantityOutputTRA, SUM(AmountOutputTRA) AS AmountOutputTRA, SUM(QuantityOutputADJ) AS QuantityOutputADJ, SUM(AmountOutputADJ) AS AmountOutputADJ, SUM(QuantityEndREC) AS QuantityEndREC, SUM(QuantityEndTRA) AS QuantityEndTRA, SUM(QuantityEnd) AS QuantityEnd, SUM(AmountEnd) AS AmountEnd FROM @WarehouseJournalTable " + "\r\n";
            queryString = queryString + "       SELECT CommodityID, SUM(AmountBegin) + SUM(AmountInputINV) + SUM(AmountInputTRA) + SUM(AmountInputADJ) - SUM(AmountOutputINV) - SUM(AmountOutputTRA) - SUM(QuantityOutputADJ) - SUM(AmountOutputADJ) AS AMTEND, SUM(QuantityEnd) AS QuantityEnd, SUM(AmountEnd) AS AmountEnd, SUM(AmountBegin) + SUM(AmountInputINV) + SUM(AmountInputTRA) + SUM(AmountInputADJ) - SUM(AmountOutputINV) - SUM(AmountOutputTRA) - SUM(QuantityOutputADJ) - SUM(AmountOutputADJ) - SUM(AmountEnd) AS AMTDIFF FROM @WarehouseJournalTable GROUP BY CommodityID order by AMTDIFF " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("WarehouseJournalTESTSummary", queryString);
        }


        #region WarehouseJournal
        private void WarehouseJournal()
        {
            string queryString = " @FromDate DateTime, @ToDate DateTime, @WarehouseIDList varchar(35), @CommodityIDList varchar(3999), @isFullJournal bit, @IsAmountIncluded bit " + "\r\n";

            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime, @LocalToDate DateTime, @LocalWarehouseIDList varchar(35), @LocalCommodityIDList varchar(3999), @LocalIsFullJournal bit, @LocalIsAmountIncluded bit " + "\r\n";
            queryString = queryString + "       SET         @LocalFromDate = @FromDate  SET @LocalToDate = @ToDate  SET @LocalWarehouseIDList = @WarehouseIDList    SET @LocalCommodityIDList = @CommodityIDList    SET @LocalIsFullJournal = @isFullJournal    SET @LocalIsAmountIncluded = @IsAmountIncluded " + "\r\n";

            queryString = queryString + "       DECLARE     @WarehouseFilter TABLE (WarehouseID int NOT NULL) " + "\r\n";
            queryString = queryString + "       IF (@LocalWarehouseIDList = '') " + "\r\n";
            queryString = queryString + "                   INSERT INTO @WarehouseFilter SELECT WarehouseID FROM Warehouses " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "                   INSERT INTO @WarehouseFilter SELECT Id FROM dbo.SplitToIntList (@LocalWarehouseIDList) " + "\r\n";


            queryString = queryString + "       DECLARE     @CommodityFilter TABLE (CommodityID int NOT NULL) " + "\r\n";
            queryString = queryString + "       IF (@LocalCommodityIDList <> '') " + "\r\n";
            queryString = queryString + "                   INSERT INTO @CommodityFilter SELECT Id FROM dbo.SplitToIntList (@LocalCommodityIDList) " + "       WHERE Id IN (SELECT CommodityID FROM Commodities WHERE CommodityTypeID IN (" + (int)GlobalEnums.CommodityTypeID.Parts + ", " + (int)GlobalEnums.CommodityTypeID.Consumables + ")) " + "\r\n";

            queryString = queryString + "       IF         (@LocalIsAmountIncluded = 1) " + "\r\n"; // IF THE CALLER NEED TO IsAmountIncluded => THEN: DOUBLE CHECK FOR IsAmountIncluded AGAIN: IsAmountIncluded = TRUE ONLY WHEN @LocalFromDate - @LocalToDate IS THE WHOLE MONTH
            queryString = queryString + "                   IF          (DATEADD(second, -1, @LocalFromDate) = dbo.EOMONTHTIME(DATEADD(second, -1, @LocalFromDate), 9999) AND @LocalToDate = dbo.EOMONTHTIME(@LocalToDate, 9999) AND @LocalToDate = dbo.EOMONTHTIME( DATEADD(second, -1, @LocalFromDate), 1)) SET @LocalIsAmountIncluded = 1 ELSE SET @LocalIsAmountIncluded = 0 " + "\r\n";

            // --GET THE BEGIN BALANCE IF AVAILABLE
            queryString = queryString + "       DECLARE     @EntryDate DateTime             SET @EntryDate = (SELECT MAX(EntryDate) AS EntryDate FROM WarehouseBalanceDetail WHERE EntryDate < @LocalFromDate) " + "\r\n";// < OR <= ??? XEM XET LAI NHE!!!!
            queryString = queryString + "       IF          @EntryDate IS NULL              SET @EntryDate = CONVERT(Datetime, '2015-04-30 23:59:59', 120) " + "\r\n";
            // --GET THE BEGIN BALANCE IF AVAILABLE.END



            queryString = queryString + "       IF         (@LocalIsFullJournal = 0 AND @LocalIsAmountIncluded = 0) " + "\r\n";

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       IF          (@LocalWarehouseIDList = '' AND @LocalCommodityIDList = '') " + "\r\n";
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(false, false, false, false) + "\r\n";
            queryString = queryString + "                       ELSE    IF  (@LocalWarehouseIDList <> '' AND @LocalCommodityIDList = '') " + "\r\n";
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(false, false, true, false) + "\r\n";
            queryString = queryString + "                       ELSE    IF  (@LocalWarehouseIDList = '' AND @LocalCommodityIDList <> '') " + "\r\n";
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(false, false, false, true) + "\r\n";
            queryString = queryString + "                       ELSE        " + "\r\n"; //(@LocalWarehouseIDList <> '' AND @LocalCommodityIDList <> '') 
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(false, false, true, true) + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "       ELSE    IF  (@LocalIsFullJournal = 1 AND @LocalIsAmountIncluded = 0) " + "\r\n";

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       IF          (@LocalWarehouseIDList = '' AND @LocalCommodityIDList = '') " + "\r\n";
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(true, false, false, false) + "\r\n";
            queryString = queryString + "                       ELSE    IF  (@LocalWarehouseIDList <> '' AND @LocalCommodityIDList = '') " + "\r\n";
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(true, false, true, false) + "\r\n";
            queryString = queryString + "                       ELSE    IF  (@LocalWarehouseIDList = '' AND @LocalCommodityIDList <> '') " + "\r\n";
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(true, false, false, true) + "\r\n";
            queryString = queryString + "                       ELSE        " + "\r\n"; //(@LocalWarehouseIDList <> '' AND @LocalCommodityIDList <> '') 
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(true, false, true, true) + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "       ELSE    IF  (@LocalIsFullJournal = 0 AND @LocalIsAmountIncluded = 1) " + "\r\n";

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       IF          (@LocalWarehouseIDList = '' AND @LocalCommodityIDList = '') " + "\r\n";
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(false, true, false, false) + "\r\n";
            queryString = queryString + "                       ELSE    IF  (@LocalWarehouseIDList <> '' AND @LocalCommodityIDList = '') " + "\r\n";
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(false, true, true, false) + "\r\n";
            queryString = queryString + "                       ELSE    IF  (@LocalWarehouseIDList = '' AND @LocalCommodityIDList <> '') " + "\r\n";
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(false, true, false, true) + "\r\n";
            queryString = queryString + "                       ELSE        " + "\r\n"; //(@LocalWarehouseIDList <> '' AND @LocalCommodityIDList <> '') 
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(false, true, true, true) + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "       ELSE        " + "\r\n"; //(@LocalIsFullJournal = 1 AND @LocalIsAmountIncluded = 1)

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       IF          (@LocalWarehouseIDList = '' AND @LocalCommodityIDList = '') " + "\r\n";
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(true, true, false, false) + "\r\n";
            queryString = queryString + "                       ELSE    IF  (@LocalWarehouseIDList <> '' AND @LocalCommodityIDList = '') " + "\r\n";
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(true, true, true, false) + "\r\n";
            queryString = queryString + "                       ELSE    IF  (@LocalWarehouseIDList = '' AND @LocalCommodityIDList <> '') " + "\r\n";
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(true, true, false, true) + "\r\n";
            queryString = queryString + "                       ELSE        " + "\r\n"; //(@LocalWarehouseIDList <> '' AND @LocalCommodityIDList <> '') 
            queryString = queryString + "                                   " + this.WarehouseJournalBuildSQLA(true, true, true, true) + "\r\n";
            queryString = queryString + "                   END " + "\r\n";


            queryString = queryString + "   END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("WarehouseJournal", queryString);
        }

        private string WarehouseJournalBuildSQLA(bool isFullJournal, bool isAmountIncluded, bool isWarehouseFilter, bool isCommodityFilter)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      WarehouseJournalMaster.GroupName, WarehouseJournalMaster.NMVNTaskID, WarehouseJournalMaster.JournalPrimaryID, WarehouseJournalMaster.JournalDate, WarehouseJournalMaster.JournalReference, LEFT(WarehouseJournalMaster.JournalDescription, 200) AS JournalDescription, " + "\r\n";
            queryString = queryString + "                   WarehouseJournalMaster.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.SalesUnit, " + "\r\n";
            queryString = queryString + "                   WarehouseJournalMaster.QuantityBeginREC, WarehouseJournalMaster.QuantityBeginTRA, WarehouseJournalMaster.AmountBegin, WarehouseJournalMaster.QuantityInputINV, WarehouseJournalMaster.UnitPriceInputINV, WarehouseJournalMaster.AmountInputINV, WarehouseJournalMaster.VATAmountInputINV, WarehouseJournalMaster.GrossAmountInputINV, WarehouseJournalMaster.AmountInputINVByAveragePrice, WarehouseJournalMaster.QuantityInputTRA, WarehouseJournalMaster.AmountInputTRA, WarehouseJournalMaster.QuantityInputADJ, WarehouseJournalMaster.AmountInputADJ, WarehouseJournalMaster.QuantityOutputINV, WarehouseJournalMaster.AmountOutputINV, WarehouseJournalMaster.QuantityOutputTRA, WarehouseJournalMaster.AmountOutputTRA, WarehouseJournalMaster.QuantityOutputADJ, WarehouseJournalMaster.AmountOutputADJ, WarehouseJournalMaster.QuantityBeginREC + WarehouseJournalMaster.QuantityInputINV + WarehouseJournalMaster.QuantityInputTRA + WarehouseJournalMaster.QuantityInputADJ - WarehouseJournalMaster.QuantityOutputINV - WarehouseJournalMaster.QuantityOutputTRA - WarehouseJournalMaster.QuantityOutputADJ AS QuantityEndREC, WarehouseJournalMaster.QuantityEndTRA, WarehouseJournalMaster.QuantityEnd, WarehouseJournalMaster.AmountEnd, " + "\r\n";

            queryString = queryString + "                   VWCommodityCategories.CommodityCategoryID, " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories.Name1 AS CommodityCategory1, " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories.Name2 AS CommodityCategory2, " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories.Name3 AS CommodityCategory3 " + "\r\n";

            queryString = queryString + "       FROM        ( " + "\r\n";

            queryString = queryString + "                       SELECT      GroupName, NMVNTaskID, JournalPrimaryID, MAX(JournalDate) AS JournalDate, MAX(JournalReference) AS JournalReference, MAX(JournalDescription) AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                   CommodityID, WarehouseID, SUM(QuantityBeginREC) AS QuantityBeginREC, SUM(QuantityBeginTRA) AS QuantityBeginTRA, SUM(AmountBegin) AS AmountBegin, SUM(QuantityInputINV) AS QuantityInputINV, AVG(UnitPriceInputINV) AS UnitPriceInputINV, SUM(AmountInputINV) AS AmountInputINV, SUM(VATAmountInputINV) AS VATAmountInputINV, SUM(GrossAmountInputINV) AS GrossAmountInputINV, SUM(AmountInputINVByAveragePrice) AS AmountInputINVByAveragePrice, SUM(QuantityInputTRA) AS QuantityInputTRA, SUM(AmountInputTRA) AS AmountInputTRA, SUM(QuantityInputADJ) AS QuantityInputADJ, SUM(AmountInputADJ) AS AmountInputADJ, SUM(QuantityOutputINV) AS QuantityOutputINV, SUM(AmountOutputINV) AS AmountOutputINV, SUM(QuantityOutputTRA) AS QuantityOutputTRA, SUM(AmountOutputTRA) AS AmountOutputTRA, SUM(QuantityOutputADJ) AS QuantityOutputADJ, SUM(AmountOutputADJ) AS AmountOutputADJ, SUM(QuantityEndTRA) AS QuantityEndTRA, SUM(QuantityEnd) AS QuantityEnd, SUM(AmountEnd) AS AmountEnd " + "\r\n";

            queryString = queryString + "                       FROM       (" + "\r\n";
            queryString = queryString + "                       " + this.WarehouseJournalBuildSQLB(isFullJournal, isAmountIncluded, isWarehouseFilter, isCommodityFilter) + "\r\n";
            queryString = queryString + "                           )WarehouseJournalUnion" + "\r\n";

            queryString = queryString + "                       GROUP BY        GroupName, NMVNTaskID, JournalPrimaryID, CommodityID, WarehouseID " + "\r\n";

            queryString = queryString + "                       HAVING          SUM(QuantityBeginREC) <> 0 OR SUM(QuantityBeginTRA) <> 0 OR SUM(QuantityInputINV) <> 0 OR SUM(QuantityInputTRA) <> 0 OR SUM(QuantityInputADJ) <> 0 OR SUM(QuantityOutputINV) <> 0 OR SUM(QuantityOutputTRA) <> 0 OR SUM(QuantityOutputADJ) <> 0 OR SUM(QuantityEndTRA) <> 0 OR SUM(QuantityEnd) <> 0 OR SUM(AmountEnd) <> 0 " + "\r\n";

            queryString = queryString + "                   ) WarehouseJournalMaster INNER JOIN " + "\r\n";

            queryString = queryString + "                   Warehouses ON WarehouseJournalMaster.WarehouseID = Warehouses.WarehouseID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Commodities ON WarehouseJournalMaster.CommodityID = Commodities.CommodityID INNER JOIN " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories ON Commodities.CommodityCategoryID = VWCommodityCategories.CommodityCategoryID " + "\r\n";

            queryString = queryString + "   END " + "\r\n";
            return queryString;

        }

        private string WarehouseJournalBuildSQLB(bool isFullJournal, bool isAmountIncluded, bool isWarehouseFilter, bool isCommodityFilter)
        {
            string queryString = "";

            // --OPENNING: PURE OPENNING + ENDING AMOUNT  //BEGIN
            queryString = queryString + "                           SELECT      IIF(EntryDate = @EntryDate, '     DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103), '  TC ' + CONVERT(VARCHAR, @LocalToDate, 103)) AS GroupName, 0 AS NMVNTaskID, 0 AS JournalPrimaryID, IIF(EntryDate = @EntryDate, @LocalFromDate - 1, @LocalToDate) AS JournalDate, '' AS JournalReference, '' AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       WarehouseBalanceDetail.CommodityID, WarehouseBalanceDetail.WarehouseID, " + (isAmountIncluded ? "IIF(EntryDate = @EntryDate, WarehouseBalanceDetail.Quantity, 0)" : "WarehouseBalanceDetail.Quantity") + " AS QuantityBeginREC, 0 AS QuantityBeginTRA, " + (isAmountIncluded ? "IIF(EntryDate = @EntryDate, WarehouseBalanceDetail.AmountCost, 0)" : "0") + " AS AmountBegin, 0 AS QuantityInputINV, 0 AS UnitPriceInputINV, 0 AS AmountInputINV, 0 AS VATAmountInputINV, 0 AS GrossAmountInputINV, 0 AS AmountInputINVByAveragePrice, 0 AS QuantityInputTRA, 0 AS AmountInputTRA, 0 AS QuantityInputADJ, 0 AS AmountInputADJ, 0 AS QuantityOutputINV, 0 AS AmountOutputINV, 0 AS QuantityOutputTRA, 0 AS AmountOutputTRA, 0 AS QuantityOutputADJ, 0 AS AmountOutputADJ, 0 AS QuantityEndTRA, " + (isFullJournal ? "IIF(EntryDate <> @EntryDate, WarehouseBalanceDetail.Quantity, 0)" : "0") + " AS QuantityEnd, " + (isAmountIncluded ? "IIF(EntryDate <> @EntryDate, WarehouseBalanceDetail.AmountCost, 0)" : "0") + " AS AmountEnd " + "\r\n";
            queryString = queryString + "                           FROM        WarehouseBalanceDetail " + "\r\n";
            queryString = queryString + "                           WHERE      (EntryDate = @EntryDate " + (isAmountIncluded ? " OR EntryDate = dbo.EOMONTHTIME(@LocalToDate, 9999)" : "") + ") " + this.WarehouseJournalWarehouseFilter("WarehouseBalanceDetail", isWarehouseFilter) + this.WarehouseJournalCommodityFilter("WarehouseBalanceDetail", isCommodityFilter) + "\r\n";
            // --OPENNING: PURE OPENNING + ENDING AMOUNT   //END


            if (isFullJournal)
            {
                // --OPENNING: PENDING STOCKTRANSFER   //BEGIN
                queryString = queryString + "                       UNION ALL" + "\r\n";
                queryString = queryString + "                       SELECT      '    PT DANG CHUYEN KHO DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103) AS GroupName, 0 AS NMVNTaskID, 0 AS JournalPrimaryID, @LocalFromDate - 1 AS JournalDate, '' AS JournalReference, '' AS JournalDescription, " + "\r\n";
                queryString = queryString + "                                   StockTransferDetails.CommodityID, StockTransfers.WarehouseID, 0 AS QuantityBeginREC, ROUND(StockTransferDetails.Quantity - StockTransferDetails.QuantityReceipt, " + (int)GlobalEnums.rndAmount + ") AS QuantityBeginTRA, " + (isAmountIncluded ? "ISNULL(ROUND((StockTransferDetails.Quantity - StockTransferDetails.QuantityReceipt) * WarehouseBalancePrice.UnitPrice, " + (int)GlobalEnums.rndAmount + "), 0)" : "0") + " AS AmountBegin, 0 AS QuantityInputINV, 0 AS UnitPriceInputINV, 0 AS AmountInputINV, 0 AS VATAmountInputINV, 0 AS GrossAmountInputINV, 0 AS AmountInputINVByAveragePrice, 0 AS QuantityInputTRA, 0 AS AmountInputTRA, 0 AS QuantityInputADJ, 0 AS AmountInputADJ, 0 AS QuantityOutputINV, 0 AS AmountOutputINV, 0 AS QuantityOutputTRA, 0 AS AmountOutputTRA, 0 AS QuantityOutputADJ, 0 AS AmountOutputADJ, 0 AS QuantityEndTRA, 0 AS QuantityEnd, 0 AS AmountEnd " + "\r\n";
                queryString = queryString + "                       FROM        StockTransfers INNER JOIN " + "\r\n";
                queryString = queryString + "                                   StockTransferDetails ON StockTransfers.EntryDate < @LocalFromDate AND ROUND(StockTransferDetails.Quantity - StockTransferDetails.QuantityReceipt, " + (int)GlobalEnums.rndAmount + ") > 0 " + this.WarehouseJournalWarehouseFilter("StockTransfers", isWarehouseFilter) + this.WarehouseJournalCommodityFilter("StockTransferDetails", isCommodityFilter) + " AND StockTransfers.StockTransferID = StockTransferDetails.StockTransferID " + "\r\n";
                if (isAmountIncluded)
                    queryString = queryString + "                               LEFT JOIN WarehouseBalancePrice ON WarehouseBalancePrice.EntryDate = @EntryDate AND StockTransferDetails.CommodityID = WarehouseBalancePrice.CommodityID " + "\r\n";


                queryString = queryString + "                       UNION ALL" + "\r\n";
                queryString = queryString + "                       SELECT      '    PT DANG CHUYEN KHO DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103) AS GroupName, 0 AS NMVNTaskID, 0 AS JournalPrimaryID, @LocalFromDate - 1 AS JournalDate, '' AS JournalReference, '' AS JournalDescription, " + "\r\n";
                queryString = queryString + "                                   GoodsReceiptDetails.CommodityID, StockTransfers.WarehouseID, 0 AS QuantityBeginREC, GoodsReceiptDetails.Quantity AS QuantityBeginTRA, " + (isAmountIncluded ? "ISNULL(ROUND(GoodsReceiptDetails.Quantity * WarehouseBalancePrice.UnitPrice, " + (int)GlobalEnums.rndAmount + "), 0)" : "0") + " AS AmountBegin, 0 AS QuantityInputINV, 0 AS UnitPriceInputINV, 0 AS AmountInputINV, 0 AS VATAmountInputINV, 0 AS GrossAmountInputINV, 0 AS AmountInputINVByAveragePrice, 0 AS QuantityInputTRA, 0 AS AmountInputTRA, 0 AS QuantityInputADJ, 0 AS AmountInputADJ, 0 AS QuantityOutputINV, 0 AS AmountOutputINV, 0 AS QuantityOutputTRA, 0 AS AmountOutputTRA, 0 AS QuantityOutputADJ, 0 AS AmountOutputADJ, 0 AS QuantityEndTRA, 0 AS QuantityEnd, 0 AS AmountEnd  " + "\r\n";
                queryString = queryString + "                       FROM        StockTransfers INNER JOIN " + "\r\n";
                queryString = queryString + "                                   GoodsReceiptDetails ON StockTransfers.EntryDate < @LocalFromDate AND GoodsReceiptDetails.EntryDate >= @LocalFromDate " + this.WarehouseJournalWarehouseFilter("StockTransfers", isWarehouseFilter) + this.WarehouseJournalCommodityFilter("GoodsReceiptDetails", isCommodityFilter) + " AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " AND StockTransfers.StockTransferID = GoodsReceiptDetails.VoucherID " + "\r\n";
                if (isAmountIncluded)
                    queryString = queryString + "                               LEFT JOIN WarehouseBalancePrice ON WarehouseBalancePrice.EntryDate = @EntryDate AND GoodsReceiptDetails.CommodityID = WarehouseBalancePrice.CommodityID " + "\r\n";
                // --OPENNING: PENDING STOCKTRANSFER   //END


                // --ENDING: PENDING STOCKTRANSFER   //BEGIN

                queryString = queryString + "                       UNION ALL" + "\r\n";
                queryString = queryString + "                       SELECT      ' TC PT DANG CHUYEN KHO ' + CONVERT(VARCHAR, @LocalToDate, 103) AS GroupName, 999999999 AS NMVNTaskID, 0 AS JournalPrimaryID, @LocalToDate AS JournalDate, '' AS JournalReference, '' AS JournalDescription, " + "\r\n";
                queryString = queryString + "                                   StockTransferDetails.CommodityID, StockTransfers.WarehouseID, 0 AS QuantityBeginREC, 0 AS QuantityBeginTRA, 0 AS AmountBegin, 0 AS QuantityInputINV, 0 AS UnitPriceInputINV, 0 AS AmountInputINV, 0 AS VATAmountInputINV, 0 AS GrossAmountInputINV, 0 AS AmountInputINVByAveragePrice, 0 AS QuantityInputTRA, 0 AS AmountInputTRA, 0 AS QuantityInputADJ, 0 AS AmountInputADJ, 0 AS QuantityOutputINV, 0 AS AmountOutputINV, 0 AS QuantityOutputTRA, 0 AS AmountOutputTRA, 0 AS QuantityOutputADJ, 0 AS AmountOutputADJ, ROUND(StockTransferDetails.Quantity - StockTransferDetails.QuantityReceipt, " + (int)GlobalEnums.rndQuantity + ") AS QuantityEndTRA, ROUND(StockTransferDetails.Quantity - StockTransferDetails.QuantityReceipt, " + (int)GlobalEnums.rndQuantity + ") AS QuantityEnd, " + (isAmountIncluded ? "ISNULL(ROUND((StockTransferDetails.Quantity - StockTransferDetails.QuantityReceipt) * WarehouseBalancePrice.UnitPrice, " + (int)GlobalEnums.rndAmount + "), 0)" : "0") + " AS AmountEnd " + "\r\n";
                queryString = queryString + "                       FROM        StockTransfers INNER JOIN " + "\r\n";
                queryString = queryString + "                                   StockTransferDetails ON StockTransfers.EntryDate <= @LocalToDate AND ROUND(StockTransferDetails.Quantity - StockTransferDetails.QuantityReceipt, " + (int)GlobalEnums.rndAmount + ") > 0 " + this.WarehouseJournalWarehouseFilter("StockTransfers", isWarehouseFilter) + this.WarehouseJournalCommodityFilter("StockTransferDetails", isCommodityFilter) + " AND StockTransfers.StockTransferID = StockTransferDetails.StockTransferID " + "\r\n";
                if (isAmountIncluded)
                    queryString = queryString + "                               LEFT JOIN WarehouseBalancePrice ON WarehouseBalancePrice.EntryDate = dbo.EOMONTHTIME(@LocalToDate, 9999) AND StockTransferDetails.CommodityID = WarehouseBalancePrice.CommodityID " + "\r\n";


                queryString = queryString + "                       UNION ALL" + "\r\n";
                queryString = queryString + "                       SELECT      ' TC PT DANG CHUYEN KHO ' + CONVERT(VARCHAR, @LocalToDate, 103) AS GroupName, 999999999 AS NMVNTaskID, 0 AS JournalPrimaryID, @LocalToDate AS JournalDate, '' AS JournalReference, '' AS JournalDescription, " + "\r\n";
                queryString = queryString + "                                   GoodsReceiptDetails.CommodityID, StockTransfers.WarehouseID, 0 AS QuantityBeginREC, 0 AS QuantityBeginTRA, 0 AS AmountBegin, 0 AS QuantityInputINV, 0 AS UnitPriceInputINV, 0 AS AmountInputINV, 0 AS VATAmountInputINV, 0 AS GrossAmountInputINV, 0 AS AmountInputINVByAveragePrice, 0 AS QuantityInputTRA, 0 AS AmountInputTRA, 0 AS QuantityInputADJ, 0 AS AmountInputADJ, 0 AS QuantityOutputINV, 0 AS AmountOutputINV, 0 AS QuantityOutputTRA, 0 AS AmountOutputTRA, 0 AS QuantityOutputADJ, 0 AS AmountOutputADJ, GoodsReceiptDetails.Quantity AS QuantityEndTRA, GoodsReceiptDetails.Quantity AS QuantityEnd, " + (isAmountIncluded ? "ISNULL(ROUND(GoodsReceiptDetails.Quantity * WarehouseBalancePrice.UnitPrice, " + (int)GlobalEnums.rndAmount + "), 0)" : "0") + " AS AmountEnd " + "\r\n";
                queryString = queryString + "                       FROM        StockTransfers INNER JOIN " + "\r\n";
                queryString = queryString + "                                   GoodsReceiptDetails ON StockTransfers.EntryDate <= @LocalToDate AND GoodsReceiptDetails.EntryDate > @LocalToDate " + this.WarehouseJournalWarehouseFilter("StockTransfers", isWarehouseFilter) + this.WarehouseJournalCommodityFilter("GoodsReceiptDetails", isCommodityFilter) + " AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " AND StockTransfers.StockTransferID = GoodsReceiptDetails.VoucherID " + "\r\n";
                if (isAmountIncluded)
                    queryString = queryString + "                               LEFT JOIN WarehouseBalancePrice ON WarehouseBalancePrice.EntryDate = dbo.EOMONTHTIME(@LocalToDate, 9999) AND GoodsReceiptDetails.CommodityID = WarehouseBalancePrice.CommodityID " + "\r\n";
                // --ENDING: PENDING STOCKTRANSFER   //END
            }




            // --INPUT: IN-TERM OPENNING + INPUT   //BEGIN
            //--------MUST USE TWO SEPERATE SQL TO GET THE GoodsReceiptTypeID (VoucherID)
            // --INTPUT.PurchaseInvoice
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, '   ' + CONVERT(VARCHAR, @LocalFromDate, 103) + ' -> ' + CONVERT(VARCHAR, @LocalToDate, 103), '     DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103)) AS GroupName, IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, " + (int)GlobalEnums.NmvnTaskID.GoodsReceipt + ", 0) AS NMVNTaskID, IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, GoodsReceiptDetails.GoodsReceiptDetailID, 0) AS JournalPrimaryID, IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, GoodsReceiptDetails.EntryDate, @LocalFromDate - 1) AS JournalDate, IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, GoodsReceipts.Reference, '') AS JournalReference, IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, Suppliers.Name + ', HĐ [' + ISNULL(PurchaseInvoices.VATInvoiceNo, '') + ' Ngày: ' + CONVERT(VARCHAR, PurchaseInvoices.VATInvoiceDate, 103) + ' (' + CONVERT(VARCHAR, PurchaseInvoices.EntryDate, 103) + ')]' , '') AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.WarehouseID, IIF(GoodsReceiptDetails.EntryDate < @LocalFromDate, GoodsReceiptDetails.Quantity, 0) AS QuantityBeginREC, 0 AS QuantityBeginTRA, 0 AS AmountBegin, IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, GoodsReceiptDetails.Quantity, 0) AS QuantityInputINV, " + (isFullJournal || isAmountIncluded ? "IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, GoodsReceiptDetails.UnitPrice, 0)" : "0") + " AS UnitPriceInputINV, " + (isFullJournal || isAmountIncluded ? "IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, GoodsReceiptDetails.Amount, 0)" : "0") + " AS AmountInputINV, " + (isFullJournal || isAmountIncluded ? "IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, GoodsReceiptDetails.VATAmount, 0)" : "0") + " AS VATAmountInputINV, " + (isFullJournal || isAmountIncluded ? "IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, GoodsReceiptDetails.GrossAmount, 0)" : "0") + " AS GrossAmountInputINV, " + (isAmountIncluded ? "IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, GoodsReceiptDetails.Quantity * WarehouseBalancePrice.UnitPrice, 0)" : "0") + " AS AmountInputINVByAveragePrice, 0 AS QuantityInputTRA, 0 AS AmountInputTRA, 0 AS QuantityInputADJ, 0 AS AmountInputADJ, 0 AS QuantityOutputINV, 0 AS AmountOutputINV, 0 AS QuantityOutputTRA, 0 AS AmountOutputTRA, 0 AS QuantityOutputADJ, 0 AS AmountOutputADJ, 0 AS QuantityEndTRA, 0 AS QuantityEnd, 0 AS AmountEnd " + "\r\n";
            queryString = queryString + "                           FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                                       GoodsReceipts ON GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " AND GoodsReceiptDetails.EntryDate > @EntryDate AND GoodsReceiptDetails.EntryDate <= @LocalToDate " + this.WarehouseJournalWarehouseFilter("GoodsReceiptDetails", isWarehouseFilter) + this.WarehouseJournalCommodityFilter("GoodsReceiptDetails", isCommodityFilter) + " AND GoodsReceiptDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       PurchaseInvoices ON GoodsReceiptDetails.VoucherID = PurchaseInvoices.PurchaseInvoiceID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       Customers Suppliers ON PurchaseInvoices.SupplierID = Suppliers.CustomerID " + "\r\n";
            if (isAmountIncluded)
                queryString = queryString + "                                   LEFT JOIN WarehouseBalancePrice ON WarehouseBalancePrice.EntryDate = dbo.EOMONTHTIME(@LocalToDate, 9999) AND GoodsReceiptDetails.CommodityID = WarehouseBalancePrice.CommodityID " + "\r\n";

            // --INTPUT.StockTransfer
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, '   ' + CONVERT(VARCHAR, @LocalFromDate, 103) + ' -> ' + CONVERT(VARCHAR, @LocalToDate, 103), '     DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103)) AS GroupName, IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, " + (int)GlobalEnums.NmvnTaskID.GoodsReceipt + ", 0) AS NMVNTaskID, IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, GoodsReceiptDetails.GoodsReceiptDetailID, 0) AS JournalPrimaryID, IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, GoodsReceiptDetails.EntryDate, @LocalFromDate - 1) AS JournalDate, IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, GoodsReceipts.Reference, '') AS JournalReference, IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, 'NHAP VCNB: ' + Locations.Name + ', PX [' + StockTransfers.Reference + ' Ngày: ' + CONVERT(VARCHAR, StockTransfers.EntryDate, 103) + ']' , '') AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.WarehouseID, IIF(GoodsReceiptDetails.EntryDate < @LocalFromDate, GoodsReceiptDetails.Quantity, 0) AS QuantityBeginREC, 0 AS QuantityBeginTRA, 0 AS AmountBegin, 0 AS QuantityInputINV, 0 AS UnitPriceInputINV, 0 AS AmountInputINV, 0 AS VATAmountInputINV, 0 AS GrossAmountInputINV, 0 AS AmountInputINVByAveragePrice, IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, GoodsReceiptDetails.Quantity, 0) AS QuantityInputTRA, " + (isAmountIncluded ? "IIF(GoodsReceiptDetails.EntryDate >= @LocalFromDate, GoodsReceiptDetails.Quantity * WarehouseBalancePrice.UnitPrice, 0)" : "0") + " AS AmountInputTRA, 0 AS QuantityInputADJ, 0 AS AmountInputADJ, 0 AS QuantityOutputINV, 0 AS AmountOutputINV, 0 AS QuantityOutputTRA, 0 AS AmountOutputTRA, 0 AS QuantityOutputADJ, 0 AS AmountOutputADJ, 0 AS QuantityEndTRA, 0 AS QuantityEnd, 0 AS AmountEnd " + "\r\n";
            queryString = queryString + "                           FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                                       GoodsReceipts ON GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " AND GoodsReceiptDetails.EntryDate > @EntryDate AND GoodsReceiptDetails.EntryDate <= @LocalToDate " + this.WarehouseJournalWarehouseFilter("GoodsReceiptDetails", isWarehouseFilter) + this.WarehouseJournalCommodityFilter("GoodsReceiptDetails", isCommodityFilter) + " AND GoodsReceiptDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       StockTransfers ON GoodsReceiptDetails.VoucherID = StockTransfers.StockTransferID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       Locations ON StockTransfers.LocationID = Locations.LocationID " + "\r\n";
            if (isAmountIncluded)
                queryString = queryString + "                                   LEFT JOIN WarehouseBalancePrice ON WarehouseBalancePrice.EntryDate = dbo.EOMONTHTIME(@LocalToDate, 9999) AND GoodsReceiptDetails.CommodityID = WarehouseBalancePrice.CommodityID " + "\r\n";

            // --INTPUT.InventoryAdjustment
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, '   ' + CONVERT(VARCHAR, @LocalFromDate, 103) + ' -> ' + CONVERT(VARCHAR, @LocalToDate, 103), '     DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103)) AS GroupName, IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, " + (int)GlobalEnums.NmvnTaskID.PartAdjustment + ", 0) AS NMVNTaskID, IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, InventoryAdjustmentDetails.InventoryAdjustmentID, 0) AS JournalPrimaryID, IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, InventoryAdjustmentDetails.EntryDate, @LocalFromDate - 1) AS JournalDate, IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, InventoryAdjustments.Reference, '') AS JournalReference, IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, Suppliers.Name + ', Đ/C: ' + Suppliers.BillingAddress, '') AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       InventoryAdjustmentDetails.CommodityID, InventoryAdjustmentDetails.WarehouseID, IIF(InventoryAdjustmentDetails.EntryDate < @LocalFromDate, InventoryAdjustmentDetails.Quantity, 0) AS QuantityBeginREC, 0 AS QuantityBeginTRA, 0 AS AmountBegin, 0 AS QuantityInputINV, 0 AS UnitPriceInputINV, 0 AS AmountInputINV, 0 AS VATAmountInputINV, 0 AS GrossAmountInputINV, 0 AS AmountInputINVByAveragePrice, 0 AS QuantityInputTRA, 0 AS AmountInputTRA, IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, InventoryAdjustmentDetails.Quantity, 0) AS QuantityInputADJ, " + (isFullJournal || isAmountIncluded ? "IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, InventoryAdjustmentDetails.Amount, 0)" : "0") + " AS AmountInputADJ, 0 AS QuantityOutputINV, 0 AS AmountOutputINV, 0 AS QuantityOutputTRA, 0 AS AmountOutputTRA, 0 AS QuantityOutputADJ, 0 AS AmountOutputADJ, 0 AS QuantityEndTRA, 0 AS QuantityEnd, 0 AS AmountEnd " + "\r\n";
            queryString = queryString + "                           FROM        InventoryAdjustmentDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                                       InventoryAdjustments ON InventoryAdjustmentDetails.EntryDate > @EntryDate AND InventoryAdjustmentDetails.EntryDate <= @LocalToDate AND InventoryAdjustmentDetails.Quantity > 0 " + this.WarehouseJournalWarehouseFilter("InventoryAdjustmentDetails", isWarehouseFilter) + this.WarehouseJournalCommodityFilter("InventoryAdjustmentDetails", isCommodityFilter) + " AND InventoryAdjustmentDetails.InventoryAdjustmentID = InventoryAdjustments.InventoryAdjustmentID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       Customers Suppliers ON InventoryAdjustmentDetails.SupplierID = Suppliers.CustomerID " + "\r\n";

            // --INPUT: IN-TERM OPENNING + INPUT   //END


            // --OUTPUT: IN-TERM OPENNING + OUTPUT //BEGIN
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      IIF(SalesInvoiceDetails.EntryDate >= @LocalFromDate, '   ' + CONVERT(VARCHAR, @LocalFromDate, 103) + ' -> ' + CONVERT(VARCHAR, @LocalToDate, 103), '     DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103)) AS GroupName, IIF(SalesInvoiceDetails.EntryDate >= @LocalFromDate, " + (int)GlobalEnums.NmvnTaskID.SalesInvoice + ", 0) AS NMVNTaskID, IIF(SalesInvoiceDetails.EntryDate >= @LocalFromDate, SalesInvoiceDetails.SalesInvoiceID, 0) AS JournalPrimaryID, IIF(SalesInvoiceDetails.EntryDate >= @LocalFromDate, SalesInvoiceDetails.EntryDate, @LocalFromDate - 1) AS JournalDate, IIF(SalesInvoiceDetails.EntryDate >= @LocalFromDate, SalesInvoices.Reference, '') AS JournalReference, IIF(SalesInvoiceDetails.EntryDate >= @LocalFromDate, Customers.Name + ', Đ/C: ' + Customers.BillingAddress, '') AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       SalesInvoiceDetails.CommodityID, SalesInvoiceDetails.WarehouseID, IIF(SalesInvoiceDetails.EntryDate < @LocalFromDate, -SalesInvoiceDetails.Quantity, 0) AS QuantityBeginREC, 0 AS QuantityBeginTRA, 0 AS AmountBegin, 0 AS QuantityInputINV, 0 AS UnitPriceInputINV, 0 AS AmountInputINV, 0 AS VATAmountInputINV, 0 AS GrossAmountInputINV, 0 AS AmountInputINVByAveragePrice, 0 AS QuantityInputTRA, 0 AS AmountInputTRA, 0 AS QuantityInputADJ, 0 AS AmountInputADJ, IIF(SalesInvoiceDetails.EntryDate >= @LocalFromDate, SalesInvoiceDetails.Quantity, 0) AS QuantityOutputINV, " + (isAmountIncluded ? "IIF(SalesInvoiceDetails.EntryDate >= @LocalFromDate, SalesInvoiceDetails.Quantity * WarehouseBalancePrice.UnitPrice, 0)" : "0") + " AS AmountOutputINV, 0 AS QuantityOutputTRA, 0 AS AmountOutputTRA, 0 AS QuantityOutputADJ, 0 AS AmountOutputADJ, 0 AS QuantityEndTRA, 0 AS QuantityEnd, 0 AS AmountEnd " + "\r\n";
            queryString = queryString + "                           FROM        SalesInvoiceDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                                       SalesInvoices ON SalesInvoiceDetails.EntryDate > @EntryDate AND SalesInvoiceDetails.EntryDate <= @LocalToDate " + this.WarehouseJournalWarehouseFilter("SalesInvoiceDetails", isWarehouseFilter) + this.WarehouseJournalCommodityFilter("SalesInvoiceDetails", isCommodityFilter) + " AND SalesInvoiceDetails.SalesInvoiceID = SalesInvoices.SalesInvoiceID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       Customers ON SalesInvoiceDetails.CustomerID = Customers.CustomerID " + "\r\n";
            if (isAmountIncluded)
                queryString = queryString + "                                   LEFT JOIN WarehouseBalancePrice ON WarehouseBalancePrice.EntryDate = dbo.EOMONTHTIME(@LocalToDate, 9999) AND SalesInvoiceDetails.CommodityID = WarehouseBalancePrice.CommodityID " + "\r\n";


            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      IIF(StockTransferDetails.EntryDate >= @LocalFromDate, '   ' + CONVERT(VARCHAR, @LocalFromDate, 103) + ' -> ' + CONVERT(VARCHAR, @LocalToDate, 103), '     DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103)) AS GroupName, IIF(StockTransferDetails.EntryDate >= @LocalFromDate, " + (int)GlobalEnums.NmvnTaskID.StockTransfer + ", 0) AS NMVNTaskID, IIF(StockTransferDetails.EntryDate >= @LocalFromDate, StockTransferDetails.StockTransferID, 0) AS JournalPrimaryID, IIF(StockTransferDetails.EntryDate >= @LocalFromDate, StockTransferDetails.EntryDate, @LocalFromDate - 1) AS JournalDate, IIF(StockTransferDetails.EntryDate >= @LocalFromDate, StockTransfers.Reference, '') AS JournalReference, IIF(StockTransferDetails.EntryDate >= @LocalFromDate, 'XUAT VCNB: ' + Warehouses.Name, '') AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       StockTransferDetails.CommodityID, StockTransferDetails.WarehouseID, IIF(StockTransferDetails.EntryDate < @LocalFromDate, -StockTransferDetails.Quantity, 0) AS QuantityBeginREC, 0 AS QuantityBeginTRA, 0 AS AmountBegin, 0 AS QuantityInputINV, 0 AS UnitPriceInputINV, 0 AS AmountInputINV, 0 AS VATAmountInputINV, 0 AS GrossAmountInputINV, 0 AS AmountInputINVByAveragePrice, 0 AS QuantityInputTRA, 0 AS AmountInputTRA, 0 AS QuantityInputADJ, 0 AS AmountInputADJ, 0 AS QuantityOutputINV, 0 AS AmountOutputINV, IIF(StockTransferDetails.EntryDate >= @LocalFromDate, StockTransferDetails.Quantity, 0) AS QuantityOutputTRA, " + (isAmountIncluded ? "IIF(StockTransferDetails.EntryDate >= @LocalFromDate, StockTransferDetails.Quantity * WarehouseBalancePrice.UnitPrice, 0)" : "0") + " AS AmountOutputTRA, 0 AS QuantityOutputADJ, 0 AS AmountOutputADJ, 0 AS QuantityEndTRA, 0 AS QuantityEnd, 0 AS AmountEnd " + "\r\n";
            queryString = queryString + "                           FROM        StockTransferDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                                       StockTransfers ON StockTransferDetails.EntryDate > @EntryDate AND StockTransferDetails.EntryDate <= @LocalToDate " + this.WarehouseJournalWarehouseFilter("StockTransferDetails", isWarehouseFilter) + this.WarehouseJournalCommodityFilter("StockTransferDetails", isCommodityFilter) + " AND StockTransferDetails.StockTransferID = StockTransfers.StockTransferID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       Warehouses ON StockTransfers.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            if (isAmountIncluded)
                queryString = queryString + "                                   LEFT JOIN WarehouseBalancePrice ON WarehouseBalancePrice.EntryDate = dbo.EOMONTHTIME(@LocalToDate, 9999) AND StockTransferDetails.CommodityID = WarehouseBalancePrice.CommodityID " + "\r\n";


            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, '   ' + CONVERT(VARCHAR, @LocalFromDate, 103) + ' -> ' + CONVERT(VARCHAR, @LocalToDate, 103), '     DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103)) AS GroupName, IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, " + (int)GlobalEnums.NmvnTaskID.PartAdjustment + ", 0) AS NMVNTaskID, IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, InventoryAdjustmentDetails.InventoryAdjustmentID, 0) AS JournalPrimaryID, IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, InventoryAdjustmentDetails.EntryDate, @LocalFromDate - 1) AS JournalDate, IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, InventoryAdjustments.Reference, '') AS JournalReference, IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, Suppliers.Name + ', Đ/C: ' + Suppliers.BillingAddress, '') AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       InventoryAdjustmentDetails.CommodityID, InventoryAdjustmentDetails.WarehouseID, IIF(InventoryAdjustmentDetails.EntryDate < @LocalFromDate, InventoryAdjustmentDetails.Quantity, 0) AS QuantityBeginREC, 0 AS QuantityBeginTRA, 0 AS AmountBegin, 0 AS QuantityInputINV, 0 AS UnitPriceInputINV, 0 AS AmountInputINV, 0 AS VATAmountInputINV, 0 AS GrossAmountInputINV, 0 AS AmountInputINVByAveragePrice, 0 AS QuantityInputTRA, 0 AS AmountInputTRA, 0 AS QuantityInputADJ, 0 AS AmountInputADJ, 0 AS QuantityOutputINV, 0 AS AmountOutputINV, 0 AS QuantityOutputTRA, 0 AS AmountOutputTRA, IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, -InventoryAdjustmentDetails.Quantity, 0) AS QuantityOutputADJ, " + (isAmountIncluded ? "IIF(InventoryAdjustmentDetails.EntryDate >= @LocalFromDate, -InventoryAdjustmentDetails.Quantity * WarehouseBalancePrice.UnitPrice, 0)" : "0") + " AS AmountOutputADJ, 0 AS QuantityEndTRA, 0 AS QuantityEnd, 0 AS AmountEnd " + "\r\n";
            queryString = queryString + "                           FROM        InventoryAdjustmentDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                                       InventoryAdjustments ON InventoryAdjustmentDetails.EntryDate > @EntryDate AND InventoryAdjustmentDetails.EntryDate <= @LocalToDate AND InventoryAdjustmentDetails.Quantity < 0 " + this.WarehouseJournalWarehouseFilter("InventoryAdjustmentDetails", isWarehouseFilter) + this.WarehouseJournalCommodityFilter("InventoryAdjustmentDetails", isCommodityFilter) + " AND InventoryAdjustmentDetails.InventoryAdjustmentID = InventoryAdjustments.InventoryAdjustmentID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       Customers Suppliers ON InventoryAdjustmentDetails.SupplierID = Suppliers.CustomerID " + "\r\n";
            if (isAmountIncluded)
                queryString = queryString + "                                   LEFT JOIN WarehouseBalancePrice ON WarehouseBalancePrice.EntryDate = dbo.EOMONTHTIME(@LocalToDate, 9999) AND InventoryAdjustmentDetails.CommodityID = WarehouseBalancePrice.CommodityID " + "\r\n";

            // --OUTPUT: IN-TERM OPENNING + OUTPUT //END


            return queryString;
        }

        private string WarehouseJournalWarehouseFilter(bool isWarehouseFilter)
        { return this.WarehouseJournalWarehouseFilter("", isWarehouseFilter); }

        private string WarehouseJournalWarehouseFilter(string tableName, bool isWarehouseFilter)
        {
            return isWarehouseFilter ? " AND " + (tableName != "" ? tableName + "." : "") + "WarehouseID IN (SELECT WarehouseID FROM @WarehouseFilter) " : "";
        }

        private string WarehouseJournalCommodityFilter(bool isCommodityFilter)
        { return this.WarehouseJournalCommodityFilter("", isCommodityFilter); }

        private string WarehouseJournalCommodityFilter(string tableName, bool isCommodityFilter)
        {
            return isCommodityFilter ? " AND " + (tableName != "" ? tableName + "." : "") + "CommodityID IN (SELECT CommodityID FROM @CommodityFilter) " : (tableName == "WarehouseBalanceDetail" ? "" : " AND " + (tableName != "" ? tableName + "." : "") + "CommodityTypeID IN (" + (int)GlobalEnums.CommodityTypeID.Parts + ", " + (int)GlobalEnums.CommodityTypeID.Consumables + ") ");
        }
        #endregion




        private void VehicleJournal()
        {
            string queryString = " @WarehouseID int, @FromDate DateTime, @ToDate DateTime " + "\r\n"; //Filter by @LocalWarehouseID to make this stored procedure run faster, but it may be removed without any effect the algorithm

            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime, @LocalToDate DateTime " + "\r\n";
            queryString = queryString + "       SET         @LocalFromDate = @FromDate      SET @LocalToDate = @ToDate " + "\r\n";

            queryString = queryString + "       IF         (@WarehouseID <= 0 ) " + "\r\n";
            queryString = queryString + "                   " + this.VehicleJournalBUILD(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "                   " + this.VehicleJournalBUILD(false) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("VehicleJournal", queryString);

        }


        private string VehicleJournalBUILD(bool isAllWarehouses)
        {
            string queryString = "" + "\r\n";

            queryString = queryString + "    BEGIN " + "\r\n";

            if (!isAllWarehouses)
            {
                queryString = queryString + "   DECLARE     @LocalWarehouseID int, @LocationID int " + "\r\n";
                queryString = queryString + "   SET         @LocalWarehouseID = @WarehouseID    " + "\r\n";
                queryString = queryString + "   SET         @LocationID = (SELECT LocationID FROM Warehouses WHERE WarehouseID = @LocalWarehouseID) " + "\r\n";
            }

            queryString = queryString + "       SELECT      Commodities.CommodityID, Commodities.Code, Commodities.Name, Commodities.SalesUnit, Commodities.LeadTime, " + "\r\n";
            queryString = queryString + "                   VehicleJournalMaster.GoodsReceiptDetailID, VehicleJournalMaster.EntryDate, VehicleJournalMaster.ChassisCode, VehicleJournalMaster.EngineCode, VehicleJournalMaster.ColorCode, " + "\r\n";
            queryString = queryString + "                   ISNULL(Warehouses.LocationID, 0) AS LocationID, ISNULL(Warehouses.WarehouseCategoryID, 0) AS WarehouseCategoryID, ISNULL(Warehouses.WarehouseID, 0) AS WarehouseID, ISNULL(Warehouses.Name, '') AS WarehouseName, " + "\r\n";
            queryString = queryString + "                   Customers.CustomerTypeID AS SupplierTypeID, Customers.CustomerID AS SupplierID, Customers.OfficialName AS SupplierName, " + "\r\n";

            queryString = queryString + "                   VehicleJournalMaster.QuantityBegin, VehicleJournalMaster.QuantityInputINV, VehicleJournalMaster.QuantityInputRTN, VehicleJournalMaster.QuantityInputTRF, VehicleJournalMaster.QuantityInputADJ, VehicleJournalMaster.QuantityInputINV + VehicleJournalMaster.QuantityInputRTN + VehicleJournalMaster.QuantityInputTRF + VehicleJournalMaster.QuantityInputADJ AS QuantityInput, " + "\r\n";
            queryString = queryString + "                   VehicleJournalMaster.QuantityIssueINV, VehicleJournalMaster.QuantityIssueTRF, VehicleJournalMaster.QuantityIssueADJ, VehicleJournalMaster.QuantityIssueINV + VehicleJournalMaster.QuantityIssueTRF + VehicleJournalMaster.QuantityIssueADJ AS QuantityIssue, " + "\r\n";
            queryString = queryString + "                   VehicleJournalMaster.QuantityBegin + VehicleJournalMaster.QuantityInputINV + VehicleJournalMaster.QuantityInputRTN + VehicleJournalMaster.QuantityInputTRF + VehicleJournalMaster.QuantityInputADJ - VehicleJournalMaster.QuantityIssueINV - VehicleJournalMaster.QuantityIssueTRF - VehicleJournalMaster.QuantityIssueADJ AS QuantityEnd, " + "\r\n";
            queryString = queryString + "                   VehicleJournalMaster.QuantityOnPurchasing, VehicleJournalMaster.QuantityOnReceipt, " + "\r\n";
            queryString = queryString + "                   VehicleJournalMaster.UnitPrice, VehicleJournalMaster.MovementMIN, VehicleJournalMaster.MovementMAX, VehicleJournalMaster.MovementAVG, " + "\r\n";

            queryString = queryString + "                   VWCommodityCategories.CommodityCategoryID, " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories.Name1 AS CommodityCategory1, " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories.Name2 AS CommodityCategory2, " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories.Name3 AS CommodityCategory3 " + "\r\n";

            queryString = queryString + "       FROM       (" + "\r\n";

            //--BEGIN-INPUT-OUTPUT-END.END
            queryString = queryString + "                   SELECT  GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.SupplierID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.WarehouseID, " + "\r\n";
            queryString = queryString + "                           GoodsReceiptDetailUnionMaster.QuantityBegin, GoodsReceiptDetailUnionMaster.QuantityInputINV, GoodsReceiptDetailUnionMaster.QuantityInputRTN, GoodsReceiptDetailUnionMaster.QuantityInputTRF, GoodsReceiptDetailUnionMaster.QuantityInputADJ, GoodsReceiptDetailUnionMaster.QuantityIssueINV, GoodsReceiptDetailUnionMaster.QuantityIssueTRF, GoodsReceiptDetailUnionMaster.QuantityIssueADJ, 0 AS QuantityOnPurchasing, 0 AS QuantityOnReceipt, GoodsReceiptDetails.UnitPrice, GoodsReceiptDetailUnionMaster.MovementMIN, GoodsReceiptDetailUnionMaster.MovementMAX, GoodsReceiptDetailUnionMaster.MovementAVG " + "\r\n";

            // NOTE 24.APR.2007: VIEC TINH GIA TON KHO (GoodsReceiptDetails.AmountCostCUR + GoodsReceiptDetails.AmountClearanceCUR)/ GoodsReceiptDetails.Quantity AS UPriceCURInventory, (GoodsReceiptDetails.AmountCostUSD + GoodsReceiptDetails.AmountClearanceUSD)/ GoodsReceiptDetails.Quantity AS UPriceNMDInventory
            // SU DUNG CONG THUC TREN CHI TAM THOI MA THOI, CO THE DAN DEN SAI SO (SU DUNG TAM THOI DE IN BAO CAO KHO CO SO LIEU)
            // SAU NAY NEN SUA LAI, SU DUNG PHEP +/ - MA THOI
            // XEM SPWHAmountCostofsalesGet DE TINH LUONG REMAIN NHE

            queryString = queryString + "                   FROM   (" + "\r\n";
            queryString = queryString + "                           SELECT  GoodsReceiptDetailUnion.GoodsReceiptDetailID, " + "\r\n";
            queryString = queryString + "                                   SUM(QuantityBegin) AS QuantityBegin, SUM(QuantityInputINV) AS QuantityInputINV, SUM(QuantityInputRTN) AS QuantityInputRTN, SUM(QuantityInputTRF) AS QuantityInputTRF, SUM(QuantityInputADJ) AS QuantityInputADJ, SUM(QuantityIssueINV) AS QuantityIssueINV, SUM(QuantityIssueTRF) AS QuantityIssueTRF, SUM(QuantityIssueADJ) AS QuantityIssueADJ, " + "\r\n";
            queryString = queryString + "                                   MIN(MovementDate) AS MovementMIN, MAX(MovementDate) AS MovementMAX, SUM((QuantityIssueINV + QuantityIssueTRF + QuantityIssueADJ) * MovementDate) / SUM(QuantityIssueINV + QuantityIssueTRF + QuantityIssueADJ) AS MovementAVG " + "\r\n";
            queryString = queryString + "                           FROM    (" + "\r\n";
            //BEGINING
            //WHINPUT
            queryString = queryString + "                                   SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssue, " + (int)GlobalEnums.rndQuantity + ") AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssueINV, 0 AS QuantityIssueTRF, 0 AS QuantityIssueADJ, NULL AS MovementDate " + "\r\n";
            queryString = queryString + "                                   FROM        GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                                   WHERE       GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND GoodsReceiptDetails.WarehouseID = @LocalWarehouseID ") + " AND GoodsReceiptDetails.EntryDate < @LocalFromDate AND GoodsReceiptDetails.Quantity > GoodsReceiptDetails.QuantityIssue " + "\r\n";

            queryString = queryString + "                                   UNION ALL " + "\r\n";
            //UNDO (CAC CAU SQL CHO INVOICE, StockTransferDetails, WHADJUST, WHASSEMBLY LA HOAN TOAN GIONG NHAU. LUU Y T/H DAT BIET: WHADJUST.QUANTITY < 0)
            //UNDO SalesInvoiceDetails
            queryString = queryString + "                                   SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, SalesInvoiceDetails.Quantity AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssueINV, 0 AS QuantityIssueTRF, 0 AS QuantityIssueADJ, NULL AS MovementDate " + "\r\n";
            queryString = queryString + "                                   FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                                               SalesInvoiceDetails ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND GoodsReceiptDetails.WarehouseID = @LocalWarehouseID ") + " AND GoodsReceiptDetails.GoodsReceiptDetailID = SalesInvoiceDetails.GoodsReceiptDetailID AND GoodsReceiptDetails.EntryDate < @LocalFromDate AND SalesInvoiceDetails.EntryDate >= @LocalFromDate " + "\r\n";

            queryString = queryString + "                                   UNION ALL " + "\r\n";
            //UNDO StockTransferDetails
            queryString = queryString + "                                   SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, StockTransferDetails.Quantity AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssueINV, 0 AS QuantityIssueTRF, 0 AS QuantityIssueADJ, NULL AS MovementDate " + "\r\n";
            queryString = queryString + "                                   FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                                               StockTransferDetails ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND GoodsReceiptDetails.WarehouseID = @LocalWarehouseID ") + " AND GoodsReceiptDetails.GoodsReceiptDetailID = StockTransferDetails.GoodsReceiptDetailID AND GoodsReceiptDetails.EntryDate < @LocalFromDate AND StockTransferDetails.EntryDate >= @LocalFromDate " + "\r\n";



            //INTPUT
            queryString = queryString + "                                   UNION ALL " + "\r\n";
            queryString = queryString + "                                   SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, 0 AS QuantityBegin, " + "\r\n";
            queryString = queryString + "                                               CASE WHEN GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " THEN GoodsReceiptDetails.Quantity ELSE 0 END AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                               CASE WHEN GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.GoodsReturn + " THEN GoodsReceiptDetails.Quantity ELSE 0 END AS QuantityInputRTN, " + "\r\n";
            queryString = queryString + "                                               CASE WHEN GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " THEN GoodsReceiptDetails.Quantity ELSE 0 END AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                               CASE WHEN GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.InventoryAdjustment + " THEN GoodsReceiptDetails.Quantity ELSE 0 END AS QuantityInputADJ, " + "\r\n";
            queryString = queryString + "                                               0 AS QuantityIssueINV, 0 AS QuantityIssueTRF, 0 AS QuantityIssueADJ, NULL AS MovementDate " + "\r\n";
            queryString = queryString + "                                   FROM        GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                                   WHERE       GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND GoodsReceiptDetails.WarehouseID = @LocalWarehouseID ") + " AND GoodsReceiptDetails.EntryDate >= @LocalFromDate AND GoodsReceiptDetails.EntryDate <= @LocalToDate " + "\r\n";

            //OUTPUT (CAC CAU SQL CHO INVOICE, StockTransferDetails, WHADJUST, WHASSEMBLY LA HOAN TOAN GIONG NHAU. LUU Y T/H DAT BIET: WHADJUST.QUANTITY < 0)
            queryString = queryString + "                                   UNION ALL " + "\r\n";
            //SalesInvoiceDetails + "\r\n";
            queryString = queryString + "                                   SELECT      SalesInvoiceDetails.GoodsReceiptDetailID, 0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, SalesInvoiceDetails.Quantity AS QuantityIssueINV, 0 AS QuantityIssueTRF, 0 AS QuantityIssueADJ, 0 AS MovementDate " + "\r\n"; //DATEDIFF(DAY, GoodsReceiptDetails.EntryDate, SalesInvoiceDetails.EntryDate) AS MovementDate
            queryString = queryString + "                                   FROM        SalesInvoiceDetails " + "\r\n";
            queryString = queryString + "                                   WHERE       SalesInvoiceDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND SalesInvoiceDetails.WarehouseID = @LocalWarehouseID ") + " AND SalesInvoiceDetails.EntryDate >= @LocalFromDate AND SalesInvoiceDetails.EntryDate <= @LocalToDate " + "\r\n";

            queryString = queryString + "                                   UNION ALL " + "\r\n";
            //StockTransferDetails
            queryString = queryString + "                                   SELECT      StockTransferDetails.GoodsReceiptDetailID, 0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssueINV, StockTransferDetails.Quantity AS QuantityIssueTRF, 0 AS QuantityIssueADJ, 0 AS MovementDate " + "\r\n"; //DATEDIFF(DAY, GoodsReceiptDetails.EntryDate, StockTransferDetails.EntryDate) AS MovementDate
            queryString = queryString + "                                   FROM        StockTransferDetails " + "\r\n";
            queryString = queryString + "                                   WHERE       StockTransferDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND StockTransferDetails.WarehouseID = @LocalWarehouseID ") + " AND StockTransferDetails.EntryDate >= @LocalFromDate AND StockTransferDetails.EntryDate <= @LocalToDate " + "\r\n";

            queryString = queryString + "                                   ) AS GoodsReceiptDetailUnion " + "\r\n";
            queryString = queryString + "                           GROUP BY GoodsReceiptDetailUnion.GoodsReceiptDetailID " + "\r\n";
            queryString = queryString + "                           ) AS GoodsReceiptDetailUnionMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                           GoodsReceiptDetails ON GoodsReceiptDetailUnionMaster.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            //--BEGIN-INPUT-OUTPUT-END.END

            queryString = queryString + "                   UNION ALL " + "\r\n";
            //--ON SHIP.BEGIN
            queryString = queryString + "                   SELECT  CONVERT(smalldatetime, '" + new DateTime(1990, 1, 1).ToString("dd/MM/yyyy") + "', 103) AS EntryDate, 0 AS GoodsReceiptDetailID, PurchaseOrderDetails.CommodityID, PurchaseOrderDetails.SupplierID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, " + "\r\n";
            queryString = queryString + "                           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssueINV, 0 AS QuantityIssueTRF, 0 AS QuantityIssueADJ, (PurchaseOrderDetails.Quantity - PurchaseOrderDetails.QuantityInvoice) AS QuantityOnPurchasing, 0 AS QuantityOnReceipt, 0 AS UnitPrice, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
            queryString = queryString + "                   FROM    PurchaseOrderDetails " + "\r\n";
            queryString = queryString + "                   WHERE   PurchaseOrderDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND PurchaseOrderDetails.LocationID = @LocationID ") + " AND PurchaseOrderDetails.EntryDate <= @LocalToDate AND PurchaseOrderDetails.Quantity > PurchaseOrderDetails.QuantityInvoice " + "\r\n";

            queryString = queryString + "                   UNION ALL " + "\r\n";

            queryString = queryString + "                   SELECT  CONVERT(smalldatetime, '" + new DateTime(1990, 1, 1).ToString("dd/MM/yyyy") + "', 103) AS EntryDate, 0 AS GoodsReceiptDetailID, PurchaseInvoiceDetails.CommodityID, PurchaseOrders.SupplierID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, " + "\r\n";
            queryString = queryString + "                           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssueINV, 0 AS QuantityIssueTRF, 0 AS QuantityIssueADJ, PurchaseInvoiceDetails.Quantity AS QuantityOnPurchasing, 0 AS QuantityOnReceipt, 0 AS UnitPrice, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
            queryString = queryString + "                   FROM    PurchaseOrders INNER JOIN " + "\r\n";
            queryString = queryString + "                           PurchaseInvoiceDetails ON PurchaseInvoiceDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND PurchaseOrders.LocationID = @LocationID ") + " AND PurchaseOrders.PurchaseOrderID = PurchaseInvoiceDetails.PurchaseOrderID " + "\r\n";
            queryString = queryString + "                   WHERE   PurchaseOrders.EntryDate <= @LocalToDate AND PurchaseInvoiceDetails.EntryDate > @LocalToDate  " + "\r\n";
            //--ON SHIP.END

            queryString = queryString + "                   UNION ALL " + "\r\n";
            //--ON INPUT.BEGIN (CAC CAU SQL DUNG CHO EWHInputVoucherTypeID.EInvoice, EWHInputVoucherTypeID.EReturn, EWHInputVoucherTypeID.EWHTransfer, EWHInputVoucherTypeID.EWHAdjust, EWHInputVoucherTypeID.EWHAssemblyMaster, EWHInputVoucherTypeID.EWHAssemblyDetail LA HOAN TOAN GIONG NHAU)
            //EWHInputVoucherTypeID.EInvoice
            queryString = queryString + "                   SELECT  CONVERT(smalldatetime, '" + new DateTime(1990, 1, 1).ToString("dd/MM/yyyy") + "', 103) AS EntryDate, 0 AS GoodsReceiptDetailID, PurchaseInvoiceDetails.CommodityID, PurchaseInvoiceDetails.SupplierID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, " + "\r\n";
            queryString = queryString + "                           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssueINV, 0 AS QuantityIssueTRF, 0 AS QuantityIssueADJ, 0 AS QuantityOnPurchasing, (PurchaseInvoiceDetails.Quantity - PurchaseInvoiceDetails.QuantityReceipt) AS QuantityOnReceipt, 0 AS UnitPrice, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
            queryString = queryString + "                   FROM    PurchaseInvoiceDetails " + "\r\n";
            queryString = queryString + "                   WHERE   PurchaseInvoiceDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND PurchaseInvoiceDetails.LocationID = @LocationID ") + " AND PurchaseInvoiceDetails.EntryDate <= @LocalToDate AND PurchaseInvoiceDetails.Quantity > PurchaseInvoiceDetails.QuantityReceipt " + "\r\n";

            queryString = queryString + "                   UNION ALL " + "\r\n";

            queryString = queryString + "                   SELECT  CONVERT(smalldatetime, '" + new DateTime(1990, 1, 1).ToString("dd/MM/yyyy") + "', 103) AS EntryDate, 0 AS GoodsReceiptDetailID, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.SupplierID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, " + "\r\n";
            queryString = queryString + "                           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssueINV, 0 AS QuantityIssueTRF, 0 AS QuantityIssueADJ, 0 AS QuantityOnPurchasing, GoodsReceiptDetails.Quantity AS QuantityOnReceipt, 0 AS UnitPrice, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
            queryString = queryString + "                   FROM    PurchaseInvoices INNER JOIN " + "\r\n";
            queryString = queryString + "                           GoodsReceiptDetails ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND PurchaseInvoices.LocationID = @LocationID ") + " AND PurchaseInvoices.PurchaseInvoiceID = GoodsReceiptDetails.VoucherID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " AND PurchaseInvoices.EntryDate <= @LocalToDate AND GoodsReceiptDetails.EntryDate > @LocalToDate " + "\r\n";

            queryString = queryString + "                   UNION ALL " + "\r\n";
            //EWHInputVoucherTypeID.EWHTransfer
            queryString = queryString + "                   SELECT  CONVERT(smalldatetime, '" + new DateTime(1990, 1, 1).ToString("dd/MM/yyyy") + "', 103) AS EntryDate, 0 AS GoodsReceiptDetailID, StockTransferDetails.CommodityID, StockTransferDetails.SupplierID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, " + "\r\n";
            queryString = queryString + "                           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssueINV, 0 AS QuantityIssueTRF, 0 AS QuantityIssueADJ, 0 AS QuantityOnPurchasing, (StockTransferDetails.Quantity - StockTransferDetails.QuantityReceipt) AS QuantityOnReceipt, 0 AS UnitPrice, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
            queryString = queryString + "                   FROM    StockTransfers INNER JOIN " + "\r\n";
            queryString = queryString + "                           StockTransferDetails ON StockTransfers.StockTransferID = StockTransferDetails.StockTransferID AND StockTransferDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND StockTransfers.WarehouseID = @LocalWarehouseID ") + " AND StockTransferDetails.EntryDate <= @LocalToDate AND StockTransferDetails.Quantity > StockTransferDetails.QuantityReceipt " + "\r\n";

            queryString = queryString + "                   UNION ALL " + "\r\n";

            queryString = queryString + "                   SELECT  CONVERT(smalldatetime, '" + new DateTime(1990, 1, 1).ToString("dd/MM/yyyy") + "', 103) AS EntryDate, 0 AS GoodsReceiptDetailID, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.SupplierID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, " + "\r\n";
            queryString = queryString + "                           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssueINV, 0 AS QuantityIssueTRF, 0 AS QuantityIssueADJ, 0 AS QuantityOnPurchasing, GoodsReceiptDetails.Quantity AS QuantityOnReceipt, 0 AS UnitPrice, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
            queryString = queryString + "                   FROM    StockTransfers INNER JOIN " + "\r\n";
            queryString = queryString + "                           GoodsReceiptDetails ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND StockTransfers.WarehouseID = @LocalWarehouseID ") + " AND StockTransfers.StockTransferID = GoodsReceiptDetails.VoucherID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " AND StockTransfers.EntryDate <= @LocalToDate AND GoodsReceiptDetails.EntryDate > @LocalToDate " + "\r\n";
            //--ON INPUT.END

            queryString = queryString + "                   ) AS VehicleJournalMaster INNER JOIN " + "\r\n";

            queryString = queryString + "                   Customers ON VehicleJournalMaster.SupplierID = Customers.CustomerID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Commodities ON VehicleJournalMaster.CommodityID = Commodities.CommodityID INNER JOIN " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories ON Commodities.CommodityCategoryID = VWCommodityCategories.CommodityCategoryID LEFT JOIN " + "\r\n";

            queryString = queryString + "                   Warehouses ON VehicleJournalMaster.WarehouseID = Warehouses.WarehouseID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            return queryString;

        }


        private void VehicleCard()
        {
            string queryString = " @WarehouseID int, @FromDate DateTime, @ToDate DateTime " + "\r\n"; //Filter by @LocalWarehouseID to make this stored procedure run faster, but it may be removed without any effect the algorithm

            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalWarehouseID int, @LocalFromDate DateTime, @LocalToDate DateTime " + "\r\n";
            queryString = queryString + "       SET         @LocalWarehouseID = @WarehouseID    SET @LocalFromDate = @FromDate      SET @LocalToDate = @ToDate " + "\r\n";

            queryString = queryString + "       DECLARE     @LocationID int " + "\r\n";
            queryString = queryString + "       SET         @LocationID = (SELECT LocationID FROM Warehouses WHERE WarehouseID = @LocalWarehouseID) " + "\r\n";

            queryString = queryString + "       SELECT      VehicleJournalMaster.GroupName, VehicleJournalMaster.SubGroupName, VehicleJournalMaster.EntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code, Commodities.Name, Commodities.SalesUnit, Commodities.LeadTime, VehicleJournalMaster.ChassisCode, VehicleJournalMaster.EngineCode, VehicleJournalMaster.ColorCode, " + "\r\n";
            queryString = queryString + "                   ISNULL(Warehouses.LocationID, 0) AS LocationID, ISNULL(Warehouses.WarehouseCategoryID, 0) AS WarehouseCategoryID, ISNULL(Warehouses.WarehouseID, 0) AS WarehouseID, ISNULL(Warehouses.Name, '') AS WarehouseName, " + "\r\n";
            queryString = queryString + "                   VehicleJournalMaster.Description, VehicleJournalMaster.QuantityDebit, VehicleJournalMaster.QuantityCredit, " + "\r\n";

            queryString = queryString + "                   VWCommodityCategories.CommodityCategoryID, " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories.Name1 AS CommodityCategory1, " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories.Name2 AS CommodityCategory2, " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories.Name3 AS CommodityCategory3 " + "\r\n";

            queryString = queryString + "       FROM       (" + "\r\n";

            //--BEGIN-INPUT-OUTPUT-END.END
            //1.BEGINING
            //1.1.WHINPUT (MUST USE TWO SEPERATE SQL TO GET THE GoodsReceiptTypeID (VoucherID))
            //1.1.1.WHINPUT.PurchaseInvoice
            queryString = queryString + "                   SELECT     'XE TAI KHO' AS GroupName, 'DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103) AS SubGroupName, GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.WarehouseID, Suppliers.Name + ', HĐ [' + ISNULL(PurchaseInvoices.VATInvoiceNo, '') + ' Ngày: ' + CONVERT(VARCHAR, PurchaseInvoices.EntryDate, 103) + ']' AS Description, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssue, " + (int)GlobalEnums.rndQuantity + ") AS QuantityDebit, 0 AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                               PurchaseInvoices ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND GoodsReceiptDetails.WarehouseID = @LocalWarehouseID AND GoodsReceiptDetails.VoucherID = PurchaseInvoices.PurchaseInvoiceID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " AND GoodsReceiptDetails.Quantity > GoodsReceiptDetails.QuantityIssue AND GoodsReceiptDetails.EntryDate < @LocalFromDate INNER JOIN " + "\r\n";
            queryString = queryString + "                               Customers Suppliers ON PurchaseInvoices.SupplierID = Suppliers.CustomerID " + "\r\n";

            queryString = queryString + "                   UNION ALL " + "\r\n";
            //1.1.2.WHINPUT.StockTransfer
            queryString = queryString + "                   SELECT     'XE TAI KHO' AS GroupName, 'DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103) AS SubGroupName, GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.WarehouseID, 'NHAP VCNB: ' + Locations.Name + ', PX [' + StockTransfers.Reference + ' Ngày: ' + CONVERT(VARCHAR, StockTransfers.EntryDate, 103) + ']' AS Description, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssue, " + (int)GlobalEnums.rndQuantity + ") AS QuantityDebit, 0 AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                               StockTransfers ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND GoodsReceiptDetails.WarehouseID = @LocalWarehouseID AND GoodsReceiptDetails.VoucherID = StockTransfers.StockTransferID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " AND GoodsReceiptDetails.Quantity > GoodsReceiptDetails.QuantityIssue AND GoodsReceiptDetails.EntryDate < @LocalFromDate INNER JOIN " + "\r\n";
            queryString = queryString + "                               Locations ON StockTransfers.LocationID = Locations.LocationID " + "\r\n";

            queryString = queryString + "                   UNION ALL " + "\r\n";
            //1.2.UNDO (CAC CAU SQL CHO INVOICE, StockTransferDetails, WHADJUST, WHASSEMBLY LA HOAN TOAN GIONG NHAU. LUU Y T/H DAT BIET: WHADJUST.QUANTITY < 0)
            //1.2.1.1.UNDO SalesInvoiceDetails (MUST USE TWO SEPERATE SQL TO GET THE GoodsReceiptTypeID (VoucherID)).PurchaseInvoice
            queryString = queryString + "                   SELECT     'XE TAI KHO' AS GroupName, 'DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103) AS SubGroupName, GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.WarehouseID, Suppliers.Name + ', HĐ [' + ISNULL(PurchaseInvoices.VATInvoiceNo, '') + ' Ngày: ' + CONVERT(VARCHAR, PurchaseInvoices.EntryDate, 103) + ']' AS Description, SalesInvoiceDetails.Quantity AS QuantityDebit, 0 AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                               SalesInvoiceDetails ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND GoodsReceiptDetails.WarehouseID = @LocalWarehouseID AND GoodsReceiptDetails.GoodsReceiptDetailID = SalesInvoiceDetails.GoodsReceiptDetailID AND GoodsReceiptDetails.EntryDate < @LocalFromDate AND SalesInvoiceDetails.EntryDate >= @LocalFromDate INNER JOIN " + "\r\n";
            queryString = queryString + "                               PurchaseInvoices ON GoodsReceiptDetails.VoucherID = PurchaseInvoices.PurchaseInvoiceID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " INNER JOIN " + "\r\n";
            queryString = queryString + "                               Customers Suppliers ON PurchaseInvoices.SupplierID = Suppliers.CustomerID " + "\r\n";

            queryString = queryString + "                   UNION ALL " + "\r\n";
            //1.2.1.2.UNDO SalesInvoiceDetails (MUST USE TWO SEPERATE SQL TO GET THE GoodsReceiptTypeID (VoucherID)).StockTransfer
            queryString = queryString + "                   SELECT     'XE TAI KHO' AS GroupName, 'DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103) AS SubGroupName, GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.WarehouseID, 'NHAP VCNB: ' + Locations.Name + ', PX [' + StockTransfers.Reference + ' Ngày: ' + CONVERT(VARCHAR, StockTransfers.EntryDate, 103) + ']' AS Description, SalesInvoiceDetails.Quantity AS QuantityDebit, 0 AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                               SalesInvoiceDetails ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND GoodsReceiptDetails.WarehouseID = @LocalWarehouseID AND GoodsReceiptDetails.GoodsReceiptDetailID = SalesInvoiceDetails.GoodsReceiptDetailID AND GoodsReceiptDetails.EntryDate < @LocalFromDate AND SalesInvoiceDetails.EntryDate >= @LocalFromDate INNER JOIN " + "\r\n";
            queryString = queryString + "                               StockTransfers ON GoodsReceiptDetails.VoucherID = StockTransfers.StockTransferID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " INNER JOIN " + "\r\n";
            queryString = queryString + "                               Locations ON StockTransfers.LocationID = Locations.LocationID " + "\r\n";

            queryString = queryString + "                   UNION ALL " + "\r\n";
            //1.2.2.1.UNDO StockTransferDetails (MUST USE TWO SEPERATE SQL TO GET THE GoodsReceiptTypeID (VoucherID)).PurchaseInvoice
            queryString = queryString + "                   SELECT     'XE TAI KHO' AS GroupName, 'DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103) AS SubGroupName, GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.WarehouseID, Suppliers.Name + ', HĐ [' + ISNULL(PurchaseInvoices.VATInvoiceNo, '') + ' Ngày: ' + CONVERT(VARCHAR, PurchaseInvoices.EntryDate, 103) + ']' AS Description, StockTransferDetails.Quantity AS QuantityDebit, 0 AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                               StockTransferDetails ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND GoodsReceiptDetails.WarehouseID = @LocalWarehouseID AND GoodsReceiptDetails.GoodsReceiptDetailID = StockTransferDetails.GoodsReceiptDetailID AND GoodsReceiptDetails.EntryDate < @LocalFromDate AND StockTransferDetails.EntryDate >= @LocalFromDate INNER JOIN " + "\r\n";
            queryString = queryString + "                               PurchaseInvoices ON GoodsReceiptDetails.VoucherID = PurchaseInvoices.PurchaseInvoiceID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " INNER JOIN " + "\r\n";
            queryString = queryString + "                               Customers Suppliers ON PurchaseInvoices.SupplierID = Suppliers.CustomerID " + "\r\n";

            queryString = queryString + "                   UNION ALL " + "\r\n";
            //1.2.2.2.UNDO StockTransferDetails (MUST USE TWO SEPERATE SQL TO GET THE GoodsReceiptTypeID (VoucherID)).StockTransfer
            queryString = queryString + "                   SELECT     'XE TAI KHO' AS GroupName, 'DAU KY ' + CONVERT(VARCHAR, DATEADD (day, -1,  @LocalFromDate), 103) AS SubGroupName, GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.WarehouseID, 'NHAP VCNB: ' + Locations.Name + ', PX [' + StockTransfers.Reference + ' Ngày: ' + CONVERT(VARCHAR, StockTransfers.EntryDate, 103) + ']' AS Description, StockTransferDetails.Quantity AS QuantityDebit, 0 AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                               StockTransferDetails ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND GoodsReceiptDetails.WarehouseID = @LocalWarehouseID AND GoodsReceiptDetails.GoodsReceiptDetailID = StockTransferDetails.GoodsReceiptDetailID AND GoodsReceiptDetails.EntryDate < @LocalFromDate AND StockTransferDetails.EntryDate >= @LocalFromDate INNER JOIN " + "\r\n";
            queryString = queryString + "                               StockTransfers ON GoodsReceiptDetails.VoucherID = StockTransfers.StockTransferID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " INNER JOIN " + "\r\n";
            queryString = queryString + "                               Locations ON StockTransfers.LocationID = Locations.LocationID " + "\r\n";

            //2.INTPUT (MUST USE TWO SEPERATE SQL TO GET THE GoodsReceiptTypeID (VoucherID))
            //2.1.INTPUT.PurchaseInvoice
            queryString = queryString + "                   UNION ALL " + "\r\n";

            queryString = queryString + "                   SELECT     'XE TAI KHO' AS GroupName, CONVERT(VARCHAR, @LocalFromDate, 103) + ' -> ' + CONVERT(VARCHAR, @LocalToDate, 103) AS SubGroupName, GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.WarehouseID, Suppliers.Name + ', HĐ [' + ISNULL(PurchaseInvoices.VATInvoiceNo, '') + ' Ngày: ' + CONVERT(VARCHAR, PurchaseInvoices.EntryDate, 103) + ']' AS Description, GoodsReceiptDetails.Quantity AS QuantityDebit, 0 AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                               PurchaseInvoices ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND GoodsReceiptDetails.WarehouseID = @LocalWarehouseID AND GoodsReceiptDetails.VoucherID = PurchaseInvoices.PurchaseInvoiceID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " AND GoodsReceiptDetails.EntryDate >= @LocalFromDate AND GoodsReceiptDetails.EntryDate <= @LocalToDate INNER JOIN " + "\r\n";
            queryString = queryString + "                               Customers Suppliers ON PurchaseInvoices.SupplierID = Suppliers.CustomerID " + "\r\n";
            //2.2.INTPUT.StockTransfer
            queryString = queryString + "                   UNION ALL " + "\r\n";

            queryString = queryString + "                   SELECT     'XE TAI KHO' AS GroupName, CONVERT(VARCHAR, @LocalFromDate, 103) + ' -> ' + CONVERT(VARCHAR, @LocalToDate, 103) AS SubGroupName, GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.WarehouseID, 'NHAP VCNB: ' + Locations.Name + ', PX [' + StockTransfers.Reference + ' Ngày: ' + CONVERT(VARCHAR, StockTransfers.EntryDate, 103) + ']' AS Description, GoodsReceiptDetails.Quantity AS QuantityDebit, 0 AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                               StockTransfers ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND GoodsReceiptDetails.WarehouseID = @LocalWarehouseID AND GoodsReceiptDetails.VoucherID = StockTransfers.StockTransferID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " AND GoodsReceiptDetails.EntryDate >= @LocalFromDate AND GoodsReceiptDetails.EntryDate <= @LocalToDate INNER JOIN " + "\r\n";
            queryString = queryString + "                               Locations ON StockTransfers.LocationID = Locations.LocationID " + "\r\n";



            //3.OUTPUT (CAC CAU SQL CHO INVOICE, StockTransferDetails, WHADJUST, WHASSEMBLY LA HOAN TOAN GIONG NHAU. LUU Y T/H DAT BIET: WHADJUST.QUANTITY < 0)
            queryString = queryString + "                   UNION ALL " + "\r\n";
            //3.1.SalesInvoiceDetails + "\r\n";
            queryString = queryString + "                   SELECT     'XE TAI KHO' AS GroupName, CONVERT(VARCHAR, @LocalFromDate, 103) + ' -> ' + CONVERT(VARCHAR, @LocalToDate, 103) AS SubGroupName, SalesInvoiceDetails.EntryDate, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.WarehouseID, Customers.Name + ', Đ/C: ' + Customers.BillingAddress AS Description, 0 AS QuantityDebit, SalesInvoiceDetails.Quantity AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        SalesInvoiceDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                               GoodsReceiptDetails ON SalesInvoiceDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND SalesInvoiceDetails.WarehouseID = @LocalWarehouseID AND SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID AND SalesInvoiceDetails.EntryDate >= @LocalFromDate AND SalesInvoiceDetails.EntryDate <= @LocalToDate INNER JOIN " + "\r\n";
            queryString = queryString + "                               Customers ON SalesInvoiceDetails.CustomerID = Customers.CustomerID " + "\r\n";

            queryString = queryString + "                   UNION ALL " + "\r\n";
            //3.2.StockTransferDetails
            queryString = queryString + "                   SELECT     'XE TAI KHO' AS GroupName, CONVERT(VARCHAR, @LocalFromDate, 103) + ' -> ' + CONVERT(VARCHAR, @LocalToDate, 103) AS SubGroupName, StockTransferDetails.EntryDate, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.WarehouseID, 'XUAT VCNB: ' + Warehouses.Name AS Description, 0 AS QuantityDebit, StockTransferDetails.Quantity AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        StockTransferDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                               GoodsReceiptDetails ON StockTransferDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND StockTransferDetails.WarehouseID = @LocalWarehouseID AND StockTransferDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID AND StockTransferDetails.EntryDate >= @LocalFromDate AND StockTransferDetails.EntryDate <= @LocalToDate INNER JOIN " + "\r\n";
            queryString = queryString + "                               StockTransfers ON StockTransferDetails.StockTransferID = StockTransfers.StockTransferID INNER JOIN " + "\r\n";
            queryString = queryString + "                               Warehouses ON StockTransfers.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            //--BEGIN-INPUT-OUTPUT-END.END



            //B.PENDING
            //B.1.PENDING.ON SHIP
            queryString = queryString + "                   UNION ALL " + "\r\n";
            //--ON SHIP.BEGIN
            queryString = queryString + "                   SELECT     'XE TREN DUONG VE' AS GroupName, 'DA DAT HANG' AS SubGroupName, PurchaseOrderDetails.EntryDate, PurchaseOrderDetails.CommodityID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, Suppliers.Name + ', ĐH [' + PurchaseOrders.ConfirmReference + ' Ngày XN: ' + CONVERT(VARCHAR, PurchaseOrders.ConfirmDate, 103) + ']' AS Description, PurchaseOrderDetails.Quantity - PurchaseOrderDetails.QuantityInvoice AS QuantityDebit, 0 AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        PurchaseOrderDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                               PurchaseOrders ON PurchaseOrderDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND PurchaseOrderDetails.LocationID = @LocationID AND PurchaseOrderDetails.EntryDate <= @LocalToDate AND PurchaseOrderDetails.Quantity > PurchaseOrderDetails.QuantityInvoice AND PurchaseOrderDetails.PurchaseOrderID = PurchaseOrders.PurchaseOrderID INNER JOIN " + "\r\n";
            queryString = queryString + "                               Customers Suppliers ON PurchaseOrderDetails.SupplierID = Suppliers.CustomerID " + "\r\n";

            queryString = queryString + "                   UNION ALL " + "\r\n";

            queryString = queryString + "                   SELECT     'XE TREN DUONG VE' AS GroupName, 'DA DAT HANG' AS SubGroupName, PurchaseOrders.EntryDate, PurchaseInvoiceDetails.CommodityID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, Suppliers.Name + ', ĐH [' + PurchaseOrders.ConfirmReference + ' Ngày XN: ' + CONVERT(VARCHAR, PurchaseOrders.ConfirmDate, 103) + ']' AS Description, PurchaseInvoiceDetails.Quantity AS QuantityDebit, 0 AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        PurchaseOrders INNER JOIN " + "\r\n";
            queryString = queryString + "                               PurchaseInvoiceDetails ON PurchaseInvoiceDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND PurchaseOrders.LocationID = @LocationID AND PurchaseOrders.PurchaseOrderID = PurchaseInvoiceDetails.PurchaseOrderID AND PurchaseOrders.EntryDate <= @LocalToDate AND PurchaseInvoiceDetails.EntryDate > @LocalToDate INNER JOIN " + "\r\n";
            queryString = queryString + "                               Customers Suppliers ON PurchaseOrders.SupplierID = Suppliers.CustomerID " + "\r\n";
            //--ON SHIP.END

            //B.1.PENDING.ON RECEIPT
            queryString = queryString + "                   UNION ALL " + "\r\n";
            //--ON INPUT.BEGIN (CAC CAU SQL DUNG CHO EWHInputVoucherTypeID.EInvoice, EWHInputVoucherTypeID.EReturn, EWHInputVoucherTypeID.EWHTransfer, EWHInputVoucherTypeID.EWHAdjust, EWHInputVoucherTypeID.EWHAssemblyMaster, EWHInputVoucherTypeID.EWHAssemblyDetail LA HOAN TOAN GIONG NHAU)
            //EWHInputVoucherTypeID.EInvoice
            queryString = queryString + "                   SELECT     'XE TREN DUONG VE' AS GroupName, 'CHO NHAP KHO' AS SubGroupName, PurchaseInvoiceDetails.EntryDate, PurchaseInvoiceDetails.CommodityID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, Suppliers.Name + ', HĐ [' + ISNULL(PurchaseInvoices.VATInvoiceNo, '') + ' Ngày: ' + CONVERT(VARCHAR, PurchaseInvoices.EntryDate, 103) + ']' AS Description, PurchaseInvoiceDetails.Quantity - PurchaseInvoiceDetails.QuantityReceipt AS QuantityDebit, 0 AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        PurchaseInvoiceDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                               PurchaseInvoices ON PurchaseInvoiceDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND PurchaseInvoiceDetails.LocationID = @LocationID AND PurchaseInvoiceDetails.EntryDate <= @LocalToDate AND PurchaseInvoiceDetails.Quantity > PurchaseInvoiceDetails.QuantityReceipt AND PurchaseInvoiceDetails.PurchaseInvoiceID = PurchaseInvoices.PurchaseInvoiceID INNER JOIN " + "\r\n";
            queryString = queryString + "                               Customers Suppliers ON PurchaseInvoiceDetails.SupplierID = Suppliers.CustomerID " + "\r\n";
            queryString = queryString + "                               " + "\r\n";

            queryString = queryString + "                   UNION ALL " + "\r\n";

            queryString = queryString + "                   SELECT     'XE TREN DUONG VE' AS GroupName, 'CHO NHAP KHO' AS SubGroupName, PurchaseInvoices.EntryDate, GoodsReceiptDetails.CommodityID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, Suppliers.Name + ', HĐ [' + ISNULL(PurchaseInvoices.VATInvoiceNo, '') + ' Ngày: ' + CONVERT(VARCHAR, PurchaseInvoices.EntryDate, 103) + ']' AS Description, GoodsReceiptDetails.Quantity AS QuantityDebit, 0 AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        PurchaseInvoices INNER JOIN " + "\r\n";
            queryString = queryString + "                               GoodsReceiptDetails ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND PurchaseInvoices.LocationID = @LocationID AND PurchaseInvoices.PurchaseInvoiceID = GoodsReceiptDetails.VoucherID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " AND PurchaseInvoices.EntryDate <= @LocalToDate AND GoodsReceiptDetails.EntryDate > @LocalToDate INNER JOIN " + "\r\n";
            queryString = queryString + "                               Customers Suppliers ON PurchaseInvoices.SupplierID = Suppliers.CustomerID " + "\r\n";

            queryString = queryString + "                   UNION ALL " + "\r\n";
            //EWHInputVoucherTypeID.EWHTransfer
            queryString = queryString + "                   SELECT     'XE TREN DUONG VE' AS GroupName, 'CHO NHAP KHO' AS SubGroupName, StockTransfers.EntryDate, StockTransferDetails.CommodityID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, 'NHAP VCNB: ' + Locations.Name + ', PX [' + StockTransfers.Reference + ' Ngày: ' + CONVERT(VARCHAR, StockTransfers.EntryDate, 103) + ']' AS Description, StockTransferDetails.Quantity - StockTransferDetails.QuantityReceipt AS QuantityDebit, 0 AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        StockTransfers INNER JOIN " + "\r\n";
            queryString = queryString + "                               StockTransferDetails ON StockTransfers.StockTransferID = StockTransferDetails.StockTransferID AND StockTransferDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND StockTransfers.WarehouseID = @LocalWarehouseID AND StockTransferDetails.EntryDate <= @LocalToDate AND StockTransferDetails.Quantity > StockTransferDetails.QuantityReceipt INNER JOIN " + "\r\n";
            queryString = queryString + "                               Locations ON StockTransfers.LocationID = Locations.LocationID " + "\r\n";

            queryString = queryString + "                   UNION ALL " + "\r\n";

            queryString = queryString + "                   SELECT     'XE TREN DUONG VE' AS GroupName, 'CHO NHAP KHO' AS SubGroupName, StockTransfers.EntryDate, GoodsReceiptDetails.CommodityID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, 'NHAP VCNB: ' + Locations.Name + ', PX [' + StockTransfers.Reference + ' Ngày: ' + CONVERT(VARCHAR, StockTransfers.EntryDate, 103) + ']' AS Description, GoodsReceiptDetails.Quantity AS QuantityDebit, 0 AS QuantityCredit " + "\r\n";
            queryString = queryString + "                   FROM        StockTransfers INNER JOIN " + "\r\n";
            queryString = queryString + "                               GoodsReceiptDetails ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + " AND StockTransfers.WarehouseID = @LocalWarehouseID AND StockTransfers.StockTransferID = GoodsReceiptDetails.VoucherID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " AND StockTransfers.EntryDate <= @LocalToDate AND GoodsReceiptDetails.EntryDate > @LocalToDate INNER JOIN " + "\r\n";
            queryString = queryString + "                               Locations ON StockTransfers.LocationID = Locations.LocationID " + "\r\n";
            //--ON INPUT.END

            queryString = queryString + "                   ) AS VehicleJournalMaster INNER JOIN " + "\r\n";

            queryString = queryString + "                   Commodities ON VehicleJournalMaster.CommodityID = Commodities.CommodityID INNER JOIN " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories ON Commodities.CommodityCategoryID = VWCommodityCategories.CommodityCategoryID LEFT JOIN " + "\r\n";

            queryString = queryString + "                   Warehouses ON VehicleJournalMaster.WarehouseID = Warehouses.WarehouseID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("VehicleCard", queryString);

        }



        private void VWCommodityCategories()
        {
            //--------A1.BIGIN
            string queryString = "   SELECT      " + "\r\n";
            queryString = queryString + "               CommodityCategoryID, Name, LimitedMonthWarranty, LimitedKilometreWarranty, ClearancePercent, CustomsPercent, ExcisePercent, VATPercent, " + "\r\n";

            queryString = queryString + "               CommodityCategoryID AS CommodityCategoryID1, Name AS Name1, " + "\r\n";
            queryString = queryString + "               0 AS CommodityCategoryID2, '' AS Name2, " + "\r\n";
            queryString = queryString + "               0 AS CommodityCategoryID3, '' AS Name3 " + "\r\n";

            queryString = queryString + "   FROM        CommodityCategories WHERE AncestorID IS NULL " + "\r\n";

            //--------A1.END
            //--------A2.BEGIN
            queryString = queryString + "   UNION ALL   " + "\r\n";

            queryString = queryString + "   SELECT      " + "\r\n";
            queryString = queryString + "               CommodityCategories2.CommodityCategoryID, CommodityCategories2.Name, CommodityCategories2.LimitedMonthWarranty, CommodityCategories2.LimitedKilometreWarranty, CommodityCategories2.ClearancePercent, CommodityCategories2.CustomsPercent, CommodityCategories2.ExcisePercent, CommodityCategories2.VATPercent, " + "\r\n";

            queryString = queryString + "               CommodityCategories1.CommodityCategoryID AS CommodityCategoryID1, CommodityCategories1.Name AS Name1, " + "\r\n";
            queryString = queryString + "               CommodityCategories2.CommodityCategoryID AS CommodityCategoryID2, CommodityCategories2.Name AS Name2, " + "\r\n";
            queryString = queryString + "               0 AS CommodityCategoryID3, '' AS Name3 " + "\r\n";

            queryString = queryString + "   FROM        " + "\r\n";

            queryString = queryString + "               (SELECT     CommodityCategoryID, Name, LimitedMonthWarranty, LimitedKilometreWarranty, ClearancePercent, CustomsPercent, ExcisePercent, VATPercent, AncestorID FROM CommodityCategories WHERE AncestorID IS NULL) AS CommodityCategories1 " + "\r\n";
            queryString = queryString + "               INNER JOIN " + "\r\n";
            queryString = queryString + "               (SELECT     CommodityCategoryID, Name, LimitedMonthWarranty, LimitedKilometreWarranty, ClearancePercent, CustomsPercent, ExcisePercent, VATPercent, AncestorID FROM CommodityCategories WHERE AncestorID IN (SELECT CommodityCategoryID FROM CommodityCategories WHERE AncestorID IS NULL)) AS CommodityCategories2 " + "\r\n";
            queryString = queryString + "               ON CommodityCategories1.CommodityCategoryID = CommodityCategories2.AncestorID " + "\r\n";
            //--------A2.END

            //--------A3.BEGIN
            queryString = queryString + "   UNION ALL   " + "\r\n";

            queryString = queryString + "   SELECT      " + "\r\n";
            queryString = queryString + "               CommodityCategories3.CommodityCategoryID, CommodityCategories3.Name, CommodityCategories3.LimitedMonthWarranty, CommodityCategories3.LimitedKilometreWarranty, CommodityCategories3.ClearancePercent, CommodityCategories3.CustomsPercent, CommodityCategories3.ExcisePercent, CommodityCategories3.VATPercent, " + "\r\n";

            queryString = queryString + "               CommodityCategories1.CommodityCategoryID AS CommodityCategoryID1, CommodityCategories1.Name AS Name1, " + "\r\n";
            queryString = queryString + "               CommodityCategories2.CommodityCategoryID AS CommodityCategoryID2, CommodityCategories2.Name AS Name2, " + "\r\n";
            queryString = queryString + "               CommodityCategories3.CommodityCategoryID AS CommodityCategoryID3, CommodityCategories3.Name AS Name3 " + "\r\n";

            queryString = queryString + "   FROM        " + "\r\n";

            queryString = queryString + "               (SELECT     CommodityCategoryID, Name, LimitedMonthWarranty, LimitedKilometreWarranty, ClearancePercent, CustomsPercent, ExcisePercent, VATPercent, AncestorID FROM CommodityCategories WHERE AncestorID IS NULL) AS CommodityCategories1 " + "\r\n";

            queryString = queryString + "               INNER JOIN " + "\r\n";
            queryString = queryString + "               (SELECT     CommodityCategoryID, Name, LimitedMonthWarranty, LimitedKilometreWarranty, ClearancePercent, CustomsPercent, ExcisePercent, VATPercent, AncestorID FROM CommodityCategories WHERE AncestorID IN (SELECT CommodityCategoryID FROM CommodityCategories WHERE AncestorID IS NULL)) AS CommodityCategories2 " + "\r\n";
            queryString = queryString + "               ON CommodityCategories1.CommodityCategoryID = CommodityCategories2.AncestorID " + "\r\n";

            queryString = queryString + "               INNER JOIN " + "\r\n";
            queryString = queryString + "               (SELECT     CommodityCategoryID, Name, LimitedMonthWarranty, LimitedKilometreWarranty, ClearancePercent, CustomsPercent, ExcisePercent, VATPercent, AncestorID FROM CommodityCategories WHERE AncestorID IN (SELECT CommodityCategoryID FROM CommodityCategories WHERE AncestorID IN (SELECT CommodityCategoryID FROM CommodityCategories WHERE AncestorID IS NULL))) AS CommodityCategories3 " + "\r\n";
            queryString = queryString + "               ON CommodityCategories2.CommodityCategoryID = CommodityCategories3.AncestorID " + "\r\n";
            //--------A3.END


            this.totalSalesPortalEntities.CreateView("VWCommodityCategories", queryString);

        }



        private void SalesInvoiceJournal()
        {
            string queryString = " @LocationID int, @CommodityTypeID int, @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF          (@CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + ") " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.CommodityTypeID.Vehicles) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Parts + ")  " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.CommodityTypeID.Parts) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Consumables + ")  " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.CommodityTypeID.Consumables) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.CommodityTypeID.Services) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("SalesInvoiceJournal", queryString);
        }


        private string SalesInvoiceJournalBuild(GlobalEnums.CommodityTypeID commodityTypeID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF          (@LocationID = 0) " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuildDetail(commodityTypeID, false) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuildDetail(commodityTypeID, true) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;

        }

        private string SalesInvoiceJournalBuildDetail(GlobalEnums.CommodityTypeID commodityTypeID, bool locationFilter)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SalesInvoiceDetails.EntryDate, Customers.CustomerID, Customers.Name AS CustomerName, Commodities.CommodityID, Commodities.Code, Commodities.Name, SalesInvoiceDetails.CommodityTypeID, SalesInvoiceDetails.WarehouseID, " + "\r\n";
            queryString = queryString + "                   Locations.Code AS LocationCode, VWCommodityCategories.CommodityCategoryID, VWCommodityCategories.Name1 AS CommodityCategory1, VWCommodityCategories.Name2 AS CommodityCategory2, VWCommodityCategories.Name3 AS CommodityCategory3, " + "\r\n";
            queryString = queryString + "                   SalesInvoiceDetails.Quantity, SalesInvoiceDetails.DiscountPercent, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount, " + "\r\n";

            if (commodityTypeID == GlobalEnums.CommodityTypeID.Vehicles)
                queryString = queryString + "               SalesInvoiceDetails.ServiceInvoiceID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.UnitPrice AS CostPrice " + "\r\n";
            else
                if (commodityTypeID == GlobalEnums.CommodityTypeID.Parts || commodityTypeID == GlobalEnums.CommodityTypeID.Consumables)
                    queryString = queryString + "           SalesInvoiceDetails.ServiceInvoiceID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, WarehouseBalancePrice.UnitPrice AS CostPrice " + "\r\n";
                else
                    queryString = queryString + "           SalesInvoiceDetails.SalesInvoiceID AS ServiceInvoiceID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS CostPrice " + "\r\n";

            queryString = queryString + "       FROM        SalesInvoiceDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                   Commodities ON SalesInvoiceDetails.EntryDate >= @FromDate AND SalesInvoiceDetails.EntryDate <= @ToDate AND SalesInvoiceDetails.CommodityTypeID = " + (int)commodityTypeID + (locationFilter ? " AND SalesInvoiceDetails.LocationID = @LocationID" : "") + " AND SalesInvoiceDetails.CommodityID = Commodities.CommodityID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON SalesInvoiceDetails.CustomerID = Customers.CustomerID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Locations ON SalesInvoiceDetails.LocationID = Locations.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories ON Commodities.CommodityCategoryID = VWCommodityCategories.CommodityCategoryID " + "\r\n";

            if (commodityTypeID == GlobalEnums.CommodityTypeID.Vehicles)
                queryString = queryString + "               INNER JOIN GoodsReceiptDetails ON SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
            else
                if (commodityTypeID == GlobalEnums.CommodityTypeID.Parts || commodityTypeID == GlobalEnums.CommodityTypeID.Consumables)
                    queryString = queryString + "           INNER JOIN WarehouseBalancePrice ON SalesInvoiceDetails.CommodityID = WarehouseBalancePrice.CommodityID AND MONTH(SalesInvoiceDetails.EntryDate) = MONTH(WarehouseBalancePrice.EntryDate) AND YEAR(SalesInvoiceDetails.EntryDate) = YEAR(WarehouseBalancePrice.EntryDate) " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }


    }
}
