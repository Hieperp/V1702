using System.Text;
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
            this.UpdateSKUBalance();
            this.SPSKUInventoryJournal();

            //this.VWCommodityCategories();
            //this.UpdateWarehouseBalance();
            //this.GetOverStockItems();
            //this.WarehouseJournal();
            //this.WarehouseJournalTESTSummary();
            //this.VehicleJournal();
            //this.VehicleCard();



            //this.SalesInvoiceJournal(); THAY THE BOI SalesInvoiceJournal MOI!
        }



        private void UpdateSKUBalance()
        {
            //////****************************UPDATE BALANCEDATE        TO         23:59:59
            ////--DECLARE @BalanceDate Datetime
            ////--SET @BalanceDate = (SELECT MIN(SKUBalanceDate) FROM SKUBalanceDetail)
            ////--IF DATEPART ( hour , @BalanceDate ) = 0 AND DATEPART ( minute , @BalanceDate )  = 0 AND DATEPART ( second , @BalanceDate )  = 0 	
            ////--UPDATE SKUBalanceDetail SET SKUBalanceDate = DATEADD( second, 59, DATEADD( minute, 59, DATEADD( hour, 23, SKUBalanceDate)))


            //lAddOrSubtraction: 1 ADD, -1-SUBTRACTION


            string queryString = " DROP PROC SPSKUBalanceUpdate " + "\r\n";
            queryString = queryString + " CREATE PROC SPSKUBalanceUpdate " + "\r\n";

            queryString = queryString + " @lAddOrSubtraction Int, @lSKUInputID Int, @lSKUOutputID Int, @GoodsIssueID Int, @lSKUTransferID Int, @lSKUAdjustID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";
            //INIT DATA TO BE INPUT OR OUTPUT.BEGIN
            queryString = queryString + "       DECLARE @lTableAction TABLE (" + "\r\n";
            queryString = queryString + "           SKUInputID int NOT NULL ," + "\r\n";
            queryString = queryString + "           CommodityID int NOT NULL ," + "\r\n";
            queryString = queryString + "           WarehouseID int NOT NULL ," + "\r\n";
            queryString = queryString + "           SKUInputDate datetime NOT NULL ," + "\r\n";
            queryString = queryString + "           Quantity float NOT NULL ," + "\r\n";
            queryString = queryString + "           AmountCostCUR float NOT NULL ," + "\r\n";
            queryString = queryString + "           AmountCostUSD float NOT NULL ," + "\r\n";
            queryString = queryString + "           AmountCostVND float NOT NULL ," + "\r\n";
            queryString = queryString + "           Remarks nvarchar (100))" + "\r\n";

            queryString = queryString + "       IF @lSKUInputID > 0 " + "\r\n";
            queryString = queryString + "           INSERT      @lTableAction " + "\r\n";
            queryString = queryString + "           SELECT      MIN(SKUInputID), CommodityID, WarehouseID, MIN(SKUInputDate), SUM(CASE @lAddOrSubtraction WHEN 1 THEN Quantity ELSE -Quantity END), SUM(AmountCostCUR), SUM(AmountCostUSD), SUM(AmountCostVND), '' AS Remarks " + "\r\n";
            queryString = queryString + "           FROM        SKUInputDetail " + "\r\n";
            queryString = queryString + "           WHERE       SKUInputID = @lSKUInputID " + "\r\n";
            queryString = queryString + "           GROUP BY    WarehouseID, CommodityID" + "\r\n";

            queryString = queryString + "       IF @lSKUOutputID > 0 " + "\r\n";
            queryString = queryString + "           INSERT @lTableAction " + "\r\n";
            queryString = queryString + "           SELECT MIN(SKUOutputID), CommodityID, WarehouseID, MIN(SKUOutputDate), SUM(CASE @lAddOrSubtraction WHEN 1 THEN Quantity ELSE -Quantity END), 0 AS AmountCostCUR, 0 AS AmountCostUSD, 0 AS AmountCostVND, '' AS Remarks " + "\r\n";
            queryString = queryString + "           FROM SKUOutputDetail " + "\r\n";
            queryString = queryString + "           WHERE SKUOutputID = @lSKUOutputID " + "\r\n";
            queryString = queryString + "           GROUP BY    WarehouseID, CommodityID" + "\r\n";

            queryString = queryString + "       IF @GoodsIssueID > 0 " + "\r\n";
            queryString = queryString + "           INSERT @lTableAction " + "\r\n";
            queryString = queryString + "           SELECT MIN(GoodsIssueID), CommodityID, WarehouseID, MIN(EntryDate), SUM(CASE @lAddOrSubtraction WHEN 1 THEN -(Quantity + FreeQuantity) ELSE (Quantity + FreeQuantity) END), 0 AS AmountCostCUR, 0 AS AmountCostUSD, 0 AS AmountCostVND, '' AS Remarks " + "\r\n"; //IF @lAddOrSubtraction = 1 THEN SAVE ELSE UNDO
            queryString = queryString + "           FROM GoodsIssueDetails " + "\r\n";
            queryString = queryString + "           WHERE GoodsIssueID = @GoodsIssueID " + "\r\n";
            queryString = queryString + "           GROUP BY    WarehouseID, CommodityID" + "\r\n";

            queryString = queryString + "       IF @lSKUTransferID > 0 " + "\r\n";
            queryString = queryString + "           INSERT      @lTableAction " + "\r\n";
            queryString = queryString + "           SELECT      MIN(SKUTransferID), CommodityID, WarehouseID, MIN(SKUTransferDate), SUM(CASE @lAddOrSubtraction WHEN 1 THEN Quantity ELSE -Quantity END), SUM(AmountCUR), SUM(AmountUSD), SUM(AmountVND), '' AS Remarks " + "\r\n";
            queryString = queryString + "           FROM        SKUTransferDetail " + "\r\n";
            queryString = queryString + "           WHERE       SKUTransferID = @lSKUTransferID " + "\r\n";
            queryString = queryString + "           GROUP BY    WarehouseID, CommodityID " + "\r\n";

            queryString = queryString + "       IF @lSKUAdjustID > 0 " + "\r\n";
            queryString = queryString + "           INSERT      @lTableAction " + "\r\n";
            queryString = queryString + "           SELECT      MIN(SKUAdjustID), CommodityID, WarehouseID, MIN(SKUAdjustDate), SUM(CASE @lAddOrSubtraction WHEN 1 THEN -Quantity ELSE Quantity END), SUM(AmountCUR), SUM(AmountUSD), SUM(AmountVND), '' AS Remarks " + "\r\n";
            queryString = queryString + "           FROM        SKUAdjustDetail " + "\r\n";
            queryString = queryString + "           WHERE       SKUAdjustID = @lSKUAdjustID AND Quantity < 0 " + "\r\n"; //OUTPUT ONLY: ADJUST: CO DAT BIET HON CAC T/H KHAC: QUANTITY < 0: NEN: SUM(CASE @lAddOrSubtraction WHEN 1 THEN -Quantity ELSE Quantity END): +/- NGUOC LAI VOI CAC T/H KHAC TI XIU
            queryString = queryString + "           GROUP BY    WarehouseID, CommodityID " + "\r\n";

            //INIT DATA TO BE INPUT OR OUTPUT.END


            queryString = queryString + "       DECLARE         @lSKUActionDate DateTime " + "\r\n";
            queryString = queryString + "       SET             @lSKUActionDate = (SELECT MAX(SKUInputDate) AS SKUInputDate FROM @lTableAction) " + "\r\n";
            queryString = queryString + "       IF              @lSKUActionDate = NULL   RETURN " + "\r\n";//Nothing to update -> Exit immediately





            queryString = queryString + "       DECLARE @lSKUBalanceID Int, @lSKUBalanceDate DateTime" + "\r\n";

            queryString = queryString + "       DECLARE @lSKUBalanceOpening DateTime, @lSKUBalanceEveryWeek DateTime" + "\r\n";
            queryString = queryString + "       SET @lSKUBalanceOpening = CONVERT(Datetime, '2009-05-16 23:59:59', 120)  " + "\r\n";//--SATURDAY: FIRT WEEK


            queryString = queryString + "       DECLARE CursorSKUBalance CURSOR LOCAL FOR SELECT MAX(SKUBalanceID) AS SKUBalanceID, MAX(SKUBalanceDate) AS SKUBalanceDate FROM SKUBalanceDetail" + "\r\n";
            queryString = queryString + "       OPEN CursorSKUBalance" + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorSKUBalance INTO @lSKUBalanceID, @lSKUBalanceDate" + "\r\n";
            queryString = queryString + "       CLOSE CursorSKUBalance DEALLOCATE CursorSKUBalance " + "\r\n";


            queryString = queryString + "       IF @lSKUBalanceID IS NULL SET @lSKUBalanceID = 0" + "\r\n";
            queryString = queryString + "       IF @lSKUBalanceDate IS NULL SET @lSKUBalanceDate = @lSKUBalanceOpening" + "\r\n";

            queryString = queryString + "       SET @lSKUBalanceEveryWeek = @lSKUBalanceDate " + "\r\n"; //--GET THE MAXIMUM OF SKUBalanceDate

            queryString = queryString + "       IF @lSKUActionDate > @lSKUBalanceDate" + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n"; //--COPY THE SAME BALANCE FOR EVERY WEEKEND UP TO THE WEEKEND CONTAIN @lSKUActionDate
            queryString = queryString + "               WHILE DATEADD(Day, 7, @lSKUActionDate) > DATEADD(Day, 7, @lSKUBalanceEveryWeek)" + "\r\n";
            queryString = queryString + "                   BEGIN" + "\r\n";
            queryString = queryString + "                       SET @lSKUBalanceEveryWeek = DATEADD(Day, 7, @lSKUBalanceEveryWeek)" + "\r\n";

            queryString = queryString + "                       SET @lSKUBalanceID = @lSKUBalanceID + 1 " + "\r\n";//--INCREASE PRIMARYID
            queryString = queryString + "                       INSERT INTO SKUBalanceDetail (SKUBalanceID, WarehouseID, CommodityID, SKUBalanceDate, Quantity, QuantityOutput, AmountCUR, AmountUSD, AmountVND, Remarks)" + "\r\n";
            queryString = queryString + "                       SELECT @lSKUBalanceID, WarehouseID, CommodityID, @lSKUBalanceEveryWeek, Quantity, QuantityOutput, AmountCUR, AmountUSD, AmountVND, Remarks FROM SKUBalanceDetail WHERE SKUBalanceDate = @lSKUBalanceDate" + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            queryString = queryString + "               SET @lSKUBalanceDate = @lSKUBalanceEveryWeek " + "\r\n";//--SET THE MAXIMUM OF SKUBalanceDate
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "       WHILE DATEADD(Day, -7, @lSKUBalanceEveryWeek) >= @lSKUActionDate " + "\r\n";//--FIND THE FIRST @lSKUBalanceEveryWeek WHICH IS GREATER THAN @lSKUActionDate
            queryString = queryString + "           SET @lSKUBalanceEveryWeek = DATEADD(Day, -7, @lSKUBalanceEveryWeek)" + "\r\n";


            queryString = queryString + "       UPDATE SKUBalanceDetail" + "\r\n";
            queryString = queryString + "       SET SKUBalanceDetail.Quantity = SKUBalanceDetail.Quantity + TableAction.Quantity" + "\r\n";
            queryString = queryString + "       FROM    @lTableAction TableAction INNER JOIN" + "\r\n";
            queryString = queryString + "               SKUBalanceDetail ON TableAction.CommodityID = SKUBalanceDetail.CommodityID AND TableAction.WarehouseID = SKUBalanceDetail.WarehouseID AND SKUBalanceDetail.SKUBalanceDate >= @lSKUActionDate" + "\r\n";

            queryString = queryString + "       WHILE @lSKUBalanceEveryWeek <= @lSKUBalanceDate" + "\r\n";
            queryString = queryString + "           BEGIN" + "\r\n";
            queryString = queryString + "               SET @lSKUBalanceID = @lSKUBalanceID + 1 " + "\r\n";//--INCREASE PRIMARYID

            queryString = queryString + "               INSERT INTO     SKUBalanceDetail (SKUBalanceID, WarehouseID, CommodityID, SKUBalanceDate, Quantity, QuantityOutput, AmountCUR, AmountUSD, AmountVND, Remarks)" + "\r\n";
            queryString = queryString + "               SELECT          @lSKUBalanceID, TableAction.WarehouseID, TableAction.CommodityID, @lSKUBalanceEveryWeek, TableAction.Quantity, 0 AS QuantityOutput, TableAction.AmountCostCUR, TableAction.AmountCostUSD, TableAction.AmountCostVND, TableAction.Remarks" + "\r\n";
            queryString = queryString + "               FROM            @lTableAction TableAction LEFT JOIN" + "\r\n";
            queryString = queryString + "                               SKUBalanceDetail ON TableAction.CommodityID = SKUBalanceDetail.CommodityID AND TableAction.WarehouseID = SKUBalanceDetail.WarehouseID AND SKUBalanceDetail.SKUBalanceDate = @lSKUBalanceEveryWeek" + "\r\n";
            queryString = queryString + "               WHERE           SKUBalanceDetail.CommodityID IS NULL " + "\r\n";//--ADD NOT-IN-LIST ITEM"

            queryString = queryString + "               SET @lSKUBalanceEveryWeek = DATEADD(Day, 7, @lSKUBalanceEveryWeek)" + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       DELETE FROM SKUBalanceDetail WHERE Quantity = 0 " + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            System.Diagnostics.Debug.WriteLine(queryString);

            //this.totalSalesPortalEntities.CreateStoredProcedure("SPSKUBalanceUpdate", queryString);



        }

        private void GetFNSKUOverStock()
        {

            DateTime pfLockedDate = new DateTime();

            string SQW = " ((@lSKUInputID = 0 AND @lSKUTransferID = 0 AND @lSKUAdjustID = 0 AND @lSKUTransferAdviceID = 0 AND @lSKUOutputID = 0) OR CommodityID IN (SELECT CommodityID FROM @lListItemCommodityAction))" + "\r\n";

            SQW = " 1 = 1 "; //05.04.2011---TAM THOI KHONG XAI DIEU KIEN TREN, NHAM MUC DICH GIUP FUNCTION CHAY NHANH HON RAT NHIEU. SAU NAY KHI DA CODE HOAN CHINH RANGE FOR COMMODITIES DUA VAO @lSKUInputID, @lSKUTransferID, @lSKUAdjustID, @lSKUTransferAdviceID, @lSKUOutputID THI NEN SU DUNG LAI CAU LENH TREN

            string queryString = " (@lSKUActionDate DateTime, @lSKUInputID Int, @lSKUTransferID Int, @lSKUAdjustID Int, @lSKUTransferAdviceID Int, @lSKUOutputID Int) " + "\r\n";
            queryString = queryString + " RETURNS @lTableOverStock TABLE (DateOverStock DateTime NOT NULL, WarehouseID int NOT NULL, WarehouseName nvarchar(100) NOT NULL, CommodityID int NOT NULL, Description nvarchar(50) NOT NULL, DescriptionOfficial nvarchar(200) NOT NULL, Quantity float NOT NULL) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            // --FILTER: ListItemCommodity NEED TO BE CHECK ONLY

            //05.04.2011---: KHONG XAI DIEU KIEN NAY: queryString = queryString + "       DECLARE @lListItemCommodityAction TABLE (CommodityID int NOT NULL) " + "\r\n"; //05.04.2011---: KHONG XAI DIEU KIEN NAY: DO XET TAT CA SP, NEN KHONG CAN XAI @lListItemCommodityAction

            //**************TAM THOI SU DUNG KHAI BAO NHU THE NAY 2 CHO: DANH SACH MAT HANG CAN KTRA VA THOI DIEM CAN KIEM TRA

            //TAM THOI: KTRA TAT CA.BEGIN

            //05.04.2011---: KHONG XAI DIEU KIEN NAY: queryString = queryString + "       INSERT @lListItemCommodityAction SELECT CommodityID FROM ListItemCommodity " + "\r\n";

            //            TAM THOI: KTRA TAT CA
            //            LOI O DAY LA: KHI EDIT (CHU KG PHAI KHI DELETE)
            //            BOI VI: KHI DELETE: KTRA NHUNG MAT HANG CO TRONG DANH SACH,
            //                    TRONG KHI DO: KHI EDIT: CHI KTRA NHUNG MAT HANG SAU KHI EDIT, TRONG KHI NHUNG MAT HANG BI BO DI KHONG CO TRONG DANH SACH SAU KHI EDIT KHONG DUOC KIEM TRA
            //                                  PHAI HIEU RANG: NHUNG MAT HANG BI BO DI SAU KHI EDIT: HOAN TOAN TUONG DUONG BI DELETE: KHONG KTRA NEN SAI HOAN TOAN
            //                                  TUM LAI: PHAI CODE THE NAO DE DAM BAO (HIEN TAI CHUA DAM BAO): PHAI UNION CA 2 DANH SACH: DANH SACH TRUOC KHI EDIT VA DANH SACH SAU KHI EDIT
            //            DO DO: TAM THOI: KTRA TAT CA, SAU NAY SUY NGHI THEM


            //'            queryString = queryString + "       IF @lSKUInputID <> 0 "
            //'            queryString = queryString + "           INSERT @lListItemCommodityAction SELECT CommodityID FROM SKUInputDetail WHERE SKUInputID = @lSKUInputID "
            //'            queryString = queryString + "       IF @lSKUTransferID <> 0 "
            //'            queryString = queryString + "           INSERT @lListItemCommodityAction SELECT CommodityID FROM SKUTransferDetail WHERE SKUTransferID = @lSKUTransferID "
            //'            queryString = queryString + "       IF @lSKUAdjustID <> 0 "
            //'            queryString = queryString + "           INSERT @lListItemCommodityAction SELECT CommodityID FROM SKUAdjustDetail WHERE SKUAdjustID = @lSKUAdjustID "
            //'            queryString = queryString + "       IF @lSKUTransferAdviceID <> 0 "
            //'            queryString = queryString + "           INSERT @lListItemCommodityAction SELECT CommodityID FROM SKUTransferAdviceDetail WHERE SKUTransferAdviceID = @lSKUTransferAdviceID "
            //'            queryString = queryString + "       IF @lSKUOutputID <> 0 "
            //'            queryString = queryString + "           INSERT @lListItemCommodityAction SELECT CommodityID FROM SKUOutputDetail WHERE SKUOutputID = @lSKUOutputID "
            //TAM THOI: KTRA TAT CA.END



            queryString = queryString + "       DECLARE @lDateTemp DateTime " + "\r\n";
            queryString = queryString + "       DECLARE @lDateBackLogMax DateTime " + "\r\n";
            // --GET THE FIRST DATE NEED TO CHECK OVER STOCK.END
            // --THE FIRST DATE IS: THE @lSKUActionDate OR THE DATE OF BACKLOG COLLECTION
            // --OPEN THE BACKLOG COLLECTION TO GET THE MIN(DATE)
            // --LATER: IF THERE IS MORE BACKLOG, LIKE DELIVERYADVICE: ADD HERE

            queryString = queryString + "       DECLARE CursorBacklog CURSOR LOCAL FOR SELECT MIN(DeliveryDate) AS DeliveryDateMIN, MAX(DeliveryDate) AS DeliveryDateMAX FROM SKUTransferAdviceDetail WHERE (ROUND(Quantity - QuantityInput, " + (int)GlobalEnums.rndQuantity + ") > 0)" + "\r\n";
            queryString = queryString + "       OPEN CursorBacklog" + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorBacklog INTO @lDateTemp, @lDateBackLogMax " + "\r\n";
            queryString = queryString + "       CLOSE CursorBacklog DEALLOCATE CursorBacklog" + "\r\n";

            queryString = queryString + "       IF @lSKUActionDate > @lDateTemp SET @lSKUActionDate = @lDateTemp" + "\r\n";


            //**************TAM THOI SU DUNG KHAI BAO NHU THE NAY 2 CHO: DANH SACH MAT HANG CAN KTRA VA THOI DIEM CAN KIEM TRA

            //TAM THOI: KTRA TU 31/05/2009: LY DO TUONG TU NHU DANH SACH MAT HANG CAN KTRA
            //BOI VI: KHI DELETE: KTRA TU NGAY SKUActionDate: NHU VAY LA OK
            //        NHUNG KHI EDIT:  SUA NGAY (NGAY SAU EDIT > NGAY TRUOC DO)
            //                         THEO CACH HIEN TAI: CHI KTRA KE TU NGAY SAU EDIT MA THOI
            //                         NHU VAY: SAI HOAN TOAN: VI DANG LY RA PHAI KTRA KE TU NGAY TRUOC KHI EDIT (NGAY CU)
            //                         TUM LAI: PHAI CODE THE NAO DE DAM BAO (HIEN TAI CHUA DAM BAO): SO SANH NGAY TRUOC VA NGAY SAU EDIT: NGAY NAO < HON THI BAT DAU KIEM TRA KE TU NGAY DO
            //
            //         LUU Y THEM: NEU CHI XET DUA TREN BACKLOG LA KHONG THOA MAN (VAN BI LOI)
            //                     BOI VI: CO THE NHUNG MAT HANG TRONG BACKLOG KTRA VAN DU
            //                     NHUNG TON KHO < 0 TAI NHUNG THOI DIEM NAO DO TRUOC BACKLOG (<0 TRONG TABLE SKUBalanceDetail): PURE OVER STOCK (CHUA XUAT CUNG DA <0 ROI)
            //                     DIEU NAY TUONG TU DOI VOI DANH SACH MAT HANG CAN KTRA O PHAN TREN
            //TAM THOI: KTRA TU 31/05/2009

            queryString = queryString + "       SET @lSKUActionDate = CONVERT(smalldatetime, '" + pfLockedDate.ToString("dd/MM/yyyy") + "',103) " + "\r\n";



            // --GET THE FIRST DATE NEED TO CHECK OVER STOCK.END


            // --GET THE BEGIN BALANCE IF AVAILABLE.BEGIN
            queryString = queryString + "       DECLARE @lTableBalance TABLE (WarehouseID int NOT NULL, CommodityID int NOT NULL, Quantity float NOT NULL)" + "\r\n";

            queryString = queryString + "       DECLARE @lSKUBalanceDateBEGIN DateTime" + "\r\n";
            queryString = queryString + "       DECLARE CursorSKUBalance CURSOR LOCAL FOR SELECT MAX(SKUBalanceDate) AS SKUBalanceDate FROM SKUBalanceDetail WHERE SKUBalanceDate <= @lSKUActionDate" + "\r\n";
            queryString = queryString + "       OPEN CursorSKUBalance" + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorSKUBalance INTO @lSKUBalanceDateBEGIN" + "\r\n";
            queryString = queryString + "       CLOSE CursorSKUBalance DEALLOCATE CursorSKUBalance" + "\r\n";


            queryString = queryString + "       IF NOT @lSKUBalanceDateBEGIN IS NULL" + "\r\n";
            queryString = queryString + "           INSERT  @lTableBalance SELECT WarehouseID, CommodityID, Quantity FROM SKUBalanceDetail WHERE SKUBalanceDate = @lSKUBalanceDateBEGIN AND " + SQW + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           SET @lSKUBalanceDateBEGIN = CONVERT(smalldatetime, '" + new DateTime(1990, 1, 1).ToString("dd/MM/yyyy") + "',103) " + "\r\n";
            // --GET THE BEGIN BALANCE IF AVAILABLE.END

            // --GET THE DATE RANGE NEED TO BE CHECKED.BEGIN
            queryString = queryString + "       DECLARE @lSKUActionDateEND DateTime" + "\r\n";
            queryString = queryString + "       DECLARE CursorSKUBalance CURSOR LOCAL FOR SELECT MAX(SKUBalanceDate) AS SKUBalanceDate FROM SKUBalanceDetail " + "\r\n";
            queryString = queryString + "       OPEN CursorSKUBalance" + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorSKUBalance INTO @lSKUActionDateEND" + "\r\n";
            queryString = queryString + "       CLOSE CursorSKUBalance DEALLOCATE CursorSKUBalance" + "\r\n";

            queryString = queryString + "       IF @lSKUActionDateEND IS NULL OR @lSKUActionDateEND < @lSKUActionDate SET @lSKUActionDateEND = @lSKUActionDate " + "\r\n";  //--CHECK UNTIL THE LAST BALANCE
            queryString = queryString + "       IF @lSKUActionDateEND < @lDateBackLogMax SET @lSKUActionDateEND = @lDateBackLogMax " + "\r\n"; //--OR CHECK UNTIL THE LAST DATE OF BACKLOG

            // --GET THE DATE RANGE NEED TO BE CHECKED.END

            queryString = queryString + "       SET @lDateTemp = @lSKUActionDate " + "\r\n";
            queryString = queryString + "       WHILE @lDateTemp <= @lSKUActionDateEND" + "\r\n";
            queryString = queryString + "           BEGIN" + "\r\n";

            // --BALANCE AT: @lSKUBalanceDateBEGIN: LOOK ON SKUBalanceDetail ONLY
            // --BALANCE AT: @lDateTemp > @lSKUBalanceDateBEGIN: SKUBalanceDetail + SUM(INPUT) - SUM(Output)
            queryString = queryString + "               INSERT INTO @lTableOverStock" + "\r\n";
            queryString = queryString + "               SELECT      @lDateTemp, WarehouseID, N'', CommodityID, N'', N'', ROUND(SUM(Quantity), " + (int)GlobalEnums.rndQuantity + ") AS Quantity" + "\r\n";
            queryString = queryString + "               FROM        (" + "\r\n";
            // --OPENNING
            queryString = queryString + "                           SELECT      SKUBalanceDetail.WarehouseID, SKUBalanceDetail.CommodityID, SKUBalanceDetail.Quantity" + "\r\n";
            queryString = queryString + "                           FROM        @lTableBalance SKUBalanceDetail" + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";
            // --INPUT
            queryString = queryString + "                           SELECT      WarehouseID, CommodityID, SUM(Quantity) AS Quantity" + "\r\n";
            queryString = queryString + "                           FROM        SKUInputDetail " + "\r\n";
            queryString = queryString + "                           WHERE       SKUInputDate > @lSKUBalanceDateBEGIN AND SKUInputDate <= @lDateTemp AND " + SQW + "\r\n";
            queryString = queryString + "                           GROUP BY    WarehouseID, CommodityID" + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";
            // --OUTPUT: TRANSFER
            queryString = queryString + "                           SELECT      WarehouseID, CommodityID, SUM(-Quantity) AS Quantity" + "\r\n";
            queryString = queryString + "                           FROM        SKUTransferDetail " + "\r\n";
            queryString = queryString + "                           WHERE       SKUTransferDate > @lSKUBalanceDateBEGIN AND SKUTransferDate <= @lDateTemp AND " + SQW + "\r\n";
            queryString = queryString + "                           GROUP BY    WarehouseID, CommodityID" + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";
            // --OUTPUT: Adjust
            queryString = queryString + "                           SELECT      WarehouseID, CommodityID, SUM(Quantity) AS Quantity" + "\r\n"; //LUU Y: Quantity < 0: XUAT KHO
            queryString = queryString + "                           FROM        SKUAdjustDetail " + "\r\n";
            queryString = queryString + "                           WHERE       SKUAdjustDate > @lSKUBalanceDateBEGIN AND SKUAdjustDate <= @lDateTemp AND Quantity < 0 AND " + SQW + "\r\n";
            queryString = queryString + "                           GROUP BY    WarehouseID, CommodityID" + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";
            // --OUTPUT: Output
            queryString = queryString + "                           SELECT      WarehouseID, CommodityID, SUM(-Quantity) AS Quantity" + "\r\n";
            queryString = queryString + "                           FROM        SKUOutputDetail " + "\r\n";
            queryString = queryString + "                           WHERE       SKUOutputDate > @lSKUBalanceDateBEGIN AND SKUOutputDate <= @lDateTemp AND " + SQW + "\r\n";
            queryString = queryString + "                           GROUP BY    WarehouseID, CommodityID" + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";

            // --BACKLOG: TRANSFERADVICE
            queryString = queryString + "                           SELECT      WarehouseID, CommodityID, SUM(-Quantity + QuantityInput) AS Quantity" + "\r\n";
            queryString = queryString + "                           FROM        SKUTransferAdviceDetail " + "\r\n";
            queryString = queryString + "                           WHERE       DeliveryDate > @lSKUBalanceDateBEGIN AND DeliveryDate <= @lDateTemp AND " + SQW + " AND InActive = 0 AND ROUND(Quantity - QuantityInput, " + (int)GlobalEnums.rndQuantity + ") > 0" + "\r\n";
            queryString = queryString + "                           GROUP BY    WarehouseID, CommodityID" + "\r\n";

            queryString = queryString + "                           )TableOverStock" + "\r\n";
            queryString = queryString + "               GROUP BY    WarehouseID, CommodityID " + "\r\n";
            queryString = queryString + "               HAVING      ROUND(SUM(Quantity), " + (int)GlobalEnums.rndQuantity + ") < 0 " + "\r\n";

            queryString = queryString + "               DECLARE @lCOUNTOverStock Int SET @lCOUNTOverStock = 0" + "\r\n";
            queryString = queryString + "               DECLARE CursorOverStock CURSOR LOCAL FOR SELECT COUNT(*) AS COUNTOverStock FROM @lTableOverStock" + "\r\n";
            queryString = queryString + "               OPEN CursorOverStock" + "\r\n";
            queryString = queryString + "               FETCH NEXT FROM CursorOverStock INTO @lCOUNTOverStock" + "\r\n";
            queryString = queryString + "               CLOSE CursorOverStock DEALLOCATE CursorOverStock" + "\r\n";

            queryString = queryString + "               IF @lCOUNTOverStock > 0 " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   UPDATE TableOverStock SET TableOverStock.Description = ListItemCommodity.Description, TableOverStock.DescriptionOfficial = ListItemCommodity.DescriptionOfficial FROM @lTableOverStock TableOverStock INNER JOIN ListItemCommodity ON TableOverStock.CommodityID = ListItemCommodity.CommodityID " + "\r\n";
            queryString = queryString + "                   UPDATE TableOverStock SET TableOverStock.WarehouseName = ListWarehouseName.Description FROM @lTableOverStock TableOverStock INNER JOIN ListWarehouseName ON TableOverStock.WarehouseID = ListWarehouseName.WarehouseID " + "\r\n";
            queryString = queryString + "                   BREAK" + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "               SET @lDateTemp = DATEADD(Day, 1, @lDateTemp)" + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       RETURN " + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            //If Not pfFN_CREATEFN(DFormConn, "FNSKUOverStock", SQL) Then GoTo ERR_HANDLER

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

        public string GET_WarehouseJournal_BUILD_SQL(string commoditiesBalanceTable, string fromDate, string toDate, string warehouseIDList, string commodityIDList, string isFullJournal, string isAmountIncluded)
        {
            string queryString = "                  DECLARE " + commoditiesBalanceTable + " TABLE (EntryDate datetime NULL, WarehouseID int NULL, CommodityID int NULL, QuantityBalance decimal(18, 2) NULL) " + "\r\n";

            if (GlobalEnums.ERPConnected)
            {
                queryString = queryString + "       DECLARE @SPSKUInventoryJournalTable TABLE " + "\r\n";
                queryString = queryString + "      (NMVNTaskID int NULL, JournalPrimaryID int NULL, JournalDate datetime NULL, JournalReference nvarchar(30) NULL, JournalDescription nvarchar(202) NULL, " + "\r\n";
                queryString = queryString + "       CommodityID int NULL, Description nvarchar(50) NULL, DescriptionOfficial nvarchar(200) NULL, DescriptionPartA nvarchar(20) NULL, DescriptionPartB nvarchar(20) NULL, DescriptionPartC nvarchar(20) NULL, DescriptionPartD nvarchar(20) NULL, UnitSales nvarchar(50) NULL, Weight float NULL, LeadTime float NULL, SellLife int NULL, " + "\r\n";
                queryString = queryString + "       WarehouseGroupID int NULL, WHLocationID int NULL, WHCategoryID int NULL, WarehouseClassID int NULL, WarehouseID int NULL, WarehouseName nvarchar(60) NULL, WarehouseOutID int NULL, WarehouseOutName nvarchar(60) NULL, " + "\r\n";
                queryString = queryString + "       QuantityBegin float NULL, QuantityInputPRO float NULL, QuantityInputINV float NULL, QuantityInputRTN float NULL, QuantityInputTRF float NULL, QuantityInputADJ float NULL, QuantityInputBLD float NULL, QuantityInputUBL float NULL, QuantityInput float NULL," + "\r\n";
                queryString = queryString + "       QuantityOutputINV float NULL, QuantityOutputGoodsIssue float NULL, QuantityOutputTRF float NULL, QuantityOutputADJ float NULL, QuantityOutputBLD float NULL, QuantityOutputUBL float NULL, QuantityOutput float NULL, QuantityOnTransfer float NULL, QuantityOnAdvice float NULL, QuantityOnTransferAdviceOut float NULL, QuantityOnTransferAdviceIn float NULL, QuantityOnProduction float NULL, UPriceNMDInventory float NULL," + "\r\n";
                queryString = queryString + "       ItemCategoryID int NULL, Description1 nvarchar(100) NULL, Description2 nvarchar(100) NULL, Description3 nvarchar(100) NULL, Description4 nvarchar(100) NULL, Description5 nvarchar(100) NULL, Description6 nvarchar(100) NULL, Description7 nvarchar(100) NULL, Description8 nvarchar(100) NULL, Description9 nvarchar(100) NULL, MaxTransferOutputDate  datetime NULL) " + "\r\n";

                queryString = queryString + "       INSERT INTO @SPSKUInventoryJournalTable EXEC ERmgrVCP.dbo.SPSKUInventoryJournal " + fromDate + ", " + toDate + ", " + commodityIDList + ", N'', N'', N'', N'', " + warehouseIDList + "\r\n";

                queryString = queryString + "       INSERT INTO " + commoditiesBalanceTable + " SELECT " + toDate + ", WarehouseID, CommodityID, SUM(QuantityBegin + QuantityInput - QuantityOutput - QuantityOnAdvice) AS QuantityBalance FROM @SPSKUInventoryJournalTable GROUP BY WarehouseID, CommodityID " + "\r\n";
            }
            else //NO USE WAREHOUSE INVENTORY: THIS CODE IS FOR USE WHEN THERE IS NO WAREHOUSE BALANCE
            {
                queryString = queryString + "       DECLARE @My01JAN2017Commodities TABLE (CommodityID int NOT NULL) INSERT INTO @My01JAN2017Commodities SELECT Id FROM dbo.SplitToIntList (" + commodityIDList + ") " + "\r\n";
                queryString = queryString + "       INSERT INTO " + commoditiesBalanceTable + " SELECT " + toDate + ", Warehouses.WarehouseID, Commodities.CommodityID, 99999 AS QuantityBalance FROM @My01JAN2017Commodities Commodities CROSS JOIN Warehouses WHERE Warehouses.WarehouseID IN (SELECT WarehouseID FROM CustomerWarehouses WHERE CustomerID = @CustomerID AND InActive = 0) " + "\r\n";
            }

            return queryString;
        }

        private enum EWarehouseClassID
        {
            EWarehouseClassL1 = 1,
            EWarehouseClassL2 = 2,
            EWarehouseClassL3 = 3,
            EWarehouseClassL4 = 4,
            EWarehouseClassL5 = 5,
            EWarehouseClassLD = 9
        };

        private enum ENMVNTaskID
        {
            ENull = 0,

            ENONE99 = -99,

            EACCMonthEndTransfer = 900001,

            EACCSpecialDebit = 101001,
            EACCSpecialCredit = 101005,

            ESInvoice = 3,
            ESReceipt = 5,
            EsReturn = 40,
            ESCredit = 41,

            ESQuotation = 1,
            ESDeliveryAdvice = 2,
            ESDeliveryAdviceApproved = 77772,
            ESSalesOrder = 199,
            ESSalesOrderApproved = 77779,

            ESContract = 4,

            ESRequest = 6,
            ESVisit = 107,

            EAddressArea = 22,
            EAddressCountry = 30,

            ECustomerCategory = 25,
            ECustomerName = 26,
            ECustomerNameNEW = 2600,

            ECustomerEmployee = 31,
            ESPurchase = 31,

            EFinancialFund = 106,
            ETermPayment = 27,
            ECustomsName = 157,

            EPayrollProductionSemi = 1070001,
            EPayrollProductionPack = 1070002,

            EMouldName = 81,
            EWorkerName = 83,
            EProductionLine = 84,

            EProductionWorkHour = 8150815,
            EProductionWorkHourApproved = 815081511,

            EProductionWorkFail = 8150816,
            EProductionWorkFailApproved = 815081611,

            EProductionCompen = 8150817,
            EProductionCompenApproved = 815081711,

            EProductionMelamin = 8150818,
            EProductionMelaminApproved = 815081811,
            EProductionMelaminRevise = 815081812,

            EProductionWeight = 8150819,
            EProductionWeightApproved = 815081911,

            EMaterialCategory = 85,
            EMaterialName = 86,
            EMaterialMeta = 87,

            EItemCommodity = 28,
            EItemCategory = 32,
            EItemType = 33,
            EItemModel = 34,
            EItemPriceCalculation = 150,
            EDItemAssembly = 36,
            EExpenseName = 55,
            ESVEquipmentError = 56,
            EInvest = 57,

            EDAssembly = 99,

            ESRequire = 37,
            ESPContract = 38,
            ESPInvoice = 39,

            ESPConfirm = 330,

            ESShipment = 39,
            ESShipmentCustomsTax = 3933332,
            ESShipmentVATTax = 3933312,

            ESPurchaseExpense = 340,
            ESPayment = 350,

            ESupplierName = 35,
            EWarehouse = 42,

            EWHAdjustType = 17,

            EAccountMaster = 49,

            EAJournalEntry = 2150,
            ELendingContract = 3150,
            EFixedAsset = 106000,


            // SERVICE
            ESSVContract = 711,
            ESSVService = 712,
            ESSVQuotation = 713,
            ESSVCos = 714,
            ESSVMeterBill = 715,
            ESSVDeliveryAdvice = 716,

            // INVENTORY
            ESWHTransferAdvice = 605,
            ESWHTransferAdviceApproved = 77773,
            ESWHInput = 610,
            ESWHOutput = 611,
            ESWHTransfer = 612,
            ESWHAdjust = 613,


            ESKUInput = 661,
            ESKUTransferAdvice = 660,
            ESKUTransferAdviceApproved = 77775,
            ESKUTransfer = 662,
            ESKUAdjust = 663,
            ESKUAdjustApproved = 6630001,
            ESKUOutput = 665,
            ESKUOutputApproved = 6650001,



            EWHAssemblyMaster = 0,
            EWHAssemblyDetail = 0,

            // PRODUCTION
            EProductionOrder = 810,
            EProductionOrderApproved = 777810,
            EProductionPlan = 811,
            EProductionPlanApproved = 8111111,
            EProductionCommand = 812,
            EProductionCommandApproved = 813,
            EProductionCommandUnApproved = 8131,

            EProductionSemi = 815,
            EProductionSemiApproved = 818,
            EProductionSemiRevise = 818111,

            EProductionImprove = 819,
            EProductionImproveApproved = 820111,
            EProductionImproveUnApproved = 820,

            EProductionPack = 816,
            EProductionPackRevise = 816111,
            EProductionRank = 825,
            EProductionRankApproved = 825111,

            EProductionDelimit = 837,
            EProductionDelimitApproved = 837111,

            EProductionFinish = 817,


            EProductionRefine = 821,
            EProductionRefineApproved = 822111,
            EProductionRefineUnApproved = 822,

            EProductionWorker = 827,
            EProductionWorkerApproved = 827111,

            EProductionMould = 828,
            EProductionMouldApproved = 828111,

            EMaterialInput = 1810,
            EMaterialInputApproved = 7771810,
            EMaterialOutput = 1820,
            EMaterialOutputApproved = 7771820,

            EMaterialTransfer = 1830,
            EMaterialTransferApproved = 7771830,

            EMaterialReduction = 1835,
            EMaterialReductionApproved = 7771835,

            EMaterialAdjust = 1850,
            EMaterialAdjustApproved = 7771850,

            EMaterialMerge = 1853,
            EMaterialMergeApproved = 7771853,

            EMaterialMergeOutput = 1855,
            EMaterialMergeOutputApproved = 7771855,

            EMaterialMergeInput = 1856,
            EMaterialMergeInputApproved = 7771856,

            // SUB PRODUCTION
            ETransactionOrder = 9380,
            ETransactionOrderApproved = 7779380,

            ETransactionRelease = 9383,
            ETransactionReleaseApproved = 7779383,

            ETransactionFinish = 9385,
            ETransactionFinishApproved = 7779385,

            ETransactionInput = 9387,
            ETransactionInputApproved = 7779387,

            ETransactionTransfer = 9389,
            ETransactionTransferApproved = 7779389,

            ETransactionAdjust = 9391,
            ETransactionAdjustApproved = 7779391,

            ETransactionOutput = 9393,
            ETransactionOutputApproved = 9393111,
            ETransactionOutputUnApproved = 93931111,

            ETransactionCommand = 9394,
            ETransactionCommandApproved = 7779394,

            EProcessAlpha = 9180,
            EProcessAlphaApproved = 7779180

        }

        private void SPSKUInventoryJournal()
        {


            //////////string wInventory = " AND (@LocalCommodityIDList = '' OR CommodityID IN (SELECT * FROM FNSplitUpIds(@LocalCommodityIDList))) " + "\r\n";
            //////////wInventory = wInventory + " AND (@LocalWarehouseGroupIDList = '' OR ListWarehouseName.WarehouseGroupID IN (SELECT * FROM FNSplitUpIds(@LocalWarehouseGroupIDList))) " + "\r\n";
            //////////wInventory = wInventory + " AND (@LocalWHLocationIDList = '' OR ListWarehouseName.WHLocationID IN (SELECT * FROM FNSplitUpIds(@LocalWHLocationIDList))) " + "\r\n";
            //////////wInventory = wInventory + " AND (@LocalWHCategoryIDList = '' OR ListWarehouseName.WHCategoryID IN (SELECT * FROM FNSplitUpIds(@LocalWHCategoryIDList))) " + "\r\n";
            //////////wInventory = wInventory + " AND (@LocalWarehouseClassIDList = '' OR ListWarehouseName.WarehouseClassID IN (SELECT * FROM FNSplitUpIds(@LocalWarehouseClassIDList))) " + "\r\n";
            //////////wInventory = wInventory + " AND (@LocalWarehouseIDList = '' OR ListWarehouseName.WarehouseID IN (SELECT * FROM FNSplitUpIds(@LocalWarehouseIDList))) " + "\r\n";

            ////////////CHU Y: PRODUCTION: KHONG PHAN BIET KHO -- REMOVE DIEU KIEN FILTER
            //////////string wProduction = " AND (@LocalCommodityIDList = '' OR CommodityID IN (SELECT * FROM FNSplitUpIds(@LocalCommodityIDList))) " + "\r\n";
            //////////wProduction = wProduction + " AND (@LocalWarehouseClassIDList = '' OR " + (int)EWarehouseClassID.EWarehouseClassL1 + " IN (SELECT * FROM FNSplitUpIds(@LocalWarehouseClassIDList)) OR " + (int)EWarehouseClassID.EWarehouseClassLD + " IN (SELECT * FROM FNSplitUpIds(@LocalWarehouseClassIDList)) OR " + (int)EWarehouseClassID.EWarehouseClassL5 + " IN (SELECT * FROM FNSplitUpIds(@LocalWarehouseClassIDList))) " + "\r\n"; //@LocalWarehouseClassIDList = '' OR @LocalWarehouseClassIDList: HOAC [L1] HOAC [LD] HOAC [L5] HOAC [L1 + LD + L5]
            //////////wProduction = wProduction + " AND (@LocalWarehouseGroupIDList = '' OR ProductionOrderMaster.WarehouseGroupID IN (SELECT * FROM FNSplitUpIds(@LocalWarehouseGroupIDList))) " + "\r\n";




            string queryString = " DROP PROC SPSKUInventoryJournal " + "\r\n";
            queryString = queryString + " CREATE PROC SPSKUInventoryJournal " + "\r\n";

            queryString = queryString + " @dDateFrom DateTime, @dDateTo DateTime, @lCommodityIDList varchar(8000), @lWarehouseGroupIDList varchar(60), @lWHLocationIDList varchar(30), @lWHCategoryIDList varchar(30), @lWarehouseClassIDList varchar(30), @lWarehouseIDList varchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON " + "\r\n";

            queryString = queryString + "       DECLARE @LocalDateFrom DateTime, @LocalDateTo DateTime, @LocalCommodityIDList varchar(8000), @LocalWarehouseGroupIDList varchar(60), @LocalWHLocationIDList varchar(30), @LocalWHCategoryIDList varchar(30), @LocalWarehouseClassIDList varchar(30), @LocalWarehouseIDList varchar(100) " + "\r\n";
            queryString = queryString + "       SET @LocalDateFrom = @dDateFrom     SET @LocalDateTo = @dDateTo     SET @LocalCommodityIDList = @lCommodityIDList       SET @LocalWarehouseGroupIDList = @lWarehouseGroupIDList       SET @LocalWHLocationIDList = @lWHLocationIDList       SET @LocalWHCategoryIDList = @lWHCategoryIDList         SET @LocalWarehouseClassIDList = @lWarehouseClassIDList         SET @LocalWarehouseIDList = @lWarehouseIDList  " + "\r\n";


            queryString = queryString + "       DECLARE     @WarehouseFilterable bit      SET @WarehouseFilterable = 0    " + "\r\n";
            queryString = queryString + "       DECLARE     @WarehouseFilter TABLE (WarehouseID int NOT NULL) " + "\r\n";

            queryString = queryString + "       IF (@LocalWarehouseIDList <> '' AND @LocalWarehouseGroupIDList = '' AND @LocalWHLocationIDList = '' AND @LocalWHCategoryIDList = '' AND @LocalWarehouseClassIDList = '') " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SET @WarehouseFilterable = 1 " + "\r\n";
            queryString = queryString + "               INSERT INTO @WarehouseFilter  SELECT Id FROM dbo.SplitToIntList (@LocalWarehouseIDList)  " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           IF (@LocalWarehouseIDList <> '' OR @LocalWarehouseGroupIDList <> '' OR @LocalWHLocationIDList <> '' OR @LocalWHCategoryIDList <> '' OR @LocalWarehouseClassIDList <> '') " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   SET @WarehouseFilterable = 1 " + "\r\n";
            queryString = queryString + "                   INSERT INTO @WarehouseFilter    SELECT WarehouseID FROM ListWarehouseName WHERE (@LocalWarehouseIDList = '' OR WarehouseID IN (SELECT * FROM FNSplitUpIds(@LocalWarehouseIDList)))            AND (@LocalWarehouseGroupIDList = '' OR WarehouseGroupID IN (SELECT * FROM FNSplitUpIds(@LocalWarehouseGroupIDList)))         AND (@LocalWHLocationIDList = '' OR WHLocationID IN (SELECT * FROM FNSplitUpIds(@LocalWHLocationIDList)))             AND (@LocalWHCategoryIDList = '' OR WHCategoryID IN (SELECT * FROM FNSplitUpIds(@LocalWHCategoryIDList)))             AND (@LocalWarehouseClassIDList = '' OR WarehouseClassID IN (SELECT * FROM FNSplitUpIds(@LocalWarehouseClassIDList)))     " + "\r\n";
            queryString = queryString + "               END " + "\r\n";



            queryString = queryString + "       DECLARE     @CommodityFilter TABLE (CommodityID int NOT NULL) " + "\r\n";
            queryString = queryString + "       IF (@LocalCommodityIDList <> '') " + "\r\n";
            queryString = queryString + "                   INSERT INTO @CommodityFilter SELECT Id FROM dbo.SplitToIntList (@LocalCommodityIDList) " + "       WHERE Id IN (SELECT CommodityID FROM ListItemCommodity WHERE ItemTypeID IN (" + (int)GlobalEnums.CommodityTypeID.Vehicles + ", " + (int)GlobalEnums.CommodityTypeID.Parts + ", " + (int)GlobalEnums.CommodityTypeID.Consumables + ")) " + "\r\n";



            queryString = queryString + "       DECLARE     @WarehouseClassFilter bit " + "\r\n";
            queryString = queryString + "       IF (@LocalWarehouseClassIDList = '')   SET @WarehouseClassFilter = 1 " + "\r\n";
            queryString = queryString + "       ELSE   SET @WarehouseClassFilter = (SELECT 1 WHERE @LocalWarehouseClassIDList = '' OR " + (int)EWarehouseClassID.EWarehouseClassL1 + " IN (SELECT * FROM FNSplitUpIds(@LocalWarehouseClassIDList)) OR " + (int)EWarehouseClassID.EWarehouseClassLD + " IN (SELECT * FROM FNSplitUpIds(@LocalWarehouseClassIDList)) OR " + (int)EWarehouseClassID.EWarehouseClassL5 + " IN (SELECT * FROM FNSplitUpIds(@LocalWarehouseClassIDList))) " + "\r\n"; //@LocalWarehouseClassIDList = '' OR @LocalWarehouseClassIDList: HOAC [L1] HOAC [LD] HOAC [L5] HOAC [L1 + LD + L5]



            queryString = queryString + "       DECLARE     @WarehouseGroupFilter TABLE (WarehouseGroupID int NOT NULL) " + "\r\n";
            queryString = queryString + "       IF (@LocalWarehouseGroupIDList <> '') " + "\r\n";
            queryString = queryString + "                   INSERT INTO @WarehouseGroupFilter SELECT Id FROM dbo.SplitToIntList (@LocalWarehouseGroupIDList) " + "\r\n";




            // --GET THE BEGIN BALANCE IF AVAILABLE.BEGIN
            queryString = queryString + "       DECLARE @lSKUBalanceDate DateTime" + "\r\n";
            queryString = queryString + "       DECLARE CursorSKUBalance CURSOR LOCAL FOR SELECT MAX(SKUBalanceDate) AS SKUBalanceDate FROM SKUBalanceDetail WHERE SKUBalanceDate < @LocalDateFrom " + "\r\n"; // < OR <= ??? XEM XET LAI NHE!!!!
            queryString = queryString + "       OPEN CursorSKUBalance" + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorSKUBalance INTO @lSKUBalanceDate" + "\r\n";
            queryString = queryString + "       CLOSE CursorSKUBalance DEALLOCATE CursorSKUBalance" + "\r\n";

            queryString = queryString + "       IF @lSKUBalanceDate IS NULL  SET @lSKUBalanceDate = CONVERT(Datetime, '2009-05-16 23:59:59', 120) " + "\r\n";
            // --GET THE BEGIN BALANCE IF AVAILABLE.END

            queryString = queryString + "       IF          (@LocalWarehouseGroupIDList = '' AND @WarehouseFilterable = 0 AND @LocalCommodityIDList = '') " + "\r\n";
            queryString = queryString + "               " + this.SPSKUInventoryJournalSQLA(false, false, false) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@LocalWarehouseGroupIDList = '' AND @WarehouseFilterable = 0 AND @LocalCommodityIDList <> '') " + "\r\n";
            queryString = queryString + "               " + this.SPSKUInventoryJournalSQLA(false, false, true) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@LocalWarehouseGroupIDList = '' AND @WarehouseFilterable = 1 AND @LocalCommodityIDList = '') " + "\r\n";
            queryString = queryString + "               " + this.SPSKUInventoryJournalSQLA(false, true, false) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@LocalWarehouseGroupIDList = '' AND @WarehouseFilterable = 1 AND @LocalCommodityIDList <> '') " + "\r\n";
            queryString = queryString + "               " + this.SPSKUInventoryJournalSQLA(false, true, true) + "\r\n";


            queryString = queryString + "       ELSE    IF  (@LocalWarehouseGroupIDList <> '' AND @WarehouseFilterable = 0 AND @LocalCommodityIDList = '') " + "\r\n";
            queryString = queryString + "               " + this.SPSKUInventoryJournalSQLA(true, false, false) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@LocalWarehouseGroupIDList <> '' AND @WarehouseFilterable = 0 AND @LocalCommodityIDList <> '') " + "\r\n";
            queryString = queryString + "               " + this.SPSKUInventoryJournalSQLA(true, false, true) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@LocalWarehouseGroupIDList <> '' AND @WarehouseFilterable = 1 AND @LocalCommodityIDList = '') " + "\r\n";
            queryString = queryString + "               " + this.SPSKUInventoryJournalSQLA(true, true, false) + "\r\n";
            queryString = queryString + "       ELSE    " + "\r\n"; //IF  (@LocalWarehouseGroupIDList <> '' AND @WarehouseFilterable = 1 AND @LocalCommodityIDList <> '') "
            queryString = queryString + "               " + this.SPSKUInventoryJournalSQLA(true, true, true) + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            System.Diagnostics.Debug.WriteLine("---");
            System.Diagnostics.Debug.WriteLine(queryString);

        }


        private string SPSKUInventoryJournalSQLA(bool isWarehouseGroupFilter, bool isWarehouseFilter, bool isCommodityFilter)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SKUJournalMaster.NMVNTaskID, SKUJournalMaster.JournalPrimaryID, SKUJournalMaster.JournalDate, SKUJournalMaster.JournalReference, LEFT(SKUJournalMaster.JournalDescription, 200) AS JournalDescription, " + "\r\n";
            queryString = queryString + "                   ListItemCommodity.CommodityID, ListItemCommodity.Description, ListItemCommodity.DescriptionOfficial, ListItemCommodity.DescriptionPartA, ListItemCommodity.DescriptionPartB, ListItemCommodity.DescriptionPartC, ListItemCommodity.DescriptionPartD, ListItemCommodity.UnitSales, ListItemCommodity.Weight, ListItemCommodity.LeadTime, VWListItemCategory.SellLife, " + "\r\n";
            queryString = queryString + "                   SKUJournalMaster.WarehouseGroupID, ISNULL(ListWarehouseName.WHLocationID, 0) AS WHLocationID, ISNULL(ListWarehouseName.WHCategoryID, 0) AS WHCategoryID, SKUJournalMaster.WarehouseClassID, SKUJournalMaster.WarehouseID, ISNULL(ListWarehouseName.Description, '') AS WarehouseName, SKUJournalMaster.WarehouseOutID, ISNULL(ListWarehouseOutName.Description, '') AS WarehouseOutName, " + "\r\n";

            queryString = queryString + "                   SKUJournalMaster.QuantityBegin, SKUJournalMaster.QuantityInputPRO, SKUJournalMaster.QuantityInputINV, SKUJournalMaster.QuantityInputRTN, SKUJournalMaster.QuantityInputTRF, SKUJournalMaster.QuantityInputADJ, SKUJournalMaster.QuantityInputBLD, SKUJournalMaster.QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                   ROUND(SKUJournalMaster.QuantityInputPRO + SKUJournalMaster.QuantityInputINV + SKUJournalMaster.QuantityInputRTN + SKUJournalMaster.QuantityInputTRF + SKUJournalMaster.QuantityInputADJ + SKUJournalMaster.QuantityInputBLD + SKUJournalMaster.QuantityInputUBL, " + (int)GlobalEnums.rndQuantity + ") AS QuantityInput, " + "\r\n";
            queryString = queryString + "                   SKUJournalMaster.QuantityOutputINV, SKUJournalMaster.QuantityOutputGoodsIssue, SKUJournalMaster.QuantityOutputTRF, SKUJournalMaster.QuantityOutputADJ, SKUJournalMaster.QuantityOutputBLD, SKUJournalMaster.QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                   ROUND(SKUJournalMaster.QuantityOutputINV + SKUJournalMaster.QuantityOutputGoodsIssue + SKUJournalMaster.QuantityOutputTRF + SKUJournalMaster.QuantityOutputADJ + SKUJournalMaster.QuantityOutputBLD + SKUJournalMaster.QuantityOutputUBL, " + (int)GlobalEnums.rndQuantity + ") AS QuantityOutput, " + "\r\n";
            queryString = queryString + "                   SKUJournalMaster.QuantityOnTransfer, SKUJournalMaster.QuantityOnAdvice, SKUJournalMaster.QuantityOnTransferAdviceOut, SKUJournalMaster.QuantityOnTransferAdviceIn, SKUJournalMaster.QuantityOnProduction, 0 AS UPriceNMDInventory, " + "\r\n";

            queryString = queryString + "                   VWListItemCategory.ItemCategoryID, " + "\r\n";
            queryString = queryString + "                   VWListItemCategory.Description1 AS ItemCategoryName1, " + "\r\n";
            queryString = queryString + "                   VWListItemCategory.Description2 AS ItemCategoryName2, " + "\r\n";
            queryString = queryString + "                   VWListItemCategory.Description3 AS ItemCategoryName3, " + "\r\n";
            queryString = queryString + "                   VWListItemCategory.Description4 AS ItemCategoryName4, " + "\r\n";
            queryString = queryString + "                   VWListItemCategory.Description5 AS ItemCategoryName5, " + "\r\n";
            queryString = queryString + "                   VWListItemCategory.Description6 AS ItemCategoryName6, " + "\r\n";
            queryString = queryString + "                   VWListItemCategory.Description7 AS ItemCategoryName7, " + "\r\n";
            queryString = queryString + "                   VWListItemCategory.Description8 AS ItemCategoryName8, " + "\r\n";
            queryString = queryString + "                   VWListItemCategory.Description9 AS ItemCategoryName9, " + "\r\n";

            queryString = queryString + "                   MaxTransferOutputUNION.MaxTransferOutputDate " + "\r\n";

            queryString = queryString + "       FROM        ( " + "\r\n";

            queryString = queryString + "                       SELECT      NMVNTaskID, JournalPrimaryID, MAX(JournalDate) AS JournalDate, MAX(JournalReference) AS JournalReference, MAX(JournalDescription) AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                   CommodityID, WarehouseGroupID, WarehouseClassID, WarehouseID, WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                   SUM(QuantityBegin) AS QuantityBegin, SUM(QuantityInputPRO) AS QuantityInputPRO, SUM(QuantityInputINV) AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                   SUM(QuantityInputRTN) AS QuantityInputRTN, SUM(QuantityInputTRF) AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                   SUM(QuantityInputADJ) AS QuantityInputADJ, SUM(QuantityInputBLD) AS QuantityInputBLD, SUM(QuantityInputUBL) AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                   SUM(QuantityOutputINV) AS QuantityOutputINV, SUM(QuantityOutputGoodsIssue) AS QuantityOutputGoodsIssue, SUM(QuantityOutputTRF) AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                   SUM(QuantityOutputADJ) AS QuantityOutputADJ, SUM(QuantityOutputBLD) AS QuantityOutputBLD, SUM(QuantityOutputUBL) AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                   SUM(QuantityOnTransfer) AS QuantityOnTransfer, SUM(QuantityOnAdvice) AS QuantityOnAdvice, SUM(QuantityOnTransferAdviceOut) AS QuantityOnTransferAdviceOut, SUM(QuantityOnTransferAdviceIn) AS QuantityOnTransferAdviceIn, SUM(QuantityOnProduction) AS QuantityOnProduction " + "\r\n";


            queryString = queryString + "                       FROM       (" + "\r\n";

            queryString = queryString + "                       " + this.SPSKUInventoryJournalSQLB(isWarehouseGroupFilter, isWarehouseFilter, isCommodityFilter) + "\r\n";

            queryString = queryString + "                           )SKUUnionMaster" + "\r\n";

            queryString = queryString + "                       GROUP BY        NMVNTaskID, JournalPrimaryID, CommodityID, WarehouseGroupID, WarehouseClassID, WarehouseID, WarehouseOutID " + "\r\n";

            // KHONG CAN THIET PHAI SU DUNG HAVING NAY: VI THUC TE, PHAI CO PHAT SINH, MOI CO DU LIEU, NEN KTRA HAVING KHONG THE TOI UU DUOC DU LIEU, KHONG DUOC LOI ICH GI
            // queryString = queryString + "                       HAVING          SUM(QuantityBegin) > 0 OR SUM(QuantityInputPRO) > 0 OR SUM(QuantityInputINV) > 0 OR SUM(QuantityInputRTN) > 0 OR SUM(QuantityInputTRF) > 0 OR SUM(QuantityInputADJ) > 0 OR SUM(QuantityInputBLD) > 0 OR SUM(QuantityInputUBL) > 0 OR SUM(QuantityOutputINV) > 0 OR SUM(QuantityOutputGoodsIssue) > 0, OR SUM(QuantityOutputTRF) > 0 OR SUM(QuantityOutputADJ) > 0 OR SUM(QuantityOutputBLD) > 0 OR SUM(QuantityOutputUBL) > 0 OR SUM(QuantityOnTransfer) > 0 OR SUM(QuantityOnAdvice) > 0 OR SUM(QuantityOnTransferAdviceOut) > 0 OR SUM(QuantityOnTransferAdviceIn) > 0 OR SUM(QuantityOnProduction) > 0 " + "\r\n";

            queryString = queryString + "                   ) SKUJournalMaster  INNER JOIN " + "\r\n";

            queryString = queryString + "                   ListItemCommodity ON SKUJournalMaster.CommodityID = ListItemCommodity.CommodityID INNER JOIN " + "\r\n";
            queryString = queryString + "                   VWListItemCategory ON ListItemCommodity.ItemCategoryID = VWListItemCategory.ItemCategoryID LEFT JOIN " + "\r\n";

            queryString = queryString + "                   ListWarehouseName ON SKUJournalMaster.WarehouseID = ListWarehouseName.WarehouseID LEFT JOIN " + "\r\n";
            queryString = queryString + "                   ListWarehouseName ListWarehouseOutName ON SKUJournalMaster.WarehouseOutID = ListWarehouseOutName.WarehouseID LEFT JOIN " + "\r\n";

            queryString = queryString + "                  (SELECT      WarehouseID, CommodityID, MAX(MaxTransferOutputDate) AS MaxTransferOutputDate " + "\r\n";
            queryString = queryString + "                   FROM       ( " + "\r\n";
            queryString = queryString + "                               SELECT      WarehouseID, CommodityID, MAX(SKUTransferDate) AS MaxTransferOutputDate " + "\r\n";
            queryString = queryString + "                               FROM        SKUTransferDetail " + "\r\n";
            queryString = queryString + "                               WHERE       SKUTransferDate <= @LocalDateTo " + "\r\n";
            queryString = queryString + "                               GROUP BY    WarehouseID, CommodityID " + "\r\n";

            queryString = queryString + "                               UNION ALL" + "\r\n";

            queryString = queryString + "                               SELECT      WarehouseID, CommodityID, MAX(SKUOutputDate) AS MaxTransferOutputDate " + "\r\n";
            queryString = queryString + "                               FROM        SKUOutputDetail " + "\r\n";
            queryString = queryString + "                               WHERE       SKUOutputDate <= @LocalDateTo " + "\r\n";
            queryString = queryString + "                               GROUP BY    WarehouseID, CommodityID " + "\r\n";
            queryString = queryString + "                              )ABC  " + "\r\n";
            queryString = queryString + "                   GROUP BY    WarehouseID, CommodityID) MaxTransferOutputUNION ON SKUJournalMaster.WarehouseID = MaxTransferOutputUNION.WarehouseID AND SKUJournalMaster.CommodityID = MaxTransferOutputUNION.CommodityID " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }


        private string SPSKUInventoryJournalSQLB(bool isWarehouseGroupFilter, bool isWarehouseFilter, bool isCommodityFilter)
        {
            string queryString = "";
            // --OPENNING: PURE OPENNING   //BEGIN
            queryString = queryString + "                           SELECT      0 AS NMVNTaskID, 0 AS JournalPrimaryID, @LocalDateFrom - 1 AS JournalDate, '' AS JournalReference, 'TON DAU KY' AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       SKUBalanceDetail.CommodityID, ListWarehouseName.WarehouseGroupID, ListWarehouseName.WarehouseClassID, SKUBalanceDetail.WarehouseID, 0 AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       SKUBalanceDetail.Quantity AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, 0 AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, 0 AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        SKUBalanceDetail INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListWarehouseName ON SKUBalanceDetail.WarehouseID = ListWarehouseName.WarehouseID " + "\r\n";
            queryString = queryString + "                           WHERE       SKUBalanceDate = @lSKUBalanceDate " + this.SPSKUInventoryJournalWarehouseFilter("SKUBalanceDetail", isWarehouseFilter) + this.SPSKUInventoryJournalCommodityFilter("SKUBalanceDetail", isCommodityFilter) + "\r\n";
            // --OPENNING: PURE OPENNING   //END



            // --INPUT: IN-TERM OPENNING + INPUT   //BEGIN
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      CASE WHEN SKUInputMaster.SKUInputDate >= @LocalDateFrom THEN " + (int)ENMVNTaskID.ESKUInput + " ELSE 0 END AS NMVNTaskID, CASE WHEN SKUInputMaster.SKUInputDate >= @LocalDateFrom THEN SKUInputMaster.SKUInputID ELSE 0 END AS JournalPrimaryID, CASE WHEN SKUInputMaster.SKUInputDate >= @LocalDateFrom THEN SKUInputMaster.SKUInputDate ELSE @LocalDateFrom - 1 END AS JournalDate, CASE WHEN SKUInputMaster.SKUInputDate >= @LocalDateFrom THEN SKUInputMaster.Reference ELSE '' END AS JournalReference, CASE WHEN SKUInputMaster.SKUInputDate >= @LocalDateFrom THEN SKUInputMaster.Description ELSE 'TON DAU KY' END AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       SKUInputDetail.CommodityID, ListWarehouseName.WarehouseGroupID, ListWarehouseName.WarehouseClassID, SKUInputDetail.WarehouseID, 0 AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       CASE WHEN SKUInputMaster.SKUInputDate < @LocalDateFrom THEN SKUInputDetail.Quantity ELSE 0 END AS QuantityBegin, CASE WHEN SKUInputMaster.SKUInputDate >= @LocalDateFrom AND SKUInputMaster.SKUInputVoucherTypeID = " + (int)ENMVNTaskID.EProductionFinish + " THEN SKUInputDetail.Quantity ELSE 0 END AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, CASE WHEN SKUInputMaster.SKUInputDate >= @LocalDateFrom AND SKUInputMaster.SKUInputVoucherTypeID = " + (int)ENMVNTaskID.ESKUTransfer + " THEN SKUInputDetail.Quantity ELSE 0 END AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       CASE WHEN SKUInputMaster.SKUInputDate >= @LocalDateFrom AND SKUInputMaster.SKUInputVoucherTypeID = " + (int)ENMVNTaskID.ESKUAdjust + " THEN SKUInputDetail.Quantity ELSE 0 END AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, 0 AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, 0 AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        SKUInputMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                       SKUInputDetail ON SKUInputMaster.SKUInputID = SKUInputDetail.SKUInputID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListWarehouseName ON SKUInputDetail.WarehouseID = ListWarehouseName.WarehouseID " + "\r\n";
            queryString = queryString + "                           WHERE       SKUInputMaster.SKUInputDate > @lSKUBalanceDate AND SKUInputMaster.SKUInputDate <= @LocalDateTo " + this.SPSKUInventoryJournalWarehouseFilter("SKUInputDetail", isWarehouseFilter) + this.SPSKUInventoryJournalCommodityFilter("SKUInputDetail", isCommodityFilter) + "\r\n";
            // --INPUT: IN-TERM OPENNING + INPUT   //END



            // --OUTPUT: IN-TERM OPENNING + OUTPUT //BEGIN
            //--ENMVNTaskID.ESKUTransfer
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      CASE WHEN SKUTransferMaster.SKUTransferDate >= @LocalDateFrom THEN " + (int)ENMVNTaskID.ESKUTransfer + " ELSE 0 END AS NMVNTaskID, CASE WHEN SKUTransferMaster.SKUTransferDate >= @LocalDateFrom THEN SKUTransferMaster.SKUTransferID ELSE 0 END AS JournalPrimaryID, CASE WHEN SKUTransferMaster.SKUTransferDate >= @LocalDateFrom THEN SKUTransferMaster.SKUTransferDate ELSE @LocalDateFrom - 1 END AS JournalDate, CASE WHEN SKUTransferMaster.SKUTransferDate >= @LocalDateFrom THEN SKUTransferMaster.Reference ELSE '' END AS JournalReference, CASE WHEN SKUTransferMaster.SKUTransferDate >= @LocalDateFrom THEN SKUTransferMaster.Description ELSE 'TON DAU KY' END AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       SKUTransferDetail.CommodityID, ListWarehouseName.WarehouseGroupID, ListWarehouseName.WarehouseClassID, SKUTransferDetail.WarehouseID, 0 AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       CASE WHEN SKUTransferMaster.SKUTransferDate < @LocalDateFrom THEN -SKUTransferDetail.Quantity ELSE 0 END AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, CASE WHEN SKUTransferMaster.SKUTransferDate >= @LocalDateFrom THEN SKUTransferDetail.Quantity ELSE 0 END AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, 0 AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, 0 AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        SKUTransferMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                       SKUTransferDetail ON SKUTransferMaster.SKUTransferID = SKUTransferDetail.SKUTransferID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListWarehouseName ON SKUTransferDetail.WarehouseID = ListWarehouseName.WarehouseID " + "\r\n";
            queryString = queryString + "                           WHERE       SKUTransferMaster.SKUTransferDate > @lSKUBalanceDate AND SKUTransferMaster.SKUTransferDate <= @LocalDateTo " + this.SPSKUInventoryJournalWarehouseFilter("SKUTransferDetail", isWarehouseFilter) + this.SPSKUInventoryJournalCommodityFilter("SKUTransferDetail", isCommodityFilter) + "\r\n";

            //--ENMVNTaskID.ESKUOutput
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      CASE WHEN SKUOutputMaster.SKUOutputDate >= @LocalDateFrom THEN " + (int)ENMVNTaskID.ESKUOutput + " ELSE 0 END AS NMVNTaskID, CASE WHEN SKUOutputMaster.SKUOutputDate >= @LocalDateFrom THEN SKUOutputMaster.SKUOutputID ELSE 0 END AS JournalPrimaryID, CASE WHEN SKUOutputMaster.SKUOutputDate >= @LocalDateFrom THEN SKUOutputMaster.SKUOutputDate ELSE @LocalDateFrom - 1 END AS JournalDate, CASE WHEN SKUOutputMaster.SKUOutputDate >= @LocalDateFrom THEN SKUOutputMaster.Reference ELSE '' END AS JournalReference, CASE WHEN SKUOutputMaster.SKUOutputDate >= @LocalDateFrom THEN SKUOutputMaster.Description ELSE 'TON DAU KY' END AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       SKUOutputDetail.CommodityID, ListWarehouseName.WarehouseGroupID, ListWarehouseName.WarehouseClassID, SKUOutputDetail.WarehouseID, 0 AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       CASE WHEN SKUOutputMaster.SKUOutputDate < @LocalDateFrom THEN -SKUOutputDetail.Quantity ELSE 0 END AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       CASE WHEN SKUOutputMaster.SKUOutputDate >= @LocalDateFrom THEN SKUOutputDetail.Quantity ELSE 0 END AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, 0 AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, 0 AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        SKUOutputMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                       SKUOutputDetail ON SKUOutputMaster.SKUOutputID = SKUOutputDetail.SKUOutputID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListWarehouseName ON SKUOutputDetail.WarehouseID = ListWarehouseName.WarehouseID " + "\r\n";
            queryString = queryString + "                           WHERE       SKUOutputMaster.SKUOutputDate > @lSKUBalanceDate AND SKUOutputMaster.SKUOutputDate <= @LocalDateTo " + this.SPSKUInventoryJournalWarehouseFilter("SKUOutputDetail", isWarehouseFilter) + this.SPSKUInventoryJournalCommodityFilter("SKUOutputDetail", isCommodityFilter) + "\r\n";

            //**************
            //--ENMVNTaskID.GoodsIssue
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      CASE WHEN GoodsIssueDetails.EntryDate >= @LocalDateFrom THEN " + (int)GlobalEnums.NmvnTaskID.GoodsIssue + " ELSE 0 END AS NMVNTaskID, CASE WHEN GoodsIssueDetails.EntryDate >= @LocalDateFrom THEN GoodsIssueDetails.GoodsIssueID ELSE 0 END AS JournalPrimaryID, CASE WHEN GoodsIssueDetails.EntryDate >= @LocalDateFrom THEN GoodsIssueDetails.EntryDate ELSE @LocalDateFrom - 1 END AS JournalDate, CASE WHEN GoodsIssueDetails.EntryDate >= @LocalDateFrom THEN '' ELSE '' END AS JournalReference, CASE WHEN GoodsIssueDetails.EntryDate >= @LocalDateFrom THEN '' ELSE 'TON DAU KY' END AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       GoodsIssueDetails.CommodityID, ListWarehouseName.WarehouseGroupID, ListWarehouseName.WarehouseClassID, GoodsIssueDetails.WarehouseID, 0 AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       CASE WHEN GoodsIssueDetails.EntryDate < @LocalDateFrom THEN -(GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity) ELSE 0 END AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, CASE WHEN GoodsIssueDetails.EntryDate >= @LocalDateFrom THEN (GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity) ELSE 0 END AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, 0 AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, 0 AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        GoodsIssueDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListWarehouseName ON GoodsIssueDetails.WarehouseID = ListWarehouseName.WarehouseID " + "\r\n";
            queryString = queryString + "                           WHERE       GoodsIssueDetails.EntryDate > @lSKUBalanceDate AND GoodsIssueDetails.EntryDate <= @LocalDateTo " + this.SPSKUInventoryJournalWarehouseFilter("GoodsIssueDetails", isWarehouseFilter) + this.SPSKUInventoryJournalCommodityFilter("GoodsIssueDetails", isCommodityFilter) + "\r\n";


            //--ENMVNTaskID.ESKUAdjust
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      CASE WHEN SKUAdjustMaster.SKUAdjustDate >= @LocalDateFrom THEN " + (int)ENMVNTaskID.ESKUAdjust + " ELSE 0 END AS NMVNTaskID, CASE WHEN SKUAdjustMaster.SKUAdjustDate >= @LocalDateFrom THEN SKUAdjustMaster.SKUAdjustID ELSE 0 END AS JournalPrimaryID, CASE WHEN SKUAdjustMaster.SKUAdjustDate >= @LocalDateFrom THEN SKUAdjustMaster.SKUAdjustDate ELSE @LocalDateFrom - 1 END AS JournalDate, CASE WHEN SKUAdjustMaster.SKUAdjustDate >= @LocalDateFrom THEN SKUAdjustMaster.Reference ELSE '' END AS JournalReference, CASE WHEN SKUAdjustMaster.SKUAdjustDate >= @LocalDateFrom THEN SKUAdjustMaster.Description ELSE 'TON DAU KY' END AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       SKUAdjustDetail.CommodityID, ListWarehouseName.WarehouseGroupID, ListWarehouseName.WarehouseClassID, SKUAdjustDetail.WarehouseID, 0 AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       CASE WHEN SKUAdjustMaster.SKUAdjustDate < @LocalDateFrom THEN SKUAdjustDetail.Quantity ELSE 0 END AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       CASE WHEN SKUAdjustMaster.SKUAdjustDate >= @LocalDateFrom THEN -SKUAdjustDetail.Quantity ELSE 0 END AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, 0 AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, 0 AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        SKUAdjustMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                       SKUAdjustDetail ON SKUAdjustMaster.SKUAdjustID = SKUAdjustDetail.SKUAdjustID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListWarehouseName ON SKUAdjustDetail.WarehouseID = ListWarehouseName.WarehouseID " + "\r\n"; //OUTPUT ONLY: ADJUST: CO DAT BIET HON CAC T/H KHAC: QUANTITY < 0: NEN: SKUAdjustDetail.Quantity: +/- NGUOC LAI VOI CAC T/H KHAC TI XIU
            queryString = queryString + "                           WHERE       SKUAdjustMaster.SKUAdjustDate > @lSKUBalanceDate AND SKUAdjustMaster.SKUAdjustDate <= @LocalDateTo AND SKUAdjustDetail.Quantity < 0 " + this.SPSKUInventoryJournalWarehouseFilter("SKUAdjustDetail", isWarehouseFilter) + this.SPSKUInventoryJournalCommodityFilter("SKUAdjustDetail", isCommodityFilter) + "\r\n";

            // --OUTPUT: IN-TERM OPENNING + OUTPUT //END

            // --ON-INPUT  //BEGIN
            //--ENMVNTaskID.ESKUTransfer
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      " + (int)ENMVNTaskID.ESKUTransfer + " AS NMVNTaskID, SKUTransferMaster.SKUTransferID AS JournalPrimaryID, SKUTransferMaster.SKUTransferDate AS JournalDate, SKUTransferMaster.Reference AS JournalReference, SKUTransferMaster.Description AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       SKUTransferDetail.CommodityID, ListWarehouseName.WarehouseGroupID, ListWarehouseName.WarehouseClassID, SKUTransferDetail.WarehouseInID AS WarehouseID, SKUTransferDetail.WarehouseID AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       ROUND(SKUTransferDetail.Quantity - SKUTransferDetail.QuantityInput, " + (int)GlobalEnums.rndQuantity + ") AS QuantityOnTransfer, 0 AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, 0 AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        SKUTransferMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                       SKUTransferDetail ON SKUTransferMaster.SKUTransferID = SKUTransferDetail.SKUTransferID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListWarehouseName ON SKUTransferDetail.WarehouseInID = ListWarehouseName.WarehouseID " + "\r\n";
            queryString = queryString + "                           WHERE       SKUTransferMaster.SKUTransferDate <= @LocalDateTo AND ROUND(SKUTransferDetail.Quantity - SKUTransferDetail.QuantityInput, " + (int)GlobalEnums.rndQuantity + ") > 0 " + this.SPSKUInventoryJournalWarehouseFilter("ListWarehouseName", isWarehouseFilter) + this.SPSKUInventoryJournalCommodityFilter("SKUTransferDetail", isCommodityFilter) + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      " + (int)ENMVNTaskID.ESKUTransfer + " AS NMVNTaskID, SKUTransferMaster.SKUTransferID AS JournalPrimaryID, SKUTransferMaster.SKUTransferDate AS JournalDate, SKUTransferMaster.Reference AS JournalReference, SKUTransferMaster.Description AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       SKUInputDetail.CommodityID, ListWarehouseName.WarehouseGroupID, ListWarehouseName.WarehouseClassID, SKUInputDetail.WarehouseID, SKUInputDetail.WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       SKUInputDetail.Quantity AS QuantityOnTransfer, 0 AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, 0 AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        SKUTransferMaster INNER JOIN " + "\r\n"; //LUU Y: SKUTransferDetail.WarehouseInID LUON LUON = SKUInputDetail.WarehouseID, NEN CAU LENH UNDO SAU DAY LUON LUON DUNG
            queryString = queryString + "                                       SKUInputMaster ON SKUTransferMaster.SKUTransferID = SKUInputMaster.SKUInputVoucherID AND SKUInputMaster.SKUInputVoucherTypeID = " + (int)ENMVNTaskID.ESKUTransfer + " INNER JOIN " + "\r\n";
            queryString = queryString + "                                       SKUInputDetail ON SKUInputMaster.SKUInputID = SKUInputDetail.SKUInputID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListWarehouseName ON SKUInputDetail.WarehouseID = ListWarehouseName.WarehouseID " + "\r\n";
            queryString = queryString + "                           WHERE       SKUTransferMaster.SKUTransferDate <= @LocalDateTo AND SKUInputMaster.SKUInputDate > @LocalDateTo " + this.SPSKUInventoryJournalWarehouseFilter("SKUInputDetail", isWarehouseFilter) + this.SPSKUInventoryJournalCommodityFilter("SKUInputDetail", isCommodityFilter) + "\r\n";
            // --ON-INPUT  //END



            // --ON-OUTPUT  //BEGIN
            //--ENMVNTaskID.DeliveryAdvice
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      " + (int)GlobalEnums.NmvnTaskID.DeliveryAdvice + " AS NMVNTaskID, DeliveryAdviceDetails.DeliveryAdviceID AS JournalPrimaryID, DeliveryAdviceDetails.EntryDate AS JournalDate, '' AS JournalReference, '' AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       DeliveryAdviceDetails.CommodityID, ListWarehouseName.WarehouseGroupID, ListWarehouseName.WarehouseClassID, DeliveryAdviceDetails.WarehouseID, 0 AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, ROUND(DeliveryAdviceDetails.Quantity + DeliveryAdviceDetails.FreeQuantity - DeliveryAdviceDetails.QuantityIssue - DeliveryAdviceDetails.FreeQuantityIssue, " + (int)GlobalEnums.rndQuantity + ") AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, 0 AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        DeliveryAdviceDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListWarehouseName ON DeliveryAdviceDetails.WarehouseID = ListWarehouseName.WarehouseID " + "\r\n";
            queryString = queryString + "                           WHERE       DeliveryAdviceDetails.EntryDate <= @LocalDateTo AND DeliveryAdviceDetails.Approved = 1 AND DeliveryAdviceDetails.InActive = 0 AND DeliveryAdviceDetails.InActivePartial = 0 AND DeliveryAdviceDetails.InActiveIssue = 0 AND ROUND(DeliveryAdviceDetails.Quantity + DeliveryAdviceDetails.FreeQuantity - DeliveryAdviceDetails.QuantityIssue - DeliveryAdviceDetails.FreeQuantityIssue, " + (int)GlobalEnums.rndQuantity + ") > 0 " + this.SPSKUInventoryJournalWarehouseFilter("ListWarehouseName", isWarehouseFilter) + this.SPSKUInventoryJournalCommodityFilter("DeliveryAdviceDetails", isCommodityFilter) + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      " + (int)GlobalEnums.NmvnTaskID.DeliveryAdvice + " AS NMVNTaskID, DeliveryAdvices.DeliveryAdviceID AS JournalPrimaryID, DeliveryAdvices.EntryDate AS JournalDate, '' AS JournalReference, '' AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       GoodsIssueDetails.CommodityID, ListWarehouseName.WarehouseGroupID, ListWarehouseName.WarehouseClassID, GoodsIssueDetails.WarehouseID, 0 AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, 0 AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        DeliveryAdvices INNER JOIN " + "\r\n";
            queryString = queryString + "                                       GoodsIssueDetails ON DeliveryAdvices.DeliveryAdviceID = GoodsIssueDetails.DeliveryAdviceID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListWarehouseName ON GoodsIssueDetails.WarehouseID = ListWarehouseName.WarehouseID " + "\r\n";
            queryString = queryString + "                           WHERE       DeliveryAdvices.EntryDate <= @LocalDateTo AND GoodsIssueDetails.EntryDate > @LocalDateTo " + this.SPSKUInventoryJournalWarehouseFilter("GoodsIssueDetails", isWarehouseFilter) + this.SPSKUInventoryJournalCommodityFilter("GoodsIssueDetails", isCommodityFilter) + "\r\n";
            // --ON-OUTPUT  //END




            // --ON-TRANSFERADVICE-OUT  //BEGIN
            queryString = queryString + "                           UNION ALL" + "\r\n";
            queryString = queryString + "                           SELECT      " + (int)ENMVNTaskID.ESKUTransferAdvice + " AS NMVNTaskID, SKUTransferAdviceMaster.SKUTransferAdviceID AS JournalPrimaryID, SKUTransferAdviceMaster.SKUTransferAdviceDate AS JournalDate, SKUTransferAdviceMaster.Reference AS JournalReference, SKUTransferAdviceMaster.Description AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       SKUTransferAdviceDetail.CommodityID, ListWarehouseName.WarehouseGroupID, ListWarehouseName.WarehouseClassID, SKUTransferAdviceDetail.WarehouseID, SKUTransferAdviceDetail.WarehouseInID AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, 0 AS QuantityOnAdvice, ROUND(SKUTransferAdviceDetail.Quantity - SKUTransferAdviceDetail.QuantityInput, " + (int)GlobalEnums.rndQuantity + ") AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, 0 AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        SKUTransferAdviceMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                       SKUTransferAdviceDetail ON SKUTransferAdviceMaster.SKUTransferAdviceID = SKUTransferAdviceDetail.SKUTransferAdviceID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListWarehouseName ON SKUTransferAdviceDetail.WarehouseID = ListWarehouseName.WarehouseID " + "\r\n";
            queryString = queryString + "                           WHERE       SKUTransferAdviceMaster.InActive = 0 AND SKUTransferAdviceMaster.SKUTransferAdviceDate <= @LocalDateTo AND ROUND(SKUTransferAdviceDetail.Quantity - SKUTransferAdviceDetail.QuantityInput, " + (int)GlobalEnums.rndQuantity + ") > 0 " + this.SPSKUInventoryJournalWarehouseFilter("SKUTransferAdviceDetail", isWarehouseFilter) + this.SPSKUInventoryJournalCommodityFilter("SKUTransferAdviceDetail", isCommodityFilter) + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      " + (int)ENMVNTaskID.ESKUTransferAdvice + " AS NMVNTaskID, SKUTransferAdviceMaster.SKUTransferAdviceID AS JournalPrimaryID, SKUTransferAdviceMaster.SKUTransferAdviceDate AS JournalDate, SKUTransferAdviceMaster.Reference AS JournalReference, SKUTransferAdviceMaster.Description AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       SKUTransferDetail.CommodityID, ListWarehouseName.WarehouseGroupID, ListWarehouseName.WarehouseClassID, SKUTransferDetail.WarehouseID, SKUTransferDetail.WarehouseInID AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, 0 AS QuantityOnAdvice, SKUTransferDetail.Quantity AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        SKUTransferAdviceMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                       SKUTransferDetail ON SKUTransferAdviceMaster.SKUTransferAdviceID = SKUTransferDetail.SKUTransferAdviceID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       SKUTransferMaster ON SKUTransferDetail.SKUTransferID = SKUTransferMaster.SKUTransferID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListWarehouseName ON SKUTransferDetail.WarehouseID = ListWarehouseName.WarehouseID " + "\r\n";
            queryString = queryString + "                           WHERE       SKUTransferAdviceMaster.SKUTransferAdviceDate <= @LocalDateTo AND SKUTransferMaster.SKUTransferDate > @LocalDateTo " + this.SPSKUInventoryJournalWarehouseFilter("SKUTransferDetail", isWarehouseFilter) + this.SPSKUInventoryJournalCommodityFilter("SKUTransferDetail", isCommodityFilter) + "\r\n";
            // --ON-TRANSFERADVICE-OUT  //END

            // --ON-TRANSFERADVICE-IN  //BEGIN
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      " + (int)ENMVNTaskID.ESKUTransferAdvice + " AS NMVNTaskID, SKUTransferAdviceMaster.SKUTransferAdviceID AS JournalPrimaryID, SKUTransferAdviceMaster.SKUTransferAdviceDate AS JournalDate, SKUTransferAdviceMaster.Reference AS JournalReference, SKUTransferAdviceMaster.Description AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       SKUTransferAdviceDetail.CommodityID, ListWarehouseName.WarehouseGroupID, ListWarehouseName.WarehouseClassID, SKUTransferAdviceDetail.WarehouseInID AS WarehouseID, SKUTransferAdviceDetail.WarehouseID AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, 0 AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, ROUND(SKUTransferAdviceDetail.Quantity - SKUTransferAdviceDetail.QuantityInput, " + (int)GlobalEnums.rndQuantity + ") AS QuantityOnTransferAdviceIn, 0 AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        SKUTransferAdviceMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                       SKUTransferAdviceDetail ON SKUTransferAdviceMaster.SKUTransferAdviceID = SKUTransferAdviceDetail.SKUTransferAdviceID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListWarehouseName ON SKUTransferAdviceDetail.WarehouseInID = ListWarehouseName.WarehouseID " + "\r\n";
            queryString = queryString + "                           WHERE       SKUTransferAdviceMaster.InActive = 0 AND SKUTransferAdviceMaster.SKUTransferAdviceDate <= @LocalDateTo AND ROUND(SKUTransferAdviceDetail.Quantity - SKUTransferAdviceDetail.QuantityInput, " + (int)GlobalEnums.rndQuantity + ") > 0 " + this.SPSKUInventoryJournalWarehouseFilter("ListWarehouseName", isWarehouseFilter) + this.SPSKUInventoryJournalCommodityFilter("SKUTransferAdviceDetail", isCommodityFilter) + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      " + (int)ENMVNTaskID.ESKUTransferAdvice + " AS NMVNTaskID, SKUTransferAdviceMaster.SKUTransferAdviceID AS JournalPrimaryID, SKUTransferAdviceMaster.SKUTransferAdviceDate AS JournalDate, SKUTransferAdviceMaster.Reference AS JournalReference, SKUTransferAdviceMaster.Description AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       SKUTransferDetail.CommodityID, ListWarehouseName.WarehouseGroupID, ListWarehouseName.WarehouseClassID, SKUTransferDetail.WarehouseInID AS WarehouseID, SKUTransferDetail.WarehouseID AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, 0 AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, SKUTransferDetail.Quantity AS QuantityOnTransferAdviceIn, 0 AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        SKUTransferAdviceMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                       SKUTransferDetail ON SKUTransferAdviceMaster.SKUTransferAdviceID = SKUTransferDetail.SKUTransferAdviceID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       SKUTransferMaster ON SKUTransferDetail.SKUTransferID = SKUTransferMaster.SKUTransferID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListWarehouseName ON SKUTransferDetail.WarehouseInID = ListWarehouseName.WarehouseID " + "\r\n";
            queryString = queryString + "                           WHERE       SKUTransferAdviceMaster.SKUTransferAdviceDate <= @LocalDateTo AND SKUTransferMaster.SKUTransferDate > @LocalDateTo " + this.SPSKUInventoryJournalWarehouseFilter("ListWarehouseName", isWarehouseFilter) + this.SPSKUInventoryJournalCommodityFilter("SKUTransferDetail", isCommodityFilter) + "\r\n";
            // --ON-TRANSFERADVICE-IN  //END


            // --ON-SHIP   //BEGIN
            //--ENMVNTaskID.EProductionOrder (DAY CHI MOI LA TON DON, THUC TE CON TON SEMI, PACKING WAREHOUSE, ON-PACKING,...

            //TON DON.BEGIN--CAU LENH XAC DINH TON DON HERE HOAN TOAN GIONG CAU LENH XAC DINH SO TON DON DAU KY/ CUOI KY CUA lfSPProductionOrderJournalBUILDSQL
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      " + (int)ENMVNTaskID.EProductionOrder + " AS NMVNTaskID, ProductionOrderMaster.ProductionOrderID AS JournalPrimaryID, ProductionOrderMaster.ProductionOrderDate AS JournalDate, ProductionOrderMaster.Reference AS JournalReference, ProductionOrderMaster.Description AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       ProductionOrderDetail.CommodityID, ProductionOrderMaster.WarehouseGroupID, " + (int)EWarehouseClassID.EWarehouseClassL1 + " AS WarehouseClassID, 0 AS WarehouseID, 0 AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, 0 AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, ROUND(ProductionOrderDetail.Quantity - ProductionOrderDetail.QuantityInput, " + (int)GlobalEnums.rndQuantity + ") AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        ProductionOrderMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ProductionOrderDetail ON ProductionOrderMaster.ProductionOrderID = ProductionOrderDetail.ProductionOrderID " + "\r\n";
            queryString = queryString + "                           WHERE       ProductionOrderMaster.ProductionOrderDate <= @LocalDateTo AND ROUND(ProductionOrderDetail.Quantity - ProductionOrderDetail.QuantityInput, " + (int)GlobalEnums.rndQuantity + ") > 0 AND ProductionOrderDetail.InActive = 0 AND @WarehouseClassFilter = 1 " + this.SPSKUInventoryJournalCommodityFilter("ProductionOrderDetail", isCommodityFilter) + this.SPSKUInventoryJournalWarehouseGroupFilter("ProductionOrderMaster", isWarehouseGroupFilter) + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      " + (int)ENMVNTaskID.EProductionOrder + " AS NMVNTaskID, ProductionOrderMaster.ProductionOrderID AS JournalPrimaryID, ProductionOrderMaster.ProductionOrderDate AS JournalDate, ProductionOrderMaster.Reference AS JournalReference, ProductionOrderMaster.Description AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       ProductionSemiDetail.CommodityID, ProductionOrderMaster.WarehouseGroupID, " + (int)EWarehouseClassID.EWarehouseClassL1 + " AS WarehouseClassID, 0 AS WarehouseID, 0 AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, 0 AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, ProductionSemiDetail.Quantity AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        ProductionSemiMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ProductionSemiDetail ON ProductionSemiMaster.ProductionSemiID = ProductionSemiDetail.ProductionSemiID AND ProductionSemiDetail.WarehouseClassID IN (" + (int)EWarehouseClassID.EWarehouseClassL1 + ", " + (int)EWarehouseClassID.EWarehouseClassLD + ", " + (int)EWarehouseClassID.EWarehouseClassL5 + ") INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ProductionOrderMaster ON ProductionSemiDetail.ProductionOrderID = ProductionOrderMaster.ProductionOrderID " + "\r\n";
            queryString = queryString + "                           WHERE       ProductionOrderMaster.ProductionOrderDate <= @LocalDateTo AND ProductionSemiMaster.ProductionSemiDate > @LocalDateTo AND @WarehouseClassFilter = 1 " + this.SPSKUInventoryJournalCommodityFilter("ProductionSemiDetail", isCommodityFilter) + this.SPSKUInventoryJournalWarehouseGroupFilter("ProductionOrderMaster", isWarehouseGroupFilter) + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      " + (int)ENMVNTaskID.EProductionOrder + " AS NMVNTaskID, ProductionOrderMaster.ProductionOrderID AS JournalPrimaryID, ProductionOrderMaster.ProductionOrderDate AS JournalDate, ProductionOrderMaster.Reference AS JournalReference, ProductionOrderMaster.Description AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       ProductionPackDetail.CommodityID, ProductionOrderMaster.WarehouseGroupID, " + (int)EWarehouseClassID.EWarehouseClassL1 + " AS WarehouseClassID, 0 AS WarehouseID, 0 AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, 0 AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, CASE WHEN ProductionPackDetail.WarehouseClassID IN (" + (int)EWarehouseClassID.EWarehouseClassL2 + ", " + (int)EWarehouseClassID.EWarehouseClassL3 + ") THEN -ProductionPackDetail.Quantity ELSE ProductionPackDetail.Quantity END AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        ProductionPackMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ProductionPackDetail ON ProductionPackMaster.ProductionPackID = ProductionPackDetail.ProductionPackID AND (         (ProductionPackDetail.WarehouseClassID IN (" + (int)EWarehouseClassID.EWarehouseClassL2 + ", " + (int)EWarehouseClassID.EWarehouseClassL3 + ") AND ProductionPackDetail.WarehouseClassSemiID IN (" + (int)EWarehouseClassID.EWarehouseClassL1 + ", " + (int)EWarehouseClassID.EWarehouseClassLD + ", " + (int)EWarehouseClassID.EWarehouseClassL5 + ")) OR (ProductionPackDetail.WarehouseClassID IN (" + (int)EWarehouseClassID.EWarehouseClassL1 + ", " + (int)EWarehouseClassID.EWarehouseClassLD + ", " + (int)EWarehouseClassID.EWarehouseClassL5 + ") AND ProductionPackDetail.WarehouseClassSemiID IN (" + (int)EWarehouseClassID.EWarehouseClassL2 + ", " + (int)EWarehouseClassID.EWarehouseClassL3 + "))         ) INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ProductionOrderMaster ON ProductionPackDetail.ProductionOrderID = ProductionOrderMaster.ProductionOrderID " + "\r\n";
            queryString = queryString + "                           WHERE       ProductionOrderMaster.ProductionOrderDate <= @LocalDateTo AND ProductionPackMaster.ProductionPackDate > @LocalDateTo AND @WarehouseClassFilter = 1 " + this.SPSKUInventoryJournalCommodityFilter("ProductionPackDetail", isCommodityFilter) + this.SPSKUInventoryJournalWarehouseGroupFilter("ProductionOrderMaster", isWarehouseGroupFilter) + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      " + (int)ENMVNTaskID.EProductionOrder + " AS NMVNTaskID, ProductionOrderMaster.ProductionOrderID AS JournalPrimaryID, ProductionOrderMaster.ProductionOrderDate AS JournalDate, ProductionOrderMaster.Reference AS JournalReference, ProductionOrderMaster.Description AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       ProductionRankDetail.CommodityID, ProductionOrderMaster.WarehouseGroupID, " + (int)EWarehouseClassID.EWarehouseClassL1 + " AS WarehouseClassID, 0 AS WarehouseID, 0 AS WarehouseOutID, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityBegin, 0 AS QuantityInputPRO, 0 AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputRTN, 0 AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputINV, 0 AS QuantityOutputGoodsIssue, 0 AS QuantityOutputTRF, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, " + "\r\n";
            queryString = queryString + "                                       0 AS QuantityOnTransfer, 0 AS QuantityOnAdvice, 0 AS QuantityOnTransferAdviceOut, 0 AS QuantityOnTransferAdviceIn, CASE WHEN ProductionRankDetail.WarehouseClassID IN (" + (int)EWarehouseClassID.EWarehouseClassL2 + ", " + (int)EWarehouseClassID.EWarehouseClassL3 + ") THEN -ProductionRankDetail.Quantity ELSE ProductionRankDetail.Quantity END AS QuantityOnProduction " + "\r\n";
            queryString = queryString + "                           FROM        ProductionRankMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ProductionRankDetail ON ProductionRankMaster.ProductionRankID = ProductionRankDetail.ProductionRankID AND (         (ProductionRankDetail.WarehouseClassID IN (" + (int)EWarehouseClassID.EWarehouseClassL2 + ", " + (int)EWarehouseClassID.EWarehouseClassL3 + ") AND ProductionRankDetail.WarehouseClassPackID IN (" + (int)EWarehouseClassID.EWarehouseClassL1 + ", " + (int)EWarehouseClassID.EWarehouseClassLD + ", " + (int)EWarehouseClassID.EWarehouseClassL5 + ")) OR (ProductionRankDetail.WarehouseClassID IN (" + (int)EWarehouseClassID.EWarehouseClassL1 + ", " + (int)EWarehouseClassID.EWarehouseClassLD + ", " + (int)EWarehouseClassID.EWarehouseClassL5 + ") AND ProductionRankDetail.WarehouseClassPackID IN (" + (int)EWarehouseClassID.EWarehouseClassL2 + ", " + (int)EWarehouseClassID.EWarehouseClassL3 + "))         ) INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ProductionOrderMaster ON ProductionRankDetail.ProductionOrderID = ProductionOrderMaster.ProductionOrderID " + "\r\n";
            queryString = queryString + "                           WHERE       ProductionOrderMaster.ProductionOrderDate <= @LocalDateTo AND ProductionRankMaster.ProductionRankDate > @LocalDateTo AND @WarehouseClassFilter = 1 " + this.SPSKUInventoryJournalCommodityFilter("ProductionRankDetail", isCommodityFilter) + this.SPSKUInventoryJournalWarehouseGroupFilter("ProductionOrderMaster", isWarehouseGroupFilter) + "\r\n";

            //TON DON.END--CAU LENH XAC DINH TON DON HERE HOAN TOAN GIONG CAU LENH XAC DINH SO TON DON DAU KY/ CUOI KY CUA lfSPProductionOrderJournalBUILDSQL
            // --ON-SHIP   //END

            return queryString;
        }


        private string SPSKUInventoryJournalWarehouseFilter(bool isWarehouseFilter)
        { return this.SPSKUInventoryJournalWarehouseFilter("", isWarehouseFilter); }

        private string SPSKUInventoryJournalWarehouseFilter(string tableName, bool isWarehouseFilter)
        {
            return isWarehouseFilter ? " AND " + (tableName != "" ? tableName + "." : "") + "WarehouseID IN (SELECT WarehouseID FROM @WarehouseFilter) " : "";
        }

        private string SPSKUInventoryJournalCommodityFilter(bool isCommodityFilter)
        { return this.SPSKUInventoryJournalCommodityFilter("", isCommodityFilter); }

        private string SPSKUInventoryJournalCommodityFilter(string tableName, bool isCommodityFilter)
        {
            return isCommodityFilter ? " AND " + (tableName != "" ? tableName + "." : "") + "CommodityID IN (SELECT CommodityID FROM @CommodityFilter) " : (tableName == "GoodsIssueDetails" ? " AND " + (tableName != "" ? tableName + "." : "") + "CommodityTypeID IN (" + (int)GlobalEnums.CommodityTypeID.Parts + ", " + (int)GlobalEnums.CommodityTypeID.Consumables + ") " : "");
        }

        private string SPSKUInventoryJournalWarehouseGroupFilter(bool isWarehouseGroupFilter)
        { return this.SPSKUInventoryJournalWarehouseGroupFilter("", isWarehouseGroupFilter); }

        private string SPSKUInventoryJournalWarehouseGroupFilter(string tableName, bool isWarehouseGroupFilter)
        {
            return isWarehouseGroupFilter ? " AND " + (tableName != "" ? tableName + "." : "") + "WarehouseGroupID IN (SELECT WarehouseGroupID FROM @WarehouseGroupFilter) " : "";
        }


        public void WarehouseJournalTESTSummary()
        {
            string queryString = " @FromDate DateTime, @ToDate DateTime " + "\r\n";

            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       " + this.GET_WarehouseJournal_BUILD_SQL_VINHLONG("@WarehouseJournalTable", "@FromDate", "@ToDate", "''", "''", "1", "1") + "\r\n";
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
