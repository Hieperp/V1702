using System;
using System.ComponentModel.DataAnnotations;

using TotalDTO;

namespace TotalPortal.ViewModels.Home
{
    public class OptionViewModel : BaseDTO
    {
        [Display(Name = "Lọc dữ liệu từ ngày")]
        public DateTime GlobalFromDate { get; set; }
        [Display(Name = "Lọc dữ liệu đến ngày")]
        public DateTime GlobalToDate { get; set; }
    }
}