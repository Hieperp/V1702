﻿using TotalBase;
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
            this.GetPendingGoodsIssueDetails();

            this.AccountInvoiceSaveRelative();
            this.AccountInvoicePostSaveValidate();

            this.AccountInvoiceInitReference();
        }

        private void GetAccountInvoiceIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      AccountInvoices.AccountInvoiceID, CAST(AccountInvoices.EntryDate AS DATE) AS EntryDate, AccountInvoices.Reference, AccountInvoices.VATInvoiceNo, Locations.Code AS LocationCode, Customers.Name + ',    ' + Customers.BillingAddress AS CustomerDescription, AccountInvoices.Description, AccountInvoices.TotalGrossAmount " + "\r\n";
            queryString = queryString + "       FROM        AccountInvoices INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON AccountInvoices.EntryDate >= @FromDate AND AccountInvoices.EntryDate <= @ToDate AND AccountInvoices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.AccountInvoice + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = AccountInvoices.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON AccountInvoices.CustomerID = Customers.CustomerID " + "\r\n";
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

            queryString = queryString + "       SELECT      AccountInvoiceDetails.AccountInvoiceDetailID, AccountInvoiceDetails.AccountInvoiceID, AccountInvoiceDetails.GoodsIssueID, AccountInvoiceDetails.GoodsIssueDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice + AccountInvoiceDetails.Quantity, 0) AS QuantityRemains, ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice + AccountInvoiceDetails.FreeQuantity, 0) AS FreeQuantityRemains, GoodsIssueDetails.ControlFreeQuantity, " + "\r\n";
            queryString = queryString + "                   AccountInvoiceDetails.Quantity, AccountInvoiceDetails.FreeQuantity, AccountInvoiceDetails.ListedPrice, AccountInvoiceDetails.DiscountPercent, AccountInvoiceDetails.UnitPrice, AccountInvoiceDetails.VATPercent, AccountInvoiceDetails.GrossPrice, AccountInvoiceDetails.Amount, AccountInvoiceDetails.VATAmount, AccountInvoiceDetails.GrossAmount, AccountInvoiceDetails.IsBonus, AccountInvoiceDetails.Remarks" + "\r\n";           

            queryString = queryString + "       FROM        AccountInvoiceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails ON AccountInvoiceDetails.AccountInvoiceID = @AccountInvoiceID AND AccountInvoiceDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetAccountInvoiceViewDetails", queryString);
        }










        private void GetPendingGoodsIssues()
        {
            string queryString = " @LocationID int, @AccountInvoiceID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, " + "\r\n";
            queryString = queryString + "                       GoodsIssues.GoodsIssueID, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.EntryDate AS GoodsIssueEntryDate, GoodsIssues.Description, GoodsIssues.Remarks, " + "\r\n";
            queryString = queryString + "                       Receivers.Code AS GoodsIssueReceiverCode, Receivers.Name AS GoodsIssueReceiverName " + "\r\n";

            queryString = queryString + "       FROM            GoodsIssues " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON GoodsIssues.LocationID = @LocationID AND GoodsIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON GoodsIssues.ReceiverID = Receivers.CustomerID " + "\r\n";

            queryString = queryString + "       WHERE           GoodsIssues.GoodsIssueID IN  " + "\r\n";

            queryString = queryString + "                      (SELECT GoodsIssueID FROM GoodsIssueDetails WHERE ROUND(Quantity - QuantityInvoice, 0) > 0 OR ROUND(FreeQuantity - FreeQuantityInvoice, 0) > 0 " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       SELECT GoodsIssueID FROM AccountInvoiceDetails WHERE AccountInvoiceID = @AccountInvoiceID)  " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingGoodsIssues", queryString);
        }

        private void GetPendingGoodsIssueConsumers()
        {
            string queryString = " @LocationID int, @AccountInvoiceID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT CustomerID FROM " + "\r\n";
            queryString = queryString + "                              (SELECT CustomerID FROM GoodsIssueDetails WHERE LocationID = @LocationID AND (ROUND(Quantity - QuantityInvoice, 0) > 0  OR ROUND(FreeQuantity - FreeQuantityInvoice, 0) > 0) " + "\r\n";
            queryString = queryString + "                               UNION ALL " + "\r\n";
            queryString = queryString + "                               SELECT CustomerID FROM AccountInvoices WHERE AccountInvoiceID = @AccountInvoiceID) CustomerReceiverPENDING " + "\r\n";
            queryString = queryString + "                      )CustomerUNION " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Customers ON CustomerUNION.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingGoodsIssueConsumers", queryString);
        }



        private void GetPendingGoodsIssueDetails()
        {
            string queryString;

            queryString = " @AccountInvoiceID Int, @LocationID Int, @GoodsIssueID Int, @CustomerID Int, @CommodityTypeID int, @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime, @GoodsIssueDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
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
            queryString = queryString + "       IF  (@CustomerID <> 0) " + "\r\n";
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
            queryString = queryString + "       IF  (@CommodityTypeID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueCustomerCommodityType(isGoodsIssueID, isCustomerID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueCustomerCommodityType(isGoodsIssueID, isCustomerID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLGoodsIssueCustomerCommodityType(bool isGoodsIssueID, bool isCustomerID, bool isCommodityTypeID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@GoodsIssueDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueCustomerCommodityTypeGoodsIssueDetailIDs(isGoodsIssueID, isCustomerID, isCommodityTypeID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueCustomerCommodityTypeGoodsIssueDetailIDs(isGoodsIssueID, isCustomerID, isCommodityTypeID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLGoodsIssueCustomerCommodityTypeGoodsIssueDetailIDs(bool isGoodsIssueID, bool isCustomerID, bool isCommodityTypeID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@AccountInvoiceID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.GetPGIDsBuildSQLNew (isGoodsIssueID, isCustomerID, isCommodityTypeID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.GetPGIDsBuildSQLEdit(isGoodsIssueID, isCustomerID, isCommodityTypeID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.GetPGIDsBuildSQLNew(isGoodsIssueID, isCustomerID, isCommodityTypeID, isGoodsIssueDetailIDs) + " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT GoodsIssueDetailID FROM AccountInvoiceDetails WHERE AccountInvoiceID = @AccountInvoiceID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.GetPGIDsBuildSQLEdit(isGoodsIssueID, isCustomerID, isCommodityTypeID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLNew(bool isGoodsIssueID, bool isCustomerID, bool isCommodityTypeID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      GoodsIssues.EntryDate, GoodsIssues.Reference, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.BillingAddress, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice, 0) AS QuantityRemains, ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice, 0) AS FreeQuantityRemains, GoodsIssueDetails.ControlFreeQuantity, " + "\r\n";
            queryString = queryString + "                   0.0 AS Quantity, 0.0 AS FreeQuantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.VATPercent, GoodsIssueDetails.GrossPrice, 0.0 AS Amount, 0.0 AS VATAmount, 0.0 AS GrossAmount, GoodsIssueDetails.IsBonus, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON " + (isGoodsIssueID ? " GoodsIssueDetails.GoodsIssueID = @GoodsIssueID " : "") + (!isGoodsIssueID && isCustomerID ? " GoodsIssueDetails.CustomerID = @CustomerID " : "") + (!isGoodsIssueID && !isCustomerID ? " GoodsIssueDetails.GoodsIssueID IN (SELECT GoodsIssueID FROM GoodsIssues WHERE EntryDate >= @FromDate AND EntryDate <= @ToDate AND LocationID = @LocationID AND OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsIssue + " AND AccessControls.AccessLevel = 2)) " : "") + " AND (ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice, 0) > 0 OR ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice, 0) > 0) AND GoodsIssueDetails.CommodityID = Commodities.CommodityID AND Commodities.IsRegularCheckUps = 0 " + (isCommodityTypeID ? " AND Commodities.CommodityTypeID = @CommodityTypeID" : "") + (isGoodsIssueDetailIDs ? " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsIssueDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON GoodsIssueDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Receivers ON GoodsIssueDetails.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssues ON GoodsIssueDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLEdit(bool isGoodsIssueID, bool isCustomerID, bool isCommodityTypeID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      GoodsIssues.EntryDate, GoodsIssues.Reference, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.BillingAddress, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice + AccountInvoiceDetails.Quantity, 0) AS QuantityRemains, ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice + AccountInvoiceDetails.FreeQuantity, 0) AS FreeQuantityRemains, GoodsIssueDetails.ControlFreeQuantity, " + "\r\n";
            queryString = queryString + "                   AccountInvoiceDetails.Quantity, AccountInvoiceDetails.FreeQuantity, AccountInvoiceDetails.ListedPrice, AccountInvoiceDetails.DiscountPercent, AccountInvoiceDetails.UnitPrice, AccountInvoiceDetails.VATPercent, AccountInvoiceDetails.GrossPrice, AccountInvoiceDetails.Amount, AccountInvoiceDetails.VATAmount, AccountInvoiceDetails.GrossAmount, AccountInvoiceDetails.IsBonus, CAST(1 AS bit) AS IsSelected " + "\r\n";            

            queryString = queryString + "       FROM        GoodsIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN AccountInvoiceDetails ON AccountInvoiceDetails.AccountInvoiceID = @AccountInvoiceID AND GoodsIssueDetails.GoodsIssueDetailID = AccountInvoiceDetails.GoodsIssueDetailID AND " + (isGoodsIssueID ? " GoodsIssueDetails.GoodsIssueID = @GoodsIssueID " : "") + (!isGoodsIssueID && isCustomerID ? " GoodsIssueDetails.CustomerID = @CustomerID " : "") + (!isGoodsIssueID && !isCustomerID ? " GoodsIssueDetails.GoodsIssueID IN (SELECT GoodsIssueID FROM GoodsIssues WHERE EntryDate >= @FromDate AND EntryDate <= @ToDate AND LocationID = @LocationID AND OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsIssue + " AND AccessControls.AccessLevel = 2)) " : "") + (isGoodsIssueDetailIDs ? " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsIssueDetailIDs))" : "") + "\r\n";
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
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày bán hàng: ' + CAST(GoodsIssueDetails.EntryDate AS nvarchar) FROM AccountInvoiceDetails INNER JOIN GoodsIssueDetails ON AccountInvoiceDetails.AccountInvoiceID = @EntityID AND AccountInvoiceDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID AND AccountInvoiceDetails.EntryDate < GoodsIssueDetails.EntryDate ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("AccountInvoicePostSaveValidate", queryArray);
        }



        private void AccountInvoiceInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("AccountInvoices", "AccountInvoiceID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.AccountInvoice));
            this.totalSalesPortalEntities.CreateTrigger("AccountInvoiceInitReference", simpleInitReference.CreateQuery());
        }
    }
}
