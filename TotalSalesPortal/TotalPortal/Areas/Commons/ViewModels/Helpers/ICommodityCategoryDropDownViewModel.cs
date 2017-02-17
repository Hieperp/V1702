using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ICommodityCategoryDropDownViewModel
    {        
        int CommodityCategoryID { get; set; }
        IEnumerable<SelectListItem> CommodityCategoryDropDown { get; set; }
    }
}

