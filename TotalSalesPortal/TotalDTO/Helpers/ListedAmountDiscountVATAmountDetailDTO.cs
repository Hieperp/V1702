using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalBase.Enums;

namespace TotalDTO.Helpers
{
    public interface IListedAmountDiscountVATAmountDetailDTO : IDiscountVATAmountDetailDTO
    {
        decimal ListedGrossPrice { get; set; }

        decimal ListedAmount { get; set; }
        decimal ListedVATAmount { get; set; }
        decimal ListedGrossAmount { get; set; }
    }

    public abstract class ListedAmountDiscountVATAmountDetailDTO : DiscountVATAmountDetailDTO, IListedAmountDiscountVATAmountDetailDTO
    {
        [Display(Name = "Giá sau thuế")]
        [UIHint("DecimalReadonly")]
        public virtual decimal ListedGrossPrice { get; set; }

        [Display(Name = "Thành tiền")]
        [UIHint("DecimalReadonly")]
        public decimal ListedAmount { get; set; }

        [Display(Name = "Thuế VAT")]
        [UIHint("DecimalReadonly")]
        public decimal ListedVATAmount { get; set; }

        [Display(Name = "Tổng cộng")]
        [UIHint("DecimalReadonly")]
        public decimal ListedGrossAmount { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if ((this.ListedPrice != 0 && this.ListedGrossPrice == 0) || (this.ListedPrice == 0 && this.ListedGrossPrice != 0)) yield return new ValidationResult("Lỗi giá gốc sau thuế", new[] { "ListedGrossPrice" });

            if (Math.Round(this.Quantity * this.ListedPrice, GlobalEnums.rndAmount, MidpointRounding.AwayFromZero) != this.ListedAmount) yield return new ValidationResult(this.CommodityCode + "Lỗi thành tiền giá gốc", new[] { "ListedAmount" });
            if (this.CalculatingTypeID != 0 && Math.Round(this.Quantity * this.ListedGrossPrice, GlobalEnums.rndAmount, MidpointRounding.AwayFromZero) != this.ListedGrossAmount) yield return new ValidationResult(this.CommodityCode + "Lỗi thành tiền giá gốc sau thuế", new[] { "ListedGrossAmount" });
            if ((this.CalculatingTypeID == 0 && Math.Round(this.ListedAmount * this.VATPercent / 100, GlobalEnums.rndAmount, MidpointRounding.AwayFromZero) != this.ListedVATAmount) || (this.ListedAmount == 0 && this.ListedVATAmount != 0) || (this.ListedAmount != 0 && this.VATPercent != 0 && this.ListedVATAmount == 0) || (this.ListedAmount != 0 && this.VATPercent == 0 && this.ListedVATAmount != 0)) yield return new ValidationResult(this.CommodityCode + "Lỗi tiền thuế giá gốc", new[] { "ListedVATAmount" });
            if (Math.Round(this.ListedAmount + this.ListedVATAmount, GlobalEnums.rndAmount, MidpointRounding.AwayFromZero) != this.ListedGrossAmount) yield return new ValidationResult(this.CommodityCode + "Lỗi thành tiền giá gốc sau thuế", new[] { "ListedGrossAmount" });

        }
    }
}