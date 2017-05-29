using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TotalDTO.Helpers
{
    public interface IQuantityDTO
    {
        decimal TotalQuantity { get; set; }
    }

    public abstract class QuantityDTO<TQuantityDetailDTO> : BaseWithDetailDTO<TQuantityDetailDTO>, IQuantityDTO
        where TQuantityDetailDTO : class, IQuantityDetailDTO
    {
        [Display(Name = "Tổng SL")]
        [Required(ErrorMessage = "Vui lòng nhập chi tiết phiếu")]
        public virtual decimal TotalQuantity { get; set; }

        protected virtual decimal GetTotalQuantity() { return this.DtoDetails().Select(o => o.Quantity).Sum(); }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.TotalQuantity != this.GetTotalQuantity()) yield return new ValidationResult("Lỗi tổng số lượng", new[] { "TotalQuantity" });
        }               
    }
}
