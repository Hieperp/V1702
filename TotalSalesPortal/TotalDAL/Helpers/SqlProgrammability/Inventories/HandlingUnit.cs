using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class HandlingUnit
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public HandlingUnit(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetHandlingUnitIndexes();

            this.GetHandlingUnitViewDetails();

            this.GetHandlingUnitPendingGoodsIssues();
            this.GetHandlingUnitPendingCustomers();
            this.GetHandlingUnitPendingGoodsIssueDetails();

            this.HandlingUnitSaveRelative();
            this.HandlingUnitPostSaveValidate();

            this.HandlingUnitEditable();

            this.HandlingUnitInitReference();
        }

        private void GetHandlingUnitIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      HandlingUnits.HandlingUnitID, CAST(HandlingUnits.EntryDate AS DATE) AS EntryDate, HandlingUnits.Reference, Locations.Code AS LocationCode, Customers.Name AS CustomerName, Receivers.Name AS ReceiverName, HandlingUnits.ShippingAddress, HandlingUnits.GoodsIssueID, HandlingUnits.GoodsIssueReferences, PackingMaterials.Code AS PackingMaterialCode, HandlingUnits.LotNo, HandlingUnits.Identification, HandlingUnits.CountIdentification, HandlingUnits.Description, HandlingUnits.TotalQuantity, HandlingUnits.TotalWeight, HandlingUnits.RealWeight, HandlingUnits.WeightDifference " + "\r\n";
            queryString = queryString + "       FROM        HandlingUnits " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON HandlingUnits.EntryDate >= @FromDate AND HandlingUnits.EntryDate <= @ToDate AND HandlingUnits.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.HandlingUnit + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = HandlingUnits.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PackingMaterials ON HandlingUnits.PackingMaterialID = PackingMaterials.PackingMaterialID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON HandlingUnits.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Receivers ON HandlingUnits.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetHandlingUnitIndexes", queryString);
        }

        private void GetHandlingUnitViewDetails()
        {
            string queryString;

            queryString = " @HandlingUnitID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      HandlingUnitDetails.HandlingUnitDetailID, HandlingUnitDetails.HandlingUnitID, HandlingUnitDetails.GoodsIssueID, HandlingUnitDetails.GoodsIssueDetailID, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.EntryDate AS GoodsIssueEntryDate, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, ROUND(GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.QuantityHandlingUnit + HandlingUnitDetails.Quantity, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   HandlingUnitDetails.Quantity, HandlingUnitDetails.UnitWeight, HandlingUnitDetails.Weight, HandlingUnitDetails.Remarks" + "\r\n";

            queryString = queryString + "       FROM        HandlingUnitDetails" + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssueDetails ON HandlingUnitDetails.HandlingUnitID = @HandlingUnitID AND HandlingUnitDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID" + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssues ON GoodsIssueDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetHandlingUnitViewDetails", queryString);
        }










        private void GetHandlingUnitPendingGoodsIssues()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          GoodsIssues.GoodsIssueID, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.EntryDate AS GoodsIssueEntryDate, GoodsIssues.Description, GoodsIssues.Remarks, " + "\r\n";
            queryString = queryString + "                       GoodsIssues.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, " + "\r\n";
            queryString = queryString + "                       GoodsIssues.ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.VATCode AS ReceiverVATCode, Receivers.AttentionName AS ReceiverAttentionName, Receivers.Telephone AS ReceiverTelephone, Receivers.BillingAddress AS ReceiverBillingAddress, ReceiverEntireTerritories.EntireName AS ReceiverEntireTerritoryEntireName, GoodsIssues.ShippingAddress " + "\r\n";

            queryString = queryString + "       FROM            GoodsIssues " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON GoodsIssues.GoodsIssueID IN (SELECT GoodsIssueID FROM GoodsIssueDetails WHERE LocationID = @LocationID AND Approved = 1 AND CommodityTypeID != " + (int)GlobalEnums.CommodityTypeID.Services + " AND ROUND(Quantity + FreeQuantity - QuantityHandlingUnit, " + (int)GlobalEnums.rndQuantity + ") > 0) AND GoodsIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON GoodsIssues.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ReceiverEntireTerritories ON Receivers.TerritoryID = ReceiverEntireTerritories.TerritoryID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetHandlingUnitPendingGoodsIssues", queryString);
        }

        private void GetHandlingUnitPendingCustomers()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          Customers.CustomerID AS CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, " + "\r\n";
            queryString = queryString + "                       Receivers.CustomerID AS ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.VATCode AS ReceiverVATCode, Receivers.AttentionName AS ReceiverAttentionName, Receivers.Telephone AS ReceiverTelephone, Receivers.BillingAddress AS ReceiverBillingAddress, ReceiverEntireTerritories.EntireName AS ReceiverEntireTerritoryEntireName, CustomerReceiverPENDING.ShippingAddress " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT CustomerID, ReceiverID, ShippingAddress FROM GoodsIssues WHERE GoodsIssueID IN (SELECT GoodsIssueID FROM GoodsIssueDetails WHERE LocationID = @LocationID AND Approved = 1 AND CommodityTypeID != " + (int)GlobalEnums.CommodityTypeID.Services + " AND ROUND(Quantity + FreeQuantity - QuantityHandlingUnit, " + (int)GlobalEnums.rndQuantity + ") > 0)) CustomerReceiverPENDING " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON CustomerReceiverPENDING.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON CustomerReceiverPENDING.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ReceiverEntireTerritories ON Receivers.TerritoryID = ReceiverEntireTerritories.TerritoryID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetHandlingUnitPendingCustomers", queryString);
        }



        private void GetHandlingUnitPendingGoodsIssueDetails()
        {
            string queryString;

            queryString = " @LocationID Int, @HandlingUnitID Int, @GoodsIssueID Int, @CustomerID Int, @ReceiverID Int, @ShippingAddress nvarchar(200), @GoodsIssueDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@GoodsIssueID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLGoodsIssue(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLGoodsIssue(false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetHandlingUnitPendingGoodsIssueDetails", queryString);
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

            queryString = queryString + "       IF (@HandlingUnitID <= 0) " + "\r\n";
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
            queryString = queryString + "                       " + this.BuildSQLNew(isGoodsIssueID, isGoodsIssueDetailIDs) + " WHERE GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT GoodsIssueDetailID FROM HandlingUnitDetails WHERE HandlingUnitID = @HandlingUnitID) " + "\r\n";
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

            queryString = queryString + "       SELECT      GoodsIssues.EntryDate AS GoodsIssueEntryDate, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, GoodsIssues.Description, ROUND(GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.QuantityHandlingUnit, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0.0 AS Quantity, Commodities.Weight AS UnitWeight, 0.0 AS Weight, CAST(0 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssues " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssueDetails ON " + (isGoodsIssueID ? " GoodsIssues.GoodsIssueID = @GoodsIssueID " : "GoodsIssues.LocationID = @LocationID AND GoodsIssues.CustomerID = @CustomerID AND GoodsIssues.ReceiverID = @ReceiverID AND GoodsIssues.ShippingAddress = @ShippingAddress") + " AND GoodsIssues.Approved = 1 AND ROUND(GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.QuantityHandlingUnit, " + (int)GlobalEnums.rndQuantity + ") > 0 AND GoodsIssues.GoodsIssueID = GoodsIssueDetails.GoodsIssueID" + (isGoodsIssueDetailIDs ? " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsIssueDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID AND Commodities.CommodityTypeID != " + (int)GlobalEnums.CommodityTypeID.Services + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isGoodsIssueID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      GoodsIssues.EntryDate AS GoodsIssueEntryDate, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, GoodsIssues.Description, ROUND(GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.QuantityHandlingUnit + HandlingUnitDetails.Quantity, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   HandlingUnitDetails.Quantity, HandlingUnitDetails.UnitWeight, HandlingUnitDetails.Weight, CAST(0 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN HandlingUnitDetails ON HandlingUnitDetails.HandlingUnitID = @HandlingUnitID AND GoodsIssueDetails.GoodsIssueDetailID = HandlingUnitDetails.GoodsIssueDetailID" + (isGoodsIssueDetailIDs ? " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsIssueDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssues ON GoodsIssueDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";

            return queryString;
        }

        private void HandlingUnitSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @GoodsIssueID int, @EntryDate DateTime, @CustomerID int, @ReceiverID int, @ShippingAddress nvarchar(200), @LotNo int; " + "\r\n";
            queryString = queryString + "       SELECT          @GoodsIssueID = GoodsIssueID, @EntryDate = EntryDate, @CustomerID = CustomerID, @ReceiverID = ReceiverID, @ShippingAddress = ShippingAddress, @LotNo = LotNo FROM HandlingUnits WHERE HandlingUnitID = @EntityID; " + "\r\n";

            queryString = queryString + "       DECLARE         @HandlingUnitID int, @Identification int; " + "\r\n";
            queryString = queryString + "       SET             @Identification = 0; " + "\r\n";

            queryString = queryString + "       DECLARE         CursorEntryDate CURSOR LOCAL FOR SELECT HandlingUnitID FROM HandlingUnits WHERE (GoodsIssueID = @GoodsIssueID OR (GoodsIssueID IS NULL AND @GoodsIssueID IS NULL)) AND YEAR(EntryDate) = YEAR(@EntryDate) AND MONTH(EntryDate) = MONTH(@EntryDate) AND DAY(EntryDate) = DAY(@EntryDate) AND CustomerID = @CustomerID AND ReceiverID = @ReceiverID AND ShippingAddress = @ShippingAddress AND LotNo = @LotNo AND (@SaveRelativeOption = 1 OR HandlingUnitID <> @EntityID) ORDER BY EntryDate; " + "\r\n";
            queryString = queryString + "       OPEN            CursorEntryDate; " + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorEntryDate INTO @HandlingUnitID; " + "\r\n";

            queryString = queryString + "       WHILE @@FETCH_STATUS = 0   " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SET                 @Identification = @Identification + 1; " + "\r\n";
            queryString = queryString + "               UPDATE              HandlingUnits SET Identification = @Identification WHERE HandlingUnitID = @HandlingUnitID " + "\r\n";
            queryString = queryString + "               FETCH NEXT FROM     CursorEntryDate INTO @HandlingUnitID; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       IF  @Identification > 0 " + "\r\n";
            queryString = queryString + "               UPDATE              HandlingUnits SET CountIdentification = @Identification WHERE (GoodsIssueID = @GoodsIssueID OR (GoodsIssueID IS NULL AND @GoodsIssueID IS NULL)) AND YEAR(EntryDate) = YEAR(@EntryDate) AND MONTH(EntryDate) = MONTH(@EntryDate) AND DAY(EntryDate) = DAY(@EntryDate) AND CustomerID = @CustomerID AND ReceiverID = @ReceiverID AND ShippingAddress = @ShippingAddress AND LotNo = @LotNo " + "\r\n";

            queryString = queryString + "       CLOSE           CursorEntryDate;    DEALLOCATE CursorEntryDate; " + "\r\n";




            queryString = queryString + "       UPDATE          GoodsIssueDetails " + "\r\n";
            queryString = queryString + "       SET             GoodsIssueDetails.QuantityHandlingUnit = ROUND(GoodsIssueDetails.QuantityHandlingUnit + HandlingUnitDetails.Quantity * @SaveRelativeOption, 0) " + "\r\n";
            queryString = queryString + "       FROM            HandlingUnitDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN GoodsIssueDetails ON HandlingUnitDetails.HandlingUnitID = @EntityID AND (GoodsIssueDetails.Approved = 1 OR @SaveRelativeOption = -1) AND HandlingUnitDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT <> (SELECT COUNT(*) FROM HandlingUnitDetails WHERE HandlingUnitID = @EntityID) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Phiếu xuất kho không tồn tại hoặc chưa duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("HandlingUnitSaveRelative", queryString);
        }

        private void HandlingUnitPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày bán hàng: ' + CAST(GoodsIssues.EntryDate AS nvarchar) FROM HandlingUnitDetails INNER JOIN GoodsIssues ON HandlingUnitDetails.HandlingUnitID = @EntityID AND HandlingUnitDetails.GoodsIssueID = GoodsIssues.GoodsIssueID AND HandlingUnitDetails.EntryDate < GoodsIssues.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng đóng gói vượt quá số lượng xuất kho: ' + CAST(ROUND(Quantity + FreeQuantity - QuantityHandlingUnit, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM GoodsIssueDetails WHERE (ROUND(Quantity + FreeQuantity - QuantityHandlingUnit, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("HandlingUnitPostSaveValidate", queryArray);
        }


        private void HandlingUnitEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = HandlingUnitID FROM HandlingUnits WHERE HandlingUnitID = @EntityID AND NOT GoodsDeliveryID IS NULL";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = HandlingUnitID FROM GoodsDeliveryDetails WHERE HandlingUnitID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("HandlingUnitEditable", queryArray);
        }


        private void HandlingUnitInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("HandlingUnits", "HandlingUnitID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.HandlingUnit));
            this.totalSalesPortalEntities.CreateTrigger("HandlingUnitInitReference", simpleInitReference.CreateQuery());
        }
    }
}