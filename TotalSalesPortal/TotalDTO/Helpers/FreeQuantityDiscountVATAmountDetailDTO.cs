using System;
using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Helpers
{
    public interface IFreeQuantityDiscountVATAmountDetailDTO : IListedAmountDiscountVATAmountDetailDTO
    {
        decimal ControlFreeQuantity { get; set; }
        decimal FreeQuantity { get; set; }
        Nullable<bool> IsBonus { get; set; }
    }

    public abstract class FreeQuantityDiscountVATAmountDetailDTO : ListedAmountDiscountVATAmountDetailDTO, IFreeQuantityDiscountVATAmountDetailDTO
    {
        [Display(Name = "SL yêu cầu để được hưởng Quà tặng")]
        [UIHint("DecimalReadonly")]
        public decimal ControlFreeQuantity { get; set; }

        [Display(Name = "QT")]        
        public virtual decimal FreeQuantity { get; set; }

        public Nullable<bool> IsBonus { get; set; }
    }
}

