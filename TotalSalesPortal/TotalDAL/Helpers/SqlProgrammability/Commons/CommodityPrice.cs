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
        }


        private void GetCommodityPriceIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      CommodityPrices.CommodityPriceID, PriceCategories.PriceCategoryID, PriceCategories.Code AS PriceCategory, CommodityPrices.CodePartA, CommodityPrices.CodePartB, CommodityPrices.CodePartC, CommodityPrices.CodePartA + ISNULL(' ' + CommodityPrices.CodePartB, '') + ' '+ CommodityPrices.CodePartC AS CodePartABC, CommodityPrices.ListedPrice, CommodityPrices.GrossPrice, CommodityPrices.Remarks " + "\r\n";
            queryString = queryString + "       FROM        PriceCategories " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityPrices ON PriceCategories.PriceCategoryID = CommodityPrices.PriceCategoryID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetCommodityPriceIndexes", queryString);
        }

    }
}
