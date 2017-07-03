using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalCore.Repositories.Commons;
using TotalModel.Models;
using TotalDTO.Commons;
using TotalPortal.APIs.Sessions;

using Microsoft.AspNet.Identity;


namespace TotalPortal.Areas.Commons.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class CommodityPriceAPIsController : Controller
    {
        private readonly ICommodityPriceAPIRepository commodityPriceAPIRepository;

        public CommodityPriceAPIsController(ICommodityPriceAPIRepository commodityPriceAPIRepository)
        {
            this.commodityPriceAPIRepository = commodityPriceAPIRepository;
        }

        public JsonResult GetCommodityPriceIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<CommodityPriceIndex> CommodityPriceIndexes = this.commodityPriceAPIRepository.GetEntityIndexes<CommodityPriceIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = CommodityPriceIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}