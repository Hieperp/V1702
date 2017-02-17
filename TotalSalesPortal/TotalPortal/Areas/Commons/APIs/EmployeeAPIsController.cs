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
    public class EmployeeAPIsController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeAPIsController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }


        public JsonResult SearchEmployees(string searchText)
        {
            var result = employeeRepository.SearchEmployees(searchText).Select(s => new { s.EmployeeID, s.Code, s.Name });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}