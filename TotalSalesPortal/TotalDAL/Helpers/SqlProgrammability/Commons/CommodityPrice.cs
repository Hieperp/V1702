using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class CommodityPrice
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public CommodityPrice(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetCommodityPriceIndexes();
            this.GetCommodityCodePart();

            this.InportCommodities();
        }


        private void GetCommodityPriceIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      CommodityPrices.CommodityPriceID, PriceCategories.PriceCategoryID, PriceCategories.Code AS PriceCategory, CommodityPrices.CodePartA, CommodityPrices.CodePartB, CommodityPrices.CodePartC, PriceCategories.Code + ' ' + CommodityPrices.CodePartA + ISNULL(' ' + CommodityPrices.CodePartB, '') + ' '+ CommodityPrices.CodePartC AS CodePartABC, CommodityPrices.ListedPrice, CommodityPrices.GrossPrice, ISNULL(CommodityWeights.Weight, 0) AS Weight, CommodityPrices.Remarks " + "\r\n";
            queryString = queryString + "       FROM        PriceCategories " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityPrices ON PriceCategories.PriceCategoryID = CommodityPrices.PriceCategoryID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN (SELECT CodePartA, CodePartC, MAX(Weight) AS Weight FROM Commodities GROUP BY CodePartA, CodePartC) CommodityWeights ON CommodityPrices.CodePartA = CommodityWeights.CodePartA AND CommodityPrices.CodePartC = CommodityWeights.CodePartC " + "\r\n";
            

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetCommodityPriceIndexes", queryString);
        }

        private void GetCommodityCodePart()
        {
            string queryString;

            queryString = " @SearchText nvarchar(60) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      DISTINCT CodePartA AS CodePart FROM Commodities WHERE CodePartA LIKE '%' + @SearchText + '%' ORDER BY CodePart " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetCommodityCodePartA", queryString);

            queryString = " @SearchText nvarchar(60) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      DISTINCT CodePartB AS CodePart FROM Commodities WHERE CodePartB LIKE @SearchText + '%' ORDER BY CodePart " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetCommodityCodePartB", queryString);

            queryString = " @SearchText nvarchar(60) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      DISTINCT CodePartC AS CodePart FROM Commodities WHERE CodePartC LIKE '%' + @SearchText + '%' ORDER BY CodePart " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetCommodityCodePartC", queryString);
        }


        private void InportCommodities()
        {
            string queryString;

            queryString = "  " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       INSERT INTO     Commodities (CommodityID, Code, OfficialCode, CodePartA, CodePartB, CodePartC, CodePartD, Name, OfficialName, OriginalName, PreviousCommodityID, CommodityBrandID, CommodityCategoryID, CommodityTypeID, SupplierID, PiecePerPack, QuantityAlert, ListedPrice, GrossPrice, PurchaseUnit, SalesUnit, Packing, Origin, Weight, LeadTime, HSCode, IsRegularCheckUps, Discontinue, Specifycation, Remarks, InActive) " + "\r\n";
            queryString = queryString + "       SELECT          CommodityID, Description AS Code, Description AS OfficialCode, DescriptionPartA AS CodePartA, DescriptionPartB AS CodePartB, DescriptionPartC AS CodePartC, DescriptionPartD AS CodePartD, Description AS Name, Description AS OfficialName, Description AS OriginalName, NULL AS PreviousCommodityID, 1 AS CommodityBrandID, ItemCategoryID AS CommodityCategoryID, 2 AS CommodityTypeID, 1 AS SupplierID, PiecePerPack, QuantityAlert, 0 AS ListedPrice, 0 AS GrossPrice, UnitSales AS PurchaseUnit, UnitSales AS SalesUnit, Packing, Origin, Weight, LeadTime, HSCode, 0 AS IsRegularCheckUps, Discontinue, Specifycation, Remarks, InActive  " + "\r\n";
            queryString = queryString + "       FROM            ERmgrVCP.dbo.ListItemCommodity " + "\r\n";
            queryString = queryString + "       WHERE           CommodityID NOT IN (SELECT CommodityID FROM Commodities) " + "\r\n";

//------UPDATE CommodityBrandID = 2: LVC
            queryString = queryString + "       UPDATE          Commodities " + "\r\n";
            queryString = queryString + "       SET             Commodities.CommodityBrandID = 2 " + "\r\n";
            queryString = queryString + "       FROM            Commodities INNER JOIN " + "\r\n";
            queryString = queryString + "                       TotalSalesPortal_Brand_LVC.dbo.PromotionCommodityCodeParts PromotionCommodityCodeParts ON Commodities.CodePartA = PromotionCommodityCodeParts.CodePartA AND Commodities.CodePartC = PromotionCommodityCodeParts.CodePartC AND PromotionCommodityCodeParts.PromotionID = 48 " + "\r\n";

//------UPDATE Weight
            queryString = queryString + "       UPDATE	        Commodities  " + "\r\n";
            queryString = queryString + "       SET		        Commodities.Weight =  ERP_Commodities.Weight " + "\r\n";
            queryString = queryString + "       FROM	        Commodities INNER JOIN ERmgrVCP.dbo.ListItemCommodity ERP_Commodities ON Commodities.CommodityID = ERP_Commodities.CommodityID  " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("InportCommodities", queryString);
        }

    }
}
