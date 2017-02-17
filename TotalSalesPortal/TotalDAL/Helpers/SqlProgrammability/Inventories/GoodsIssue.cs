﻿using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{

    public class GoodsIssue
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public GoodsIssue(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetGoodsIssueIndexes();

            this.GetPendingDeliveryAdvices();
            this.GetPendingDeliveryAdviceCustomers();

            this.GetGoodsIssueViewDetails();

            this.GoodsIssueSaveRelative();
            this.GoodsIssuePostSaveValidate();

            this.GoodsIssueEditable();
            this.GoodsIssueApproved();

            this.GoodsIssueToggleApproved();

            this.GoodsIssueInitReference();

            this.GoodsIssueSheet();
        }

        private void GetGoodsIssueIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      GoodsIssues.GoodsIssueID, CAST(GoodsIssues.EntryDate AS DATE) AS EntryDate, GoodsIssues.Reference, Locations.Code AS LocationCode, Customers.Name AS CustomerName, CASE WHEN GoodsIssues.CustomerID = GoodsIssues.ReceiverID THEN '' ELSE Receivers.Name + ', ' END + GoodsIssues.ShippingAddress AS ReceiverDescription, GoodsIssues.DeliveryAdviceReferences, GoodsIssues.TotalQuantity, GoodsIssues.TotalFreeQuantity, GoodsIssues.TotalListedGrossAmount, GoodsIssues.TotalGrossAmount, GoodsIssues.Description, GoodsIssues.Approved " + "\r\n";
            queryString = queryString + "       FROM        GoodsIssues " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON GoodsIssues.EntryDate >= @FromDate AND GoodsIssues.EntryDate <= @ToDate AND GoodsIssues.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsIssue + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = GoodsIssues.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Customers ON GoodsIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Receivers ON GoodsIssues.ReceiverID = Receivers.CustomerID " + "\r\n";            
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetGoodsIssueIndexes", queryString);
        }

        private void GetPendingDeliveryAdvices()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          DeliveryAdvices.DeliveryAdviceID, DeliveryAdvices.Reference AS DeliveryAdviceReference, DeliveryAdvices.EntryDate AS DeliveryAdviceEntryDate, DeliveryAdvices.Description, DeliveryAdvices.Remarks, " + "\r\n";
            queryString = queryString + "                       DeliveryAdvices.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, " + "\r\n";
            queryString = queryString + "                       DeliveryAdvices.ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.VATCode AS ReceiverVATCode, Receivers.AttentionName AS ReceiverAttentionName, Receivers.Telephone AS ReceiverTelephone, Receivers.BillingAddress AS ReceiverBillingAddress, ReceiverEntireTerritories.EntireName AS ReceiverEntireTerritoryEntireName, DeliveryAdvices.ShippingAddress " + "\r\n";

            queryString = queryString + "       FROM            DeliveryAdvices " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON DeliveryAdvices.DeliveryAdviceID IN (SELECT DeliveryAdviceID FROM DeliveryAdviceDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0 AND InActiveIssue = 0 AND (ROUND(Quantity - QuantityIssue, " + (int)GlobalEnums.rndQuantity + ") > 0 OR ROUND(FreeQuantity - FreeQuantityIssue, " + (int)GlobalEnums.rndQuantity + ") > 0)) AND DeliveryAdvices.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON DeliveryAdvices.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ReceiverEntireTerritories ON Receivers.TerritoryID = ReceiverEntireTerritories.TerritoryID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingDeliveryAdvices", queryString);
        }

        private void GetPendingDeliveryAdviceCustomers()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Customers.CustomerID AS CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, " + "\r\n";
            queryString = queryString + "                       Receivers.CustomerID AS ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.VATCode AS ReceiverVATCode, Receivers.AttentionName AS ReceiverAttentionName, Receivers.Telephone AS ReceiverTelephone, Receivers.BillingAddress AS ReceiverBillingAddress, ReceiverEntireTerritories.EntireName AS ReceiverEntireTerritoryEntireName, CustomerReceiverPENDING.ShippingAddress " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT CustomerID, ReceiverID, ShippingAddress FROM DeliveryAdvices WHERE DeliveryAdviceID IN (SELECT DeliveryAdviceID FROM DeliveryAdviceDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0 AND InActiveIssue = 0 AND (ROUND(Quantity - QuantityIssue, " + (int)GlobalEnums.rndQuantity + ") > 0 OR ROUND(FreeQuantity - FreeQuantityIssue, " + (int)GlobalEnums.rndQuantity + ") > 0))) CustomerReceiverPENDING " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON CustomerReceiverPENDING.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON CustomerReceiverPENDING.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ReceiverEntireTerritories ON Receivers.TerritoryID = ReceiverEntireTerritories.TerritoryID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingDeliveryAdviceCustomers", queryString);
        }


        #region GetGoodsIssueViewDetails

        private void GetGoodsIssueViewDetails()
        {
            string queryString;

            SqlProgrammability.Inventories.Inventories inventories = new Inventories(this.totalSalesPortalEntities);

            queryString = " @GoodsIssueID Int, @LocationID Int, @DeliveryAdviceID Int, @CustomerID Int, @ReceiverID Int, @ShippingAddress nvarchar(200), @IsReadonly bit " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @EntryDate DateTime       DECLARE @WarehouseIDList varchar(35)         DECLARE @CommodityIDList varchar(3999) " + "\r\n";

            queryString = queryString + "       IF (@GoodsIssueID > 0) ";
            queryString = queryString + "           SELECT  @EntryDate = EntryDate FROM GoodsIssues WHERE GoodsIssueID = @GoodsIssueID " + "\r\n";
            queryString = queryString + "       IF (@EntryDate IS NULL) ";
            queryString = queryString + "           SELECT  @EntryDate = GetDate() " + "\r\n";

            queryString = queryString + "       IF (@DeliveryAdviceID > 0) ";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID AS varchar)  FROM DeliveryAdviceDetails WHERE DeliveryAdviceID = @DeliveryAdviceID FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "               SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID AS varchar)  FROM DeliveryAdviceDetails WHERE DeliveryAdviceID = @DeliveryAdviceID FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @WarehouseCommodities TABLE (WarehouseID int NOT NULL, CommodityID int NOT NULL) " + "\r\n";
            queryString = queryString + "               INSERT INTO @WarehouseCommodities       SELECT      DeliveryAdviceDetails.WarehouseID, DeliveryAdviceDetails.CommodityID        FROM    DeliveryAdviceDetails INNER JOIN DeliveryAdvices ON DeliveryAdviceDetails.LocationID = @LocationID AND DeliveryAdviceDetails.CustomerID = @CustomerID AND DeliveryAdviceDetails.ReceiverID = @ReceiverID AND DeliveryAdvices.ShippingAddress = @ShippingAddress AND DeliveryAdviceDetails.Approved = 1 AND DeliveryAdviceDetails.InActive = 0 AND DeliveryAdviceDetails.InActivePartial = 0 AND DeliveryAdviceDetails.InActiveIssue = 0 AND (ROUND(DeliveryAdviceDetails.Quantity - DeliveryAdviceDetails.QuantityIssue, " + (int)GlobalEnums.rndQuantity + ") > 0 OR ROUND(DeliveryAdviceDetails.FreeQuantity - DeliveryAdviceDetails.FreeQuantityIssue, " + (int)GlobalEnums.rndQuantity + ") > 0) AND DeliveryAdviceDetails.DeliveryAdviceID = DeliveryAdvices.DeliveryAdviceID " + "\r\n";
            queryString = queryString + "               INSERT INTO @WarehouseCommodities       SELECT      GoodsIssueDetails.WarehouseID, GoodsIssueDetails.CommodityID        FROM    GoodsIssueDetails WHERE GoodsIssueID = @GoodsIssueID ";

            queryString = queryString + "               SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID AS varchar)  FROM (SELECT DISTINCT WarehouseID FROM @WarehouseCommodities) PendingWarehouses FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "               SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID AS varchar)  FROM (SELECT DISTINCT CommodityID FROM @WarehouseCommodities) PendingCommodities FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "       " + inventories.GET_WarehouseJournal_BUILD_SQL("@WarehouseJournalTable", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0") + "\r\n";

            queryString = queryString + "       IF (@DeliveryAdviceID > 0) ";
            queryString = queryString + "           " + this.BuildSQL(true);
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           " + this.BuildSQL(false);

            queryString = queryString + "   END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetGoodsIssueViewDetails", queryString);

        }

        private string BuildSQL(bool isDeliveryAdviceID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF (@GoodsIssueID <= 0) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               " + this.BuildSQLNew(isDeliveryAdviceID) + "\r\n";
            queryString = queryString + "               ORDER BY Commodities.CommodityTypeID, Commodities.Code, DeliveryAdviceDetails.EntryDate, DeliveryAdviceDetails.DeliveryAdviceID, DeliveryAdviceDetails.DeliveryAdviceDetailID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "           IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLEdit(isDeliveryAdviceID) + "\r\n";
            queryString = queryString + "                   ORDER BY Commodities.CommodityTypeID, Commodities.Code, DeliveryAdviceDetails.EntryDate, DeliveryAdviceDetails.DeliveryAdviceID, DeliveryAdviceDetails.DeliveryAdviceDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "           ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isDeliveryAdviceID) + " WHERE DeliveryAdviceDetails.DeliveryAdviceDetailID NOT IN (SELECT DeliveryAdviceDetailID FROM GoodsIssueDetails WHERE GoodsIssueID = @GoodsIssueID) " + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLEdit(isDeliveryAdviceID) + "\r\n";
            queryString = queryString + "                   ORDER BY Commodities.CommodityTypeID, Commodities.Code, DeliveryAdviceDetails.EntryDate, DeliveryAdviceDetails.DeliveryAdviceID, DeliveryAdviceDetails.DeliveryAdviceDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLNew(bool isDeliveryAdviceID)
        {
            string queryString;

            queryString = "                     SELECT          DeliveryAdviceDetails.DeliveryAdviceID, DeliveryAdvices.Reference AS DeliveryAdviceReference, DeliveryAdviceDetails.EntryDate AS DeliveryAdviceDate, 0 AS GoodsIssueDetailID, 0 AS GoodsIssueID, DeliveryAdviceDetails.DeliveryAdviceDetailID, NULL AS VoidTypeID, CAST(NULL AS nvarchar(50)) AS VoidTypeCode, CAST(NULL AS nvarchar(50)) AS VoidTypeName, NULL AS VoidClassID, " + "\r\n";
            queryString = queryString + "                       DeliveryAdviceDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, DeliveryAdviceDetails.CalculatingTypeID, ROUND(DeliveryAdviceDetails.Quantity - DeliveryAdviceDetails.QuantityIssue, 0) AS QuantityRemains, ROUND(DeliveryAdviceDetails.FreeQuantity - DeliveryAdviceDetails.FreeQuantityIssue, 0) AS FreeQuantityRemains, IIF(Commodities.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Services + ", DeliveryAdviceDetails.Quantity + DeliveryAdviceDetails.FreeQuantity, CAST(ISNULL(CommoditiesAvailable.QuantityAvailable, 0) AS decimal(18, 2))) AS QuantityAvailable, DeliveryAdviceDetails.ControlFreeQuantity, " + "\r\n";
            queryString = queryString + "                       0.0 AS Quantity, 0.0 AS FreeQuantity, DeliveryAdviceDetails.ListedPrice, DeliveryAdviceDetails.DiscountPercent, DeliveryAdviceDetails.UnitPrice, DeliveryAdviceDetails.VATPercent, DeliveryAdviceDetails.ListedGrossPrice, DeliveryAdviceDetails.GrossPrice, 0.0 AS ListedAmount, 0.0 AS Amount, 0.0 AS ListedVATAmount, 0.0 AS VATAmount, 0.0 AS ListedGrossAmount, 0.0 AS GrossAmount, DeliveryAdviceDetails.IsBonus, DeliveryAdviceDetails.Remarks " + "\r\n";

            queryString = queryString + "       FROM            DeliveryAdviceDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN " + (isDeliveryAdviceID ? "Commodities" : "DeliveryAdvices") + " ON " + (isDeliveryAdviceID ? "DeliveryAdviceDetails.DeliveryAdviceID = @DeliveryAdviceID" : "DeliveryAdviceDetails.LocationID = @LocationID AND DeliveryAdviceDetails.CustomerID = @CustomerID AND DeliveryAdviceDetails.ReceiverID = @ReceiverID AND DeliveryAdvices.ShippingAddress = @ShippingAddress") + " AND DeliveryAdviceDetails.Approved = 1 AND DeliveryAdviceDetails.InActive = 0 AND DeliveryAdviceDetails.InActivePartial = 0 AND DeliveryAdviceDetails.InActiveIssue = 0 AND (ROUND(DeliveryAdviceDetails.Quantity - DeliveryAdviceDetails.QuantityIssue, " + (int)GlobalEnums.rndQuantity + ") > 0 OR ROUND(DeliveryAdviceDetails.FreeQuantity - DeliveryAdviceDetails.FreeQuantityIssue, " + (int)GlobalEnums.rndQuantity + ") > 0) AND " + (isDeliveryAdviceID ? "DeliveryAdviceDetails.CommodityID = Commodities.CommodityID" : "DeliveryAdviceDetails.DeliveryAdviceID = DeliveryAdvices.DeliveryAdviceID") + "\r\n";
            queryString = queryString + "                       " + (isDeliveryAdviceID ? "" : "INNER JOIN Commodities ON DeliveryAdviceDetails.CommodityID = Commodities.CommodityID ") + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses ON DeliveryAdviceDetails.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                       " + (isDeliveryAdviceID ? "INNER JOIN DeliveryAdvices ON DeliveryAdviceDetails.DeliveryAdviceID = DeliveryAdvices.DeliveryAdviceID " : "") + "\r\n";
            queryString = queryString + "                       LEFT JOIN (SELECT WarehouseID, CommodityID, SUM(QuantityBegin) AS QuantityAvailable FROM @WarehouseJournalTable GROUP BY WarehouseID, CommodityID) CommoditiesAvailable ON DeliveryAdviceDetails.WarehouseID = CommoditiesAvailable.WarehouseID AND DeliveryAdviceDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isDeliveryAdviceID)
        {
            string queryString;

            queryString = "                     SELECT          DeliveryAdviceDetails.DeliveryAdviceID, DeliveryAdvices.Reference AS DeliveryAdviceReference, DeliveryAdviceDetails.EntryDate AS DeliveryAdviceDate, GoodsIssueDetails.GoodsIssueDetailID, GoodsIssueDetails.GoodsIssueID, DeliveryAdviceDetails.DeliveryAdviceDetailID, VoidTypes.VoidTypeID, VoidTypes.Code AS VoidTypeCode, VoidTypes.Name AS VoidTypeName, VoidTypes.VoidClassID, " + "\r\n";
            queryString = queryString + "                       DeliveryAdviceDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, GoodsIssueDetails.CalculatingTypeID, ROUND(DeliveryAdviceDetails.Quantity - DeliveryAdviceDetails.QuantityIssue + GoodsIssueDetails.Quantity, 0) AS QuantityRemains, ROUND(DeliveryAdviceDetails.FreeQuantity - DeliveryAdviceDetails.FreeQuantityIssue + GoodsIssueDetails.FreeQuantity, 0) AS FreeQuantityRemains, IIF(Commodities.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Services + ", DeliveryAdviceDetails.Quantity + DeliveryAdviceDetails.FreeQuantity, ROUND(CAST(ISNULL(CommoditiesAvailable.QuantityAvailable, 0) AS decimal(18, 2)) + GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity, 0)) AS QuantityAvailable, DeliveryAdviceDetails.ControlFreeQuantity, " + "\r\n";
            queryString = queryString + "                       GoodsIssueDetails.Quantity, GoodsIssueDetails.FreeQuantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.VATPercent, GoodsIssueDetails.ListedGrossPrice, GoodsIssueDetails.GrossPrice, GoodsIssueDetails.ListedAmount, GoodsIssueDetails.Amount, GoodsIssueDetails.ListedVATAmount, GoodsIssueDetails.VATAmount, GoodsIssueDetails.ListedGrossAmount, GoodsIssueDetails.GrossAmount, GoodsIssueDetails.IsBonus, GoodsIssueDetails.Remarks " + "\r\n";

            queryString = queryString + "       FROM            GoodsIssueDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN DeliveryAdviceDetails ON GoodsIssueDetails.GoodsIssueID = @GoodsIssueID AND GoodsIssueDetails.DeliveryAdviceDetailID = DeliveryAdviceDetails.DeliveryAdviceDetailID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses ON DeliveryAdviceDetails.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                       INNER JOIN DeliveryAdvices ON DeliveryAdviceDetails.DeliveryAdviceID = DeliveryAdvices.DeliveryAdviceID " + "\r\n";
            queryString = queryString + "                       LEFT JOIN VoidTypes ON GoodsIssueDetails.VoidTypeID = VoidTypes.VoidTypeID " + "\r\n";
            queryString = queryString + "                       LEFT JOIN (SELECT WarehouseID, CommodityID, SUM(QuantityBegin) AS QuantityAvailable FROM @WarehouseJournalTable GROUP BY WarehouseID, CommodityID) CommoditiesAvailable ON DeliveryAdviceDetails.WarehouseID = CommoditiesAvailable.WarehouseID AND DeliveryAdviceDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n";

            return queryString;
        }

        #endregion GetGoodsIssueViewDetails


        private void GoodsIssueSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE          DeliveryAdviceDetails " + "\r\n";
            queryString = queryString + "       SET             DeliveryAdviceDetails.QuantityIssue = ROUND(DeliveryAdviceDetails.QuantityIssue + GoodsIssueDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), DeliveryAdviceDetails.FreeQuantityIssue = ROUND(DeliveryAdviceDetails.FreeQuantityIssue + GoodsIssueDetails.FreeQuantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "       FROM            GoodsIssueDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN DeliveryAdviceDetails ON ((DeliveryAdviceDetails.Approved = 1 AND DeliveryAdviceDetails.InActive = 0 AND DeliveryAdviceDetails.InActivePartial = 0 AND DeliveryAdviceDetails.InActiveIssue = 0) OR @SaveRelativeOption = -1) AND (GoodsIssueDetails.Quantity <> 0 OR GoodsIssueDetails.FreeQuantity <> 0) AND GoodsIssueDetails.GoodsIssueID = @EntityID AND GoodsIssueDetails.DeliveryAdviceDetailID = DeliveryAdviceDetails.DeliveryAdviceDetailID " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = (SELECT COUNT(*) FROM GoodsIssueDetails WHERE GoodsIssueID = @EntityID AND (GoodsIssueDetails.Quantity <> 0 OR GoodsIssueDetails.FreeQuantity <> 0)) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE      DeliveryAdvices SET DeliveryAdvices.TotalQuantityIssue = ROUND(TotalDeliveryAdviceDetails.TotalQuantityIssue, " + (int)GlobalEnums.rndQuantity + "), DeliveryAdvices.TotalFreeQuantityIssue = ROUND(TotalDeliveryAdviceDetails.TotalFreeQuantityIssue, " + (int)GlobalEnums.rndQuantity + ") FROM DeliveryAdvices INNER JOIN (SELECT DeliveryAdviceID, SUM(QuantityIssue) AS TotalQuantityIssue, SUM(FreeQuantityIssue) AS TotalFreeQuantityIssue FROM DeliveryAdviceDetails WHERE DeliveryAdviceID IN (SELECT DeliveryAdviceID FROM GoodsIssueDetails WHERE GoodsIssueID = @EntityID) GROUP BY DeliveryAdviceID) TotalDeliveryAdviceDetails ON DeliveryAdvices.DeliveryAdviceID = TotalDeliveryAdviceDetails.DeliveryAdviceID; " + "\r\n";

            queryString = queryString + "               UPDATE      DeliveryAdviceDetails SET DeliveryAdviceDetails.InActiveIssue = 0 WHERE DeliveryAdviceDetails.DeliveryAdviceDetailID IN (SELECT DeliveryAdviceDetailID FROM GoodsIssueDetails WHERE GoodsIssueID = @EntityID) " + "\r\n";
            queryString = queryString + "               UPDATE      DeliveryAdviceDetails SET DeliveryAdviceDetails.InActiveIssue = DeliveryAdviceInActiveIssue.InActiveIssue " + "\r\n";
            queryString = queryString + "               FROM        DeliveryAdviceDetails " + "\r\n";
            queryString = queryString + "                           INNER JOIN (SELECT GoodsIssueDetails.DeliveryAdviceDetailID, MAX(CAST(VoidTypes.InActive AS int)) AS InActiveIssue FROM GoodsIssueDetails INNER JOIN VoidTypes ON GoodsIssueDetails.VoidTypeID = VoidTypes.VoidTypeID AND GoodsIssueDetails.GoodsIssueID <> (@EntityID * -@SaveRelativeOption) AND GoodsIssueDetails.DeliveryAdviceDetailID IN (SELECT DeliveryAdviceDetailID FROM GoodsIssueDetails WHERE GoodsIssueID = @EntityID) GROUP BY GoodsIssueDetails.DeliveryAdviceDetailID) AS DeliveryAdviceInActiveIssue " + "\r\n"; //THIS GoodsIssueDetails.GoodsIssueID <> (@EntityID * -@SaveRelativeOption) => EXCLUSIVE @EntityID WHEN UNDO
            queryString = queryString + "                           ON DeliveryAdviceDetails.DeliveryAdviceDetailID = DeliveryAdviceInActiveIssue.DeliveryAdviceDetailID " + "\r\n";

            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Đề nghị giao hàng không tồn tại, chưa duyệt hoặc đã hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            //queryString = queryString + "       SET             @SaveRelativeOption = -@SaveRelativeOption" + "\r\n";
            //queryString = queryString + "       EXEC            UpdateWarehouseBalance @SaveRelativeOption, 0, 0, @EntityID, 0 ";

            this.totalSalesPortalEntities.CreateStoredProcedure("GoodsIssueSaveRelative", queryString);

        }

        private void GoodsIssuePostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày phiếu yêu cầu: ' + CAST(DeliveryAdvices.EntryDate AS nvarchar) FROM GoodsIssueDetails INNER JOIN DeliveryAdvices ON GoodsIssueDetails.GoodsIssueID = @EntityID AND GoodsIssueDetails.DeliveryAdviceID = DeliveryAdvices.DeliveryAdviceID AND GoodsIssueDetails.EntryDate < DeliveryAdvices.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất kho vượt quá số lượng yêu cầu: ' + CAST(ROUND(Quantity - QuantityIssue, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) + ' OR free quantity: ' + CAST(ROUND(FreeQuantity - FreeQuantityIssue, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM DeliveryAdviceDetails WHERE (ROUND(Quantity - QuantityIssue, " + (int)GlobalEnums.rndQuantity + ") < 0) OR (ROUND(FreeQuantity - FreeQuantityIssue, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("GoodsIssuePostSaveValidate", queryArray);
        }


        private void GoodsIssueApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsIssueID FROM GoodsIssues WHERE GoodsIssueID = @EntityID AND Approved = 1";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("GoodsIssueApproved", queryArray);
        }


        private void GoodsIssueEditable()
        {
            string[] queryArray = new string[3];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsIssueID FROM HandlingUnitDetails WHERE GoodsIssueID = @EntityID ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = GoodsIssueID FROM ReceiptDetails WHERE GoodsIssueID = @EntityID ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = GoodsIssueID FROM AccountInvoiceDetails WHERE GoodsIssueID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("GoodsIssueEditable", queryArray);
        }


        private void GoodsIssueToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      GoodsIssues  SET Approved = @Approved, ApprovedDate = GetDate() WHERE GoodsIssueID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           UPDATE          GoodsIssueDetails  SET Approved = @Approved WHERE GoodsIssueID = @EntityID ; " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, 'hủy', '')  + ' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GoodsIssueToggleApproved", queryString);
        }


        private void GoodsIssueInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("GoodsIssues", "GoodsIssueID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.GoodsIssue));
            this.totalSalesPortalEntities.CreateTrigger("GoodsIssueInitReference", simpleInitReference.CreateQuery());
        }


        private void GoodsIssueSheet()
        {
            string queryString = " @GoodsIssueID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalGoodsIssueID int    SET @LocalGoodsIssueID = @GoodsIssueID" + "\r\n";

            queryString = queryString + "       SELECT          GoodsIssues.GoodsIssueID, GoodsIssues.EntryDate, GoodsIssues.Reference, GoodsIssues.Remarks, " + "\r\n";
            queryString = queryString + "                       Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, Receivers.CustomerID AS ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.Telephone AS ReceiverTelephone, Receivers.AttentionName AS ReceiverAttentionName, Receivers.BillingAddress AS ReceiverBillingAddress, " + "\r\n";
            queryString = queryString + "                       Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.Name AS CommodityName, " + "\r\n";
            queryString = queryString + "                       GoodsIssueDetails.Quantity, GoodsIssueDetails.FreeQuantity, GoodsIssueDetails.GrossPrice, GoodsIssueDetails.GrossAmount " + "\r\n";

            queryString = queryString + "       FROM            GoodsIssues " + "\r\n";
            queryString = queryString + "                       INNER JOIN GoodsIssueDetails ON GoodsIssues.GoodsIssueID = @LocalGoodsIssueID AND GoodsIssues.GoodsIssueID = GoodsIssueDetails.GoodsIssueID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON GoodsIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers AS Receivers ON GoodsIssues.ReceiverID = Receivers.CustomerID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GoodsIssueSheet", queryString);

        }

    }
}
