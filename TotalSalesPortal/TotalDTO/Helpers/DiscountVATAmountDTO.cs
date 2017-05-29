using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Helpers
{
    public interface IDiscountVATAmountDTO : IVATAmountDTO
    {
        decimal AverageDiscountPercent { get; set; }
    }

    public abstract class DiscountVATAmountDTO<TDiscountVATAmountDetailDTO> : VATAmountDTO<TDiscountVATAmountDetailDTO>, IDiscountVATAmountDTO
        where TDiscountVATAmountDetailDTO : class, IDiscountVATAmountDetailDTO
    {
        [Display(Name = "Bình quân CK")]
        public decimal AverageDiscountPercent { get; set; }
    }
}
