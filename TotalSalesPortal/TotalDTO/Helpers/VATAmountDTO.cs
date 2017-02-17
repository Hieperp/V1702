using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Helpers
{
    public abstract class VATAmountDTO<TVATAmountDetailDTO> : AmountDTO<TVATAmountDetailDTO>
        where TVATAmountDetailDTO : class, IVATAmountDetailDTO
    {
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

        protected virtual decimal GetTotalVATAmount() { return this.DtoDetails().Select(o => o.VATAmount).Sum(); }
        protected virtual decimal GetTotalGrossAmount() { return this.DtoDetails().Select(o => o.GrossAmount).Sum(); }
    }
}
