using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IApproverDropDownViewModel
    {
        [Display(Name = "Người duyệt")]
        int ApproverID { get; set; }
        IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }
}
