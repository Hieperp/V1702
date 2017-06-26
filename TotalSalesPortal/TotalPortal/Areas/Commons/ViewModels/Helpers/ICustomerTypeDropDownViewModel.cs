using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ICustomerTypeDropDownViewModel
    {
        [Display(Name = "Loại khách hàng")]
        Nullable<int> CustomerTypeID { get; set; }
        [Display(Name = "Loại khách hàng")]
        IEnumerable<SelectListItem> CustomerTypeSelectList { get; set; }
    }
}

