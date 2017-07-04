using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using Microsoft.AspNet.Identity;

using TotalBase.Enums;

using TotalCore.Repositories.Commons;

using TotalDTO.Commons;
using TotalDAL.Repositories;
using TotalModel.Models;

using TotalService.Commons;
using TotalPortal.APIs.Sessions;

namespace TotalPortal.Areas.Commons.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class PromotionAPIsController : Controller
    {
        private readonly IPromotionAPIRepository promotionAPIRepository;

        public PromotionAPIsController(IPromotionAPIRepository promotionAPIRepository)
        {
            this.promotionAPIRepository = promotionAPIRepository;
        }



        public JsonResult GetPromotionIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<PromotionIndex> promotionIndexes = this.promotionAPIRepository.GetEntityIndexes<PromotionIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = promotionIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }



        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetPromotionByCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? customerID)
        {
            if (customerID == null) return Json(null);

            var result = promotionAPIRepository.GetPromotionByCustomers((int) customerID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddPromotionCustomers(int? promotionID, int? customerID)
        {
            try
            {
                this.promotionAPIRepository.AddPromotionCustomers(promotionID, customerID);
                return Json(new { AddResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { AddResult = "Trùng chương trình, hoặc " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult RemovePromotionCustomers(int? promotionID, int? customerID)
        {
            try
            {
                this.promotionAPIRepository.RemovePromotionCustomers(promotionID, customerID);
                return Json(new { RemoveResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { RemoveResult = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}