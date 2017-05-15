using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Sales
{
    public class SalesReturn
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public SalesReturn(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetSalesReturnIndexes();

            this.GetSalesReturnViewDetails();

            this.GetSalesReturnPendingCustomers();
            this.GetSalesReturnPendingGoodsIssues();
            this.GetSalesReturnPendingGoodsIssueDetails();

            this.SalesReturnSaveRelative();
            this.SalesReturnPostSaveValidate();

            this.SalesReturnApproved();
            this.SalesReturnEditable();
            this.SalesReturnVoidable();

            this.SalesReturnToggleApproved();

            this.SalesReturnInitReference();

        }


        private void GetSalesReturnIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SalesReturns.SalesReturnID, CAST(SalesReturns.EntryDate AS DATE) AS EntryDate, SalesReturns.Reference, Locations.Code AS LocationCode, Customers.Name AS CustomerName, CASE WHEN SalesReturns.CustomerID = SalesReturns.ReceiverID THEN '' ELSE Receivers.Name + ', ' END + SalesReturns.ShippingAddress AS ReceiverDescription, SalesReturns.GoodsIssueCodes, SalesReturns.Description, SalesReturns.TotalQuantity, SalesReturns.TotalFreeQuantity, SalesReturns.TotalListedGrossAmount, SalesReturns.TotalGrossAmount, SalesReturns.Approved " + "\r\n";
            queryString = queryString + "       FROM        SalesReturns " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON SalesReturns.EntryDate >= @FromDate AND SalesReturns.EntryDate <= @ToDate AND SalesReturns.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.SalesReturn + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = SalesReturns.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON SalesReturns.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Receivers ON SalesReturns.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetSalesReturnIndexes", queryString);
        }

        #region X


        private void GetSalesReturnViewDetails()
        {
            string queryString;

            queryString = " @SalesReturnID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SalesReturnDetails.SalesReturnDetailID, SalesReturnDetails.SalesReturnID, SalesReturnDetails.GoodsIssueID, SalesReturnDetails.GoodsIssueDetailID, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.Code AS GoodsIssueCode, GoodsIssues.EntryDate AS GoodsIssueEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, SalesReturnDetails.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, VoidTypes.VoidTypeID, VoidTypes.Code AS VoidTypeCode, VoidTypes.Name AS VoidTypeName, VoidTypes.VoidClassID, SalesReturnDetails.CalculatingTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(ISNULL(GoodsIssueDetails.Quantity, 0) - ISNULL(GoodsIssueDetails.QuantityReturned, 0) + SalesReturnDetails.Quantity, 0) AS QuantityRemains, ROUND(ISNULL(GoodsIssueDetails.FreeQuantity, 0) - ISNULL(GoodsIssueDetails.FreeQuantityReturned, 0) + SalesReturnDetails.FreeQuantity, 0) AS FreeQuantityRemains, " + "\r\n";
            queryString = queryString + "                   SalesReturnDetails.Quantity, SalesReturnDetails.ControlFreeQuantity, SalesReturnDetails.FreeQuantity, SalesReturnDetails.ListedPrice, SalesReturnDetails.DiscountPercent, SalesReturnDetails.UnitPrice, SalesReturnDetails.VATPercent, SalesReturnDetails.ListedGrossPrice, SalesReturnDetails.GrossPrice, SalesReturnDetails.ListedAmount, SalesReturnDetails.Amount, SalesReturnDetails.ListedVATAmount, SalesReturnDetails.VATAmount, SalesReturnDetails.ListedGrossAmount, SalesReturnDetails.GrossAmount, SalesReturnDetails.IsBonus, SalesReturnDetails.InActivePartial, SalesReturnDetails.InActivePartialDate, SalesReturnDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        SalesReturnDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON SalesReturnDetails.SalesReturnID = @SalesReturnID AND SalesReturnDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Warehouses ON SalesReturnDetails.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN GoodsIssueDetails ON SalesReturnDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN GoodsIssues ON GoodsIssueDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetSalesReturnViewDetails", queryString);
        }





        #region Y

        private void GetSalesReturnPendingGoodsIssues()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          GoodsIssues.GoodsIssueID, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.Code AS GoodsIssueCode, GoodsIssues.EntryDate AS GoodsIssueEntryDate, GoodsIssues.PriceCategoryID, PriceCategories.Code AS PriceCategoryCode, GoodsIssues.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName, GoodsIssues.VATPercent, GoodsIssues.PaymentTermID, GoodsIssues.SalespersonID, Employees.Name AS SalespersonName, GoodsIssues.Description, GoodsIssues.Remarks, " + "\r\n";
            queryString = queryString + "                       GoodsIssues.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName, Customers.Birthday AS CustomerBirthday, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, Customers.ShippingAddress AS CustomerShippingAddress, Customers.TerritoryID AS CustomerTerritoryID, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, Customers.SalespersonID AS CustomerSalespersonID, N'' AS CustomerSalespersonName, Customers.PriceCategoryID AS CustomerPriceCategoryID, N'' AS CustomerPriceCategoryCode, " + "\r\n";
            queryString = queryString + "                       GoodsIssues.ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.OfficialName AS ReceiverOfficialName, Receivers.Birthday AS ReceiverBirthday, Receivers.VATCode AS ReceiverVATCode, Receivers.AttentionName AS ReceiverAttentionName, Receivers.Telephone AS ReceiverTelephone, Receivers.BillingAddress AS ReceiverBillingAddress, Receivers.ShippingAddress AS ReceiverShippingAddress, Receivers.TerritoryID AS ReceiverTerritoryID, ReceiverEntireTerritories.EntireName AS ReceiverEntireTerritoryEntireName, Receivers.SalespersonID AS ReceiverSalespersonID, N'' AS ReceiverSalespersonName, Receivers.PriceCategoryID AS ReceiverPriceCategoryID, N'' AS ReceiverPriceCategoryCode, GoodsIssues.ShippingAddress " + "\r\n";

            queryString = queryString + "       FROM            GoodsIssues " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON GoodsIssues.GoodsIssueID IN (SELECT GoodsIssueID FROM GoodsIssueDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0  AND ROUND(Quantity + FreeQuantity - QuantityReturned - FreeQuantityReturned, " + (int)GlobalEnums.rndQuantity + ") > 0) AND GoodsIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON GoodsIssues.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ReceiverEntireTerritories ON Receivers.TerritoryID = ReceiverEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN PriceCategories ON GoodsIssues.PriceCategoryID = PriceCategories.PriceCategoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses ON GoodsIssues.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Employees ON GoodsIssues.SalespersonID = Employees.EmployeeID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetSalesReturnPendingGoodsIssues", queryString);
        }

        private void GetSalesReturnPendingCustomers()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          CustomerReceiverPENDING.PriceCategoryID, PriceCategories.Code AS PriceCategoryCode, CustomerReceiverPENDING.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName, CustomerReceiverPENDING.VATPercent, CustomerCategories.PaymentTermID, Customers.SalespersonID, Employees.Name AS SalespersonName, N'' AS Description, N'' AS Remarks, " + "\r\n";
            queryString = queryString + "                       CustomerReceiverPENDING.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName, Customers.Birthday AS CustomerBirthday, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, Customers.ShippingAddress AS CustomerShippingAddress, Customers.TerritoryID AS CustomerTerritoryID, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, Customers.SalespersonID AS CustomerSalespersonID, N'' AS CustomerSalespersonName, Customers.PriceCategoryID AS CustomerPriceCategoryID, N'' AS CustomerPriceCategoryCode, " + "\r\n";
            queryString = queryString + "                       CustomerReceiverPENDING.ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.OfficialName AS ReceiverOfficialName, Receivers.Birthday AS ReceiverBirthday, Receivers.VATCode AS ReceiverVATCode, Receivers.AttentionName AS ReceiverAttentionName, Receivers.Telephone AS ReceiverTelephone, Receivers.BillingAddress AS ReceiverBillingAddress, Receivers.ShippingAddress AS ReceiverShippingAddress, Receivers.TerritoryID AS ReceiverTerritoryID, ReceiverEntireTerritories.EntireName AS ReceiverEntireTerritoryEntireName, Receivers.SalespersonID AS ReceiverSalespersonID, N'' AS ReceiverSalespersonName, Receivers.PriceCategoryID AS ReceiverPriceCategoryID, N'' AS ReceiverPriceCategoryCode, CustomerReceiverPENDING.ShippingAddress " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT CustomerID, ReceiverID, PriceCategoryID, WarehouseID, " + (GlobalEnums.VATbyRow ? "0.0 AS" : "") + " VATPercent, ShippingAddress FROM GoodsIssues WHERE GoodsIssueID IN (SELECT GoodsIssueID FROM GoodsIssueDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0  AND ROUND(Quantity + FreeQuantity - QuantityReturned - FreeQuantityReturned, " + (int)GlobalEnums.rndQuantity + ") > 0)) CustomerReceiverPENDING " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON CustomerReceiverPENDING.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON CustomerReceiverPENDING.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ReceiverEntireTerritories ON Receivers.TerritoryID = ReceiverEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN PriceCategories ON CustomerReceiverPENDING.PriceCategoryID = PriceCategories.PriceCategoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses ON CustomerReceiverPENDING.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Employees ON Customers.SalespersonID = Employees.EmployeeID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetSalesReturnPendingCustomers", queryString);
        }



        private void GetSalesReturnPendingGoodsIssueDetails()
        {
            string queryString;

            SqlProgrammability.Inventories.Inventories inventories = new SqlProgrammability.Inventories.Inventories(this.totalSalesPortalEntities);

            queryString = " @LocationID Int, @SalesReturnID Int, @GoodsIssueID Int, @CustomerID Int, @ReceiverID Int, @PriceCategoryID Int, @WarehouseID Int, @ShippingAddress nvarchar(200), @VATPercent decimal(18, 2), @EntryDate DateTime, @GoodsIssueDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @WarehouseIDList varchar(35)         DECLARE @CommodityIDList varchar(3999) " + "\r\n";

            queryString = queryString + "       IF (@GoodsIssueID > 0) ";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID AS varchar)  FROM (SELECT DISTINCT WarehouseID FROM GoodsIssueDetails WHERE GoodsIssueID = @GoodsIssueID) DistinctWarehouses FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "               SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID AS varchar)  FROM GoodsIssueDetails WHERE GoodsIssueID = @GoodsIssueID FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @WarehouseCommodities TABLE (WarehouseID int NOT NULL, CommodityID int NOT NULL) " + "\r\n";
            queryString = queryString + "               INSERT INTO @WarehouseCommodities       SELECT      GoodsIssueDetails.WarehouseID, GoodsIssueDetails.CommodityID                        FROM    GoodsIssueDetails INNER JOIN GoodsIssues ON GoodsIssueDetails.LocationID = @LocationID AND GoodsIssueDetails.CustomerID = @CustomerID AND GoodsIssueDetails.ReceiverID = @ReceiverID AND GoodsIssues.PriceCategoryID = @PriceCategoryID AND GoodsIssues.WarehouseID = @WarehouseID AND GoodsIssues.ShippingAddress = @ShippingAddress " + (GlobalEnums.VATbyRow ? "" : "AND GoodsIssues.VATPercent = @VATPercent") + " AND GoodsIssueDetails.Approved = 1 AND GoodsIssueDetails.InActive = 0 AND GoodsIssueDetails.InActivePartial = 0 AND (ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityReturned, " + (int)GlobalEnums.rndQuantity + ") > 0 OR ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityReturned, " + (int)GlobalEnums.rndQuantity + ") > 0) AND GoodsIssueDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";
            queryString = queryString + "               INSERT INTO @WarehouseCommodities       SELECT      SalesReturnDetails.WarehouseID, SalesReturnDetails.CommodityID                FROM    SalesReturnDetails WHERE SalesReturnID = @SalesReturnID ";

            queryString = queryString + "               SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID AS varchar)  FROM (SELECT DISTINCT WarehouseID FROM @WarehouseCommodities) PendingWarehouses FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "               SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID AS varchar)  FROM (SELECT DISTINCT CommodityID FROM @WarehouseCommodities) PendingCommodities FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "       " + inventories.GET_WarehouseJournal_BUILD_SQL("@CommoditiesBalance", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0") + "\r\n";


            queryString = queryString + "       IF  (@GoodsIssueID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLGoodsIssue(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLGoodsIssue(false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetSalesReturnPendingGoodsIssueDetails", queryString);
        }

        private string BuildSQLGoodsIssue(bool isGoodsIssueID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@GoodsIssueDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLGoodsIssueGoodsIssueDetailIDs(isGoodsIssueID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLGoodsIssueGoodsIssueDetailIDs(isGoodsIssueID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLGoodsIssueGoodsIssueDetailIDs(bool isGoodsIssueID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SalesReturnID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isGoodsIssueID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isGoodsIssueID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLNew(isGoodsIssueID, isGoodsIssueDetailIDs) + " WHERE GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT GoodsIssueDetailID FROM SalesReturnDetails WHERE SalesReturnID = @SalesReturnID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isGoodsIssueID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLNew(bool isGoodsIssueID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.Code AS GoodsIssueCode, GoodsIssues.EntryDate AS GoodsIssueEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, GoodsIssueDetails.CalculatingTypeID, GoodsIssueDetails.VATbyRow, " + "\r\n";
            queryString = queryString + "                   ISNULL(CommoditiesBalance.QuantityBalance, 0) AS QuantityAvailable, ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityReturned, 0) AS QuantityRemains, ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityReturned, 0) AS FreeQuantityRemains, " + "\r\n";
            queryString = queryString + "                   0 AS Quantity, GoodsIssueDetails.ControlFreeQuantity, 0 AS FreeQuantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.VATPercent, GoodsIssueDetails.ListedGrossPrice, GoodsIssueDetails.GrossPrice, 0 AS ListedAmount, 0 AS Amount, 0 AS ListedVATAmount, 0 AS VATAmount, 0 AS ListedGrossAmount, 0 AS GrossAmount, GoodsIssueDetails.IsBonus, GoodsIssues.Description, GoodsIssueDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssues " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssueDetails ON " + (isGoodsIssueID ? " GoodsIssues.GoodsIssueID = @GoodsIssueID " : "GoodsIssues.LocationID = @LocationID AND GoodsIssues.CustomerID = @CustomerID AND GoodsIssues.ReceiverID = @ReceiverID AND GoodsIssues.PriceCategoryID = @PriceCategoryID AND GoodsIssues.WarehouseID = @WarehouseID AND GoodsIssues.ShippingAddress = @ShippingAddress " + (GlobalEnums.VATbyRow ? "" : "AND GoodsIssues.VATPercent = @VATPercent")) + " AND GoodsIssueDetails.Approved = 1 AND GoodsIssueDetails.InActive = 0 AND GoodsIssueDetails.InActivePartial = 0 AND ROUND(GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.QuantityReturned - GoodsIssueDetails.FreeQuantityReturned, " + (int)GlobalEnums.rndQuantity + ") > 0 AND GoodsIssues.GoodsIssueID = GoodsIssueDetails.GoodsIssueID" + (isGoodsIssueDetailIDs ? " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsIssueDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Warehouses ON GoodsIssueDetails.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN @CommoditiesBalance CommoditiesBalance ON GoodsIssueDetails.WarehouseID = CommoditiesBalance.WarehouseID AND GoodsIssueDetails.CommodityID = CommoditiesBalance.CommodityID " + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isGoodsIssueID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";
            //NO NEED TO UNDO QuantityAvailable -THE WAREHOUSE BALANCE- FOR THIS EDIT QUERY: BECAUSE: THIS STORED PROCEDURE ONLY BE CALLED WHEN Approved = 0 => BECAUSE OF THIS (HAVE NOT UPPROVED YET): THIS DELIVERYADVICE QUANTITY DOES NOT EFFECT THE WAREHOUSE BALANCE
            queryString = queryString + "       SELECT      GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.Code AS GoodsIssueCode, GoodsIssues.EntryDate AS GoodsIssueEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, GoodsIssueDetails.CalculatingTypeID, GoodsIssueDetails.VATbyRow, " + "\r\n";
            queryString = queryString + "                   ISNULL(CommoditiesBalance.QuantityBalance, 0) AS QuantityAvailable, ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityReturned + SalesReturnDetails.Quantity, 0) AS QuantityRemains, ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityReturned + SalesReturnDetails.FreeQuantity, 0) AS FreeQuantityRemains, " + "\r\n";
            queryString = queryString + "                   0 AS Quantity, GoodsIssueDetails.ControlFreeQuantity, 0 AS FreeQuantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.VATPercent, GoodsIssueDetails.ListedGrossPrice, GoodsIssueDetails.GrossPrice, 0 AS ListedAmount, 0 AS Amount, 0 AS ListedVATAmount, 0 AS VATAmount, 0 AS ListedGrossAmount, 0 AS GrossAmount, GoodsIssueDetails.IsBonus, GoodsIssues.Description, GoodsIssueDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN SalesReturnDetails ON SalesReturnDetails.SalesReturnID = @SalesReturnID AND GoodsIssueDetails.GoodsIssueDetailID = SalesReturnDetails.GoodsIssueDetailID" + (isGoodsIssueDetailIDs ? " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsIssueDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Warehouses ON GoodsIssueDetails.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssues ON GoodsIssueDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN @CommoditiesBalance CommoditiesBalance ON GoodsIssueDetails.WarehouseID = CommoditiesBalance.WarehouseID AND GoodsIssueDetails.CommodityID = CommoditiesBalance.CommodityID " + "\r\n";

            return queryString;
        }

        #endregion Y




        private void SalesReturnSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   IF (SELECT HasGoodsIssue FROM SalesReturns WHERE SalesReturnID = @EntityID) = 1 " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           UPDATE          GoodsIssueDetails " + "\r\n";
            queryString = queryString + "           SET             GoodsIssueDetails.QuantityReturned = ROUND(GoodsIssueDetails.QuantityReturned + SalesReturnDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), GoodsIssueDetails.FreeQuantityReturned = ROUND(GoodsIssueDetails.FreeQuantityReturned + SalesReturnDetails.FreeQuantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "           FROM            SalesReturnDetails " + "\r\n";
            queryString = queryString + "                           INNER JOIN GoodsIssueDetails ON ((GoodsIssueDetails.Approved = 1 AND GoodsIssueDetails.InActive = 0 AND GoodsIssueDetails.InActivePartial = 0) OR @SaveRelativeOption = -1) AND SalesReturnDetails.SalesReturnID = @EntityID AND SalesReturnDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> (SELECT COUNT(*) FROM SalesReturnDetails WHERE SalesReturnID = @EntityID) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE     @msg NVARCHAR(300) = N'Đơn hàng không tồn tại, chưa duyệt hoặc đã hủy' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            //UPDATE ERmgrVCP.BEGIN
            queryString = queryString + "   EXEC        ERmgrVCP.dbo.SalesReturnSaveRelative @EntityID, @SaveRelativeOption "; //WHEN SAVE: SHOULD ADD TO ERmgrVCP FIRST, THEN CALL SPSKUBalanceUpdate
            //UPDATE ERmgrVCP.END

            this.totalSalesPortalEntities.CreateStoredProcedure("SalesReturnSaveRelative", queryString);

            queryString = " USE ERmgrVCP    GO " + "\r\n";
            queryString = queryString + " DROP PROC SalesReturnSaveRelative " + "\r\n";
            queryString = queryString + " CREATE PROC SalesReturnSaveRelative " + "\r\n";
            queryString = queryString + " @EntityID int, @SaveRelativeOption int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               INSERT INTO SalesReturns SELECT * FROM TotalSalesPortal.dbo.SalesReturns WHERE SalesReturnID = @EntityID " + "\r\n";
            queryString = queryString + "               INSERT INTO SalesReturnDetails SELECT * FROM TotalSalesPortal.dbo.SalesReturnDetails WHERE SalesReturnID = @EntityID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DELETE FROM SalesReturnDetails WHERE SalesReturnID = @EntityID " + "\r\n";
            queryString = queryString + "               DELETE FROM SalesReturns WHERE SalesReturnID = @EntityID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            System.Diagnostics.Debug.WriteLine(queryString);

        }

        private void SalesReturnPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày đặt hàng: ' + CAST(GoodsIssues.EntryDate AS nvarchar) FROM SalesReturnDetails INNER JOIN GoodsIssues ON SalesReturnDetails.SalesReturnID = @EntityID AND SalesReturnDetails.GoodsIssueID = GoodsIssues.GoodsIssueID AND SalesReturnDetails.EntryDate < GoodsIssues.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng đặt hàng: ' + CAST(ROUND(Quantity - QuantityReturned, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) + ' OR free quantity: ' + CAST(ROUND(FreeQuantity - FreeQuantityReturned, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM GoodsIssueDetails WHERE (ROUND(Quantity - QuantityReturned, " + (int)GlobalEnums.rndQuantity + ") < 0) OR (ROUND(FreeQuantity - FreeQuantityReturned, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("SalesReturnPostSaveValidate", queryArray);
        }




        private void SalesReturnApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SalesReturnID FROM SalesReturns WHERE SalesReturnID = @EntityID AND Approved = 1";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("SalesReturnApproved", queryArray);
        }


        private void SalesReturnEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SalesReturnID FROM SalesReturns WHERE SalesReturnID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = SalesReturnID FROM GoodsIssueDetails WHERE SalesReturnID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("SalesReturnEditable", queryArray);
        }

        private void SalesReturnVoidable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SalesReturnID FROM SalesReturns WHERE SalesReturnID = @EntityID AND Approved = 0"; //Must approve in order to allow void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = SalesReturnID FROM GoodsIssueDetails WHERE SalesReturnID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("SalesReturnVoidable", queryArray);
        }


        private void SalesReturnToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      SalesReturns  SET Approved = @Approved, ApprovedDate = GetDate(), InActive = 0, InActivePartial = 0, InActiveDate = NULL, VoidTypeID = NULL WHERE SalesReturnID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          SalesReturnDetails  SET Approved = @Approved, InActive = 0, InActivePartial = 0, InActivePartialDate = NULL, VoidTypeID = NULL WHERE SalesReturnID = @EntityID ; " + "\r\n";

            queryString = queryString + "               UPDATE          ERmgrVCP.dbo.SalesReturns  SET Approved = @Approved, ApprovedDate = GetDate(), InActive = 0, InActivePartial = 0, InActiveDate = NULL, VoidTypeID = NULL WHERE SalesReturnID = @EntityID " + "\r\n";
            queryString = queryString + "               UPDATE          ERmgrVCP.dbo.SalesReturnDetails  SET Approved = @Approved, InActive = 0, InActivePartial = 0, InActivePartialDate = NULL, VoidTypeID = NULL WHERE SalesReturnID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, 'hủy', '')  + ' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("SalesReturnToggleApproved", queryString);
        }

        private void SalesReturnInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("SalesReturns", "SalesReturnID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.SalesReturn));
            this.totalSalesPortalEntities.CreateTrigger("SalesReturnInitReference", simpleInitReference.CreateQuery());
        }


        #endregion
    }
}
