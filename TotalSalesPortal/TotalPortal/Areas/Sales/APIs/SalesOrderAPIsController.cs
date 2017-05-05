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
    public class SalesOrderAPIsController : Controller
    {
        private readonly ISalesOrderAPIRepository salesOrderAPIRepository;

        public SalesOrderAPIsController(ISalesOrderAPIRepository salesOrderAPIRepository)
        {
            this.salesOrderAPIRepository = salesOrderAPIRepository;
        }


        public JsonResult GetSalesOrderIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<SalesOrderIndex> salesOrderIndexes = this.salesOrderAPIRepository.GetEntityIndexes<SalesOrderIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = salesOrderIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
