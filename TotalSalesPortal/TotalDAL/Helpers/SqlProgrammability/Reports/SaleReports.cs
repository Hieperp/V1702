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
            this.GoodsIssueJournal();
            this.AccountInvoiceJournal();

            this.GoodsIssueBalance();

            this.SalesJournal();
            this.StatementOfAccount();

            this.StatementOfWarehouses();
        }



        private void GoodsIssueJournal()
        {
            string queryString;

            queryString = " @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime         SET @LocalFromDate = @FromDate" + "\r\n";
            queryString = queryString + "       DECLARE     @LocalToDate DateTime           SET @LocalToDate = @ToDate" + "\r\n";

            queryString = queryString + "       SELECT      GoodsIssues.GoodsIssueID, 'Inventories/GoodsIssues' AS TaskAction, GoodsIssues.EntryDate, DATEADD(Month, DateDiff(Month, 0, GoodsIssues.EntryDate), 0) AS EntryMonth, GoodsIssues.Reference, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, CustomerCategories.Name AS CustomerCategoryName, Employees.Name AS SalespersonName, GoodsIssues.Description, GoodsIssues.DeliveryAdviceReferences, DeliveryAdvices.Reference AS DeliveryAdviceReference, " + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails.GoodsIssueDetailID, GoodsIssueDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.Name AS CommodityName, Commodities.Weight AS UnitWeight, " + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails.Quantity, GoodsIssueDetails.FreeQuantity, GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity AS LineQuantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.ListedGrossPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.ListedAmount, GoodsIssueDetails.Amount, ROUND(GoodsIssueDetails.ListedAmount - GoodsIssueDetails.Amount, " + (int)GlobalEnums.rndAmount + ") AS DiscountAmount, GoodsIssues.TotalQuantity, GoodsIssues.TotalAmount, GoodsIssues.TradeDiscountAmount, GoodsIssues.TotalTaxableAmount, GoodsIssues.TotalVATAmount, GoodsIssues.TotalGrossAmount " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssues " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssueDetails ON GoodsIssues.EntryDate >= @LocalFromDate AND GoodsIssues.EntryDate <= @LocalToDate AND GoodsIssues.GoodsIssueID = GoodsIssueDetails.GoodsIssueID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON GoodsIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN DeliveryAdvices ON GoodsIssueDetails.DeliveryAdviceID = DeliveryAdvices.DeliveryAdviceID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees ON DeliveryAdvices.SalespersonID = Employees.EmployeeID " + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GoodsIssueJournal", queryString);
        }


        private void AccountInvoiceJournal()
        {
            string queryString;

            queryString = " @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime         SET @LocalFromDate = @FromDate" + "\r\n";
            queryString = queryString + "       DECLARE     @LocalToDate DateTime           SET @LocalToDate = @ToDate" + "\r\n";

            queryString = queryString + "       SELECT      AccountInvoices.AccountInvoiceID, 'Accounts/AccountInvoices' AS TaskAction, AccountInvoices.EntryDate, AccountInvoices.Reference, AccountInvoices.VATInvoiceNo, AccountInvoices.VATInvoiceDate, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName, Customers.VATCode, CustomerCategories.Name AS CustomerCategoryName, Customers.BillingAddress, AccountInvoices.Description, " + "\r\n";
            queryString = queryString + "                   AccountInvoiceDetails.AccountInvoiceDetailID, AccountInvoiceDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.OfficialName, Commodities.Weight AS UnitWeight, " + "\r\n";
            queryString = queryString + "                   AccountInvoiceDetails.Quantity, AccountInvoiceDetails.FreeQuantity, AccountInvoiceDetails.ListedPrice, AccountInvoiceDetails.DiscountPercent, AccountInvoiceDetails.UnitPrice, AccountInvoiceDetails.ListedAmount, AccountInvoiceDetails.Amount, ROUND(AccountInvoiceDetails.ListedAmount - AccountInvoiceDetails.Amount, " + (int)GlobalEnums.rndAmount + ") AS DiscountAmount, AccountInvoices.TotalQuantity, AccountInvoices.TotalAmount, AccountInvoices.TradeDiscountAmount, AccountInvoices.TotalTaxableAmount, AccountInvoices.TotalVATAmount, AccountInvoices.TotalGrossAmount " + "\r\n";

            queryString = queryString + "       FROM        AccountInvoices " + "\r\n";
            queryString = queryString + "                   INNER JOIN AccountInvoiceDetails ON AccountInvoices.VATInvoiceDate >= @LocalFromDate AND AccountInvoices.VATInvoiceDate <= @LocalToDate AND AccountInvoices.AccountInvoiceID = AccountInvoiceDetails.AccountInvoiceID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON AccountInvoices.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON AccountInvoiceDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("AccountInvoiceJournal", queryString);
        }



        private void GoodsIssueBalance()
        {
            string queryString = " (@ToDate DateTime, @CustomerCategoryID int, @CustomerID int) " + "\r\n";
            queryString = queryString + " RETURNS @GoodsIssueBalances TABLE (NmvnTaskID int NOT NULL, TaskAction varchar(60) NOT NULL, EntryID int NOT NULL, EntryDate datetime NOT NULL, Reference nvarchar(10) NULL, CustomerID int NOT NULL, CustomerCode nvarchar(50) NOT NULL, CustomerName nvarchar(100) NOT NULL, BillingAddress nvarchar(200) NOT NULL, CustomerCategoryID int NOT NULL, CustomerCategoryName nvarchar(100) NOT NULL, EntryAmount decimal(18, 2) NOT NULL, BalanceAmount decimal(18, 2) NOT NULL) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalToDate DateTime, @LocalCustomerCategoryID int, @LocalCustomerID int         SET @LocalToDate = @ToDate          SET @LocalCustomerCategoryID = @CustomerCategoryID            SET @LocalCustomerID = @CustomerID " + "\r\n";


            queryString = queryString + "       IF (@LocalCustomerID > 0) ";
            queryString = queryString + "           " + this.GoodsIssueBalanceSQLA(true);
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           " + this.GoodsIssueBalanceSQLA(false);


            queryString = queryString + "       IF (@LocalCustomerCategoryID > 0 AND @LocalCustomerID <= 0) ";
            queryString = queryString + "           " + this.GoodsIssueBalanceSQLB(true);
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           " + this.GoodsIssueBalanceSQLB(false);

            queryString = queryString + "       RETURN " + "\r\n";

            queryString = queryString + "   END " + "\r\n";


            this.totalSalesPortalEntities.CreateUserDefinedFunction("GoodsIssueBalance", queryString);
        }


        private string GoodsIssueBalanceSQLA(bool isCustomerID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       INSERT INTO @GoodsIssueBalances (NmvnTaskID, TaskAction, EntryID, EntryDate, Reference, CustomerID, CustomerCode, CustomerName, BillingAddress, CustomerCategoryID, CustomerCategoryName, EntryAmount, BalanceAmount) " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.GoodsIssue + " AS NmvnTaskID, 'Inventories/GoodsIssues' AS TaskAction, GoodsIssues.GoodsIssueID, GoodsIssues.EntryDate, GoodsIssues.Reference, GoodsIssues.CustomerID, '' AS CustomerCode, '' AS CustomerName, '' AS BillingAddress, 0 AS CustomerCategoryID, '' AS CustomerCategoryName, GoodsIssues.TotalGrossAmount, GoodsIssueBalances.BalanceAmount " + "\r\n";
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




            queryString = queryString + "       INSERT INTO @GoodsIssueBalances (NmvnTaskID, TaskAction, EntryID, EntryDate, Reference, CustomerID, CustomerCode, CustomerName, BillingAddress, CustomerCategoryID, CustomerCategoryName, EntryAmount, BalanceAmount) " + "\r\n";

            queryString = queryString + "       SELECT      NmvnTaskID, MAX(TaskAction), EntryID, MAX(EntryDate), MAX(Reference), MAX(CustomerID), '' AS CustomerCode, '' AS CustomerName, '' AS BillingAddress, 0 AS CustomerCategoryID, '' AS CustomerCategoryName, MAX(TotalCreditAmount), SUM(BalanceAmount)" + "\r\n";

            queryString = queryString + "       FROM       (SELECT      " + (int)GlobalEnums.NmvnTaskID.Receipt + " AS NmvnTaskID, 'Accounts/Receipts' AS TaskAction, ReceiptID AS EntryID, Reference, EntryDate, CustomerID, TotalDepositAmount AS TotalCreditAmount, -ROUND(TotalDepositAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        Receipts " + "\r\n";
            queryString = queryString + "                               WHERE       " + (isCustomerID ? "CustomerID = @LocalCustomerID AND" : "") + " EntryDate <= @LocalToDate AND ReceiptTypeID = " + GlobalReceiptTypeID.ReceiveMoney + " AND ROUND(TotalDepositAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";
            queryString = queryString + "                               UNION ALL   " + "\r\n";
            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.Receipt + " AS NmvnTaskID, 'Accounts/Receipts' AS TaskAction, AdvanceReceipts.ReceiptID AS EntryID, AdvanceReceipts.Reference, AdvanceReceipts.EntryDate, AdvanceReceipts.CustomerID, AdvanceReceipts.TotalDepositAmount AS TotalCreditAmount, -ROUND(Receipts.TotalReceiptAmount + Receipts.TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        Receipts AS AdvanceReceipts INNER JOIN Receipts ON " + (isCustomerID ? "AdvanceReceipts.CustomerID = @LocalCustomerID AND" : "") + " AdvanceReceipts.EntryDate <= @LocalToDate AND Receipts.EntryDate > @LocalToDate AND AdvanceReceipts.ReceiptID = Receipts.AdvanceReceiptID " + "\r\n";


            queryString = queryString + "                               UNION ALL   " + "\r\n";

            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.SalesReturn + " AS NmvnTaskID, 'Sales/SalesReturns' AS TaskAction, SalesReturnID AS EntryID, Reference, EntryDate, CustomerID, TotalGrossAmount AS TotalCreditAmount, -ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        SalesReturns " + "\r\n";
            queryString = queryString + "                               WHERE       " + (isCustomerID ? "CustomerID = @LocalCustomerID AND" : "") + " EntryDate <= @LocalToDate AND ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";
            queryString = queryString + "                               UNION ALL   " + "\r\n";
            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.SalesReturn + " AS NmvnTaskID, 'Sales/SalesReturns' AS TaskAction, SalesReturns.SalesReturnID AS EntryID, SalesReturns.Reference, SalesReturns.EntryDate, SalesReturns.CustomerID, SalesReturns.TotalGrossAmount AS TotalCreditAmount, -ROUND(Receipts.TotalReceiptAmount + Receipts.TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        SalesReturns INNER JOIN Receipts ON " + (isCustomerID ? "SalesReturns.CustomerID = @LocalCustomerID AND" : "") + " SalesReturns.EntryDate <= @LocalToDate AND Receipts.EntryDate > @LocalToDate AND SalesReturns.SalesReturnID = Receipts.SalesReturnID " + "\r\n";


            queryString = queryString + "                               UNION ALL   " + "\r\n";

            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.CreditNote + " AS NmvnTaskID, 'Accounts/CreditNotes' AS TaskAction, CreditNoteID AS EntryID, Reference, EntryDate, CustomerID, TotalGrossAmount AS TotalCreditAmount, -ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        CreditNotes " + "\r\n";
            queryString = queryString + "                               WHERE       " + (isCustomerID ? "CustomerID = @LocalCustomerID AND" : "") + " EntryDate <= @LocalToDate AND ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";
            queryString = queryString + "                               UNION ALL   " + "\r\n";
            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.Receipt + " AS NmvnTaskID, 'Accounts/CreditNotes' AS TaskAction, CreditNotes.CreditNoteID AS EntryID, CreditNotes.Reference, CreditNotes.EntryDate, CreditNotes.CustomerID, CreditNotes.TotalGrossAmount AS TotalCreditAmount, -ROUND(Receipts.TotalReceiptAmount + Receipts.TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        CreditNotes INNER JOIN Receipts ON " + (isCustomerID ? "CreditNotes.CustomerID = @LocalCustomerID AND" : "") + " CreditNotes.EntryDate <= @LocalToDate AND Receipts.EntryDate > @LocalToDate AND CreditNotes.CreditNoteID = Receipts.CreditNoteID " + "\r\n";
            queryString = queryString + "                   ) AS CustomerCreditCollections " + "\r\n";
            queryString = queryString + "       GROUP BY    NmvnTaskID, EntryID " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }


        private string GoodsIssueBalanceSQLB(bool isCustomerCategoryID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       UPDATE      Balances " + "\r\n";
            queryString = queryString + "       SET         Balances.CustomerCode = Customers.Code, Balances.CustomerName = Customers.Name, Balances.BillingAddress = Customers.BillingAddress, Balances.CustomerCategoryID = Customers.CustomerCategoryID, Balances.CustomerCategoryName = CustomerCategories.Name " + "\r\n";
            queryString = queryString + "       FROM        @GoodsIssueBalances Balances " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON " + (isCustomerCategoryID ? " Customers.CustomerCategoryID = @LocalCustomerCategoryID AND" : "") + " Balances.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";

            queryString = queryString + "       " + (isCustomerCategoryID ? " DELETE FROM @GoodsIssueBalances WHERE CustomerCode = '' " : "") + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }



        private void SalesJournal()
        {
            string queryString;

            queryString = " @FromDate DateTime, @ToDate DateTime, @CustomerCategoryID int, @CustomerID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime, @LocalToDate DateTime, @LocalCustomerCategoryID int, @LocalCustomerID int         SET @LocalFromDate = @FromDate         SET @LocalToDate = @ToDate          SET @LocalCustomerCategoryID = @CustomerCategoryID            SET @LocalCustomerID = @CustomerID " + "\r\n";

            queryString = queryString + "       IF (@LocalCustomerID > 0) ";
            queryString = queryString + "           " + this.SalesJournalSQL(false, true);
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           IF (@LocalCustomerCategoryID > 0) ";
            queryString = queryString + "               " + this.SalesJournalSQL(true, false);
            queryString = queryString + "           ELSE ";
            queryString = queryString + "               " + this.SalesJournalSQL(false, false);

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("SalesJournal", queryString);
        }

        private string SalesJournalSQL(bool isCustomerCategoryID, bool isCustomerID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";


            queryString = queryString + "       SELECT      NmvnTaskID, TaskAction, CustomerID, CustomerCode, CustomerName, BillingAddress, CustomerCategoryID, CustomerCategoryName, " + "\r\n";
            queryString = queryString + "                   EntryID, EntryDate, Reference, BalanceAmount AS BeginningAmount, 0 AS SumListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalFluctuationAmount, BalanceAmount AS EndingAmount " + "\r\n";
            queryString = queryString + "       FROM        dbo.GoodsIssueBalance(DATEADD(second, -1, @LocalFromDate), @LocalCustomerCategoryID, @LocalCustomerID) " + "\r\n";

            queryString = queryString + "       UNION ALL " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.GoodsIssue + " AS NmvnTaskID, 'Inventories/GoodsIssues' AS TaskAction, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.BillingAddress, Customers.CustomerCategoryID, CustomerCategories.Name AS CustomerCategoryName, " + "\r\n";
            queryString = queryString + "                   GoodsIssues.GoodsIssueID AS EntryID, GoodsIssues.EntryDate, GoodsIssues.Reference, 0 AS BeginningAmount, GoodsIssues.SumListedGrossAmount, GoodsIssues.TotalGrossAmount, ROUND(GoodsIssues.SumListedGrossAmount - GoodsIssues.TotalGrossAmount, " + (int)GlobalEnums.rndAmount + ") AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalFluctuationAmount, GoodsIssues.TotalGrossAmount AS EndingAmount " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssues " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON " + (isCustomerID ? "GoodsIssues.CustomerID = @LocalCustomerID AND" : "") + (isCustomerCategoryID ? " Customers.CustomerCategoryID = @LocalCustomerCategoryID AND" : "") + " GoodsIssues.EntryDate >= @LocalFromDate AND GoodsIssues.EntryDate <= @LocalToDate AND GoodsIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";

            queryString = queryString + "       UNION ALL " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.SalesReturn + " AS NmvnTaskID, 'Sales/SalesReturns' AS TaskAction, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.BillingAddress, Customers.CustomerCategoryID, CustomerCategories.Name AS CustomerCategoryName, " + "\r\n";
            queryString = queryString + "                   SalesReturns.SalesReturnID AS EntryID, SalesReturns.EntryDate, SalesReturns.Reference, 0 AS BeginningAmount, 0 AS SumListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, SalesReturns.TotalGrossAmount AS TotalReturnAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalFluctuationAmount, -SalesReturns.TotalGrossAmount AS EndingAmount " + "\r\n";

            queryString = queryString + "       FROM        SalesReturns " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON " + (isCustomerID ? "SalesReturns.CustomerID = @LocalCustomerID AND" : "") + (isCustomerCategoryID ? " Customers.CustomerCategoryID = @LocalCustomerCategoryID AND" : "") + " SalesReturns.EntryDate >= @LocalFromDate AND SalesReturns.EntryDate <= @LocalToDate AND SalesReturns.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";



            queryString = queryString + "       UNION ALL " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.CreditNote + " AS NmvnTaskID, 'Accounts/CreditNotes' AS TaskAction, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.BillingAddress, Customers.CustomerCategoryID, CustomerCategories.Name AS CustomerCategoryName, " + "\r\n";
            queryString = queryString + "                   CreditNotes.CreditNoteID AS EntryID, CreditNotes.EntryDate, CreditNotes.Reference, 0 AS BeginningAmount, 0 AS SumListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, CreditNotes.TotalGrossAmount AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalFluctuationAmount, -CreditNotes.TotalGrossAmount AS EndingAmount " + "\r\n";

            queryString = queryString + "       FROM        CreditNotes " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON " + (isCustomerID ? "CreditNotes.CustomerID = @LocalCustomerID AND" : "") + (isCustomerCategoryID ? " Customers.CustomerCategoryID = @LocalCustomerCategoryID AND" : "") + " CreditNotes.EntryDate >= @LocalFromDate AND CreditNotes.EntryDate <= @LocalToDate AND CreditNotes.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";



            queryString = queryString + "       UNION ALL " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.Receipt + " AS NmvnTaskID, 'Accounts/Receipts' AS TaskAction, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.BillingAddress, Customers.CustomerCategoryID, CustomerCategories.Name AS CustomerCategoryName, " + "\r\n";
            queryString = queryString + "                   Receipts.ReceiptID AS EntryID, Receipts.EntryDate, Receipts.Reference, 0 AS BeginningAmount, 0 AS SumListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalCreditAmount, CASE Receipts.MonetaryAccountID WHEN 3 THEN Receipts.TotalDepositAmount ELSE 0 END AS TotalCashReceiptAmount, CASE Receipts.MonetaryAccountID WHEN 3 THEN 0 ELSE Receipts.TotalDepositAmount END TotalBankTransferAmount, Receipts.TotalCashDiscount, Receipts.TotalFluctuationAmount, -Receipts.TotalDepositAmount - Receipts.TotalCashDiscount - Receipts.TotalFluctuationAmount AS EndingAmount " + "\r\n";

            queryString = queryString + "       FROM        Receipts " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON " + (isCustomerID ? "Receipts.CustomerID = @LocalCustomerID AND" : "") + (isCustomerCategoryID ? " Customers.CustomerCategoryID = @LocalCustomerCategoryID AND" : "") + " Receipts.ReceiptTypeID = " + GlobalReceiptTypeID.ReceiveMoney + " AND Receipts.EntryDate >= @LocalFromDate AND Receipts.EntryDate <= @LocalToDate AND Receipts.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";


            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }


        public void StatementOfAccount()
        {
            string queryString;

            queryString = " @FromDate DateTime, @ToDate DateTime, @CustomerCategoryID int, @CustomerID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime, @LocalToDate DateTime, @LocalCustomerCategoryID int, @LocalCustomerID int         SET @LocalFromDate = @FromDate         SET @LocalToDate = @ToDate          SET @LocalCustomerCategoryID = @CustomerCategoryID            SET @LocalCustomerID = @CustomerID " + "\r\n";

            queryString = queryString + "       DECLARE         @StatementTable TABLE (NmvnTaskID int NULL, TaskAction nvarchar(50) NULL, CustomerID int NOT NULL, CustomerCode nvarchar(50) NOT NULL, CustomerName nvarchar(100) NOT NULL, BillingAddress nvarchar(200) NOT NULL, CustomerCategoryID int NOT NULL, CustomerCategoryName nvarchar(100) NOT NULL, EntryID int NULL, EntryDate datetime NULL, GoodsIssueID int NULL, GoodsIssueReference nvarchar(30) NULL, CountGoodsIssueID int, GoodsIssueListedGrossAmount float NOT NULL, GoodsIssueGrossAmount float NOT NULL, GoodsIssueDiscountAmount float NOT NULL, AccountInvoiceID int NULL, AccountInvoiceReference nvarchar(30) NULL, VATInvoiceNo nvarchar(30) NULL, GoodsIssueReferences nvarchar(200) NULL, SerialID int NULL, AccountInvoiceListedGrossAmount float NOT NULL, AccountInvoiceGrossAmount float NOT NULL, AccountInvoiceDiscountAmount float NOT NULL, BeginningAmount float NOT NULL, SumListedGrossAmount float NOT NULL, TotalGrossAmount float NOT NULL, TotalDiscountAmount float NOT NULL, TotalReturnAmount float NOT NULL, TotalCreditAmount float NOT NULL, TotalCashReceiptAmount float NOT NULL, TotalBankTransferAmount float NOT NULL, TotalCashDiscount float NOT NULL, TotalFluctuationAmount float NOT NULL, EndingAmount float NOT NULL, EndingAmountInWords nvarchar(100) NULL) " + "\r\n";
            queryString = queryString + "       DECLARE         @SalesJournalTable TABLE (NmvnTaskID int NULL, TaskAction nvarchar(50) NULL, CustomerID int NOT NULL, CustomerCode nvarchar(50) NOT NULL, CustomerName nvarchar(100) NOT NULL, BillingAddress nvarchar(200) NOT NULL, CustomerCategoryID int NOT NULL, CustomerCategoryName nvarchar(100) NOT NULL, EntryID int NULL, EntryDate datetime NOT NULL, Reference nvarchar(30) NULL, BeginningAmount float NOT NULL, SumListedGrossAmount float NOT NULL, TotalGrossAmount float NOT NULL, TotalDiscountAmount float NOT NULL, TotalReturnAmount float NOT NULL, TotalCreditAmount float NOT NULL, TotalCashReceiptAmount float NOT NULL, TotalBankTransferAmount float NOT NULL, TotalCashDiscount float NOT NULL, TotalFluctuationAmount float NOT NULL, EndingAmount float NOT NULL) " + "\r\n";

            queryString = queryString + "       IF (@LocalCustomerID > 0) " + "\r\n";
            queryString = queryString + "           " + this.StatementOfAccountSQL(false, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           IF (@LocalCustomerCategoryID > 0) " + "\r\n";
            queryString = queryString + "               " + this.StatementOfAccountSQL(true, false) + "\r\n";
            queryString = queryString + "           ELSE " + "\r\n";
            queryString = queryString + "               " + this.StatementOfAccountSQL(false, false) + "\r\n";


            queryString = queryString + "       SELECT * FROM  @StatementTable " + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            System.Diagnostics.Debug.WriteLine("--------");
            System.Diagnostics.Debug.WriteLine(queryString);
            this.totalSalesPortalEntities.CreateStoredProcedure("StatementOfAccount", queryString);




            //DECLARE @FromDate DateTime, @ToDate DateTime, @CustomerCategoryID int, @CustomerID int 

            //SET @FromDate = CONVERT(DATETIME, '2017-05-01 00:00:00', 102)
            //SET @ToDate = CONVERT(DATETIME, '2017-05-31 00:00:00', 102)
            //SET @CustomerCategoryID = 0
            //SET @CustomerID = 0


            //EXEC StatementOfAccount @FromDate, @ToDate,  0, 0


        }


        private string StatementOfAccountSQL(bool isCustomerCategoryID, bool isCustomerID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       INSERT INTO     @SalesJournalTable EXEC SalesJournal @LocalFromDate, @LocalToDate, @LocalCustomerCategoryID, @LocalCustomerID " + "\r\n";


            queryString = queryString + "       INSERT INTO     @StatementTable (NmvnTaskID, TaskAction, CustomerID, CustomerCode, CustomerName, BillingAddress, CustomerCategoryID, CustomerCategoryName, EntryID, EntryDate, GoodsIssueID, GoodsIssueReference, CountGoodsIssueID, GoodsIssueListedGrossAmount, GoodsIssueGrossAmount, GoodsIssueDiscountAmount, AccountInvoiceID, AccountInvoiceReference, VATInvoiceNo, GoodsIssueReferences, SerialID, AccountInvoiceListedGrossAmount, AccountInvoiceGrossAmount, AccountInvoiceDiscountAmount, BeginningAmount, SumListedGrossAmount, TotalGrossAmount, TotalDiscountAmount, TotalReturnAmount, TotalCreditAmount, TotalCashReceiptAmount, TotalBankTransferAmount, TotalCashDiscount, TotalFluctuationAmount, EndingAmount, EndingAmountInWords) " + "\r\n";
            queryString = queryString + "       SELECT          " + (int)GlobalEnums.NmvnTaskID.AccountInvoice + " AS NmvnTaskID, 'Accounts/AccountInvoices' AS TaskAction, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.BillingAddress, Customers.CustomerCategoryID, CustomerCategories.Name AS CustomerCategoryName, AccountInvoices.AccountInvoiceID AS EntryID, CAST(AccountInvoices.VATInvoiceDate AS Date) AS EntryDate, AccountInvoices.GoodsIssueFirstID AS GoodsIssueID, NULL AS GoodsIssueReference, 0 AS CountGoodsIssueID, 0 AS GoodsIssueListedGrossAmount, 0 AS GoodsIssueGrossAmount, 0 AS GoodsIssueDiscountAmount, AccountInvoices.AccountInvoiceID, AccountInvoices.Reference AS AccountInvoiceReference, AccountInvoices.VATInvoiceNo, AccountInvoices.GoodsIssueReferences, 0 AS SerialID, AccountInvoices.SumListedGrossAmount AS AccountInvoiceListedGrossAmount, AccountInvoices.TotalGrossAmount AS AccountInvoiceGrossAmount, ROUND(AccountInvoices.SumListedGrossAmount - AccountInvoices.TotalGrossAmount, " + (int)GlobalEnums.rndAmount + ") AS AccountInvoiceDiscountAmount, 0 AS BeginningAmount, 0 AS SumListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalFluctuationAmount, 0 AS EndingAmount, NULL AS EndingAmountInWords " + "\r\n";
            queryString = queryString + "       FROM            AccountInvoices " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON " + (isCustomerID ? "AccountInvoices.ConsumerID = @LocalCustomerID AND" : "") + (isCustomerCategoryID ? " Customers.CustomerCategoryID = @LocalCustomerCategoryID AND" : "") + " AccountInvoices.VATInvoiceDate >= @LocalFromDate AND AccountInvoices.VATInvoiceDate <= @LocalToDate AND AccountInvoices.ConsumerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";

            queryString = queryString + "       UPDATE          StatementTable " + "\r\n"; //THE PURPOSE OF THIS IS: Set SerialID = SomeDistinctValue (Here we use AccountInvoiceID as DistinctValue) FOR THESE AccountInvoices WHICH ARE: THERE ARE MORE THAN 1 AccountInvoice WITH THE SAME GoodsIssueFirstID AT THE SAME DATE (WE USE CONDITION: AccountInvoiceID <> MINAccountInvoiceID TO SET THE SerialID OF THE SECOND, THE THIRDTH, THE FOURTH ... TO AccountInvoiceID, KEEP THE SerialID = 0 FOR ROW WHICH IS AccountInvoiceID = MINAccountInvoiceID)
            queryString = queryString + "       SET             StatementTable.SerialID = StatementTable.AccountInvoiceID " + "\r\n"; //NOTE 1: DEFAULT OF SerialID IS 0, UNLESS THERE ARE MORE THAN 1 AccountInvoice WITH THE SAME GoodsIssueFirstID AT THE SAME DATE
            queryString = queryString + "       FROM            @StatementTable StatementTable " + "\r\n";//NOTE 2: SerialID WILL BE ONLY USED IN REPORT FOR GROUP BY ENTRYDATE
            queryString = queryString + "                       INNER JOIN (SELECT          CustomerID, EntryDate, GoodsIssueID, MIN(AccountInvoiceID) AS MINAccountInvoiceID " + "\r\n";
            queryString = queryString + "                                   FROM            @StatementTable StatementTable " + "\r\n"; //NOTE 3: WE SHOULD RUN THIS UPDATE RIGHT AFTER WE INSERT AccountInvoices TO THE StatementTable.
            queryString = queryString + "                                   GROUP BY        CustomerID, EntryDate, GoodsIssueID " + "\r\n"; //NOTE 4: VERY IMPORTANT: THIS SHOULD DO WHEN THERE IS NO INSERT TO StatementTable BEFORE. OTHERWISE, WE SHOULD USE THE WHERE CLAUSE TO UPDATE THE AccountInvoice ROWS ONLY
            queryString = queryString + "                                   HAVING          COUNT(*) > 1) AS MoreInvoicePerDatePerIssue ON StatementTable.CustomerID = MoreInvoicePerDatePerIssue.CustomerID AND StatementTable.EntryDate = MoreInvoicePerDatePerIssue.EntryDate AND StatementTable.GoodsIssueID = MoreInvoicePerDatePerIssue.GoodsIssueID AND StatementTable.AccountInvoiceID <> MoreInvoicePerDatePerIssue.MINAccountInvoiceID " + "\r\n";


            queryString = queryString + "       INSERT INTO     @StatementTable (NmvnTaskID, TaskAction, CustomerID, CustomerCode, CustomerName, BillingAddress, CustomerCategoryID, CustomerCategoryName, EntryID, EntryDate, GoodsIssueID, GoodsIssueReference, CountGoodsIssueID, GoodsIssueListedGrossAmount, GoodsIssueGrossAmount, GoodsIssueDiscountAmount, AccountInvoiceID, AccountInvoiceReference, VATInvoiceNo, GoodsIssueReferences, SerialID, AccountInvoiceListedGrossAmount, AccountInvoiceGrossAmount, AccountInvoiceDiscountAmount, BeginningAmount, SumListedGrossAmount, TotalGrossAmount, TotalDiscountAmount, TotalReturnAmount, TotalCreditAmount, TotalCashReceiptAmount, TotalBankTransferAmount, TotalCashDiscount, TotalFluctuationAmount, EndingAmount, EndingAmountInWords) " + "\r\n";
            queryString = queryString + "       SELECT          NmvnTaskID, TaskAction, CustomerID, CustomerCode, CustomerName, BillingAddress, CustomerCategoryID, CustomerCategoryName, EntryID, CAST(EntryDate AS Date) AS EntryDate, EntryID AS GoodsIssueID, Reference, 1 AS CountGoodsIssueID, SumListedGrossAmount AS GoodsIssueListedGrossAmount, TotalGrossAmount AS GoodsIssueGrossAmount, TotalDiscountAmount AS GoodsIssueDiscountAmount, NULL AS AccountInvoiceID, NULL AS AccountInvoiceReference, NULL AS VATInvoiceNo, NULL AS GoodsIssueReferences, 0 AS SerialID, 0 AS AccountInvoiceListedGrossAmount, 0 AS AccountInvoiceGrossAmount, 0 AS AccountInvoiceDiscountAmount, 0 AS BeginningAmount, 0 AS SumListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalFluctuationAmount, 0 AS EndingAmount, NULL AS EndingAmountInWords " + "\r\n";
            queryString = queryString + "       FROM            @SalesJournalTable " + "\r\n";
            queryString = queryString + "       WHERE           NmvnTaskID = " + (int)GlobalEnums.NmvnTaskID.GoodsIssue + " AND EntryDate >= @LocalFromDate AND EntryDate <= @LocalToDate " + "\r\n";

            queryString = queryString + "       INSERT INTO     @StatementTable (NmvnTaskID, TaskAction, CustomerID, CustomerCode, CustomerName, BillingAddress, CustomerCategoryID, CustomerCategoryName, EntryID, EntryDate, GoodsIssueID, GoodsIssueReference, CountGoodsIssueID, GoodsIssueListedGrossAmount, GoodsIssueGrossAmount, GoodsIssueDiscountAmount, AccountInvoiceID, AccountInvoiceReference, VATInvoiceNo, GoodsIssueReferences, SerialID, AccountInvoiceListedGrossAmount, AccountInvoiceGrossAmount, AccountInvoiceDiscountAmount, BeginningAmount, SumListedGrossAmount, TotalGrossAmount, TotalDiscountAmount, TotalReturnAmount, TotalCreditAmount, TotalCashReceiptAmount, TotalBankTransferAmount, TotalCashDiscount, TotalFluctuationAmount, EndingAmount, EndingAmountInWords) " + "\r\n";
            queryString = queryString + "       SELECT          DISTINCT NULL AS NmvnTaskID, NULL AS TaskAction, CustomerID, CustomerCode, CustomerName, BillingAddress, CustomerCategoryID, CustomerCategoryName, NULL AS EntryID, NULL AS EntryDate, NULL AS GoodsIssueID, NULL AS Reference, 0 AS CountGoodsIssueID, 0 AS GoodsIssueListedGrossAmount, 0 AS GoodsIssueGrossAmount, 0 AS GoodsIssueDiscountAmount, NULL AS AccountInvoiceID, NULL AS AccountInvoiceReference, NULL AS VATInvoiceNo, NULL AS GoodsIssueReferences, NULL AS SerialID, 0 AS AccountInvoiceListedGrossAmount, 0 AS AccountInvoiceGrossAmount, 0 AS AccountInvoiceDiscountAmount, 0 AS BeginningAmount, 0 AS SumListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalFluctuationAmount, 0 AS EndingAmount, NULL AS EndingAmountInWords " + "\r\n";
            queryString = queryString + "       FROM            @SalesJournalTable " + "\r\n";
            queryString = queryString + "       WHERE           CustomerID NOT IN (SELECT CustomerID FROM @StatementTable) " + "\r\n";

            queryString = queryString + "       UPDATE          StatementTable " + "\r\n";
            queryString = queryString + "       SET             StatementTable.BeginningAmount = SalesJournalSummary.BeginningAmount, StatementTable.SumListedGrossAmount = SalesJournalSummary.SumListedGrossAmount, StatementTable.TotalGrossAmount = SalesJournalSummary.TotalGrossAmount, StatementTable.TotalDiscountAmount = SalesJournalSummary.TotalDiscountAmount, StatementTable.TotalReturnAmount = SalesJournalSummary.TotalReturnAmount, StatementTable.TotalCreditAmount = SalesJournalSummary.TotalCreditAmount, StatementTable.TotalCashReceiptAmount = SalesJournalSummary.TotalCashReceiptAmount, StatementTable.TotalBankTransferAmount = SalesJournalSummary.TotalBankTransferAmount, StatementTable.TotalCashDiscount = SalesJournalSummary.TotalCashDiscount, StatementTable.TotalFluctuationAmount = SalesJournalSummary.TotalFluctuationAmount, StatementTable.EndingAmount = SalesJournalSummary.EndingAmount, StatementTable.EndingAmountInWords = dbo.SayVND(SalesJournalSummary.EndingAmount) " + "\r\n";
            queryString = queryString + "       FROM            @StatementTable StatementTable " + "\r\n";
            queryString = queryString + "                       INNER JOIN (SELECT          CustomerID, SUM(BeginningAmount) AS BeginningAmount, SUM(SumListedGrossAmount) AS SumListedGrossAmount, SUM(TotalGrossAmount) AS TotalGrossAmount, SUM(TotalDiscountAmount) AS TotalDiscountAmount, SUM(TotalReturnAmount) AS TotalReturnAmount, SUM(TotalCreditAmount) AS TotalCreditAmount, SUM(TotalCashReceiptAmount) AS TotalCashReceiptAmount, SUM(TotalBankTransferAmount) AS TotalBankTransferAmount, SUM(TotalCashDiscount) AS TotalCashDiscount, SUM(TotalFluctuationAmount) AS TotalFluctuationAmount, SUM(EndingAmount) AS EndingAmount " + "\r\n";
            queryString = queryString + "                                   FROM            @SalesJournalTable " + "\r\n";
            queryString = queryString + "                                   GROUP BY        CustomerID) SalesJournalSummary ON StatementTable.CustomerID = SalesJournalSummary.CustomerID" + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }


        private void StatementOfWarehouses()
        {
            string queryString;

            queryString = " @ToDate DateTime " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime         SET @LocalFromDate = DATEADD(Month, DateDiff(Month, 0, @ToDate) - 12, 0) " + "\r\n"; //BACKWARD 12 MONTHS
            queryString = queryString + "       DECLARE     @LocalToDate DateTime           SET @LocalToDate = @ToDate" + "\r\n";

            SqlProgrammability.Inventories.Inventories inventories = new SqlProgrammability.Inventories.Inventories(this.totalSalesPortalEntities);

            queryString = queryString + "       " + inventories.GET_WarehouseJournal_BUILD_SQL(null, "@LocalToDate", "@LocalToDate", "", "", "0", "0", (int)GlobalEnums.WarehouseClassID.L1 + "," + (int)GlobalEnums.WarehouseClassID.L5 + "," + (int)GlobalEnums.WarehouseClassID.LD, (int)GlobalEnums.WarehouseGroupID.TT + "") + "\r\n";
            queryString = queryString + "       " + inventories.GET_SPProductionOrderJournalTable("@LocalToDate", "@LocalToDate", (int)GlobalEnums.WarehouseClassID.L1 + "," + (int)GlobalEnums.WarehouseClassID.L5 + "," + (int)GlobalEnums.WarehouseClassID.LD) + "\r\n";
            //TAM THOI: (int)GlobalEnums.WarehouseGroupID.TT + ""  --- CHI LAY HANG THI TRUONGC

            queryString = queryString + "       SELECT      Commodities.CommodityID, MIN(Commodities.Code) AS CommodityCode, MIN(Commodities.Name) AS CommodityName, WarehouseRawData.EntryMonth, SUM(QuantityGoodsIssue) AS QuantityGoodsIssue, SUM (QuantityBalance) AS QuantityBalance, SUM(QuantityOnProduction) AS QuantityOnProduction, SUM(QuantityEndSemi) AS QuantityEndSemi, SUM(QuantityEndImprove) AS QuantityEndImprove, SUM(QuantityEndPack) AS QuantityEndPack, SUM(QuantityEndRefine) AS QuantityEndRefine, SUM(QuantityEndRank) AS QuantityEndRank, SUM(QuantityEndFinish) AS QuantityEndFinish " + "\r\n";

            queryString = queryString + "       FROM        (" + "\r\n";

            queryString = queryString + "                   SELECT      CommodityID, DATEADD(Month, DateDiff(Month, 0, EntryDate), 0) AS EntryMonth, Quantity + FreeQuantity AS QuantityGoodsIssue, 0 AS QuantityBalance, 0 AS QuantityOnProduction, 0 AS QuantityEndSemi, 0 AS QuantityEndImprove, 0 AS QuantityEndPack, 0 AS QuantityEndRefine, 0 AS QuantityEndRank, 0 AS QuantityEndFinish " + "\r\n";
            queryString = queryString + "                   FROM        GoodsIssueDetails WHERE WarehouseID = 34 AND EntryDate >= @LocalFromDate AND EntryDate <= @LocalToDate " + "\r\n";
            //TAM THOI: WarehouseID = 34  --- CHI LAY HANG THI TRUONGC

            queryString = queryString + "                   UNION ALL   " + "\r\n";

            queryString = queryString + "                   SELECT      CommodityID, DATEADD(Month, DateDiff(Month, 0, @LocalToDate), 0) AS EntryMonth, 0  AS QuantityGoodsIssue, SUM(QuantityBegin + QuantityInput - QuantityOutput - QuantityOnAdvice - QuantityOnTransferAdviceOut) AS QuantityBalance, 0 AS QuantityOnProduction, 0 AS QuantityEndSemi, 0 AS QuantityEndImprove, 0 AS QuantityEndPack, 0 AS QuantityEndRefine, 0 AS QuantityEndRank, 0 AS QuantityEndFinish " + "\r\n";
            queryString = queryString + "                   FROM        @SPSKUInventoryJournalTable WHERE WHLocationID = 90 AND (QuantityBegin + QuantityInput - QuantityOutput - QuantityOnAdvice - QuantityOnTransferAdviceOut) <> 0 GROUP BY CommodityID " + "\r\n"; ////Phân Xưởng ONLY

            queryString = queryString + "                   UNION ALL   " + "\r\n";

            queryString = queryString + "                   SELECT      CommodityID, DATEADD(Month, DateDiff(Month, 0, @LocalToDate), 0) AS EntryMonth, 0  AS QuantityGoodsIssue, 0 AS QuantityBalance, SUM(QuantityEndOrder) AS QuantityOnProduction, SUM(QuantityEndSemi) AS QuantityEndSemi, SUM(QuantityEndImprove) AS QuantityEndImprove, SUM(QuantityEndPack) AS QuantityEndPack, SUM(QuantityEndRefine) AS QuantityEndRefine, SUM(QuantityEndRank) AS QuantityEndRank, SUM(QuantityEndFinish) AS QuantityEndFinish " + "\r\n";
            queryString = queryString + "                   FROM        @SPProductionOrderJournalTable WHERE WarehouseGroupID = 4 AND (QuantityEndSemi <> 0 OR QuantityEndImprove <> 0 OR QuantityEndPack <> 0 OR QuantityEndRefine <> 0 OR QuantityEndRank <> 0 OR QuantityEndFinish <> 0 OR QuantityEndOrder <> 0) GROUP BY CommodityID " + "\r\n";
            //TAM THOI: WarehouseGroupID = 4  --- CHI LAY HANG THI TRUONGC
            queryString = queryString + "                   ) WarehouseRawData " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON WarehouseRawData.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       GROUP BY    Commodities.CommodityID, WarehouseRawData.EntryMonth " + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("StatementOfWarehouses", queryString);
        }

    }
}
