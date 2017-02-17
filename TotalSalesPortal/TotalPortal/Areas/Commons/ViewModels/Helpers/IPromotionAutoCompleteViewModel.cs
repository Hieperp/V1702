using System;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IPromotionAutoCompleteViewModel
    {
        Nullable<int> PromotionID { get; set; }
        [Display(Name = "Chương trình khuyến mãi")]
        string PromotionCode { get; set; }
    }
}
