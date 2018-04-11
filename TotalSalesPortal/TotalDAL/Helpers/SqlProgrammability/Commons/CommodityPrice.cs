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

    }
}
