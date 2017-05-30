using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalBase.Enums;

namespace TotalDTO.Helpers
{
    public interface IVATAmountDetailDTO : IAmountDetailDTO
    {
        bool VATbyRow { get; set; }

        decimal TradeDiscountRate { get; set; }

        decimal VATPercent { get; set; }
        decimal GrossPrice { get; set; }
        decimal VATAmount { get; set; }
        decimal GrossAmount { get; set; }
    }

    public abstract class VATAmountDetailDTO : AmountDetailDTO, IVATAmountDetailDTO
    {
        public bool VATbyRow { get; set; }

        [Display(Name = "CK")]
        public virtual decimal TradeDiscountRate { get; set; }

        [Display(Name = "VAT")]
        [UIHint("DecimalReadonly")]
        public decimal VATPercent { get; set; }

        [Display(Name = "Giá sau thuế")]
        [UIHint("DecimalReadonly")] //[UIHint("Decimal")]
        public virtual decimal GrossPrice { get; set; }

        [Display(Name = "Thuế VAT")]
        [UIHint("DecimalReadonly")]
        public decimal VATAmount { get; set; }

        [Display(Name = "Tổng cộng")]
        [UIHint("DecimalReadonly")]
        public decimal GrossAmount { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if ((this.UnitPrice != 0 && this.GrossPrice == 0) || (this.UnitPrice == 0 && this.GrossPrice != 0)) yield return new ValidationResult("Lỗi giá sau thuế", new[] { "GrossPrice" });
            if (this.CalculatingTypeID != 0 && Math.Round(this.Quantity * this.GrossPrice, GlobalEnums.rndAmount, MidpointRounding.AwayFromZero) != this.GrossAmount) yield return new ValidationResult("Lỗi thành tiền sau thuế", new[] { "GrossAmount" });
            if ((this.CalculatingTypeID == 0 && Math.Round(this.Amount * this.VATPercent / 100, GlobalEnums.rndAmount, MidpointRounding.AwayFromZero) != this.VATAmount) || (this.Amount == 0 && this.VATAmount != 0) || (this.Amount != 0 && this.VATPercent != 0 && this.VATAmount == 0) || (this.Amount != 0 && this.VATPercent == 0 && this.VATAmount != 0)) yield return new ValidationResult("Lỗi tiền thuế", new[] { "VATAmount" });
            if (Math.Round(this.Amount + this.VATAmount, GlobalEnums.rndAmount, MidpointRounding.AwayFromZero) != this.GrossAmount) yield return new ValidationResult("Lỗi thành tiền sau thuế", new[] { "GrossAmount" });
        }
    }
}
