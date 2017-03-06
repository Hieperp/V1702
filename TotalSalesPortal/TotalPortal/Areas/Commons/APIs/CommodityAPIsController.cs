using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalBase.Enums;

using TotalCore.Repositories.Commons;

using TotalDTO.Commons;
using TotalDAL.Repositories;
using TotalService.Commons;



using TotalModel.Models;


namespace TotalPortal.Areas.Commons.APIs
{
    public class CommodityAPIsController : Controller
    {
        private readonly ICommodityRepository commodityRepository;

        public CommodityAPIsController(ICommodityRepository commodityRepository)
        {
            this.commodityRepository = commodityRepository;
        }


        


        public JsonResult SearchCommodities(string searchText, string commodityTypeIDList, bool? isOnlyAlphaNumericString)
        {
            var result = commodityRepository.SearchCommodities(searchText, commodityTypeIDList, isOnlyAlphaNumericString).Select(s => new { s.CommodityID, s.Code, s.Name, s.CommodityTypeID, CommodityCategoryLimitedKilometreWarranty = s.CommodityCategory.LimitedKilometreWarranty, CommodityCategoryLimitedMonthWarranty = s.CommodityCategory.LimitedMonthWarranty, s.GrossPrice, s.CommodityCategory.VATPercent });

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        //public JsonResult GetCommoditiesInGoodsReceipts(int? locationID, string searchText, int? salesInvoiceID, int? stockTransferID, int? inventoryAdjustmentID)
        //{
        //    var result = commodityRepository.GetCommoditiesInGoodsReceipts(locationID, searchText, salesInvoiceID, stockTransferID, inventoryAdjustmentID).Select(s => new { s.GoodsReceiptDetailID, s.SupplierID, s.CommodityID, s.CommodityCode, s.CommodityName, s.CommodityTypeID, s.WarehouseID, s.WarehouseCode, s.ChassisCode, s.EngineCode, s.ColorCode, s.QuantityAvailable, s.GrossPrice, s.DiscountPercent, s.VATPercent });

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        //public JsonResult GetCommoditiesInWarehouses(int? locationID, DateTime? entryDate, string searchText, bool includeCommoditiesOutOfStock, int? salesInvoiceID, int? stockTransferID, int? inventoryAdjustmentID)
        //{
        //    var result = commodityRepository.GetCommoditiesInWarehouses(locationID, entryDate, searchText, includeCommoditiesOutOfStock, salesInvoiceID, stockTransferID, inventoryAdjustmentID).Select(s => new { s.CommodityID, s.CommodityCode, s.CommodityName, s.CommodityTypeID, s.WarehouseID, s.WarehouseCode, s.QuantityAvailable, s.GrossPrice, s.VATPercent });

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        //public JsonResult GetCommoditiesAvailables(int? locationID, DateTime? entryDate, string searchText)
        //{
        //    var result = commodityRepository.GetCommoditiesAvailables(locationID, entryDate, searchText).Select(s => new { s.CommodityID, s.CommodityCode, s.CommodityName, s.CommodityTypeID, s.WarehouseID, s.WarehouseCode, s.QuantityAvailable, s.GrossPrice, s.VATPercent });

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        //public JsonResult GetVehicleAvailables(int? locationID, DateTime? entryDate, string searchText)
        //{
        //    var result = commodityRepository.GetVehicleAvailables(locationID, entryDate, searchText).Select(s => new { s.CommodityID, s.CommodityCode, s.CommodityName, s.CommodityTypeID, s.WarehouseID, s.WarehouseCode, s.QuantityAvailable, s.GrossPrice, s.VATPercent });

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}


        /// <summary>
        /// This function is designed to use by import function only
        /// Never to use by orther area
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCommodityImport(int? locationID, int? customerID, int? priceCategoryID, int? promotionID, DateTime? entryDate, string searchText)
        {
            try
            {
                var commodityResult = new { CommodityID = 0, CommodityCode = "", CommodityName = "", CommodityTypeID = 0, WarehouseID = 0, WarehouseCode = "", QuantityAvailable = new decimal(0), ControlFreeQuantity = new decimal(0), ListedPrice = new decimal(0), GrossPrice = new decimal(0), DiscountPercent = new decimal(0), VATPercent = new decimal(0) };

                var result = commodityRepository.GetCommodityAvailables(locationID, customerID, priceCategoryID, promotionID, entryDate, searchText).Select(s => new { s.CommodityID, s.CommodityCode, s.CommodityName, s.CommodityTypeID, s.WarehouseID, s.WarehouseCode, s.QuantityAvailable, s.ControlFreeQuantity, s.ListedPrice, s.GrossPrice, s.DiscountPercent, s.VATPercent, s.Bookable });
                if (result.Count() > 0 && (bool)result.First().Bookable)
                    commodityResult = new { CommodityID = result.First().CommodityID, CommodityCode = result.First().CommodityCode, CommodityName = result.First().CommodityName, CommodityTypeID = result.First().CommodityTypeID, WarehouseID = result.First().WarehouseID, WarehouseCode = result.First().WarehouseCode, QuantityAvailable = (decimal)result.First().QuantityAvailable, ControlFreeQuantity = (decimal)result.First().ControlFreeQuantity, ListedPrice = (decimal)result.First().ListedPrice, GrossPrice = (decimal)result.First().GrossPrice, DiscountPercent = (decimal)result.First().DiscountPercent, VATPercent = (decimal)result.First().VATPercent };

                return Json(commodityResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { CommodityID = 0, CommodityCode = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetCommodityAvailables(int? locationID, int? customerID, int? priceCategoryID, int? promotionID, DateTime? entryDate, string searchText)
        {
            var result = commodityRepository.GetCommodityAvailables(locationID, customerID, priceCategoryID, promotionID, entryDate, searchText).Select(s => new { s.CommodityID, s.CommodityCode, s.CommodityName, s.CommodityTypeID, s.WarehouseID, s.WarehouseCode, s.QuantityAvailable, s.ControlFreeQuantity, s.ListedPrice, s.GrossPrice, s.DiscountPercent, s.VATPercent, s.Bookable });

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCommodities([DataSourceRequest] DataSourceRequest request, int commodityCategoryID, int commodityTypeID)
        {
            var commodities = this.commodityRepository.SearchCommoditiesByIndex(commodityCategoryID, commodityTypeID);

            DataSourceResult response = commodities.ToDataSourceResult(request, o => new CommodityPrimitiveDTO
            {
                CommodityID = o.CommodityID,
                Code = o.Code,
                Name = o.Name,
                OfficialName = o.OfficialName,
                OriginalName = o.OriginalName,
                //CommodityTypeName = o.CommodityType.Name,
                //CommodityCategoryName = o.CommodityCategory.Name,
                GrossPrice = o.GrossPrice,
                Remarks = o.Remarks
            });
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}