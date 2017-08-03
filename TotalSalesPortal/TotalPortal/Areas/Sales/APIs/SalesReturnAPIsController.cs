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
    public class SalesReturnAPIsController : Controller
    {
        private readonly ISalesReturnAPIRepository salesReturnAPIRepository;

        public SalesReturnAPIsController(ISalesReturnAPIRepository salesReturnAPIRepository)
        {
            this.salesReturnAPIRepository = salesReturnAPIRepository;
        }


        public JsonResult GetSalesReturnIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<SalesReturnIndex> salesReturnIndexes = this.salesReturnAPIRepository.GetEntityIndexes<SalesReturnIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = salesReturnIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }





        public JsonResult GetGoodsIssues([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? customerID, int? receiverID, DateTime? fromDate, DateTime? toDate)
        {
            var result = this.salesReturnAPIRepository.GetGoodsIssues(locationID, customerID, receiverID, fromDate, toDate);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingGoodsIssueDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? salesReturnID, int? goodsIssueID, int? customerID, int? receiverID, int? tradePromotionID, decimal? vATPercent, DateTime? fromDate, DateTime? toDate, string goodsIssueDetailIDs, bool isReadonly)
        {
            var result = this.salesReturnAPIRepository.GetPendingGoodsIssueDetails(locationID, salesReturnID, goodsIssueID, customerID, receiverID, tradePromotionID, vATPercent, fromDate, toDate, goodsIssueDetailIDs, isReadonly);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }



    }
}
