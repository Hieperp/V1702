using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IPaymentTermDropDownViewModel
    {
        [Display(Name = "Phương thức TT")]
        int PaymentTermID { get; set; }
        IEnumerable<SelectListItem> PaymentTermSelectList { get; set; }
    }
}

