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
        public decimal ListedPrice { get; set; }

        [Display(Name = "CK")] //[UIHint("DecimalReadonly")] //[UIHint("Decimal")]
        public virtual decimal DiscountPercent { get; set; }
    }
}
