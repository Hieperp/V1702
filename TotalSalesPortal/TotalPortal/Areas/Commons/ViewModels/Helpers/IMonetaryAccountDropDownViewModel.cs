using System;

using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IMonetaryAccountDropDownViewModel
    {
        [Display(Name = "Phương thức TT")]
        Nullable<int> MonetaryAccountID { get; set; }
        IEnumerable<SelectListItem> MonetaryAccountSelectList { get; set; }
    }
}
