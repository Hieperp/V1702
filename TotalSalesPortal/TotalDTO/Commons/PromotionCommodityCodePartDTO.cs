using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using System.Collections.Generic;

namespace TotalDTO.Commons
{
    public class PromotionCommodityCodePartDTO : BaseModel, IBaseModel, IPrimitiveEntity
    {
        public int GetID() { return this.PromotionCommodityCodePartID; }

        public int PromotionCommodityCodePartID { get; set; }
        public int PromotionID { get; set; }

        public int CommodityBrandID { get; set; }

        [Display(Name = "Mã sản phẩm")]
        [UIHint("StringReadonly")]
        public virtual string CodePartA { get; set; }

        [Display(Name = "Mã bông")]
        [UIHint("StringReadonly")]
        public virtual string CodePartB { get; set; }

        [Display(Name = "Nguyên liệu")]
        [UIHint("StringReadonly")]
        public virtual string CodePartC { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if ((this.CodePartA == null || this.CodePartA == "") && (this.CodePartB == null || this.CodePartB == "") && (this.CodePartC == null || this.CodePartC == "")) yield return new ValidationResult("Vui lòng chọn ít nhất một yếu tố mã sản phẩm, mã bông hay nguyên liệu", new[] { "CodePartA" });
        }
    }
}
