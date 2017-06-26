using System;

using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ITerritoryDropDownViewModel
    {
        [Display(Name = "Địa bàn")]
        int TerritoryID { get; set; }
        IEnumerable<SelectListItem> TerritorySelectList { get; set; }
    }
}
