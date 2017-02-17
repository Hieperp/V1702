using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IVehicleDropDownViewModel
    {
        [Display(Name = "Biển số xe")]
        int VehicleID { get; set; }
        IEnumerable<SelectListItem> VehicleSelectList { get; set; }
    }
}
