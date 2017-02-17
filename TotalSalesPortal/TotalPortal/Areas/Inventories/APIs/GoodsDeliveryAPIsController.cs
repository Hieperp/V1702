using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web.UI;

using Microsoft.AspNet.Identity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Inventories;

using TotalCore.Repositories.Inventories;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.APIs.Sessions;

namespace TotalPortal.Areas.Inventories.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class GoodsDeliveryAPIsController : Controller
    {
        private readonly IGoodsDeliveryAPIRepository goodsDeliveryAPIRepository;

        public GoodsDeliveryAPIsController(IGoodsDeliveryAPIRepository goodsDeliveryAPIRepository)
        {
            this.goodsDeliveryAPIRepository = goodsDeliveryAPIRepository;
        }

        public JsonResult GetGoodsDeliveryIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<GoodsDeliveryIndex> goodsDeliveryIndexes = this.goodsDeliveryAPIRepository.GetEntityIndexes<GoodsDeliveryIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = goodsDeliveryIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetReceivers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.goodsDeliveryAPIRepository.GetReceivers(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingHandlingUnits([DataSourceRequest] DataSourceRequest dataSourceRequest, int? goodsDeliveryID, int? receiverID, string handlingUnitIDs, bool isReadonly)
        {
            var result = this.goodsDeliveryAPIRepository.GetPendingHandlingUnits(goodsDeliveryID, receiverID, handlingUnitIDs, false);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }


    }

}