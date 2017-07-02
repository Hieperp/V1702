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
    public class EmployeeAPIsController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IEmployeeAPIRepository employeeAPIRepository;

        public EmployeeAPIsController(IEmployeeRepository employeeRepository, IEmployeeAPIRepository employeeAPIRepository)
        {
            this.employeeRepository = employeeRepository;
            this.employeeAPIRepository = employeeAPIRepository;
        }

        public JsonResult GetEmployeeIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<EmployeeIndex> EmployeeIndexes = this.employeeAPIRepository.GetEntityIndexes<EmployeeIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = EmployeeIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchEmployees(string searchText)
        {
            var result = employeeRepository.SearchEmployees(searchText).Select(s => new { s.EmployeeID, s.Code, s.Name });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}