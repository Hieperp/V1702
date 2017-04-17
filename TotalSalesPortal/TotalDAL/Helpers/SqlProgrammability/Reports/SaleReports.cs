using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Reports
{
    public class SaleReports
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public SaleReports(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.DeliveryAdviceJournal();
            this.GoodsIssueJournal();
            this.GoodsIssueBalance();
            this.SalesJournal();

            this.HandlingUnitSheet();
            this.GoodsDeliverySheet();
        }

        private void DeliveryAdviceJournal()
        {
            string queryString;

            queryString = " @DeliveryAdviceID int, @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalDeliveryAdviceID int      SET @LocalDeliveryAdviceID = @DeliveryAdviceID" + "\r\n";
            queryString = queryString + "       DECLARE     @LocalFromDate DateTime         SET @LocalFromDate = @FromDate" + "\r\n";
            queryString = queryString + "       DECLARE     @LocalToDate DateTime           SET @LocalToDate = @ToDate" + "\r\n";

            queryString = queryString + "       SELECT      DeliveryAdvices.DeliveryAdviceID, DeliveryAdvices.EntryDate, DeliveryAdvices.Reference, DeliveryAdvices.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, DeliveryAdvices.ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, " + "\r\n";
            queryString = queryString + "                   DeliveryAdviceDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, " + "\r\n";
            queryString = queryString + "                   DeliveryAdviceDetails.Quantity, DeliveryAdviceDetails.QuantityIssue, DeliveryAdviceDetails.Quantity - DeliveryAdviceDetails.QuantityIssue AS QuantityRemains, DeliveryAdviceDetails.FreeQuantity, DeliveryAdviceDetails.FreeQuantityIssue, DeliveryAdviceDetails.FreeQuantity - DeliveryAdviceDetails.FreeQuantityIssue AS FreeQuantityRemains, DeliveryAdviceDetails.GrossAmount, " + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails.GoodsIssueID, GoodsIssueDetails.EntryDate AS GoodsIssueEntryDate, GoodsIssueDetails.Quantity AS GoodsIssueQuantity, GoodsIssueDetails.FreeQuantity AS GoodsIssueFreeQuantity, " + "\r\n";
            queryString = queryString + "                   DeliveryAdviceDetails.InActive, DeliveryAdviceDetails.InActivePartial, DeliveryAdviceDetails.InActiveIssue, ISNULL(VoidDeliveryAdvices.Name, '') + IIF(DeliveryAdvices.VoidTypeID <> 0 AND DeliveryAdviceDetails.VoidTypeID <> 0, ', ', '') + ISNULL(VoidDeliveryAdviceDetails.Name, '') AS VoidDeliveryAdviceName, VoidGoodsIssues.Name AS VoidGoodsIssueName " + "\r\n";

            queryString = queryString + "       FROM        DeliveryAdvices " + "\r\n";
            queryString = queryString + "                   INNER JOIN DeliveryAdviceDetails ON (DeliveryAdvices.DeliveryAdviceID = @LocalDeliveryAdviceID OR (@LocalDeliveryAdviceID = 0 AND DeliveryAdvices.EntryDate >= @LocalFromDate AND DeliveryAdvices.EntryDate <= @LocalToDate)) AND DeliveryAdvices.DeliveryAdviceID = DeliveryAdviceDetails.DeliveryAdviceID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON DeliveryAdvices.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers AS Receivers ON DeliveryAdvices.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON DeliveryAdviceDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN GoodsIssueDetails ON DeliveryAdviceDetails.DeliveryAdviceDetailID = GoodsIssueDetails.DeliveryAdviceDetailID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes AS VoidDeliveryAdvices ON DeliveryAdvices.VoidTypeID = VoidDeliveryAdvices.VoidTypeID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes AS VoidDeliveryAdviceDetails ON DeliveryAdviceDetails.VoidTypeID = VoidDeliveryAdviceDetails.VoidTypeID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes AS VoidGoodsIssues ON GoodsIssueDetails.VoidTypeID = VoidGoodsIssues.VoidTypeID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("DeliveryAdviceJournal", queryString);
        }



        private void GoodsDeliverySheet()
        {
            string queryString;

            queryString = " @GoodsDeliveryID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalGoodsDeliveryID int      SET @LocalGoodsDeliveryID = @GoodsDeliveryID" + "\r\n";

            queryString = queryString + "       SELECT      GoodsDeliveries.GoodsDeliveryID, GoodsDeliveries.EntryDate, GoodsDeliveries.Reference, Vehicles.Name AS VehicleName, Drivers.Name AS DriverName, Collectors.Name AS CollectorName, HandlingUnits.CustomerID, Customers.Name AS CustomerName, HandlingUnits.ReceiverID, Receivers.Name AS ReceiverName, CASE WHEN HandlingUnits.CustomerID = HandlingUnits.ReceiverID THEN '' ELSE Receivers.Name + ', ' END + HandlingUnits.ShippingAddress AS ShippingAddress, " + "\r\n";
            queryString = queryString + "                   HandlingUnits.GoodsIssueReferences, HandlingUnits.PackingMaterialID, HandlingUnits.TotalQuantity AS Quantity, HandlingUnits.TotalWeight AS Weight, HandlingUnits.RealWeight " + "\r\n";
            queryString = queryString + "       FROM        GoodsDeliveries " + "\r\n";
            queryString = queryString + "                   INNER JOIN HandlingUnits ON GoodsDeliveries.GoodsDeliveryID = @LocalGoodsDeliveryID AND GoodsDeliveries.GoodsDeliveryID = HandlingUnits.GoodsDeliveryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON HandlingUnits.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers AS Receivers ON HandlingUnits.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Vehicles ON GoodsDeliveries.VehicleID = Vehicles.VehicleID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees AS Drivers ON GoodsDeliveries.DriverID = Drivers.EmployeeID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees AS Collectors ON GoodsDeliveries.CollectorID = Collectors.EmployeeID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GoodsDeliverySheet", queryString);
        }


        private void HandlingUnitSheet()
        {
            string queryString;

            queryString = " @HandlingUnitID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalHandlingUnitID int      SET @LocalHandlingUnitID = @HandlingUnitID" + "\r\n";

            queryString = queryString + "       SELECT      HandlingUnits.HandlingUnitID, HandlingUnits.EntryDate, HandlingUnits.GoodsIssueReferences, HandlingUnits.Identification, HandlingUnits.CountIdentification, HandlingUnits.TotalWeight, HandlingUnits.RealWeight, PackingMaterials.PrintedLabel AS PackingMaterialPrintedLabel, HandlingUnits.Description, HandlingUnits.Remarks, " + "\r\n";
            queryString = queryString + "                   HandlingUnits.ShippingAddress, Customers.Name AS ReceiverName, Customers.CustomerCategoryID AS ReceiverCategoryID, CustomerCategories.Code AS ReceiverCategoryCode, Customers.VendorCode, Customers.VendorCategory, PackagingStaffs.Name AS PackagingStaffName, Commodities.Code, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, HandlingUnitDetails.Quantity " + "\r\n";
            queryString = queryString + "       FROM        HandlingUnits " + "\r\n";
            queryString = queryString + "                   INNER JOIN HandlingUnitDetails ON HandlingUnits.HandlingUnitID = @LocalHandlingUnitID AND HandlingUnits.HandlingUnitID = HandlingUnitDetails.HandlingUnitID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssueDetails ON HandlingUnitDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON HandlingUnits.ReceiverID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees PackagingStaffs ON HandlingUnits.PackagingStaffID = PackagingStaffs.EmployeeID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PackingMaterials ON HandlingUnits.PackingMaterialID = PackingMaterials.PackingMaterialID " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("HandlingUnitSheet", queryString);
        }



        private void GoodsIssueJournal()
        {
            string queryString;

            queryString = " @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime         SET @LocalFromDate = @FromDate" + "\r\n";
            queryString = queryString + "       DECLARE     @LocalToDate DateTime           SET @LocalToDate = @ToDate" + "\r\n";

            queryString = queryString + "       SELECT      GoodsIssues.GoodsIssueID, GoodsIssues.EntryDate, GoodsIssues.Reference, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, CustomerCategories.Name AS CustomerCategoryName, Employees.Name AS SalespersonName, GoodsIssues.Description, " + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails.GoodsIssueDetailID, GoodsIssueDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.Weight AS UnitWeight, " + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails.Quantity, GoodsIssueDetails.FreeQuantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.ListedAmount, GoodsIssueDetails.Amount, ROUND(GoodsIssueDetails.ListedAmount - GoodsIssueDetails.Amount, " + (int)GlobalEnums.rndAmount + ") AS DiscountAmount " + "\r\n";
            
            queryString = queryString + "       FROM        GoodsIssues " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssueDetails ON GoodsIssues.EntryDate >= @LocalFromDate AND GoodsIssues.EntryDate <= @LocalToDate AND GoodsIssues.GoodsIssueID = GoodsIssueDetails.GoodsIssueID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON GoodsIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN DeliveryAdvices ON GoodsIssues.DeliveryAdviceID = DeliveryAdvices.DeliveryAdviceID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees ON DeliveryAdvices.SalespersonID = Employees.EmployeeID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GoodsIssueJournal", queryString);
        }

        private void SalesJournal()
        {
            string queryString;

            queryString = " @FromDate DateTime, @ToDate DateTime, @CustomerID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime, @LocalToDate DateTime, @LocalCustomerID int         SET @LocalFromDate = @FromDate         SET @LocalToDate = @ToDate          SET @LocalCustomerID = @CustomerID " + "\r\n";

            queryString = queryString + "       IF (@LocalCustomerID > 0) ";
            queryString = queryString + "           " + this.SalesJournalSQL(true);
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           " + this.SalesJournalSQL(false);

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("SalesJournal", queryString);
        }

        private string SalesJournalSQL(bool isCustomerID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";


            queryString = queryString + "       SELECT      NmvnTaskID, CustomerID, CustomerCode, CustomerName, CustomerCategoryName, " + "\r\n";
            queryString = queryString + "                   EntryID, EntryDate, Reference, BalanceAmount, 0 AS TotalListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalFluctuationAmount " + "\r\n";
            queryString = queryString + "       FROM        dbo.GoodsIssueBalance(DATEADD(second, -1, @LocalFromDate), @LocalCustomerID) " + "\r\n";

            queryString = queryString + "       UNION ALL " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.GoodsIssue + " AS NmvnTaskID, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, CustomerCategories.Name AS CustomerCategoryName, " + "\r\n";
            queryString = queryString + "                   GoodsIssues.GoodsIssueID AS EntryID, GoodsIssues.EntryDate, GoodsIssues.Reference, 0 AS BalanceAmount, GoodsIssues.TotalListedGrossAmount, GoodsIssues.TotalGrossAmount, ROUND(GoodsIssues.TotalListedGrossAmount - GoodsIssues.TotalGrossAmount, " + (int)GlobalEnums.rndAmount + ") AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount,  0 AS TotalCashDiscount, 0 AS TotalFluctuationAmount " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssues " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON " + (isCustomerID ? "GoodsIssues.CustomerID = @LocalCustomerID AND" : "") + " GoodsIssues.EntryDate >= @LocalFromDate AND GoodsIssues.EntryDate <= @LocalToDate AND GoodsIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";

            queryString = queryString + "       UNION ALL " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.SalesReturn + " AS NmvnTaskID, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, CustomerCategories.Name AS CustomerCategoryName, " + "\r\n";
            queryString = queryString + "                   SalesReturns.SalesReturnID AS EntryID, SalesReturns.EntryDate, SalesReturns.Reference, 0 AS BalanceAmount, 0 AS TotalListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, SalesReturns.TotalGrossAmount AS TotalReturnAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount,  0 AS TotalCashDiscount, 0 AS TotalFluctuationAmount " + "\r\n";

            queryString = queryString + "       FROM        SalesReturns " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON " + (isCustomerID ? "SalesReturns.CustomerID = @LocalCustomerID AND" : "") + " SalesReturns.EntryDate >= @LocalFromDate AND SalesReturns.EntryDate <= @LocalToDate AND SalesReturns.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";



            queryString = queryString + "       UNION ALL " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.CreditNote + " AS NmvnTaskID, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, CustomerCategories.Name AS CustomerCategoryName, " + "\r\n";
            queryString = queryString + "                   CreditNotes.CreditNoteID AS EntryID, CreditNotes.EntryDate, CreditNotes.Reference, 0 AS BalanceAmount, 0 AS TotalListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, CreditNotes.TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount,  0 AS TotalCashDiscount, 0 AS TotalFluctuationAmount " + "\r\n";

            queryString = queryString + "       FROM        CreditNotes " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON " + (isCustomerID ? "CreditNotes.CustomerID = @LocalCustomerID AND" : "") + " CreditNotes.EntryDate >= @LocalFromDate AND CreditNotes.EntryDate <= @LocalToDate AND CreditNotes.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";



            queryString = queryString + "       UNION ALL " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.Receipt + " AS NmvnTaskID, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, CustomerCategories.Name AS CustomerCategoryName, " + "\r\n";
            queryString = queryString + "                   Receipts.ReceiptID AS EntryID, Receipts.EntryDate, Receipts.Reference, 0 AS BalanceAmount, 0 AS TotalListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalCreditAmount, CASE Receipts.MonetaryAccountID WHEN 3 THEN Receipts.TotalDepositAmount ELSE 0 END AS TotalCashReceiptAmount, CASE Receipts.MonetaryAccountID WHEN 3 THEN 0 ELSE Receipts.TotalDepositAmount END TotalBankTransferAmount, Receipts.TotalCashDiscount, Receipts.TotalFluctuationAmount " + "\r\n";

            queryString = queryString + "       FROM        Receipts " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON " + (isCustomerID ? "Receipts.CustomerID = @LocalCustomerID AND" : "") + " Receipts.ReceiptTypeID = " + GlobalReceiptTypeID.ReceiveMoney + " AND Receipts.EntryDate >= @LocalFromDate AND Receipts.EntryDate <= @LocalToDate AND Receipts.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";


            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }


        private void GoodsIssueBalance()
        {
            string queryString = " (@ToDate DateTime, @CustomerID int) " + "\r\n";
            queryString = queryString + " RETURNS @GoodsIssueBalances TABLE (NmvnTaskID int NOT NULL, EntryID int NOT NULL, EntryDate datetime NOT NULL, Reference nvarchar(10) NULL, CustomerID int NOT NULL, CustomerCode nvarchar(50) NOT NULL, CustomerName nvarchar(100) NOT NULL, CustomerCategoryName nvarchar(100) NOT NULL, EntryAmount decimal(18, 2) NOT NULL, BalanceAmount decimal(18, 2) NOT NULL) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalToDate DateTime, @LocalCustomerID int         SET @LocalToDate = @ToDate  SET @LocalCustomerID = @CustomerID " + "\r\n";


            queryString = queryString + "       IF (@LocalCustomerID > 0) ";
            queryString = queryString + "           " + this.GoodsIssueBalanceSQL(true);
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           " + this.GoodsIssueBalanceSQL(false);


            queryString = queryString + "       UPDATE      Balances " + "\r\n";
            queryString = queryString + "       SET         Balances.CustomerCode = Customers.Code, Balances.CustomerName = Customers.Name, Balances.CustomerCategoryName = CustomerCategories.Name " + "\r\n";
            queryString = queryString + "       FROM        @GoodsIssueBalances Balances " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON Balances.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";



            queryString = queryString + "       RETURN " + "\r\n";

            queryString = queryString + "   END " + "\r\n";


            this.totalSalesPortalEntities.CreateUserDefinedFunction("GoodsIssueBalance", queryString);
        }


        private string GoodsIssueBalanceSQL(bool isCustomerID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       INSERT INTO @GoodsIssueBalances (NmvnTaskID, EntryID, EntryDate, Reference, CustomerID, CustomerCode, CustomerName, CustomerCategoryName, EntryAmount, BalanceAmount) " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.GoodsIssue + " AS NmvnTaskID, GoodsIssues.GoodsIssueID, GoodsIssues.EntryDate, GoodsIssues.Reference, GoodsIssues.CustomerID, '' AS CustomerCode, '' AS CustomerName, '' AS CustomerCategoryName, GoodsIssues.TotalGrossAmount, GoodsIssueBalances.BalanceAmount " + "\r\n";
            queryString = queryString + "       FROM       (SELECT     GoodsIssueID, SUM(BalanceAmount) AS BalanceAmount " + "\r\n";
            queryString = queryString + "                   FROM        (" + "\r\n";
            queryString = queryString + "                               SELECT      GoodsIssues.GoodsIssueID, ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount - GoodsIssues.TotalCashDiscount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        GoodsIssues " + "\r\n"; //Raw GoodsIssues Balance
            queryString = queryString + "                               WHERE       " + (isCustomerID ? "CustomerID = @LocalCustomerID AND" : "") + " EntryDate <= @LocalToDate AND ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount - GoodsIssues.TotalCashDiscount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";

            queryString = queryString + "                               UNION ALL " + "\r\n"; //UNDO Receipts

            queryString = queryString + "                               SELECT      GoodsIssues.GoodsIssueID, ReceiptDetails.ReceiptAmount + ReceiptDetails.CashDiscount AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        GoodsIssues INNER JOIN ReceiptDetails ON " + (isCustomerID ? "GoodsIssues.CustomerID = @LocalCustomerID AND" : "") + " GoodsIssues.EntryDate <= @LocalToDate AND ReceiptDetails.EntryDate > @LocalToDate AND GoodsIssues.GoodsIssueID = ReceiptDetails.GoodsIssueID" + "\r\n";
            queryString = queryString + "                               ) AS GoodsIssueUnions " + "\r\n";
            queryString = queryString + "                   GROUP BY GoodsIssueID) GoodsIssueBalances " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssues ON GoodsIssueBalances.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";




            queryString = queryString + "       INSERT INTO @GoodsIssueBalances (NmvnTaskID, EntryID, EntryDate, Reference, CustomerID, CustomerCode, CustomerName, CustomerCategoryName, EntryAmount, BalanceAmount) " + "\r\n";

            queryString = queryString + "       SELECT      NmvnTaskID, EntryID, MAX(EntryDate), MAX(Reference), MAX(CustomerID), '' AS CustomerCode, '' AS CustomerName, '' AS CustomerCategoryName, MAX(TotalCreditAmount), SUM(BalanceAmount)" + "\r\n";

            queryString = queryString + "       FROM       (SELECT      " + (int)GlobalEnums.NmvnTaskID.Receipt + " AS NmvnTaskID, ReceiptID AS EntryID, Reference, EntryDate, CustomerID, TotalDepositAmount AS TotalCreditAmount, -ROUND(TotalDepositAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        Receipts " + "\r\n";
            queryString = queryString + "                               WHERE       " + (isCustomerID ? "CustomerID = @LocalCustomerID AND" : "") + " EntryDate <= @LocalToDate AND ReceiptTypeID = " + GlobalReceiptTypeID.ReceiveMoney + " AND ROUND(TotalDepositAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";
            queryString = queryString + "                               UNION ALL   " + "\r\n";
            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.Receipt + " AS NmvnTaskID, AdvanceReceipts.ReceiptID AS EntryID, AdvanceReceipts.Reference, AdvanceReceipts.EntryDate, AdvanceReceipts.CustomerID, AdvanceReceipts.TotalDepositAmount AS TotalCreditAmount, -ROUND(Receipts.TotalReceiptAmount + Receipts.TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        Receipts AS AdvanceReceipts INNER JOIN Receipts ON " + (isCustomerID ? "AdvanceReceipts.CustomerID = @LocalCustomerID AND" : "") + " AdvanceReceipts.EntryDate <= @LocalToDate AND Receipts.EntryDate > @LocalToDate AND AdvanceReceipts.ReceiptID = Receipts.AdvanceReceiptID " + "\r\n";


            queryString = queryString + "                               UNION ALL   " + "\r\n";

            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.SalesReturn + " AS NmvnTaskID, SalesReturnID AS EntryID, Reference, EntryDate, CustomerID, TotalGrossAmount AS TotalCreditAmount, -ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        SalesReturns " + "\r\n";
            queryString = queryString + "                               WHERE       " + (isCustomerID ? "CustomerID = @LocalCustomerID AND" : "") + " EntryDate <= @LocalToDate AND ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";
            queryString = queryString + "                               UNION ALL   " + "\r\n";
            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.SalesReturn + " AS NmvnTaskID, SalesReturns.SalesReturnID AS EntryID, SalesReturns.Reference, SalesReturns.EntryDate, SalesReturns.CustomerID, SalesReturns.TotalGrossAmount AS TotalCreditAmount, -ROUND(Receipts.TotalReceiptAmount + Receipts.TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        SalesReturns INNER JOIN Receipts ON " + (isCustomerID ? "SalesReturns.CustomerID = @LocalCustomerID AND" : "") + " SalesReturns.EntryDate <= @LocalToDate AND Receipts.EntryDate > @LocalToDate AND SalesReturns.SalesReturnID = Receipts.SalesReturnID " + "\r\n";


            queryString = queryString + "                               UNION ALL   " + "\r\n";

            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.CreditNote + " AS NmvnTaskID, CreditNoteID AS EntryID, Reference, EntryDate, CustomerID, TotalCreditAmount AS TotalCreditAmount, -ROUND(TotalCreditAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        CreditNotes " + "\r\n";
            queryString = queryString + "                               WHERE       " + (isCustomerID ? "CustomerID = @LocalCustomerID AND" : "") + " EntryDate <= @LocalToDate AND ROUND(TotalCreditAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";
            queryString = queryString + "                               UNION ALL   " + "\r\n";
            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.Receipt + " AS NmvnTaskID, CreditNotes.CreditNoteID AS EntryID, CreditNotes.Reference, CreditNotes.EntryDate, CreditNotes.CustomerID, CreditNotes.TotalCreditAmount AS TotalCreditAmount, -ROUND(Receipts.TotalReceiptAmount + Receipts.TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        CreditNotes INNER JOIN Receipts ON " + (isCustomerID ? "CreditNotes.CustomerID = @LocalCustomerID AND" : "") + " CreditNotes.EntryDate <= @LocalToDate AND Receipts.EntryDate > @LocalToDate AND CreditNotes.CreditNoteID = Receipts.CreditNoteID " + "\r\n";
            queryString = queryString + "                   ) AS CustomerCreditCollections " + "\r\n";
            queryString = queryString + "       GROUP BY    NmvnTaskID, EntryID " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

    }
}
