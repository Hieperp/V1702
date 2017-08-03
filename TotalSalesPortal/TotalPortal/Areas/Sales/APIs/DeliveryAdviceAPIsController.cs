using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web.UI;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Sales;

using TotalCore.Repositories.Sales;
using TotalPortal.Areas.Sales.ViewModels;
using TotalPortal.APIs.Sessions;

using Microsoft.AspNet.Identity;

namespace TotalPortal.Areas.Sales.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class DeliveryAdviceAPIsController : Controller
    {
        private readonly IDeliveryAdviceAPIRepository deliveryAdviceAPIRepository;

        public DeliveryAdviceAPIsController(IDeliveryAdviceAPIRepository deliveryAdviceAPIRepository)
        {
            this.deliveryAdviceAPIRepository = deliveryAdviceAPIRepository;
        }


        public JsonResult GetDeliveryAdviceIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<DeliveryAdviceIndex> deliveryAdviceIndexes = this.deliveryAdviceAPIRepository.GetEntityIndexes<DeliveryAdviceIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = deliveryAdviceIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }





        public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.deliveryAdviceAPIRepository.GetCustomers(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalesOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.deliveryAdviceAPIRepository.GetSalesOrders(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPendingSalesOrderDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? deliveryAdviceID, int? salesOrderID, int? customerID, int? receiverID, int? priceCategoryID, int? warehouseID, string shippingAddress, int? tradePromotionID, decimal? vatPercent, DateTime? entryDate, string salesOrderDetailIDs, bool isReadonly)
        {
            var result = this.deliveryAdviceAPIRepository.GetPendingSalesOrderDetails(locationID, deliveryAdviceID, salesOrderID, customerID, receiverID, priceCategoryID, warehouseID, shippingAddress, tradePromotionID, vatPercent, entryDate, salesOrderDetailIDs, false);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }



    }
}
