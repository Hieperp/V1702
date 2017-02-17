using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IServiceContractTypeDropDownViewModel
    {
        int ServiceContractTypeID { get; set; }
        [Display(Name = "Loại dịch vụ")]
        IEnumerable<SelectListItem> ServiceContractTypeDropDown { get; set; }
    }
}

