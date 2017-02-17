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
    public class PromotionAPIsController : Controller
    {
        private readonly IPromotionRepository promotionRepository;

        public PromotionAPIsController(IPromotionRepository promotionRepository)
        {
            this.promotionRepository = promotionRepository;
        }


        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetPromotionByCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? customerID)
        {
            if (customerID == null) return Json(null);

            var result = promotionRepository.GetPromotionByCustomers((int) customerID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }
    }
}