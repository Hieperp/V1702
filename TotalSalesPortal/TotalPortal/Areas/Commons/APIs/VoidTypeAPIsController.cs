using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalCore.Repositories.Commons;
using TotalModel.Models;
using TotalDTO.Commons;

namespace TotalPortal.Areas.Commons.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class VoidTypeAPIsController : Controller
    {
        private readonly IVoidTypeRepository voidTypeRepository;

        public VoidTypeAPIsController(IVoidTypeRepository voidTypeRepository)
        {
            this.voidTypeRepository = voidTypeRepository;
        }


        public JsonResult SearchVoidTypes(string searchText)
        {
            var result = voidTypeRepository.SearchVoidTypes(searchText).Select(s => new { s.VoidTypeID, s.Code, s.Name, s.VoidClassID });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}