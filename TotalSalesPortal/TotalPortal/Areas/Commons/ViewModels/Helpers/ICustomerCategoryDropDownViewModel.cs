using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ICustomerCategoryDropDownViewModel
    {
        [Display(Name = "Kênh khách hàng")]
        Nullable<int> CustomerCategoryID { get; set; }
        [Display(Name = "Kênh khách hàng")]
        IEnumerable<SelectListItem> CustomerCategorySelectList { get; set; }
    }
}

