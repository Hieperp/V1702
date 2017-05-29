using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Helpers
{
    public interface IFreeQuantityDiscountVATAmountDTO : IListedAmountDiscountVATAmountDTO
    {
        decimal TotalFreeQuantity { get; set; }
    }

    public abstract class FreeQuantityDiscountVATAmountDTO<TFreeQuantityDiscountVATAmountDetailDTO> : ListedAmountDiscountVATAmountDTO<TFreeQuantityDiscountVATAmountDetailDTO>, IFreeQuantityDiscountVATAmountDTO
        where TFreeQuantityDiscountVATAmountDetailDTO : class, IFreeQuantityDiscountVATAmountDetailDTO
    {
        [Display(Name = "TC quà tặng")]
        public decimal TotalFreeQuantity { get; set; }



        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.TotalFreeQuantity != this.GetTotalFreeQuantity()) yield return new ValidationResult("Lỗi tổng số lượng quà tặng", new[] { "TotalFreeQuantity" });
        }

        protected virtual decimal GetTotalFreeQuantity() { return this.DtoDetails().Select(o => o.FreeQuantity).Sum(); }
    }

}
