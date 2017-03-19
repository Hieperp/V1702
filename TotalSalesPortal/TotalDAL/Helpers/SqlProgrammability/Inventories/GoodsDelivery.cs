using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class GoodsDelivery
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public GoodsDelivery(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetGoodsDeliveryIndexes();

            this.GetGoodsDeliveryViewDetails();
            
            this.GetPendingHandlingUnitReceivers();
            this.GetPendingHandlingUnits();

            this.GoodsDeliverySaveRelative();
            this.GoodsDeliveryPostSaveValidate();

            this.GoodsDeliveryInitReference();
        }

        private void GetGoodsDeliveryIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      GoodsDeliveries.GoodsDeliveryID, CAST(GoodsDeliveries.EntryDate AS DATE) AS EntryDate, GoodsDeliveries.Reference, Locations.Code AS LocationCode, ISNULL(Receivers.Name + ',    ' + Receivers.BillingAddress, N'Phiếu giao hàng gộp chung của nhiều khách hàng') AS ReceiverDescription, Vehicles.Name AS VehicleName, Collectors.Name AS CollectorName, GoodsDeliveries.Description, GoodsDeliveries.TotalQuantity, GoodsDeliveries.TotalWeight, GoodsDeliveries.TotalRealWeight " + "\r\n";
            queryString = queryString + "       FROM        GoodsDeliveries " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON GoodsDeliveries.EntryDate >= @FromDate AND GoodsDeliveries.EntryDate <= @ToDate AND GoodsDeliveries.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsDelivery + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = GoodsDeliveries.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Vehicles ON GoodsDeliveries.VehicleID = Vehicles.VehicleID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees Collectors ON GoodsDeliveries.CollectorID = Collectors.EmployeeID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Customers Receivers ON GoodsDeliveries.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetGoodsDeliveryIndexes", queryString);
        }

        private void GetGoodsDeliveryViewDetails()
        {
            string queryString;

            queryString = " @GoodsDeliveryID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      GoodsDeliveryDetails.GoodsDeliveryDetailID, GoodsDeliveryDetails.GoodsDeliveryID, HandlingUnits.HandlingUnitID, HandlingUnits.EntryDate, HandlingUnits.GoodsIssueReferences, CAST(HandlingUnits.Identification AS varchar) + '/' + CAST(HandlingUnits.CountIdentification AS varchar) AS HandlingUnitIdentification, PackingMaterials.PrintedLabel, HandlingUnits.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, HandlingUnits.ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, HandlingUnits.ShippingAddress, " + "\r\n";
            queryString = queryString + "                   GoodsDeliveryDetails.Quantity, GoodsDeliveryDetails.Weight, GoodsDeliveryDetails.RealWeight, GoodsDeliveryDetails.Remarks" + "\r\n";

            queryString = queryString + "       FROM        GoodsDeliveryDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN HandlingUnits ON GoodsDeliveryDetails.GoodsDeliveryID = @GoodsDeliveryID AND GoodsDeliveryDetails.HandlingUnitID = HandlingUnits.HandlingUnitID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON HandlingUnits.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Receivers ON HandlingUnits.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PackingMaterials ON HandlingUnits.PackingMaterialID = PackingMaterials.PackingMaterialID " + "\r\n";

            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetGoodsDeliveryViewDetails", queryString);
        }











        private void GetPendingHandlingUnitReceivers()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Receivers.CustomerID AS ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.VATCode AS ReceiverVATCode, Receivers.AttentionName AS ReceiverAttentionName, Receivers.Telephone AS ReceiverTelephone, Receivers.BillingAddress AS ReceiverBillingAddress, EntireTerritories.EntireName AS EntireTerritoryEntireName " + "\r\n";
            queryString = queryString + "       FROM            Customers Receivers " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ON Receivers.CustomerID IN (SELECT ReceiverID FROM HandlingUnits WHERE LocationID = @LocationID AND GoodsDeliveryID IS NULL) AND Receivers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingHandlingUnitReceivers", queryString);
        }



        private void GetPendingHandlingUnits()
        {
            string queryString;

            queryString = " @GoodsDeliveryID Int, @ReceiverID Int, @HandlingUnitIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@ReceiverID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssue(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssue(false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingHandlingUnits", queryString);
        }

        private string GetPGIDsBuildSQLGoodsIssue(bool isReceiverID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@HandlingUnitIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueHandlingUnitIDs(isReceiverID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueHandlingUnitIDs(isReceiverID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLGoodsIssueHandlingUnitIDs(bool isReceiverID, bool isHandlingUnitIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@GoodsDeliveryID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.GetPGIDsBuildSQLNew(isReceiverID, isHandlingUnitIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY Receivers.Name, Receivers.Code, HandlingUnits.EntryDate, HandlingUnits.LotNo, HandlingUnits.Identification " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.GetPGIDsBuildSQLEdit(isReceiverID, isHandlingUnitIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY Receivers.Name, Receivers.Code, HandlingUnits.EntryDate, HandlingUnits.LotNo, HandlingUnits.Identification " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.GetPGIDsBuildSQLNew(isReceiverID, isHandlingUnitIDs) + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.GetPGIDsBuildSQLEdit(isReceiverID, isHandlingUnitIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY Receivers.Name, Receivers.Code, HandlingUnits.EntryDate, HandlingUnits.LotNo, HandlingUnits.Identification " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLNew(bool isReceiverID, bool isHandlingUnitIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      HandlingUnits.HandlingUnitID, HandlingUnits.EntryDate, HandlingUnits.GoodsIssueReferences, HandlingUnits.LotNo, HandlingUnits.Identification, CAST(HandlingUnits.Identification AS varchar) + '/' + CAST(HandlingUnits.CountIdentification AS varchar) AS HandlingUnitIdentification, PackingMaterials.PrintedLabel, HandlingUnits.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, HandlingUnits.ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.Telephone AS ReceiverTelephone, HandlingUnits.ShippingAddress, " + "\r\n";
            queryString = queryString + "                   HandlingUnits.TotalQuantity AS Quantity, HandlingUnits.TotalWeight AS Weight, HandlingUnits.RealWeight, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        HandlingUnits " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Receivers ON " + (isReceiverID ? " HandlingUnits.ReceiverID = @ReceiverID AND " : "") + "HandlingUnits.GoodsDeliveryID IS NULL AND HandlingUnits.ReceiverID = Receivers.CustomerID " + (isHandlingUnitIDs ? " AND HandlingUnits.HandlingUnitID NOT IN (SELECT Id FROM dbo.SplitToIntList (@HandlingUnitIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON HandlingUnits.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PackingMaterials ON HandlingUnits.PackingMaterialID = PackingMaterials.PackingMaterialID " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLEdit(bool isReceiverID, bool isHandlingUnitIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      HandlingUnits.HandlingUnitID, HandlingUnits.EntryDate, HandlingUnits.GoodsIssueReferences, HandlingUnits.LotNo, HandlingUnits.Identification, CAST(HandlingUnits.Identification AS varchar) + '/' + CAST(HandlingUnits.CountIdentification AS varchar) AS HandlingUnitIdentification, PackingMaterials.PrintedLabel, HandlingUnits.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, HandlingUnits.ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.Telephone AS ReceiverTelephone, HandlingUnits.ShippingAddress, " + "\r\n";
            queryString = queryString + "                   HandlingUnits.TotalQuantity AS Quantity, HandlingUnits.TotalWeight AS Weight, HandlingUnits.RealWeight, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        HandlingUnits " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Receivers ON HandlingUnits.GoodsDeliveryID = @GoodsDeliveryID AND HandlingUnits.ReceiverID = Receivers.CustomerID " + (isHandlingUnitIDs ? " AND HandlingUnits.HandlingUnitID NOT IN (SELECT Id FROM dbo.SplitToIntList (@HandlingUnitIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON HandlingUnits.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PackingMaterials ON HandlingUnits.PackingMaterialID = PackingMaterials.PackingMaterialID " + "\r\n";

            return queryString;
        }


        private void GoodsDeliverySaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           UPDATE      HandlingUnits" + "\r\n";
            queryString = queryString + "           SET         HandlingUnits.GoodsDeliveryID = GoodsDeliveryDetails.GoodsDeliveryID " + "\r\n";
            queryString = queryString + "           FROM        HandlingUnits INNER JOIN" + "\r\n";
            queryString = queryString + "                       GoodsDeliveryDetails ON GoodsDeliveryDetails.GoodsDeliveryID = @EntityID AND HandlingUnits.HandlingUnitID = GoodsDeliveryDetails.HandlingUnitID " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 
            queryString = queryString + "           UPDATE      HandlingUnits" + "\r\n";
            queryString = queryString + "           SET         GoodsDeliveryID = NULL " + "\r\n";
            queryString = queryString + "           WHERE       GoodsDeliveryID = @EntityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GoodsDeliverySaveRelative", queryString);
        }

        private void GoodsDeliveryPostSaveValidate()
        {
            string[] queryArray = new string[1];
            
            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày đóng hàng: ' + CAST(HandlingUnits.EntryDate AS nvarchar) FROM GoodsDeliveryDetails INNER JOIN HandlingUnits ON GoodsDeliveryDetails.GoodsDeliveryID = @EntityID AND GoodsDeliveryDetails.HandlingUnitID = HandlingUnits.HandlingUnitID AND GoodsDeliveryDetails.EntryDate < HandlingUnits.EntryDate ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("GoodsDeliveryPostSaveValidate", queryArray);
        }



        private void GoodsDeliveryInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("GoodsDeliveries", "GoodsDeliveryID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.GoodsDelivery));
            this.totalSalesPortalEntities.CreateTrigger("GoodsDeliveryInitReference", simpleInitReference.CreateQuery());
        }
    }
}