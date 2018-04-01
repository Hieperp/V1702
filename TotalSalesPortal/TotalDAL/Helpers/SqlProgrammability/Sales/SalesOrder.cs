using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Sales
{
    public class SalesOrder
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public SalesOrder(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetSalesOrderIndexes();

            this.GetSalesOrderViewDetails();
            this.SalesOrderSaveRelative();
            this.SalesOrderPostSaveValidate();

            this.SalesOrderApproved();
            this.SalesOrderEditable();
            this.SalesOrderVoidable();

            this.SalesOrderToggleApproved();
            this.SalesOrderToggleVoid();
            this.SalesOrderToggleVoidDetail();

            this.SalesOrderInitReference();
        }


        private void GetSalesOrderIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SalesOrders.SalesOrderID, CAST(SalesOrders.EntryDate AS DATE) AS EntryDate, SalesOrders.Reference, SalesOrders.Code, Locations.Code AS LocationCode, Customers.Name AS CustomerName, CASE WHEN SalesOrders.CustomerID = SalesOrders.ReceiverID THEN '' ELSE Receivers.Name + ', ' END + SalesOrders.ShippingAddress AS ReceiverDescription, Quotations.EntryDate AS QuotationDate, Quotations.Reference AS QuotationReference, ISNULL(VoidTypes.Name, CASE SalesOrders.InActivePartial WHEN 1 THEN N'Hủy một phần đh' ELSE N'' END) AS VoidTypeName, SalesOrders.Description, SalesOrders.TotalQuantity, SalesOrders.TotalQuantityAdvice, SalesOrders.TotalFreeQuantity, SalesOrders.TotalFreeQuantityAdvice, SalesOrders.TotalListedGrossAmount, SalesOrders.TotalGrossAmount, SalesOrders.Approved, SalesOrders.InActive, SalesOrders.InActivePartial " + "\r\n";
            queryString = queryString + "       FROM        SalesOrders " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON SalesOrders.EntryDate >= @FromDate AND SalesOrders.EntryDate <= @ToDate AND SalesOrders.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.SalesOrder + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = SalesOrders.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON SalesOrders.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Receivers ON SalesOrders.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Quotations ON SalesOrders.QuotationID = Quotations.QuotationID" + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON SalesOrders.VoidTypeID = VoidTypes.VoidTypeID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetSalesOrderIndexes", queryString);
        }

        #region X


        private void GetSalesOrderViewDetails()
        {
            string queryString;
            SqlProgrammability.Inventories.Inventories inventories = new Inventories.Inventories(this.totalSalesPortalEntities);

            queryString = " @SalesOrderID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @EntryDate DateTime       DECLARE @LocationID varchar(35)       DECLARE @CustomerID int         DECLARE @WarehouseIDList varchar(555)         DECLARE @CommodityIDList varchar(3999) " + "\r\n";
            queryString = queryString + "       SELECT      @EntryDate = EntryDate, @LocationID = LocationID, @CustomerID = CustomerID FROM SalesOrders WHERE SalesOrderID = @SalesOrderID " + "\r\n";
            queryString = queryString + "       IF          @EntryDate IS NULL          SET @EntryDate = CONVERT(Datetime, '31/12/2000', 103)" + "\r\n";
            queryString = queryString + "       SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID AS varchar)  FROM SalesOrderDetails WHERE SalesOrderID = @SalesOrderID FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "       SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID AS varchar)  FROM SalesOrderDetails WHERE SalesOrderID = @SalesOrderID FOR XML PATH('')) ,1,1,'') " + "\r\n";

            queryString = queryString + "       " + inventories.GET_WarehouseJournal_BUILD_SQL("@CommoditiesBalance", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0", (int)GlobalEnums.WarehouseClassID.L1 + "," + (int)GlobalEnums.WarehouseClassID.L5 + "," + (int)GlobalEnums.WarehouseClassID.LD) + "\r\n";

            queryString = queryString + "       SELECT      SalesOrderDetails.SalesOrderDetailID, SalesOrderDetails.SalesOrderID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, SalesOrderDetails.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, VoidTypes.VoidTypeID, VoidTypes.Code AS VoidTypeCode, VoidTypes.Name AS VoidTypeName, VoidTypes.VoidClassID, SalesOrderDetails.CalculatingTypeID, " + "\r\n";
            queryString = queryString + "                   ISNULL(CommoditiesBalance.QuantityBalance, 0) AS QuantityAvailable, SalesOrderDetails.Quantity, SalesOrderDetails.ControlFreeQuantity, SalesOrderDetails.FreeQuantity, SalesOrderDetails.ListedPrice, SalesOrderDetails.DiscountPercent, SalesOrderDetails.UnitPrice, SalesOrderDetails.TradeDiscountRate, SalesOrderDetails.VATPercent, SalesOrderDetails.ListedGrossPrice, SalesOrderDetails.GrossPrice, SalesOrderDetails.ListedAmount, SalesOrderDetails.Amount, SalesOrderDetails.ListedVATAmount, SalesOrderDetails.VATAmount, SalesOrderDetails.ListedGrossAmount, SalesOrderDetails.GrossAmount, SalesOrderDetails.IsBonus, SalesOrderDetails.InActivePartial, SalesOrderDetails.InActivePartialDate, SalesOrderDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        SalesOrderDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON SalesOrderDetails.SalesOrderID = @SalesOrderID AND SalesOrderDetails.CommodityID = Commodities.CommodityID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Warehouses ON SalesOrderDetails.WarehouseID = Warehouses.WarehouseID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   VoidTypes ON SalesOrderDetails.VoidTypeID = VoidTypes.VoidTypeID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   @CommoditiesBalance CommoditiesBalance ON SalesOrderDetails.WarehouseID = CommoditiesBalance.WarehouseID AND SalesOrderDetails.CommodityID = CommoditiesBalance.CommodityID " + "\r\n"; //SUM(QuantityBeginQuantityEndREC) 

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetSalesOrderViewDetails", queryString);
        }

        private void SalesOrderSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            //UPDATE ERmgrVCP.BEGIN
            queryString = queryString + "               EXEC        ERmgrVCP.dbo.SalesOrderSaveRelative @EntityID, @SaveRelativeOption "; //WHEN SAVE: SHOULD ADD TO ERmgrVCP FIRST, THEN CALL SPSKUBalanceUpdate
            //UPDATE ERmgrVCP.END

            this.totalSalesPortalEntities.CreateStoredProcedure("SalesOrderSaveRelative", queryString);

            queryString = " USE ERmgrVCP    GO " + "\r\n";
            queryString = queryString + " DROP PROC SalesOrderSaveRelative " + "\r\n";
            queryString = queryString + " CREATE PROC SalesOrderSaveRelative " + "\r\n";
            queryString = queryString + " @EntityID int, @SaveRelativeOption int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               INSERT INTO SalesOrders SELECT * FROM TotalSalesPortal.dbo.SalesOrders WHERE SalesOrderID = @EntityID " + "\r\n";
            queryString = queryString + "               INSERT INTO SalesOrderDetails SELECT * FROM TotalSalesPortal.dbo.SalesOrderDetails WHERE SalesOrderID = @EntityID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DELETE FROM SalesOrderDetails WHERE SalesOrderID = @EntityID " + "\r\n";
            queryString = queryString + "               DELETE FROM SalesOrders WHERE SalesOrderID = @EntityID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            System.Diagnostics.Debug.WriteLine(queryString);

        }

        private void SalesOrderPostSaveValidate()
        {
            string[] queryArray = new string[1];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = 'TEST Date: ' + CAST(EntryDate AS nvarchar) FROM SalesOrders WHERE SalesOrderID = @EntityID "; //FOR TEST TO BREAK OUT WHEN SAVE -> CHECK ROLL BACK OF TRANSACTION
            //queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Service Date: ' + CAST(ServiceInvoices.EntryDate AS nvarchar) FROM SalesOrders INNER JOIN SalesOrders AS ServiceInvoices ON SalesOrders.SalesOrderID = @EntityID AND SalesOrders.ServiceInvoiceID = ServiceInvoices.SalesOrderID AND SalesOrders.EntryDate < ServiceInvoices.EntryDate ";
            queryArray[0] = TotalDAL.Helpers.SqlProgrammability.Sales.DeliveryAdvice.postSaveValidateTradePromotion(GlobalEnums.NmvnTaskID.SalesOrder);

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("SalesOrderPostSaveValidate", queryArray);
        }




        private void SalesOrderApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SalesOrderID FROM SalesOrders WHERE SalesOrderID = @EntityID AND Approved = 1";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("SalesOrderApproved", queryArray);
        }


        private void SalesOrderEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SalesOrderID FROM SalesOrders WHERE SalesOrderID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = SalesOrderID FROM DeliveryAdviceDetails WHERE SalesOrderID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("SalesOrderEditable", queryArray);
        }

        private void SalesOrderVoidable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SalesOrderID FROM SalesOrders WHERE SalesOrderID = @EntityID AND Approved = 0"; //Must approve in order to allow void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = SalesOrderID FROM DeliveryAdviceDetails WHERE SalesOrderID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("SalesOrderVoidable", queryArray);
        }


        private void SalesOrderToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      SalesOrders  SET Approved = @Approved, ApprovedDate = GetDate() WHERE SalesOrderID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          SalesOrderDetails  SET Approved = @Approved WHERE SalesOrderID = @EntityID ; " + "\r\n";

            queryString = queryString + "               UPDATE          ERmgrVCP.dbo.SalesOrders  SET Approved = @Approved, ApprovedDate = GetDate() WHERE SalesOrderID = @EntityID " + "\r\n";
            queryString = queryString + "               UPDATE          ERmgrVCP.dbo.SalesOrderDetails  SET Approved = @Approved WHERE SalesOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, 'hủy', '')  + ' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("SalesOrderToggleApproved", queryString);
        }

        private void SalesOrderToggleVoid()
        {
            string queryString = " @EntityID int, @InActive bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      SalesOrders  SET InActive = @InActive, InActiveDate = GetDate(), VoidTypeID = IIF(@InActive = 1, @VoidTypeID, NULL) WHERE SalesOrderID = @EntityID AND InActive = ~@InActive" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          SalesOrderDetails  SET InActive = @InActive WHERE SalesOrderID = @EntityID ; " + "\r\n";

            queryString = queryString + "               UPDATE          ERmgrVCP.dbo.SalesOrders  SET InActive = @InActive, InActiveDate = GetDate(), VoidTypeID = IIF(@InActive = 1, @VoidTypeID, NULL) WHERE SalesOrderID = @EntityID " + "\r\n";
            queryString = queryString + "               UPDATE          ERmgrVCP.dbo.SalesOrderDetails  SET InActive = @InActive WHERE SalesOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActive = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            this.totalSalesPortalEntities.CreateStoredProcedure("SalesOrderToggleVoid", queryString);
        }

        private void SalesOrderToggleVoidDetail()
        {
            string queryString = " @EntityID int, @EntityDetailID int, @InActivePartial bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      SalesOrderDetails  SET InActivePartial = @InActivePartial, InActivePartialDate = GetDate(), VoidTypeID = IIF(@InActivePartial = 1, @VoidTypeID, NULL) WHERE SalesOrderID = @EntityID AND SalesOrderDetailID = @EntityDetailID AND InActivePartial = ~@InActivePartial ; " + "\r\n";
            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE         @MaxInActivePartial bit     SET @MaxInActivePartial = (SELECT MAX(CAST(InActivePartial AS int)) FROM SalesOrderDetails WHERE SalesOrderID = @EntityID) ;" + "\r\n";
            queryString = queryString + "               UPDATE          SalesOrders  SET InActivePartial = @MaxInActivePartial WHERE SalesOrderID = @EntityID ; " + "\r\n";

            queryString = queryString + "               UPDATE          ERmgrVCP.dbo.SalesOrderDetails  SET InActivePartial = @InActivePartial, InActivePartialDate = GetDate(), VoidTypeID = IIF(@InActivePartial = 1, @VoidTypeID, NULL) WHERE SalesOrderID = @EntityID AND SalesOrderDetailID = @EntityDetailID ; " + "\r\n";
            queryString = queryString + "               UPDATE          ERmgrVCP.dbo.SalesOrders  SET InActivePartial = @MaxInActivePartial WHERE SalesOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActivePartial = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            this.totalSalesPortalEntities.CreateStoredProcedure("SalesOrderToggleVoidDetail", queryString);
        }


        private void SalesOrderInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("SalesOrders", "SalesOrderID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.SalesOrder));
            this.totalSalesPortalEntities.CreateTrigger("SalesOrderInitReference", simpleInitReference.CreateQuery());
        }


        #endregion
    }
}
