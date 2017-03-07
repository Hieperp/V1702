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

            queryString = queryString + "       SELECT      GoodsDeliveries.GoodsDeliveryID, GoodsDeliveries.EntryDate, GoodsDeliveries.Reference, Vehicles.Name AS VehicleName, Drivers.Name AS DriverName, Collectors.Name AS CollectorName, Receivers.Name AS ReceiverName, HandlingUnits.ShippingAddress, " + "\r\n";
            queryString = queryString + "                   HandlingUnits.GoodsIssueReferences, HandlingUnits.PackingMaterialID, HandlingUnits.TotalQuantity AS Quantity, HandlingUnits.TotalWeight AS Weight, HandlingUnits.RealWeight " + "\r\n";
            queryString = queryString + "       FROM        GoodsDeliveries " + "\r\n";
            queryString = queryString + "                   INNER JOIN HandlingUnits ON GoodsDeliveries.GoodsDeliveryID = @LocalGoodsDeliveryID AND GoodsDeliveries.GoodsDeliveryID = HandlingUnits.GoodsDeliveryID " + "\r\n";
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

            queryString = queryString + "       SELECT      HandlingUnits.HandlingUnitID, HandlingUnits.EntryDate, HandlingUnits.GoodsIssueReferences, HandlingUnits.Identification, HandlingUnits.CountIdentification, HandlingUnits.TotalWeight, HandlingUnits.RealWeight, PackingMaterials.PrintedLabel AS PackingMaterialPrintedLabel, " + "\r\n";
            queryString = queryString + "                   HandlingUnits.ShippingAddress, Customers.Name AS ReceiverName, Customers.CustomerCategoryID AS ReceiverCategoryID, Customers.VendorCode, Customers.VendorCategory, PackagingStaffs.Name AS PackagingStaffName, Commodities.Code, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, HandlingUnitDetails.Quantity " + "\r\n";
            queryString = queryString + "       FROM        HandlingUnits " + "\r\n";
            queryString = queryString + "                   INNER JOIN HandlingUnitDetails ON HandlingUnits.HandlingUnitID = @LocalHandlingUnitID AND HandlingUnits.HandlingUnitID = HandlingUnitDetails.HandlingUnitID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssueDetails ON HandlingUnitDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON HandlingUnits.ReceiverID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees PackagingStaffs ON HandlingUnits.PackagingStaffID = PackagingStaffs.EmployeeID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PackingMaterials ON HandlingUnits.PackingMaterialID = PackingMaterials.PackingMaterialID " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("HandlingUnitSheet", queryString);
        }


    }
}
