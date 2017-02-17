using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ICustomerCategoryDropDownViewModel
    {
        [Display(Name = "Phân khúc KH")]
        int CustomerCategoryID { get; set; }
        [Display(Name = "Phân khúc KH")]
        IEnumerable<SelectListItem> CustomerCategoryDropDown { get; set; }
    }
}

