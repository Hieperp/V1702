using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Commons
{
    public class CommodityPricePrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.CommodityPrice; } }

        public int GetID() { return this.CommodityPriceID; }
        public void SetID(int id) { this.CommodityPriceID = id; }

        public int CommodityPriceID { get; set; }

        [Display(Name = "Bảng giá")]
        [Required(ErrorMessage = "Vui lòng chọn bảng giá")]
        public int PriceCategoryID { get; set; }

        [Display(Name = "Mã sản phẩm")]
        [Required(ErrorMessage = "Vui lòng nhập mã sản phẩm")]
        public string CodePartA { get; set; }

        [Display(Name = "Mã bông")]
        public string CodePartB { get; set; }

        [Display(Name = "Nguyên liệu")]
        [Required(ErrorMessage = "Vui lòng nhập nguyên liệu")]
        public string CodePartC { get; set; }

        [Display(Name = "Giá chưa thuế")]
        public decimal ListedPrice { get; set; }

        [Display(Name = "Giá đã bao gồm thuế VAT")]
        public decimal GrossPrice { get; set; }

        public override int PreparedPersonID { get { return 1; } }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if ((this.ListedPrice == 0 && this.GrossPrice == 0) || (this.ListedPrice != 0 && this.GrossPrice != 0)) yield return new ValidationResult("Vui lòng nhập một trong hai loại giá: chưa thuế và đã bao gồm thuế", new[] { "ListedPrice" });
        }

    }

    public class CommodityPriceDTO : CommodityPricePrimitiveDTO
    {
    }
}
