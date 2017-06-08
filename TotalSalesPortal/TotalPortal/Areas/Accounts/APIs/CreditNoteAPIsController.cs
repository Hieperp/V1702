using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web.UI;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using TotalModel.Models;

using TotalCore.Repositories.Accounts;
using TotalPortal.APIs.Sessions;

using Microsoft.AspNet.Identity;

namespace TotalPortal.Areas.Accounts.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class CreditNoteAPIsController : Controller
    {
        private readonly ICreditNoteAPIRepository creditNoteAPIRepository;

        public CreditNoteAPIsController(ICreditNoteAPIRepository creditNoteAPIRepository)
        {
            this.creditNoteAPIRepository = creditNoteAPIRepository;
        }


        public JsonResult GetCreditNoteIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<CreditNoteIndex> creditNoteIndexes = this.creditNoteAPIRepository.GetEntityIndexes<CreditNoteIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = creditNoteIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
