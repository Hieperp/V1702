using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class CommodityRepository : GenericRepository<Commodity>, ICommodityRepository
    {
        public CommodityRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, null, null, "CommodityDeletable")
        {
        }

        public IList<Commodity> SearchCommodities(string searchText, string commodityTypeIDList, bool? isOnlyAlphaNumericString)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;

            if (isOnlyAlphaNumericString != null && (bool)isOnlyAlphaNumericString) searchText = TotalBase.CommonExpressions.AlphaNumericString(searchText);

            var queryable = this.TotalSalesPortalEntities.Commodities.Where(wi => (bool)wi.InActive != true).Where(w => w.Code.Contains(searchText) || w.OfficialCode.Contains(searchText) || w.Name.Contains(searchText)).Include(i => i.CommodityCategory);
            if (commodityTypeIDList != null)
            {
                List<int> listCommodityTypeID = commodityTypeIDList.Split(',').Select(n => int.Parse(n)).ToList();
                queryable = queryable.Where(w => listCommodityTypeID.Contains(w.CommodityTypeID));
            }

            List<Commodity> commodities = queryable.ToList();

            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return commodities;
        }

        public IList<Commodity> SearchCommoditiesByIndex(int commodityCategoryID, int commodityTypeID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Commodity> commodities = this.TotalSalesPortalEntities.Commodities.Where(w => w.InActive != true && (w.CommodityCategoryID == commodityCategoryID || w.CommodityTypeID == commodityTypeID)).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return commodities;
        }

        //public IList<CommoditiesInGoodsReceipt> GetCommoditiesInGoodsReceipts(int? locationID, string searchText, int? salesInvoiceID, int? stockTransferID, int? inventoryAdjustmentID)
        //{
        //    List<CommoditiesInGoodsReceipt> commoditiesInGoodsReceipts = this.TotalSalesPortalEntities.GetCommoditiesInGoodsReceipts(locationID, searchText, salesInvoiceID, stockTransferID, inventoryAdjustmentID).ToList();

        //    return commoditiesInGoodsReceipts;
        //}

        //public IList<CommoditiesInWarehouse> GetCommoditiesInWarehouses(int? locationID, DateTime? entryDate, string searchText, bool includeCommoditiesOutOfStock, int? salesInvoiceID, int? stockTransferID, int? inventoryAdjustmentID)
        //{
        //    List<CommoditiesInWarehouse> commoditiesInWarehouses;

        //    if (!includeCommoditiesOutOfStock)
        //        commoditiesInWarehouses = this.TotalSalesPortalEntities.GetCommoditiesInWarehouses(locationID, entryDate, searchText, salesInvoiceID, stockTransferID, inventoryAdjustmentID).ToList();
        //    else
        //        commoditiesInWarehouses = this.TotalSalesPortalEntities.GetCommoditiesInWarehousesIncludeOutOfStock(locationID, entryDate, searchText, salesInvoiceID, stockTransferID, inventoryAdjustmentID).ToList();

        //    return commoditiesInWarehouses;
        //}


        //public IList<CommoditiesAvailable> GetCommoditiesAvailables(int? locationID, DateTime? entryDate, string searchText)
        //{
        //    List<CommoditiesAvailable> commoditiesAvailables = this.TotalSalesPortalEntities.GetCommoditiesAvailables(locationID, entryDate, searchText).ToList();

        //    return commoditiesAvailables;
        //}

        //public IList<VehicleAvailable> GetVehicleAvailables(int? locationID, DateTime? entryDate, string searchText)
        //{
        //    List<VehicleAvailable> vehicleAvailables = this.TotalSalesPortalEntities.GetVehicleAvailables(locationID, entryDate, searchText).ToList();

        //    return vehicleAvailables;
        //}

        public IList<CommodityBase> GetCommodityBases(string commodityTypeIDList, string searchText, bool? isOnlyAlphaNumericString)
        {
            if (isOnlyAlphaNumericString != null && (bool)isOnlyAlphaNumericString) searchText = TotalBase.CommonExpressions.AlphaNumericString(searchText);
            List<CommodityBase> commodityBases = this.TotalSalesPortalEntities.GetCommodityBases(commodityTypeIDList, searchText).ToList();

            return commodityBases;
        }

        public IList<CommodityAvailable> GetCommodityAvailables(int? locationID, int? customerID, int? warehouseID, int? priceCategoryID, int? promotionID, DateTime? entryDate, string searchText)
        {
            List<CommodityAvailable> commodityAvailables = this.TotalSalesPortalEntities.GetCommodityAvailables(locationID, customerID, warehouseID, priceCategoryID, promotionID, entryDate, searchText).ToList();

            return commodityAvailables;
        }


    }
}
