using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IPriceTermDropDownViewModel
    {
        [Display(Name = "Loại giá")]
        int PriceTermID { get; set; }
        IEnumerable<SelectListItem> PriceTermSelectList { get; set; }
    }
}

