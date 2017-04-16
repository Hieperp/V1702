using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalBase.Enums;

namespace TotalDTO.Helpers
{
    public abstract class VATAmountDTO<TVATAmountDetailDTO> : AmountDTO<TVATAmountDetailDTO>
        where TVATAmountDetailDTO : class, IVATAmountDetailDTO
    {
        protected VATAmountDTO() { this.VATPercent = 10; }

        [Display(Name = "VAT")]
        [UIHint("DecimalReadonly")]
        public decimal VATPercent { get; set; }

        [Display(Name = "Tổng tiền thuế")]
        public decimal TotalVATAmount { get; set; }
        [Display(Name = "Tổng cộng")]
        public decimal TotalGrossAmount { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.TotalVATAmount != this.GetTotalVATAmount()) yield return new ValidationResult("Lỗi tổng tiền thuế", new[] { "TotalVATAmount" });
            if (this.TotalGrossAmount != this.GetTotalGrossAmount()) yield return new ValidationResult("Lỗi tổng tiền sau thuế", new[] { "TotalGrossAmount" });
        }

        protected virtual decimal GetTotalVATAmount()
        {
            if (GlobalEnums.VATbyRow)
                return this.DtoDetails().Select(o => o.VATAmount).Sum();
            else
                return Math.Round(this.GetTotalAmount() * this.VATPercent / 100, GlobalEnums.rndAmount, MidpointRounding.AwayFromZero);
        }

        protected virtual decimal GetTotalGrossAmount()
        {
            if (GlobalEnums.VATbyRow)
                return this.DtoDetails().Select(o => o.GrossAmount).Sum();
            else
                return Math.Round(this.GetTotalAmount() + this.GetTotalVATAmount(), GlobalEnums.rndAmount, MidpointRounding.AwayFromZero);
        }
    }
}
