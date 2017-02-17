using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ICommodityTypeDropDownViewModel
    {        
        int CommodityTypeID { get; set; }
        IEnumerable<SelectListItem> CommodityTypeDropDown { get; set; }
    }
}

