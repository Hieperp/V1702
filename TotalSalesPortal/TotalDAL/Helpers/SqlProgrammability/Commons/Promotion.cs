using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;


namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Promotion
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public Promotion(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetPromotionIndexes();

            this.GetPromotionByCustomers();

            this.GetPromotionCustomerCategories();

            this.AddPromotionCustomerCategories();
            this.RemovePromotionCustomerCategories();

            this.AddPromotionCustomers();
            this.RemovePromotionCustomers();

            this.PromotionSaveRelative();

            this.PromotionApproved();
            this.PromotionEditable();
            this.PromotionDeletable();
            this.PromotionVoidable();

            this.PromotionToggleApproved();
            this.PromotionToggleVoid();

            this.PromotionInitReference();
        }

        private void GetPromotionIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Promotions.PromotionID, CAST(Promotions.EntryDate AS DATE) AS EntryDate, Promotions.Reference, CommodityBrands.CommodityBrandID, CommodityBrands.Name AS Brand, ISNULL(CustomerCategories.Name, N'Thị trường') AS Category, " + "\r\n";
            queryString = queryString + "                   Promotions.Code, Promotions.Name, Promotions.StartDate, Promotions.EndDate, Promotions.DiscountPercent, Promotions.ControlFreeQuantity, Promotions.ApplyToAllCustomers, Promotions.ApplyToAllCommodities, Promotions.ApplyToTradeDiscount, Promotions.ApplyToSales, Promotions.ApplyToReturns, Promotions.Remarks, Promotions.Approved, Promotions.InActive " + "\r\n";

            queryString = queryString + "       FROM        Promotions " + "\r\n";
            queryString = queryString + "                   INNER JOIN  CommodityBrands ON Promotions.EndDate >= GetDate() AND Promotions.CommodityBrandID = CommodityBrands.CommodityBrandID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN   (SELECT PromotionID, MIN(CustomerCategoryID) AS CustomerCategoryID FROM PromotionCustomerCategories GROUP BY PromotionID) AS DERIVEDPromotionCustomerCategories ON Promotions.PromotionID = DERIVEDPromotionCustomerCategories.PromotionID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN   CustomerCategories ON DERIVEDPromotionCustomerCategories.CustomerCategoryID = CustomerCategories.CustomerCategoryID  " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPromotionIndexes", queryString);
        }


        private void GetPromotionByCustomers()
        {
            string queryString;

            string queryWhere = "((@ApplyToSalesVersusReturns >= 0 AND Promotions.ApplyToSales = 1) OR (@ApplyToSalesVersusReturns <= 0 AND Promotions.ApplyToReturns = 1)) AND Promotions.InActive = 0 AND (Promotions.ApplyToTradeDiscount = @FilterApplyToTradeDiscount OR @FilterApplyToTradeDiscount IS NULL) AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate ";

            queryString = " @CustomerID int, @ApplyToSalesVersusReturns int, @FilterApplyToTradeDiscount int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@CustomerID IS NULL) ";
            queryString = queryString + "           BEGIN ";
            queryString = queryString + "               SELECT * FROM Promotions WHERE " + queryWhere + "\r\n";
            queryString = queryString + "           END ";
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           BEGIN ";

            queryString = queryString + "               DECLARE @CustomerCategoryID int " + "\r\n";
            queryString = queryString + "               SELECT  @CustomerCategoryID = CustomerCategoryID FROM Customers WHERE CustomerID = @CustomerID "; //GET @CustomerCategoryID OF @CustomerID

            queryString = queryString + "               SELECT * FROM Promotions WHERE PromotionID IN " + "\r\n";
            queryString = queryString + "                  (SELECT Promotions.PromotionID FROM Promotions WHERE " + queryWhere + " AND Promotions.ApplyToAllCustomers = 1 " + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   SELECT Promotions.PromotionID FROM Promotions INNER JOIN PromotionCustomers ON " + queryWhere + " AND PromotionCustomers.CustomerID = @CustomerID AND Promotions.PromotionID = PromotionCustomers.PromotionID " + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   SELECT Promotions.PromotionID FROM Promotions INNER JOIN PromotionCustomerCategories ON " + queryWhere + " AND Promotions.PromotionID = PromotionCustomerCategories.PromotionID INNER JOIN AncestorCustomerCategories ON PromotionCustomerCategories.CustomerCategoryID = AncestorCustomerCategories.AncestorID AND AncestorCustomerCategories.CustomerCategoryID = @CustomerCategoryID " + "\r\n";
            queryString = queryString + "                  )" + "\r\n";
            queryString = queryString + "               ORDER BY ApplyToAllCommodities DESC, Code, Name ";
            queryString = queryString + "           END ";
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPromotionByCustomers", queryString);
        }


        private void GetPromotionCustomerCategories()
        {
            string queryString;

            queryString = " @PromotionID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT * FROM CustomerCategories WHERE CustomerCategoryID IN (SELECT CustomerCategoryID FROM PromotionCustomerCategories WHERE PromotionID = @PromotionID) " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPromotionCustomerCategories", queryString);
        }



        private void AddPromotionCustomerCategories()
        {
            string queryString;

            queryString = " @PromotionID int, @CustomerCategoryID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       INSERT INTO     PromotionCustomerCategories (PromotionID, CustomerCategoryID, InActive) " + "\r\n";
            queryString = queryString + "       VALUES          (@PromotionID, @CustomerCategoryID, 0) " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("AddPromotionCustomerCategories", queryString);
        }

        private void RemovePromotionCustomerCategories()
        {
            string queryString;

            queryString = " @PromotionID int, @CustomerCategoryID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       DELETE FROM     PromotionCustomerCategories " + "\r\n";
            queryString = queryString + "       WHERE           PromotionID = @PromotionID AND CustomerCategoryID = @CustomerCategoryID " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("RemovePromotionCustomerCategories", queryString);

        }



        private void AddPromotionCustomers()
        {
            string queryString;

            queryString = " @PromotionID int, @CustomerID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       INSERT INTO     PromotionCustomers (PromotionID, CustomerID, InActive) " + "\r\n";
            queryString = queryString + "       VALUES          (@PromotionID, @CustomerID, 0) " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("AddPromotionCustomers", queryString);
        }

        private void RemovePromotionCustomers()
        {
            string queryString;

            queryString = " @PromotionID int, @CustomerID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       DELETE FROM     PromotionCustomers " + "\r\n";
            queryString = queryString + "       WHERE           PromotionID = @PromotionID AND CustomerID = @CustomerID " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("RemovePromotionCustomers", queryString);

        }




        private void PromotionSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           IF (SELECT COUNT(*) FROM PromotionCommodityCodeParts WHERE PromotionID = @EntityID) = 0 " + "\r\n";
            queryString = queryString + "               INSERT INTO     PromotionCommodityBrands (PromotionID, CommodityBrandID, InActive) " + "\r\n";
            queryString = queryString + "               SELECT          PromotionID, CommodityBrandID, 0 FROM Promotions WHERE PromotionID = @EntityID " + "\r\n";
            queryString = queryString + "       END " + "\r\n";
            queryString = queryString + "   ELSE " + "\r\n";
            queryString = queryString + "           DELETE FROM PromotionCommodityBrands WHERE PromotionID = @EntityID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("PromotionSaveRelative", queryString);
        }


        private void PromotionApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PromotionID FROM Promotions WHERE PromotionID = @EntityID AND Approved = 1";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("PromotionApproved", queryArray);
        }


        private void PromotionEditable()
        {
            string[] queryArray = new string[6];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PromotionID FROM Promotions WHERE PromotionID = @EntityID AND (InActive = 1 OR StartDate <= GETDATE()) "; //Don't allow approve after void

            queryArray[1] = " SELECT TOP 1 @FoundEntity = SalesOrderID FROM SalesOrders WHERE PromotionID = @EntityID OR TradePromotionID = @EntityID ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = DeliveryAdviceID FROM DeliveryAdvices WHERE PromotionID = @EntityID OR TradePromotionID = @EntityID ";
            queryArray[3] = " SELECT TOP 1 @FoundEntity = SalesReturnID FROM SalesReturns WHERE PromotionID = @EntityID OR TradePromotionID = @EntityID ";
            queryArray[4] = " SELECT TOP 1 @FoundEntity = GoodsIssueID FROM GoodsIssues WHERE TradePromotionID = @EntityID ";
            queryArray[5] = " SELECT TOP 1 @FoundEntity = AccountInvoiceID FROM AccountInvoices WHERE TradePromotionID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("PromotionEditable", queryArray);
        }

        private void PromotionDeletable()
        {
            string[] queryArray = new string[0];

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("PromotionDeletable", queryArray);
        }

        private void PromotionVoidable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = PromotionID FROM Promotions WHERE PromotionID = @EntityID AND Approved = 0"; //Must approve in order to allow void

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("PromotionVoidable", queryArray);
        }


        private void PromotionToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      Promotions  SET Approved = @Approved, ApprovedDate = GetDate(), InActive = 0, InActiveDate = NULL WHERE PromotionID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT <> 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, 'hủy', '')  + ' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("PromotionToggleApproved", queryString);
        }

        private void PromotionToggleVoid()
        {
            string queryString = " @EntityID int, @InActive bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      Promotions  SET InActive = @InActive, InActiveDate = GetDate() WHERE PromotionID = @EntityID AND InActive = ~@InActive" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT <> 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActive = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            this.totalSalesPortalEntities.CreateStoredProcedure("PromotionToggleVoid", queryString);
        }



        private void PromotionInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("Promotions", "PromotionID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.Promotion));
            this.totalSalesPortalEntities.CreateTrigger("PromotionInitReference", simpleInitReference.CreateQuery());
        }

    }
}
