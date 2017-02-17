using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Helpers
{
    public abstract class AmountDTO<TAmountDetailDTO> : QuantityDTO<TAmountDetailDTO>
        where TAmountDetailDTO : class, IAmountDetailDTO
    {
        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.TotalAmount != this.GetTotalAmount()) yield return new ValidationResult("Lỗi tổng thành tiền", new[] { "TotalAmount" });
        }

        protected virtual decimal GetTotalAmount() { return this.DtoDetails().Select(o => o.Amount).Sum(); }
    }
}
