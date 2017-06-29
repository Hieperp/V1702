using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Accounts
{
    public class AccountInvoice
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public AccountInvoice(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetAccountInvoiceIndexes();

            this.GetAccountInvoiceViewDetails();

            this.GetPendingGoodsIssues();
            this.GetPendingGoodsIssueConsumers();
            this.GetPendingGoodsIssueReceivers();

            this.GetPendingGoodsIssueDetails();

            this.AccountInvoiceSaveRelative();
            this.AccountInvoicePostSaveValidate();

            this.AccountInvoiceInitReference();

            this.AccountInvoiceSheet();
        }

        private void GetAccountInvoiceIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      AccountInvoices.AccountInvoiceID, CAST(AccountInvoices.EntryDate AS DATE) AS EntryDate, AccountInvoices.Reference, AccountInvoices.VATInvoiceNo, AccountInvoices.VATInvoiceDate, Locations.Code AS LocationCode, AccountInvoices.GoodsIssueReferences, GoodsIssues.EntryDate AS GoodsIssueEntryDate, Customers.Name AS CustomerName, Customers.BillingAddress, AccountInvoices.Description, AccountInvoices.TotalQuantity, AccountInvoices.TotalListedGrossAmount, AccountInvoices.TotalGrossAmount " + "\r\n";
            queryString = queryString + "       FROM        AccountInvoices " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON AccountInvoices.EntryDate >= @FromDate AND AccountInvoices.EntryDate <= @ToDate AND AccountInvoices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.AccountInvoice + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = AccountInvoices.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON AccountInvoices.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN  GoodsIssues ON AccountInvoices.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetAccountInvoiceIndexes", queryString);
        }

        private void GetAccountInvoiceViewDetails()
        {
            string queryString;

            queryString = " @AccountInvoiceID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      AccountInvoiceDetails.AccountInvoiceDetailID, AccountInvoiceDetails.AccountInvoiceID, AccountInvoiceDetails.GoodsIssueID, AccountInvoiceDetails.GoodsIssueDetailID, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.EntryDate AS GoodsIssueEntryDate, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, AccountInvoiceDetails.CalculatingTypeID, AccountInvoiceDetails.VATbyRow, ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice + AccountInvoiceDetails.Quantity, 0) AS QuantityRemains, ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice + AccountInvoiceDetails.FreeQuantity, 0) AS FreeQuantityRemains, GoodsIssueDetails.ControlFreeQuantity, " + "\r\n";
            queryString = queryString + "                   AccountInvoiceDetails.Quantity, AccountInvoiceDetails.FreeQuantity, AccountInvoiceDetails.ListedPrice, AccountInvoiceDetails.DiscountPercent, AccountInvoiceDetails.UnitPrice, AccountInvoiceDetails.TradeDiscountRate, AccountInvoiceDetails.VATPercent, AccountInvoiceDetails.ListedGrossPrice, AccountInvoiceDetails.GrossPrice, AccountInvoiceDetails.ListedAmount, AccountInvoiceDetails.Amount, AccountInvoiceDetails.ListedVATAmount, AccountInvoiceDetails.VATAmount, AccountInvoiceDetails.ListedGrossAmount, AccountInvoiceDetails.GrossAmount, AccountInvoiceDetails.IsBonus, AccountInvoiceDetails.Remarks " + "\r\n";

            queryString = queryString + "       FROM        AccountInvoiceDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssueDetails ON AccountInvoiceDetails.AccountInvoiceID = @AccountInvoiceID AND AccountInvoiceDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssues ON GoodsIssueDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetAccountInvoiceViewDetails", queryString);
        }








        private void GetPendingGoodsIssues()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          GoodsIssues.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Receivers.CustomerID AS ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.VATCode AS ReceiverVATCode, Receivers.AttentionName AS ReceiverAttentionName, Receivers.Telephone AS ReceiverTelephone, Receivers.BillingAddress AS ReceiverBillingAddress, ReceiverEntireTerritories.EntireName AS ReceiverEntireTerritoryEntireName, " + "\r\n";
            queryString = queryString + "                       GoodsIssues.GoodsIssueID, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.EntryDate AS GoodsIssueEntryDate, Customers.Code AS GoodsIssueCustomerCode, Customers.Name AS GoodsIssueCustomerName, GoodsIssues.PaymentTermID, GoodsIssues.TradeDiscountRate, GoodsIssues.VATPercent, GoodsIssues.Description, GoodsIssues.Remarks " + "\r\n";            

            queryString = queryString + "       FROM            GoodsIssues " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON GoodsIssues.GoodsIssueID IN (SELECT GoodsIssueID FROM GoodsIssueDetails WHERE Approved = 1 AND LocationID = @LocationID AND EntryDate > DATEADD(day, -30, GetDate()) AND ROUND(Quantity - QuantityInvoice, " + (int)GlobalEnums.rndQuantity + ") > 0 OR ROUND(FreeQuantity - FreeQuantityInvoice, " + (int)GlobalEnums.rndQuantity + ") > 0) AND GoodsIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON GoodsIssues.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ReceiverEntireTerritories ON Receivers.TerritoryID = ReceiverEntireTerritories.TerritoryID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingGoodsIssues", queryString);
        }

        private void GetPendingGoodsIssueConsumers()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, CustomerCategories.PaymentTermID, PENDINGCustomers.TradeDiscountRate, PENDINGCustomers.VATPercent " + "\r\n";

            queryString = queryString + "       FROM            Customers " + "\r\n";
            queryString = queryString + "                       INNER JOIN (SELECT DISTINCT CustomerID, TradeDiscountRate, " + (GlobalEnums.VATbyRow ? "0.0 AS" : "") + " VATPercent FROM GoodsIssueDetails WHERE LocationID = @LocationID AND Approved = 1 AND (ROUND(Quantity - QuantityInvoice, " + (int)GlobalEnums.rndQuantity + ") > 0 OR ROUND(FreeQuantity - FreeQuantityInvoice, " + (int)GlobalEnums.rndQuantity + ") > 0)) PENDINGCustomers ON Customers.CustomerID = PENDINGCustomers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingGoodsIssueConsumers", queryString);
        }


        private void GetPendingGoodsIssueReceivers()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          PENDINGReceivers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, PENDINGReceivers.ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.VATCode AS ReceiverVATCode, Receivers.AttentionName AS ReceiverAttentionName, Receivers.Telephone AS ReceiverTelephone, Receivers.BillingAddress AS ReceiverBillingAddress, ReceiverEntireTerritories.EntireName AS ReceiverEntireTerritoryEntireName, ReceiverCategories.PaymentTermID, PENDINGReceivers.TradeDiscountRate, PENDINGReceivers.VATPercent " + "\r\n";

            queryString = queryString + "       FROM            Customers " + "\r\n";
            queryString = queryString + "                       INNER JOIN (SELECT DISTINCT CustomerID, ReceiverID, TradeDiscountRate, " + (GlobalEnums.VATbyRow ? "0.0 AS" : "") + " VATPercent FROM GoodsIssueDetails WHERE LocationID = @LocationID AND Approved = 1 AND (ROUND(Quantity - QuantityInvoice, " + (int)GlobalEnums.rndQuantity + ") > 0 OR ROUND(FreeQuantity - FreeQuantityInvoice, " + (int)GlobalEnums.rndQuantity + ") > 0)) PENDINGReceivers ON Customers.CustomerID = PENDINGReceivers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON PENDINGReceivers.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ReceiverEntireTerritories ON Receivers.TerritoryID = ReceiverEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN CustomerCategories ReceiverCategories ON Receivers.CustomerCategoryID = ReceiverCategories.CustomerCategoryID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingGoodsIssueReceivers", queryString);
        }


        //WE HAVE 3 OPTIONS TO FILTER GoodsIssues TO ISSUE INVOICE: GetPendingGoodsIssues, GetPendingGoodsIssueConsumers, GetPendingGoodsIssueReceivers
        //ALL OF THESE 3 OPTIONS: WE ALWAYS HAVE A SPECIFIC CustomerID. THIS MEANS: 
        //+GetPendingGoodsIssues: WE CAN ISSUE INVOICE FOR A SPECIFIC GOODSISSUE (OF THE SELECTED CUSTOMER)
        //+GetPendingGoodsIssueConsumers: WE CAN ISSUE INVOICE FOR A SPECIFIC CUSTOMER (COMBINE MULTI GOODSISSUES OF THE SELECTED CUSTOMER)
        //+GetPendingGoodsIssueReceivers: WE CAN ISSUE INVOICE FOR A SPECIFIC CUSTOMER WITH A SPECIFIC RECEIVER (COMBINE MULTI GOODSISSUES OF THE SELECTED CUSTOMER AND THE SELECTED RECEIVER)
        private void GetPendingGoodsIssueDetails()
        {
            string queryString;

            queryString = " @AccountInvoiceID Int, @LocationID Int, @GoodsIssueID Int, @CustomerID Int, @ReceiverID Int, @TradeDiscountRate decimal(18, 2), @VATPercent decimal(18, 2), @CommodityTypeID int, @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime, @GoodsIssueDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@GoodsIssueID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssue(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssue(false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingGoodsIssueDetails", queryString);
        }
        

        private string GetPGIDsBuildSQLGoodsIssue(bool isGoodsIssueID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@CustomerID <> 0 AND @ReceiverID IS NULL) " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueCustomer(isGoodsIssueID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueCustomer(isGoodsIssueID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLGoodsIssueCustomer(bool isGoodsIssueID, bool isCustomerID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@CustomerID <> 0 AND @ReceiverID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueReceiver(isGoodsIssueID, isCustomerID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueReceiver(isGoodsIssueID, isCustomerID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLGoodsIssueReceiver(bool isGoodsIssueID, bool isCustomerID, bool isReceiverID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@CommodityTypeID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueCustomerCommodityType(isGoodsIssueID, isCustomerID, isReceiverID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueCustomerCommodityType(isGoodsIssueID, isCustomerID, isReceiverID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLGoodsIssueCustomerCommodityType(bool isGoodsIssueID, bool isCustomerID, bool isReceiverID, bool isCommodityTypeID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@GoodsIssueDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueCustomerCommodityTypeGoodsIssueDetailIDs(isGoodsIssueID, isCustomerID, isReceiverID, isCommodityTypeID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueCustomerCommodityTypeGoodsIssueDetailIDs(isGoodsIssueID, isCustomerID, isReceiverID, isCommodityTypeID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLGoodsIssueCustomerCommodityTypeGoodsIssueDetailIDs(bool isGoodsIssueID, bool isCustomerID, bool isReceiverID, bool isCommodityTypeID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@AccountInvoiceID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.GetPGIDsBuildSQLNew(isGoodsIssueID, isCustomerID, isReceiverID, isCommodityTypeID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.GetPGIDsBuildSQLEdit(isGoodsIssueID, isCustomerID, isReceiverID, isCommodityTypeID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.GetPGIDsBuildSQLNew(isGoodsIssueID, isCustomerID, isReceiverID, isCommodityTypeID, isGoodsIssueDetailIDs) + " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT GoodsIssueDetailID FROM AccountInvoiceDetails WHERE AccountInvoiceID = @AccountInvoiceID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.GetPGIDsBuildSQLEdit(isGoodsIssueID, isCustomerID, isReceiverID, isCommodityTypeID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLNew(bool isGoodsIssueID, bool isCustomerID, bool isReceiverID, bool isCommodityTypeID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.EntryDate AS GoodsIssueEntryDate, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.BillingAddress, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, GoodsIssueDetails.CalculatingTypeID, GoodsIssueDetails.VATbyRow, ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice, 0) AS QuantityRemains, ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice, 0) AS FreeQuantityRemains, GoodsIssueDetails.ControlFreeQuantity, " + "\r\n";
            queryString = queryString + "                   0.0 AS Quantity, 0.0 AS FreeQuantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.TradeDiscountRate, GoodsIssueDetails.VATPercent, GoodsIssueDetails.ListedGrossPrice, GoodsIssueDetails.GrossPrice, 0.0 AS ListedAmount, 0.0 AS Amount, 0.0 AS ListedVATAmount, 0.0 AS VATAmount, 0.0 AS ListedGrossAmount, 0.0 AS GrossAmount, GoodsIssueDetails.IsBonus, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON " + (isGoodsIssueID ? " GoodsIssueDetails.GoodsIssueID = @GoodsIssueID " : "") + (!isGoodsIssueID && isCustomerID ? " GoodsIssueDetails.CustomerID = @CustomerID " : "") + (!isGoodsIssueID && !isCustomerID && isReceiverID ? " GoodsIssueDetails.CustomerID = @CustomerID AND GoodsIssueDetails.ReceiverID = @ReceiverID " : "") + (!isGoodsIssueID && !isCustomerID && !isReceiverID ? " GoodsIssueDetails.GoodsIssueID IN (SELECT GoodsIssueID FROM GoodsIssues WHERE EntryDate >= @FromDate AND EntryDate <= @ToDate AND LocationID = @LocationID AND OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsIssue + " AND AccessControls.AccessLevel = 2)) " : "") + (!isGoodsIssueID ? " AND GoodsIssueDetails.TradeDiscountRate = @TradeDiscountRate " : "") + (!GlobalEnums.VATbyRow && !isGoodsIssueID ? " AND GoodsIssueDetails.VATPercent = @VATPercent " : "") + " AND (ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice, 0) > 0 OR ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice, 0) > 0) AND GoodsIssueDetails.CommodityID = Commodities.CommodityID AND GoodsIssueDetails.Approved = 1 AND Commodities.IsRegularCheckUps = 0 " + (isCommodityTypeID ? " AND Commodities.CommodityTypeID = @CommodityTypeID" : "") + (isGoodsIssueDetailIDs ? " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsIssueDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON GoodsIssueDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Receivers ON GoodsIssueDetails.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssues ON GoodsIssueDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLEdit(bool isGoodsIssueID, bool isCustomerID, bool isReceiverID, bool isCommodityTypeID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.EntryDate AS GoodsIssueEntryDate, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.BillingAddress, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, AccountInvoiceDetails.CalculatingTypeID, AccountInvoiceDetails.VATbyRow, ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice + AccountInvoiceDetails.Quantity, 0) AS QuantityRemains, ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice + AccountInvoiceDetails.FreeQuantity, 0) AS FreeQuantityRemains, GoodsIssueDetails.ControlFreeQuantity, " + "\r\n";
            queryString = queryString + "                   AccountInvoiceDetails.Quantity, AccountInvoiceDetails.FreeQuantity, AccountInvoiceDetails.ListedPrice, AccountInvoiceDetails.DiscountPercent, AccountInvoiceDetails.UnitPrice, AccountInvoiceDetails.TradeDiscountRate, AccountInvoiceDetails.VATPercent, AccountInvoiceDetails.ListedGrossPrice, AccountInvoiceDetails.GrossPrice, AccountInvoiceDetails.ListedAmount, AccountInvoiceDetails.Amount, AccountInvoiceDetails.ListedVATAmount, AccountInvoiceDetails.VATAmount, AccountInvoiceDetails.ListedGrossAmount, AccountInvoiceDetails.GrossAmount, AccountInvoiceDetails.IsBonus, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN AccountInvoiceDetails ON AccountInvoiceDetails.AccountInvoiceID = @AccountInvoiceID AND GoodsIssueDetails.GoodsIssueDetailID = AccountInvoiceDetails.GoodsIssueDetailID AND GoodsIssueDetails.Approved = 1 " + (!isGoodsIssueID && !isCustomerID && !isReceiverID ? " AND GoodsIssueDetails.GoodsIssueID IN (SELECT GoodsIssueID FROM GoodsIssues WHERE EntryDate >= @FromDate AND EntryDate <= @ToDate AND LocationID = @LocationID AND OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsIssue + " AND AccessControls.AccessLevel = 2)) " : "") + (isGoodsIssueDetailIDs ? " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsIssueDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID AND Commodities.IsRegularCheckUps = 0 " + (isCommodityTypeID ? " AND Commodities.CommodityTypeID = @CommodityTypeID" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON GoodsIssueDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Receivers ON GoodsIssueDetails.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssues ON GoodsIssueDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";

            return queryString;
        }
       

        private void AccountInvoiceSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       UPDATE          GoodsIssueDetails " + "\r\n";
            queryString = queryString + "       SET             GoodsIssueDetails.QuantityInvoice = ROUND(GoodsIssueDetails.QuantityInvoice + AccountInvoiceDetails.Quantity * @SaveRelativeOption, 0), GoodsIssueDetails.FreeQuantityInvoice = ROUND(GoodsIssueDetails.FreeQuantityInvoice + AccountInvoiceDetails.FreeQuantity * @SaveRelativeOption, 0) " + "\r\n";
            queryString = queryString + "       FROM            AccountInvoiceDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                       GoodsIssueDetails ON AccountInvoiceDetails.AccountInvoiceID = @EntityID AND AccountInvoiceDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("AccountInvoiceSaveRelative", queryString);
        }

        private void AccountInvoicePostSaveValidate()
        {
            string[] queryArray = new string[3];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày xuất kho: ' + CAST(GoodsIssueDetails.EntryDate AS nvarchar) FROM AccountInvoiceDetails INNER JOIN GoodsIssueDetails ON AccountInvoiceDetails.AccountInvoiceID = @EntityID AND AccountInvoiceDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID AND AccountInvoiceDetails.EntryDate < GoodsIssueDetails.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Ngày xuất kho: ' + CAST(CAST(GoodsIssueDetails.EntryDate AS Date) AS nvarchar) + N' (Ngày HĐ phải sau ngày xuất kho)' FROM AccountInvoiceDetails INNER JOIN GoodsIssueDetails ON AccountInvoiceDetails.AccountInvoiceID = @EntityID AND AccountInvoiceDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID AND AccountInvoiceDetails.VATInvoiceDate < CAST(GoodsIssueDetails.EntryDate AS Date) ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất hóa đơn vượt quá số lượng xuất kho: ' + CAST(ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) + ' OR free quantity: ' + CAST(ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM AccountInvoiceDetails INNER JOIN GoodsIssueDetails ON AccountInvoiceDetails.AccountInvoiceID = @EntityID AND AccountInvoiceDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID AND (ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice, " + (int)GlobalEnums.rndQuantity + ") < 0 OR ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("AccountInvoicePostSaveValidate", queryArray);
        }



        private void AccountInvoiceInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("AccountInvoices", "AccountInvoiceID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.AccountInvoice));
            this.totalSalesPortalEntities.CreateTrigger("AccountInvoiceInitReference", simpleInitReference.CreateQuery());
        }

        private void AccountInvoiceSheet()
        {
            string queryString = " @AccountInvoiceID int, @GroupByLine bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalAccountInvoiceID int    SET @LocalAccountInvoiceID = @AccountInvoiceID" + "\r\n";
            queryString = queryString + "       DECLARE         @CustomerCategoryID int    SELECT  @CustomerCategoryID = Customers.CustomerCategoryID FROM AccountInvoices INNER JOIN Customers ON AccountInvoices.AccountInvoiceID = @LocalAccountInvoiceID AND AccountInvoices.CustomerID = Customers.CustomerID " + "\r\n";
            
            queryString = queryString + "       SELECT          AccountInvoices.AccountInvoiceID, AccountInvoices.EntryDate, AccountInvoices.Reference, AccountInvoices.VATInvoiceNo, AccountInvoices.VATInvoiceDate, " + "\r\n";
            queryString = queryString + "                       Customers.CustomerID, Customers.OfficialName AS CustomerOfficialName, Customers.VATCode, Customers.BillingAddress, PaymentTerms.Name AS PaymentTermName, " + "\r\n";
            queryString = queryString + "                       AccountInvoiceCollections.AccountInvoiceDetailID, AccountInvoiceCollections.IsFreebie, Commodities.CommodityID, Commodities.Code, Commodities.CodePartA, Commodities.CodePartB, ISNULL(Commodities.CodePartA + CASE WHEN @GroupByLine = 1 THEN '' ELSE ' ' + Commodities.CodePartB END, AccountInvoiceCollections.LineDescription) AS LineDescription, CommoditySKU.CodeSKU, Commodities.OfficialName, Commodities.SalesUnit, " + "\r\n";
            queryString = queryString + "                       AccountInvoiceCollections.Quantity, AccountInvoiceCollections.ListedPrice, AccountInvoiceCollections.ListedAmount, AccountInvoiceCollections.DiscountPercent, ROUND(AccountInvoiceCollections.ListedAmount - AccountInvoiceCollections.Amount, " + (int)GlobalEnums.rndAmount + ") AS DiscountAmount, AccountInvoiceCollections.UnitPrice, AccountInvoiceCollections.Amount, " + (GlobalEnums.VATbyRow ? "AccountInvoiceCollections.VATPercent" : "AccountInvoices.VATPercent") + " AS VATPercent, AccountInvoiceCollections.VATAmount, AccountInvoiceCollections.GrossAmount, " + "\r\n";
            queryString = queryString + "                       AccountInvoices.TotalQuantity, AccountInvoices.TotalAmount, AccountInvoices.TotalVATAmount, AccountInvoices.TotalGrossAmount, dbo.SayVND(AccountInvoices.TotalGrossAmount) AS TotalGrossAmountInWords, AccountInvoices.Description " + "\r\n";

            queryString = queryString + "       FROM            (SELECT AccountInvoiceID, AccountInvoiceDetailID, CommodityID, N'' AS LineDescription, Quantity, ListedPrice, ListedAmount, DiscountPercent, UnitPrice, Amount, VATPercent, VATAmount, GrossAmount, 0 AS IsFreebie FROM AccountInvoiceDetails WHERE AccountInvoiceID = @LocalAccountInvoiceID " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       SELECT AccountInvoiceID, AccountInvoiceDetailID, CommodityID, N'' AS LineDescription, FreeQuantity AS Quantity, 0 AS ListedPrice, 0 AS ListedAmount, 0 AS DiscountPercent, 0 AS UnitPrice, 0 AS Amount, 0 AS VATPercent, 0 AS VATAmount, 0 AS GrossAmount, 1 AS IsFreebie FROM AccountInvoiceDetails WHERE AccountInvoiceID = @LocalAccountInvoiceID AND FreeQuantity <> 0 " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       SELECT AccountInvoiceID, -1 AS AccountInvoiceDetailID, -1 AS CommodityID, N'Chiết khấu: ' + CAST(CAST(TradeDiscountRate AS Decimal(18, 1)) AS nvarchar(10)) + '%' AS LineDescription, 0 AS Quantity, 0 AS ListedPrice, -TradeDiscountAmount AS ListedAmount, 0 AS DiscountPercent, 0 AS UnitPrice, -TradeDiscountAmount AS Amount, 0 AS VATPercent, 0 AS VATAmount, 0 AS GrossAmount, 2 AS IsFreebie FROM AccountInvoices WHERE AccountInvoiceID = @LocalAccountInvoiceID AND TradeDiscountAmount <> 0) AS AccountInvoiceCollections " + "\r\n";
            
            queryString = queryString + "                       INNER JOIN AccountInvoices ON AccountInvoiceCollections.AccountInvoiceID = AccountInvoices.AccountInvoiceID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON AccountInvoices.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN PaymentTerms ON AccountInvoices.PaymentTermID = PaymentTerms.PaymentTermID " + "\r\n";
            queryString = queryString + "                       LEFT JOIN Commodities ON AccountInvoiceCollections.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                       LEFT JOIN CommoditySKU ON CommoditySKU.CustomerCategoryID = @CustomerCategoryID AND Commodities.CodePartA = CommoditySKU.CodePartA AND Commodities.CodePartB = CommoditySKU.CodePartB " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("AccountInvoiceSheet", queryString);
        }

    }
}
