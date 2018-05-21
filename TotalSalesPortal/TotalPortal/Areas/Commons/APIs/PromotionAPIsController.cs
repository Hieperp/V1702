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
        public JsonResult GetPromotionByCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? customerID, int? applyToSalesVersusReturns, int? filterApplyToTradeDiscount, bool? forDropDownList)
        {
            if (customerID == null) return Json(null);

            IList<Promotion> result = promotionAPIRepository.GetPromotionByCustomers(customerID, applyToSalesVersusReturns, filterApplyToTradeDiscount); //filterApplyToTradeDiscount == 0 || 1 || -1 => WHERE: -1 MEAN: SELECT ALL
            if (forDropDownList != null && (bool)forDropDownList)
            {
                if (result.Count() > 0 && result.FirstOrDefault(w => w.ApplyToAllCommodities) == null)
                    result.Insert(0, new Promotion() { Specs = "KHÔNG ÁP DỤNG CK TỔNG  [CK 1 DÒNG]" });

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }






        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetPromotionCustomerCategories([DataSourceRequest] DataSourceRequest dataSourceRequest, int? promotionID)
        {
            if (promotionID == null) return Json(null);

            var result = promotionAPIRepository.GetPromotionCustomerCategories((int)promotionID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult AddPromotionCustomerCategories(int? promotionID, int? customerCategoryID)
        {
            try
            {
                this.promotionAPIRepository.AddPromotionCustomerCategories(promotionID, customerCategoryID);
                return Json(new { AddResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { AddResult = "Trùng chương trình, hoặc " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult RemovePromotionCustomerCategories(int? promotionID, int? customerCategoryID)
        {
            try
            {
                this.promotionAPIRepository.RemovePromotionCustomerCategories(promotionID, customerCategoryID);
                return Json(new { RemoveResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { RemoveResult = ex.Message }, JsonRequestBehavior.AllowGet);
            }
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