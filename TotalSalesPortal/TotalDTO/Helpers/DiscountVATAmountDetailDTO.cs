using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Helpers
{
    public interface IDiscountVATAmountDetailDTO : IVATAmountDetailDTO
    {
        decimal ListedPrice { get; set; }
        decimal DiscountPercent { get; set; }
    }

    public abstract class DiscountVATAmountDetailDTO : VATAmountDetailDTO, IDiscountVATAmountDetailDTO
    {
        [Display(Name = "Đơn giá")]
        [UIHint("DecimalReadonly")]
        [Range(1, 999999999, ErrorMessage = "Sản phẩm chưa có giá bán")]
        public virtual decimal ListedPrice { get; set; }

        [Display(Name = "CK")] //[UIHint("DecimalReadonly")] //[UIHint("Decimal")]
        public virtual decimal DiscountPercent { get; set; }
    }
}
