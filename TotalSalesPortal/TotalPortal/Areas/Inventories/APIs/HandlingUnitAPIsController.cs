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
    public class HandlingUnitAPIsController : Controller
    {
        private readonly IHandlingUnitAPIRepository handlingUnitAPIRepository;

        public HandlingUnitAPIsController(IHandlingUnitAPIRepository handlingUnitAPIRepository)
        {
            this.handlingUnitAPIRepository = handlingUnitAPIRepository;
        }

        public JsonResult GetHandlingUnitIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<HandlingUnitIndex> handlingUnitIndexes = this.handlingUnitAPIRepository.GetEntityIndexes<HandlingUnitIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = handlingUnitIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.handlingUnitAPIRepository.GetCustomers(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGoodsIssues([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.handlingUnitAPIRepository.GetGoodsIssues(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPendingGoodsIssueDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? handlingUnitID, int? goodsIssueID, int? customerID, int? receiverID, string shippingAddress, string addressee, string goodsIssueDetailIDs, bool isReadonly)
        {
            var result = this.handlingUnitAPIRepository.GetPendingGoodsIssueDetails(locationID, handlingUnitID, goodsIssueID, customerID, receiverID, shippingAddress, addressee, goodsIssueDetailIDs, false);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }


    }

}