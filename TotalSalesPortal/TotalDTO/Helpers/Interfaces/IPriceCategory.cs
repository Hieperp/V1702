using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Helpers.Interfaces
{
    public interface IPriceCategory
    {
        [Required]
        [Display(Name = "Bảng giá")]
        int PriceCategoryID { get; set; }
        [Display(Name = "Bảng giá")]
        string PriceCategoryCode { get; set; }
    }
}