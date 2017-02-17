using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ISupplierDropDownViewModel
    {
        Nullable<int> SupplierID { get; set; }
        IEnumerable<SelectListItem> SupplierDropDown { get; set; }
    }
}
