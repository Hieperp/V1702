using System;

using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IPriceCategoryDropDownViewModel
    {
        [Display(Name = "Đơn giá")]
        int PriceCategoryID { get; set; }
        IEnumerable<SelectListItem> PriceCategorySelectList { get; set; }
    }
}
