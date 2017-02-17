using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Helpers
{
    public abstract class ListedAmountDiscountVATAmountDTO<TListedAmountDiscountVATAmountDetailDTO> : DiscountVATAmountDTO<TListedAmountDiscountVATAmountDetailDTO>
        where TListedAmountDiscountVATAmountDetailDTO : class, IListedAmountDiscountVATAmountDetailDTO
    {
        [Display(Name = "Tổng tiền")]
        public decimal TotalListedAmount { get; set; }
        [Display(Name = "Tổng tiền thuế")]
        public decimal TotalListedVATAmount { get; set; }
        [Display(Name = "Tổng cộng")]
        public decimal TotalListedGrossAmount { get; set; }

        
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.TotalListedAmount != this.GetTotalListedAmount()) yield return new ValidationResult("Lỗi tổng thành tiền giá gốc", new[] { "TotalListedAmount" });

            if (this.TotalListedVATAmount != this.GetTotalListedVATAmount()) yield return new ValidationResult("Lỗi tổng tiền thuế giá gốc", new[] { "TotalListedVATAmount" });
            if (this.TotalListedGrossAmount != this.GetTotalListedGrossAmount()) yield return new ValidationResult("Lỗi tổng tiền sau thuế giá gốc", new[] { "TotalListedGrossAmount" });
        }

        protected virtual decimal GetTotalListedAmount() { return this.DtoDetails().Select(o => o.ListedAmount).Sum(); }
        protected virtual decimal GetTotalListedVATAmount() { return this.DtoDetails().Select(o => o.ListedVATAmount).Sum(); }
        protected virtual decimal GetTotalListedGrossAmount() { return this.DtoDetails().Select(o => o.ListedGrossAmount).Sum(); }
    }

}
