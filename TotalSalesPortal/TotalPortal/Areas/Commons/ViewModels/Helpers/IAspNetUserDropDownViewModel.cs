using System.Web.Mvc;
using System.Collections.Generic;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IAspNetUserDropDownViewModel
    {
        int UserID { get; set; }
        IEnumerable<SelectListItem> AspNetUserDropDown { get; set; }
    }
}

