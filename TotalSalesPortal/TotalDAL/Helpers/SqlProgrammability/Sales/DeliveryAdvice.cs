using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Sales
{
    public class DeliveryAdvice
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public DeliveryAdvice(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetDeliveryAdviceIndexes();

            this.GetPromotionByCustomers();

            //this.GetCommoditiesInWarehouses("GetVehicleAvailables", true, false, false, false);
            this.GetCommoditiesInWarehouses("GetCommodityAvailables", false, true, true, false, false, false); //GetPartAvailables
            //this.GetCommoditiesInWarehouses("GetCommoditiesInWarehouses", false, true, true, false);
            //this.GetCommoditiesInWarehouses("GetCommoditiesAvailables", true, true, false, false);

            //this.GetCommoditiesInWarehouses("GetCommoditiesInWarehousesIncludeOutOfStock", false, true, true, true);

            this.GetDeliveryAdviceViewDetails();
            this.DeliveryAdviceSaveRelative();
            this.DeliveryAdvicePostSaveValidate();

            this.DeliveryAdviceApproved();
            this.DeliveryAdviceEditable();
            this.DeliveryAdviceVoidable();

            this.DeliveryAdviceToggleApproved();
            this.DeliveryAdviceToggleVoid();
            this.DeliveryAdviceToggleVoidDetail();

            this.DeliveryAdviceInitReference();

        }


        private void GetDeliveryAdviceIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      DeliveryAdvices.DeliveryAdviceID, CAST(DeliveryAdvices.EntryDate AS DATE) AS EntryDate, DeliveryAdvices.Reference, Locations.Code AS LocationCode, Customers.Name AS CustomerName, CASE WHEN DeliveryAdvices.CustomerID = DeliveryAdvices.ReceiverID THEN '' ELSE Receivers.Name + ', ' END + DeliveryAdvices.ShippingAddress AS ReceiverDescription, SalesOrders.EntryDate AS SalesOrderDate, SalesOrders.Reference AS SalesOrderReference, ISNULL(VoidTypes.Name, CASE DeliveryAdvices.InActivePartial WHEN 1 THEN N'Hủy một phần đh' ELSE N'' END) AS VoidTypeName, DeliveryAdvices.TotalQuantity, DeliveryAdvices.TotalQuantityIssue, DeliveryAdvices.TotalFreeQuantity, DeliveryAdvices.TotalFreeQuantityIssue, DeliveryAdvices.TotalListedGrossAmount, DeliveryAdvices.TotalGrossAmount, DeliveryAdvices.Approved, DeliveryAdvices.InActive, DeliveryAdvices.InActivePartial " + "\r\n";
            queryString = queryString + "       FROM        DeliveryAdvices " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON DeliveryAdvices.EntryDate >= @FromDate AND DeliveryAdvices.EntryDate <= @ToDate AND DeliveryAdvices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.DeliveryAdvice + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = DeliveryAdvices.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON DeliveryAdvices.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Receivers ON DeliveryAdvices.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN SalesOrders ON DeliveryAdvices.SalesOrderID = SalesOrders.SalesOrderID" + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON DeliveryAdvices.VoidTypeID = VoidTypes.VoidTypeID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetDeliveryAdviceIndexes", queryString);
        }


        private void GetPromotionByCustomers()
        {
            string queryString;

            queryString = " @CustomerID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@CustomerID IS NULL) ";
            queryString = queryString + "           BEGIN ";
            queryString = queryString + "               SELECT * FROM Promotions WHERE Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate " + "\r\n"; 
            queryString = queryString + "           END ";
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           BEGIN ";

            queryString = queryString + "               DECLARE @CustomerCategoryID int " + "\r\n";
            queryString = queryString + "               SELECT  @CustomerCategoryID = CustomerCategoryID FROM Customers WHERE CustomerID = @CustomerID "; //GET @CustomerCategoryID OF @CustomerID

            queryString = queryString + "               SELECT * FROM Promotions WHERE PromotionID IN " + "\r\n"; 
            queryString = queryString + "                  (SELECT Promotions.PromotionID FROM Promotions WHERE Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND Promotions.ApplyToAllCustomers = 1 " + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   SELECT Promotions.PromotionID FROM Promotions INNER JOIN PromotionCustomers ON Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND PromotionCustomers.CustomerID = @CustomerID AND Promotions.PromotionID = PromotionCustomers.PromotionID " + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   SELECT Promotions.PromotionID FROM Promotions INNER JOIN PromotionCustomerCategoryies ON Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND Promotions.PromotionID = PromotionCustomerCategoryies.PromotionID INNER JOIN AncestorCustomerCategories ON PromotionCustomerCategoryies.CustomerCategoryID = AncestorCustomerCategories.AncestorID AND AncestorCustomerCategories.CustomerCategoryID = @CustomerCategoryID " + "\r\n";
            queryString = queryString + "                  )" + "\r\n";
            queryString = queryString + "               ORDER BY Code, Name ";
            queryString = queryString + "           END ";
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPromotionByCustomers", queryString);
        }

        /// <summary>
        /// Get QuantityAvailable (Remaining) Commodities BY EVERY (WarehouseID, CommodityID)
        /// </summary>
        private void GetCommoditiesInWarehouses(string storedProcedureName, bool withCommoditiesInGoodsReceipts, bool withCommoditiesInWarehouses, bool withCommodityTypeServices, bool getSavedData, bool includeCommoditiesOutOfStock, bool isUsingWarehouseBalancePrice)
        {
            //HIEN TAI, SU DUNG CHUNG CAC CAU SQL DE TAO RA NHIEU PHIEN BAN StoredProcedure, MUC DICH: NHAM DE QUAN LY CODE TUONG TU NHAU
            //VI VAY, CODE NAY TUAN THEO QUY UOC SAU: (CHI LA QUY UOC THOI, KHONG PHAI DIEU KIEN RANG BUOC GI CA)
            //withCommoditiesInGoodsReceipts = TRUE  and withCommoditiesInWarehouses = FALSE and getSavedData = FALSE: GetVehicleAvailables
            //withCommoditiesInGoodsReceipts = FALSE and withCommoditiesInWarehouses = TRUE  and getSavedData = FALSE: GetPartAvailables
            //withCommoditiesInGoodsReceipts = FALSE and withCommoditiesInWarehouses = TRUE  and getSavedData = TRUE: GetCommoditiesInWarehouses
            //withCommoditiesInGoodsReceipts = TRUE  and withCommoditiesInWarehouses = TRUE  and getSavedData = FALSE: GetCommoditiesAvailables
            //THEO QUY UOC NAY THI: getSavedData = TRUE ONLY WHEN: GetCommoditiesInWarehouses: DUOC SU DUNG TRONG GoodsIssue, StockTransfer, InventoryAdjustment
            //HAI TRUONG HOP: GetVehicleAvailables VA GetPartAvailables: DUOC SU DUNG TRONG VehicleTransferOrder VA PartTransferOrder (TAT NHIEN, SE CON DUOC SU DUNG TRONG NHUNG TRUONG HOP KHAC KHI KHONG YEU CAU LAY getSavedData, TUY NHIEN, HIEN TAI CHUA CO SU DUNG NOI NAO KHAC)
            //TRUONG HOP CUOI CUNG: GetCommoditiesAvailables: CUNG GIONG NHU GetVehicleAvailables VA GetPartAvailables, TUY NHIEN, NO BAO GOM CA Vehicle VA PartANDConsumable (get both Vehicles and Parts/ Consumables on the same view) (HIEN TAI, GetCommoditiesAvailables: CHUA DUOC SU DUNG O CHO NAO CA, CHI DE DANH SAU NAY CAN THIET THI SU DUNG THOI)


            //BO SUNG NGAY 08-JUN-2016: includeCommoditiesOutOfStock = TRUE: CHI DUY NHAT AP DUNG CHO GetCommoditiesInWarehousesIncludeOutOfStock. CAC T/H KHAC CHUA XEM XET, DO CHUA CO NHU CAU SU DUNG. NEU CO NHU CAU -> THI CO THE XEM XET LAI SQL QUERY


            string queryString = " @LocationID int, @CustomerID int, @PriceCategoryID int, @PromotionID int, @EntryDate DateTime, @SearchText nvarchar(60) " + (getSavedData ? ", @GoodsIssueID int, @StockTransferID int, @InventoryAdjustmentID int " : "") + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       DECLARE @Commodities TABLE (CommodityID int NOT NULL, Code nvarchar(50) NOT NULL, CodePartA nvarchar(20) NOT NULL, CodePartB nvarchar(20) NOT NULL, CodePartC nvarchar(20) NOT NULL, Name nvarchar(200) NOT NULL, GrossPrice decimal(18, 2) NOT NULL, DiscountPercent decimal(18, 2) NOT NULL, ControlFreeQuantity decimal(18, 2) NOT NULL, CommodityTypeID int NOT NULL, CommodityCategoryID int NOT NULL)" + "\r\n";
            queryString = queryString + "       DECLARE @CommoditiesAvailable TABLE (WarehouseID int NOT NULL, CommodityID int NOT NULL, QuantityAvailable decimal(18, 2) NOT NULL, Bookable bit NULL)" + "\r\n";
            queryString = queryString + "       DECLARE @HasCommoditiesAvailable int SET @HasCommoditiesAvailable = 0" + "\r\n";

            queryString = queryString + "       INSERT INTO @Commodities SELECT TOP 10 CommodityID, Code, CodePartA, CodePartB, CodePartC, Name, 0 AS GrossPrice, 0 AS DiscountPercent, 0 AS ControlFreeQuantity, CommodityTypeID, CommodityCategoryID FROM Commodities WHERE CommodityTypeID IN (" + (withCommoditiesInGoodsReceipts ? "" + (int)GlobalEnums.CommodityTypeID.Vehicles : "") + (withCommoditiesInGoodsReceipts && withCommoditiesInWarehouses ? ", " : "") + (withCommoditiesInWarehouses ? (int)GlobalEnums.CommodityTypeID.Parts + ", " + (int)GlobalEnums.CommodityTypeID.Consumables : "") + (withCommodityTypeServices & (withCommoditiesInGoodsReceipts || withCommoditiesInWarehouses) ? ", " : "") + (withCommodityTypeServices ? "" + (int)GlobalEnums.CommodityTypeID.Services : "") + ") AND (Code LIKE '%' + @SearchText + '%' OR Name LIKE '%' + @SearchText + '%') " + "\r\n";


            queryString = queryString + "       IF (@@ROWCOUNT > 0) " + "\r\n";
            {
                queryString = queryString + "       BEGIN ";
                if (!isUsingWarehouseBalancePrice) //GET 
                {
                    queryString = queryString + "   UPDATE  Commodities SET Commodities.GrossPrice = CommodityPrices.GrossPrice FROM @Commodities Commodities INNER JOIN CommodityPrices ON CommodityPrices.PriceCategoryID = @PriceCategoryID AND Commodities.CodePartA = CommodityPrices.CodePartA AND Commodities.CodePartC = CommodityPrices.CodePartC AND CommodityPrices.CodePartB IS NULL "; //UPDATE GrossPrice BY @PriceCategoryID
                    queryString = queryString + "   UPDATE  Commodities SET Commodities.GrossPrice = CommodityPrices.GrossPrice FROM @Commodities Commodities INNER JOIN CommodityPrices ON CommodityPrices.PriceCategoryID = @PriceCategoryID AND Commodities.CodePartA = CommodityPrices.CodePartA AND Commodities.CodePartC = CommodityPrices.CodePartC AND Commodities.CodePartB = CommodityPrices.CodePartB "; //UPDATE GrossPrice BY @PriceCategoryID

                    queryString = queryString + "   IF (NOT @PromotionID IS NULL) ";
                    queryString = queryString + "       BEGIN ";

                    queryString = queryString + "           DECLARE @DiscountPercent decimal(18, 2), @ControlFreeQuantity decimal(18, 2), @ApplyToAllCommodities bit ";
                    queryString = queryString + "           SELECT @DiscountPercent = DiscountPercent, @ControlFreeQuantity = ControlFreeQuantity, @ApplyToAllCommodities = ApplyToAllCommodities FROM Promotions WHERE PromotionID = @PromotionID AND InActive = 0 AND GetDate() >= StartDate AND GetDate() <= EndDate ";

                    queryString = queryString + "           IF (NOT @DiscountPercent IS NULL OR NOT @ControlFreeQuantity IS NULL) ";
                    queryString = queryString + "               BEGIN ";
                    queryString = queryString + "                   IF (@ApplyToAllCommodities = 1) ";
                    queryString = queryString + "                       UPDATE @Commodities SET DiscountPercent = @DiscountPercent, ControlFreeQuantity = @ControlFreeQuantity "; //All Commodities 
                    queryString = queryString + "                   ELSE ";
                    queryString = queryString + "                       UPDATE @Commodities SET DiscountPercent = @DiscountPercent, ControlFreeQuantity = @ControlFreeQuantity WHERE CommodityID IN (";
                    queryString = queryString + "                           SELECT CommodityID FROM @Commodities Commodities INNER JOIN PromotionCommodityCodeParts ON PromotionCommodityCodeParts.PromotionID = @PromotionID AND ((Commodities.CodePartA = PromotionCommodityCodeParts.CodePartA AND Commodities.CodePartC = PromotionCommodityCodeParts.CodePartC) OR (Commodities.CodePartA = PromotionCommodityCodeParts.CodePartA AND PromotionCommodityCodeParts.CodePartC = N'') OR (Commodities.CodePartC = PromotionCommodityCodeParts.CodePartC AND PromotionCommodityCodeParts.CodePartA = N'')) "; //By CodeParts (BOTH CodePartA AND CodePartB UNION CodePartA Only UNION CodePartB Only)
                    queryString = queryString + "                           UNION ALL ";
                    queryString = queryString + "                           SELECT CommodityID FROM PromotionCommodities WHERE PromotionID = @PromotionID"; //Concrete Commodities
                    queryString = queryString + "                           UNION ALL ";
                    queryString = queryString + "                           SELECT  CommodityID FROM @Commodities WHERE CommodityCategoryID IN "; //By CommodityCategories
                    queryString = queryString + "                                  (SELECT  AncestorCommodityCategories.CommodityCategoryID " ;
                    queryString = queryString + "                                   FROM    PromotionCommodityCategoryies ";
                    queryString = queryString + "                                           INNER JOIN AncestorCommodityCategories ON PromotionCommodityCategoryies.PromotionID = @PromotionID AND PromotionCommodityCategoryies.CommodityCategoryID = AncestorCommodityCategories.AncestorID ";
                    queryString = queryString + "                                  )";
                    queryString = queryString + "                       )";

                    queryString = queryString + "               END ";

                    queryString = queryString + "       END ";

                    queryString = queryString + "   ELSE "; //(@PromotionID IS NULL)
                    queryString = queryString + "       BEGIN ";

                    queryString = queryString + "           DECLARE @CustomerCategoryID int, @PromotionIDList varchar(3999) " + "\r\n";

                    queryString = queryString + "           SELECT  @CustomerCategoryID = CustomerCategoryID FROM Customers WHERE CustomerID = @CustomerID "; //GET @CustomerCategoryID OF @CustomerID

                    queryString = queryString + "           SELECT  @PromotionIDList = STUFF((SELECT ',' + CAST(PromotionID AS varchar) " + "\r\n"; //CONVERT TO @PromotionIDList OF @CustomerID, USING @PromotionIDList AS FILTER FOR PromotionCommodities OR PromotionCommodityCategoryies AT NEXT STEP 
                    queryString = queryString + "           FROM    (" + "\r\n";
                    queryString = queryString + "                    SELECT DISTINCT PromotionID FROM " + "\r\n"; //GET DISTINCT PromotionID BY: @CustomerID UNION @CustomerCategoryID
                    queryString = queryString + "                          (SELECT Promotions.PromotionID FROM Promotions WHERE Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND Promotions.ApplyToAllCustomers = 1 " + "\r\n";
                    queryString = queryString + "                           UNION ALL " + "\r\n";
                    queryString = queryString + "                           SELECT Promotions.PromotionID FROM Promotions INNER JOIN PromotionCustomers ON Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND PromotionCustomers.CustomerID = @CustomerID AND Promotions.PromotionID = PromotionCustomers.PromotionID " + "\r\n";
                    queryString = queryString + "                           UNION ALL " + "\r\n";
                    queryString = queryString + "                           SELECT Promotions.PromotionID FROM Promotions INNER JOIN PromotionCustomerCategoryies ON Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND Promotions.PromotionID = PromotionCustomerCategoryies.PromotionID INNER JOIN AncestorCustomerCategories ON PromotionCustomerCategoryies.CustomerCategoryID = AncestorCustomerCategories.AncestorID AND AncestorCustomerCategories.CustomerCategoryID = @CustomerCategoryID " + "\r\n";
                    queryString = queryString + "                          )UNIONPromotions " + "\r\n";
                    queryString = queryString + "                   ) DISTINCTUNIONPromotions " + "\r\n";
                    queryString = queryString + "           FOR XML PATH('')) ,1,1,'') " + "\r\n";





                    queryString = queryString + "           DECLARE @UNIONPromotions TABLE (CommodityID int NOT NULL, DiscountPercent decimal(18, 2) NOT NULL, ControlFreeQuantity decimal(18, 2) NOT NULL)" + "\r\n";

                    queryString = queryString + "           INSERT INTO @UNIONPromotions (CommodityID, DiscountPercent, ControlFreeQuantity) ";
                    queryString = queryString + "           SELECT  Commodities.CommodityID, Promotions.DiscountPercent, Promotions.ControlFreeQuantity ";
                    queryString = queryString + "           FROM    Promotions "; //All Commodities 
                    queryString = queryString + "                   CROSS JOIN @Commodities Commodities WHERE Promotions.ApplyToAllCommodities = 1 AND Promotions.PromotionID IN (SELECT Id FROM dbo.SplitToIntList (@PromotionIDList)) ";
                    queryString = queryString + "           UNION ALL ";
                    queryString = queryString + "           SELECT  Commodities.CommodityID, Promotions.DiscountPercent, Promotions.ControlFreeQuantity ";
                    queryString = queryString + "           FROM    Promotions "; //By CodeParts (BOTH CodePartA AND CodePartB UNION CodePartA Only UNION CodePartB Only)
                    queryString = queryString + "                   INNER JOIN  PromotionCommodityCodeParts ON Promotions.PromotionID IN (SELECT Id FROM dbo.SplitToIntList (@PromotionIDList)) AND Promotions.PromotionID = PromotionCommodityCodeParts.PromotionID ";
                    queryString = queryString + "                   INNER JOIN  @Commodities Commodities ON (Commodities.CodePartA = PromotionCommodityCodeParts.CodePartA AND Commodities.CodePartC = PromotionCommodityCodeParts.CodePartC) OR (Commodities.CodePartA = PromotionCommodityCodeParts.CodePartA AND PromotionCommodityCodeParts.CodePartC = N'') OR (Commodities.CodePartC = PromotionCommodityCodeParts.CodePartC AND PromotionCommodityCodeParts.CodePartA = N'') ";
                    queryString = queryString + "           UNION ALL ";
                    queryString = queryString + "           SELECT  PromotionCommodities.CommodityID, Promotions.DiscountPercent, Promotions.ControlFreeQuantity ";
                    queryString = queryString + "           FROM    Promotions "; //Concrete Commodities
                    queryString = queryString + "                   INNER JOIN  PromotionCommodities ON Promotions.PromotionID IN (SELECT Id FROM dbo.SplitToIntList (@PromotionIDList)) AND Promotions.PromotionID = PromotionCommodities.PromotionID ";
                    queryString = queryString + "           UNION ALL ";
                    queryString = queryString + "           SELECT  Commodities.CommodityID, Promotions.DiscountPercent, Promotions.ControlFreeQuantity ";
                    queryString = queryString + "           FROM    Promotions "; //By CommodityCategories
                    queryString = queryString + "                   INNER JOIN  PromotionCommodityCategoryies ON Promotions.PromotionID IN (SELECT Id FROM dbo.SplitToIntList (@PromotionIDList)) AND Promotions.PromotionID = PromotionCommodityCategoryies.PromotionID ";
                    queryString = queryString + "                   INNER JOIN  AncestorCommodityCategories ON PromotionCommodityCategoryies.CommodityCategoryID = AncestorCommodityCategories.AncestorID ";
                    queryString = queryString + "                   INNER JOIN  @Commodities Commodities ON AncestorCommodityCategories.CommodityCategoryID = Commodities.CommodityCategoryID ";





                    queryString = queryString + "           UPDATE  Commodities SET Commodities.DiscountPercent = OVERPARTITIONPromotions.DiscountPercent "; //UPDATE @Commodities.DiscountPercent BY MAX DiscountPercent
                    queryString = queryString + "           FROM    @Commodities Commodities INNER JOIN ";
                    queryString = queryString + "                   (SELECT CommodityID, DiscountPercent, ROW_NUMBER() OVER (PARTITION BY CommodityID ORDER BY DiscountPercent DESC) AS RowNo ";
                    queryString = queryString + "                    FROM   @UNIONPromotions ";                                        
                    queryString = queryString + "                   ) OVERPARTITIONPromotions ON Commodities.CommodityID = OVERPARTITIONPromotions.CommodityID AND OVERPARTITIONPromotions.RowNo = 1 ";

                    queryString = queryString + "           UPDATE  Commodities SET Commodities.ControlFreeQuantity = OVERPARTITIONPromotions.ControlFreeQuantity "; //UPDATE @Commodities.ControlFreeQuantity BY MIN ControlFreeQuantity
                    queryString = queryString + "           FROM    @Commodities Commodities INNER JOIN ";
                    queryString = queryString + "                   (SELECT CommodityID, ControlFreeQuantity, ROW_NUMBER() OVER (PARTITION BY CommodityID ORDER BY ControlFreeQuantity) AS RowNo ";
                    queryString = queryString + "                    FROM   @UNIONPromotions ";
                    queryString = queryString + "                   ) OVERPARTITIONPromotions ON Commodities.CommodityID = OVERPARTITIONPromotions.CommodityID AND OVERPARTITIONPromotions.RowNo = 1 ";

                    
                    queryString = queryString + "       END ";

                }

                queryString = queryString + "       " + this.GetCommoditiesInWarehousesBuildSQL(withCommoditiesInGoodsReceipts, withCommoditiesInWarehouses, withCommodityTypeServices, getSavedData, includeCommoditiesOutOfStock, isUsingWarehouseBalancePrice) + "\r\n";
                queryString = queryString + "       END ";
            }
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetCommoditiesInWarehousesRETURNNothing() + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure(storedProcedureName, queryString);
        }

        private string GetCommoditiesInWarehousesGETAvailable(bool withCommoditiesInGoodsReceipts, bool withCommoditiesInWarehouses, bool withCommodityTypeServices, bool getSavedData)
        {
            string queryString = "";

            if (withCommoditiesInGoodsReceipts)
            {//GET QuantityAvailable IN GoodsReceiptDetails FOR GlobalEnums.CommodityTypeID.Vehicles
                queryString = queryString + "               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "               SELECT          WarehouseID, CommodityID, ROUND(Quantity - QuantityIssue, 0) AS QuantityAvailable " + "\r\n";
                queryString = queryString + "               FROM            GoodsReceiptDetails " + "\r\n";
                queryString = queryString + "               WHERE           CommodityTypeID IN (" + (int)GlobalEnums.CommodityTypeID.Vehicles + ") AND ROUND(Quantity - QuantityIssue, 0) > 0 AND WarehouseID IN (SELECT WarehouseID FROM Warehouses WHERE LocationID = @LocationID) AND CommodityID IN (SELECT CommodityID FROM @Commodities) " + "\r\n";

                queryString = queryString + "               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
            }

            if (withCommoditiesInWarehouses)
            {
                //GET QuantityEndREC IN WarehouseJournal
                SqlProgrammability.Inventories.Inventories inventories = new Inventories.Inventories(this.totalSalesPortalEntities);

                queryString = queryString + "               DECLARE @WarehouseIDList varchar(555)        DECLARE @CommodityIDList varchar(3999) " + "\r\n";
                queryString = queryString + "               SELECT  @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID AS varchar) FROM Warehouses WHERE LocationID = @LocationID FOR XML PATH('')) ,1,1,'') " + "\r\n";
                queryString = queryString + "               SELECT  @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID AS varchar) FROM @Commodities FOR XML PATH('')) ,1,1,'') " + "\r\n";


                queryString = queryString + "               " + inventories.GET_WarehouseJournal_BUILD_SQL("@WarehouseJournalTable", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0") + "\r\n";

                queryString = queryString + "               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "               SELECT          WarehouseID, CommodityID, QuantityBegin AS QuantityAvailable " + "\r\n"; //QuantityEndREC
                queryString = queryString + "               FROM            @WarehouseJournalTable " + "\r\n";

                queryString = queryString + "               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
            }

            if (getSavedData)
            {//GET SavedData
                queryString = queryString + "               IF (@GoodsIssueID > 0) " + "\r\n";
                queryString = queryString + "                   BEGIN " + "\r\n";
                queryString = queryString + "                               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "                               SELECT          WarehouseID, CommodityID, Quantity AS QuantityAvailable " + "\r\n";
                queryString = queryString + "                               FROM            GoodsIssueDetails " + "\r\n";
                queryString = queryString + "                               WHERE           GoodsIssueID = @GoodsIssueID AND LocationID = @LocationID AND CommodityID IN (SELECT CommodityID FROM @Commodities) " + "\r\n";

                queryString = queryString + "                               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
                queryString = queryString + "                   END " + "\r\n";

                queryString = queryString + "               IF (@StockTransferID > 0) " + "\r\n";
                queryString = queryString + "                   BEGIN " + "\r\n";
                queryString = queryString + "                               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "                               SELECT          WarehouseID, CommodityID, Quantity AS QuantityAvailable " + "\r\n";
                queryString = queryString + "                               FROM            StockTransferDetails " + "\r\n";
                queryString = queryString + "                               WHERE           StockTransferID = @StockTransferID AND LocationID = @LocationID AND CommodityID IN (SELECT CommodityID FROM @Commodities) " + "\r\n";

                queryString = queryString + "                               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
                queryString = queryString + "                   END " + "\r\n";

                queryString = queryString + "             IF (@InventoryAdjustmentID > 0) " + "\r\n";
                queryString = queryString + "                   BEGIN " + "\r\n";
                queryString = queryString + "                               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "                               SELECT          WarehouseID, CommodityID, -Quantity AS QuantityAvailable " + "\r\n";
                queryString = queryString + "                               FROM            InventoryAdjustmentDetails " + "\r\n";
                queryString = queryString + "                               WHERE           InventoryAdjustmentID = @InventoryAdjustmentID AND LocationID = @LocationID AND CommodityID IN (SELECT CommodityID FROM @Commodities) " + "\r\n";

                queryString = queryString + "                               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
                queryString = queryString + "                   END " + "\r\n";
            }

            if (withCommodityTypeServices)  //GET SERVICE ITEM
            {
                queryString = queryString + "                               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "                               SELECT          (SELECT TOP 1 WarehouseID FROM CustomerWarehouses WHERE CustomerID = @CustomerID AND InActive = 0) AS WarehouseID, CommodityID, 100000000000 AS QuantityAvailable " + "\r\n";
                queryString = queryString + "                               FROM            @Commodities " + "\r\n";
                queryString = queryString + "                               WHERE           CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Services + "\r\n";

                queryString = queryString + "                               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
            }

            queryString = queryString + "               UPDATE @CommoditiesAvailable SET Bookable = 1 WHERE WarehouseID IN (SELECT WarehouseID FROM CustomerWarehouses WHERE CustomerID = @CustomerID AND InActive = 0)" + "\r\n";


            return queryString;
        }

        private string GetCommoditiesInWarehousesBuildSQL(bool withCommoditiesInGoodsReceipts, bool withCommoditiesInWarehouses, bool withCommodityTypeServices, bool getSavedData, bool includeCommoditiesOutOfStock, bool isUsingWarehouseBalancePrice)
        {
            string queryString = "                  BEGIN " + "\r\n";

            queryString = queryString + "               " + this.GetCommoditiesInWarehousesGETAvailable(withCommoditiesInGoodsReceipts, withCommoditiesInWarehouses, withCommodityTypeServices, getSavedData) + "\r\n";


            if (isUsingWarehouseBalancePrice)
            {
                queryString = queryString + "       DECLARE @CurrentWarehouseBalancePrice TABLE (CommodityID int NOT NULL, UnitPrice decimal(18, 2) NOT NULL)" + "\r\n";
                queryString = queryString + "       INSERT INTO @CurrentWarehouseBalancePrice SELECT CommodityID, UnitPrice FROM (SELECT EntryDate, CommodityID, UnitPrice, ROW_NUMBER() OVER (PARTITION BY CommodityID ORDER BY EntryDate DESC) AS RowNo FROM WarehouseBalancePrice WHERE CommodityID IN (SELECT CommodityID FROM @Commodities) AND EntryDate <= dbo.EOMONTHTIME(@EntryDate, 9999)) WarehouseBalancePriceWithRowNo WHERE RowNo = 1" + "\r\n";
            }


            if (!includeCommoditiesOutOfStock)
                queryString = queryString + "       IF (@HasCommoditiesAvailable > 0) " + "\r\n";

            queryString = queryString + "                   SELECT          " + (!includeCommoditiesOutOfStock ? "" : "TOP 50") + "Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + (!isUsingWarehouseBalancePrice ? "Commodities.GrossPrice" : "ROUND(CAST(ISNULL(CurrentWarehouseBalancePrice.UnitPrice, 0) AS decimal(18, 2)) * (1 + CommodityCategories.VATPercent/100), " + (int)GlobalEnums.rndAmount + ") AS GrossPrice") + ", Commodities.DiscountPercent, Commodities.ControlFreeQuantity, CommodityCategories.VATPercent, " + (!includeCommoditiesOutOfStock ? "" : "ISNULL(") + "Warehouses.WarehouseID" + (!includeCommoditiesOutOfStock ? "" : ", DEFAULTWarehouses.WarehouseID) AS WarehouseID") + ", " + (!includeCommoditiesOutOfStock ? "" : "ISNULL(") + "Warehouses.Code" + (!includeCommoditiesOutOfStock ? "" : ", DEFAULTWarehouses.Code)") + " AS WarehouseCode, " + (!includeCommoditiesOutOfStock ? "" : "ISNULL(") + "CommoditiesAvailable.QuantityAvailable" + (!includeCommoditiesOutOfStock ? "" : ", CAST(0 AS decimal(18, 2)) ) AS QuantityAvailable") + ", " + (!includeCommoditiesOutOfStock ? "CommoditiesAvailable.Bookable" : "CAST(1 AS bit) AS Bookable") + " \r\n";
            queryString = queryString + "                   FROM            @Commodities Commodities INNER JOIN " + "\r\n";
            queryString = queryString + "                                   CommodityCategories ON Commodities.CommodityCategoryID = CommodityCategories.CommodityCategoryID " + (!includeCommoditiesOutOfStock ? "INNER JOIN" : "LEFT JOIN") + "\r\n";
            queryString = queryString + "                                  (SELECT WarehouseID, CommodityID, SUM(QuantityAvailable) AS QuantityAvailable, Bookable FROM @CommoditiesAvailable GROUP BY WarehouseID, CommodityID, Bookable) CommoditiesAvailable ON Commodities.CommodityID = CommoditiesAvailable.CommodityID " + (!includeCommoditiesOutOfStock ? "INNER JOIN" : "LEFT JOIN") + "\r\n";
            queryString = queryString + "                                   Warehouses ON CommoditiesAvailable.WarehouseID = Warehouses.WarehouseID " + "\r\n";


            if (isUsingWarehouseBalancePrice)
                queryString = queryString + "                               LEFT JOIN @CurrentWarehouseBalancePrice CurrentWarehouseBalancePrice ON Commodities.CommodityID = CurrentWarehouseBalancePrice.CommodityID " + "\r\n";

            if (includeCommoditiesOutOfStock)
                queryString = queryString + "                               LEFT JOIN (SELECT TOP 1 WarehouseID, Code FROM Warehouses WHERE LocationID = @LocationID) DEFAULTWarehouses ON DEFAULTWarehouses.WarehouseID <> 0 " + "\r\n";

            queryString = queryString + "                   ORDER BY        CommodityCode, Bookable DESC " + "\r\n";
            if (!includeCommoditiesOutOfStock)
            {
                queryString = queryString + "       ELSE " + "\r\n";
                queryString = queryString + "               " + this.GetCommoditiesInWarehousesRETURNNothing() + "\r\n";
            }

            queryString = queryString + "           END " + "\r\n";

            return queryString;
        }




        private string GetCommoditiesInWarehousesRETURNNothing()
        {
            string queryString = "                          BEGIN " + "\r\n";

            queryString = queryString + "                       SELECT      CommodityID, Code AS CommodityCode, Name AS CommodityName, Commodities.CommodityTypeID, Commodities.GrossPrice, 0.0 AS DiscountPercent, 0.0 AS ControlFreeQuantity, 0.0 AS VATPercent, 0 AS WarehouseID, '' AS WarehouseCode, CAST(0 AS decimal(18, 2)) AS QuantityAvailable, CAST(0 AS bit) AS Bookable " + "\r\n";
            queryString = queryString + "                       FROM        @Commodities Commodities " + "\r\n";
            queryString = queryString + "                       WHERE       CommodityID IS NULL " + "\r\n"; //ALWAYS RETURN NOTHING

            queryString = queryString + "                   END " + "\r\n";

            return queryString;
        }




        #region X


        private void GetDeliveryAdviceViewDetails()
        {
            string queryString;
            SqlProgrammability.Inventories.Inventories inventories = new Inventories.Inventories(this.totalSalesPortalEntities);

            queryString = " @DeliveryAdviceID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @EntryDate DateTime       DECLARE @LocationID varchar(35)       DECLARE @CustomerID int         DECLARE @WarehouseIDList varchar(555)         DECLARE @CommodityIDList varchar(3999) " + "\r\n";
            queryString = queryString + "       SELECT      @EntryDate = EntryDate, @LocationID = LocationID, @CustomerID = CustomerID FROM DeliveryAdvices WHERE DeliveryAdviceID = @DeliveryAdviceID " + "\r\n";
            queryString = queryString + "       IF          @EntryDate IS NULL          SET @EntryDate = CONVERT(Datetime, '31/12/2000', 103)" + "\r\n";
            queryString = queryString + "       SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID AS varchar)  FROM Warehouses WHERE LocationID = @LocationID FOR XML PATH('')) ,1,1,'') " + "\r\n";//The best way is get the @WarehouseIDList from table DeliveryAdviceDetails, but we don't want the stored procedure read from DeliveryAdviceDetails to save the resource
            queryString = queryString + "       SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID AS varchar)  FROM DeliveryAdviceDetails WHERE DeliveryAdviceID = @DeliveryAdviceID FOR XML PATH('')) ,1,1,'') " + "\r\n";

            queryString = queryString + "       " + inventories.GET_WarehouseJournal_BUILD_SQL("@WarehouseJournalTable", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0") + "\r\n";

            queryString = queryString + "       SELECT      DeliveryAdviceDetails.DeliveryAdviceDetailID, DeliveryAdviceDetails.DeliveryAdviceID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, DeliveryAdviceDetails.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, VoidTypes.VoidTypeID, VoidTypes.Code AS VoidTypeCode, VoidTypes.Name AS VoidTypeName, VoidTypes.VoidClassID, DeliveryAdviceDetails.CalculatingTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(CAST(ISNULL(CommoditiesAvailable.QuantityAvailable, 0) AS decimal(18, 2)) + DeliveryAdviceDetails.Quantity, 0) AS QuantityAvailable, DeliveryAdviceDetails.Quantity, DeliveryAdviceDetails.ControlFreeQuantity, DeliveryAdviceDetails.FreeQuantity, DeliveryAdviceDetails.ListedPrice, DeliveryAdviceDetails.DiscountPercent, DeliveryAdviceDetails.UnitPrice, DeliveryAdviceDetails.VATPercent, DeliveryAdviceDetails.ListedGrossPrice, DeliveryAdviceDetails.GrossPrice, DeliveryAdviceDetails.ListedAmount, DeliveryAdviceDetails.Amount, DeliveryAdviceDetails.ListedVATAmount, DeliveryAdviceDetails.VATAmount, DeliveryAdviceDetails.ListedGrossAmount, DeliveryAdviceDetails.GrossAmount, DeliveryAdviceDetails.IsBonus, DeliveryAdviceDetails.InActivePartial, DeliveryAdviceDetails.InActivePartialDate, DeliveryAdviceDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        DeliveryAdviceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON DeliveryAdviceDetails.DeliveryAdviceID = @DeliveryAdviceID AND DeliveryAdviceDetails.CommodityID = Commodities.CommodityID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Warehouses ON DeliveryAdviceDetails.WarehouseID = Warehouses.WarehouseID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   VoidTypes ON DeliveryAdviceDetails.VoidTypeID = VoidTypes.VoidTypeID LEFT JOIN" + "\r\n";
            queryString = queryString + "                  (SELECT WarehouseID, CommodityID, SUM(QuantityBegin) AS QuantityAvailable FROM @WarehouseJournalTable GROUP BY WarehouseID, CommodityID) CommoditiesAvailable ON DeliveryAdviceDetails.WarehouseID = CommoditiesAvailable.WarehouseID AND DeliveryAdviceDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n"; //SUM(QuantityBeginQuantityEndREC) 

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetDeliveryAdviceViewDetails", queryString);
        }

        private void DeliveryAdviceSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            //queryString = queryString + "       EXEC        DeliveryAdviceUpdateQuotation @EntityID, @SaveRelativeOption " + "\r\n";

            //queryString = queryString + "       SET         @SaveRelativeOption = -@SaveRelativeOption" + "\r\n";
            //queryString = queryString + "       EXEC        UpdateWarehouseBalance @SaveRelativeOption, 0, @EntityID, 0, 0 ";

            this.totalSalesPortalEntities.CreateStoredProcedure("DeliveryAdviceSaveRelative", queryString);
        }

        private void DeliveryAdvicePostSaveValidate()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Service Date: ' + CAST(ServiceInvoices.EntryDate AS nvarchar) FROM DeliveryAdvices INNER JOIN DeliveryAdvices AS ServiceInvoices ON DeliveryAdvices.DeliveryAdviceID = @EntityID AND DeliveryAdvices.ServiceInvoiceID = ServiceInvoices.DeliveryAdviceID AND DeliveryAdvices.EntryDate < ServiceInvoices.EntryDate ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("DeliveryAdvicePostSaveValidate", queryArray);
        }




        private void DeliveryAdviceApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = DeliveryAdviceID FROM DeliveryAdvices WHERE DeliveryAdviceID = @EntityID AND Approved = 1";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("DeliveryAdviceApproved", queryArray);
        }


        private void DeliveryAdviceEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = DeliveryAdviceID FROM DeliveryAdvices WHERE DeliveryAdviceID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = DeliveryAdviceID FROM GoodsIssueDetails WHERE DeliveryAdviceID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("DeliveryAdviceEditable", queryArray);
        }

        private void DeliveryAdviceVoidable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = DeliveryAdviceID FROM DeliveryAdvices WHERE DeliveryAdviceID = @EntityID AND Approved = 0"; //Must approve in order to allow void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = DeliveryAdviceID FROM GoodsIssueDetails WHERE DeliveryAdviceID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("DeliveryAdviceVoidable", queryArray);
        }


        private void DeliveryAdviceToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      DeliveryAdvices  SET Approved = @Approved, ApprovedDate = GetDate() WHERE DeliveryAdviceID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           UPDATE          DeliveryAdviceDetails  SET Approved = @Approved WHERE DeliveryAdviceID = @EntityID ; " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, 'hủy', '')  + ' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("DeliveryAdviceToggleApproved", queryString);
        }

        private void DeliveryAdviceToggleVoid()
        {
            string queryString = " @EntityID int, @InActive bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      DeliveryAdvices  SET InActive = @InActive, InActiveDate = GetDate(), VoidTypeID = IIF(@InActive = 1, @VoidTypeID, NULL) WHERE DeliveryAdviceID = @EntityID AND InActive = ~@InActive" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           UPDATE          DeliveryAdviceDetails  SET InActive = @InActive WHERE DeliveryAdviceID = @EntityID ; " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActive = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            this.totalSalesPortalEntities.CreateStoredProcedure("DeliveryAdviceToggleVoid", queryString);
        }

        private void DeliveryAdviceToggleVoidDetail()
        {
            string queryString = " @EntityID int, @EntityDetailID int, @InActivePartial bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      DeliveryAdviceDetails  SET InActivePartial = @InActivePartial, InActivePartialDate = GetDate(), VoidTypeID = IIF(@InActivePartial = 1, @VoidTypeID, NULL) WHERE DeliveryAdviceID = @EntityID AND DeliveryAdviceDetailID = @EntityDetailID AND InActivePartial = ~@InActivePartial ; " + "\r\n";
            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           UPDATE          DeliveryAdvices  SET InActivePartial = (SELECT MAX(CAST(InActivePartial AS int)) FROM DeliveryAdviceDetails WHERE DeliveryAdviceID = @EntityID) WHERE DeliveryAdviceID = @EntityID ; " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActivePartial = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            this.totalSalesPortalEntities.CreateStoredProcedure("DeliveryAdviceToggleVoidDetail", queryString);
        }


        private void DeliveryAdviceInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("DeliveryAdvices", "DeliveryAdviceID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.DeliveryAdvice));
            this.totalSalesPortalEntities.CreateTrigger("DeliveryAdviceInitReference", simpleInitReference.CreateQuery());
        }


        #endregion
    }
}
