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
using System;


namespace TotalPortal.Areas.Commons.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class SearchAPIsController : Controller
    {
        private readonly ISearchAPIRepository searchAPIRepository;

        public SearchAPIsController(ISearchAPIRepository searchAPIRepository)
        {
            this.searchAPIRepository = searchAPIRepository;
        }

        public JsonResult SearchWarehouseEntries([DataSourceRequest] DataSourceRequest request, DateTime fromDate, DateTime toDate, String codePartA, String codePartB)
        {
            if (codePartA.Trim().Length >= 3)
            {
                IList<WarehouseEntry> warehouseEntries = this.searchAPIRepository.SearchWarehouseEntries(User.Identity.GetUserId(), fromDate, toDate, codePartA, codePartB);

                DataSourceResult response = warehouseEntries.ToDataSourceResult(request);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new List<WarehouseEntry>(), JsonRequestBehavior.AllowGet);
        }
    }
}