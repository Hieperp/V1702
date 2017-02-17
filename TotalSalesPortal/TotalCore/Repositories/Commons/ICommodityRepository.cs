using System;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalCore.Repositories.Commons
{
    public interface ICommodityRepository : IGenericRepository<Commodity>
    {
        IList<Commodity> SearchCommodities(string searchText, string commodityTypeIDList, bool? isOnlyAlphaNumericString);
        IList<Commodity> SearchCommoditiesByIndex(int commodityCategoryID, int commodityTypeID);
        //IList<CommoditiesInGoodsReceipt> GetCommoditiesInGoodsReceipts(int? locationID, string searchText, int? salesInvoiceID, int? stockTransferID, int? inventoryAdjustmentID);
        //IList<CommoditiesInWarehouse> GetCommoditiesInWarehouses(int? locationID, DateTime? entryDate, string searchText, bool includeCommoditiesOutOfStock, int? salesInvoiceID, int? stockTransferID, int? inventoryAdjustmentID);

        //IList<CommoditiesAvailable> GetCommoditiesAvailables(int? locationID, DateTime? entryDate, string searchText);
        //IList<VehicleAvailable> GetVehicleAvailables(int? locationID, DateTime? entryDate, string searchText);
        IList<CommodityAvailable> GetCommodityAvailables(int? locationID, int? customerID, int? priceCategoryID, int? promotionID, DateTime? entryDate, string searchText);
    }
}