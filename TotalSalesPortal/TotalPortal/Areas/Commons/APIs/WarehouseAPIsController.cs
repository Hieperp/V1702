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
    public class WarehouseAPIsController : Controller
    {
        private readonly IWarehouseRepository warehouseRepository;

        public WarehouseAPIsController(IWarehouseRepository warehouseRepository)
        {
            this.warehouseRepository = warehouseRepository;
        }


        public JsonResult GetWarehouses(int customerID, string searchText, string warehouseTaskIDList)
        {
            var result = warehouseRepository.GetWarehouses(customerID, searchText, warehouseTaskIDList).Select(s => new { s.WarehouseID, s.Code, s.Name });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}