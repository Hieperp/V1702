using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalBase.Enums;

namespace TotalDTO.Helpers
{
    public interface IListedAmountDiscountVATAmountDTO : IDiscountVATAmountDTO
    {
        decimal TotalListedAmount { get; set; }

        decimal ListedTradeDiscountAmount { get; set; }
        decimal TotalListedTaxableAmount { get; set; }
        
        decimal TotalListedVATAmount { get; set; }
        decimal TotalListedGrossAmount { get; set; }
    }

    public abstract class ListedAmountDiscountVATAmountDTO<TListedAmountDiscountVATAmountDetailDTO> : DiscountVATAmountDTO<TListedAmountDiscountVATAmountDetailDTO>, IListedAmountDiscountVATAmountDTO
        where TListedAmountDiscountVATAmountDetailDTO : class, IListedAmountDiscountVATAmountDetailDTO
    {
        [Display(Name = "Tổng tiền")]
        public decimal TotalListedAmount { get; set; }

        public decimal ListedTradeDiscountAmount { get; set; }
        public decimal TotalListedTaxableAmount { get; set; }

        [Display(Name = "Tổng tiền thuế")]
        public decimal TotalListedVATAmount { get; set; }
        [Display(Name = "Tổng cộng")]
        public decimal TotalListedGrossAmount { get; set; }

        
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.TotalListedAmount != this.GetTotalListedAmount()) yield return new ValidationResult("Lỗi tổng thành tiền giá gốc", new[] { "TotalListedAmount" });

            if (this.ListedTradeDiscountAmount != this.GetListedTradeDiscountAmount()) yield return new ValidationResult("Lỗi chiết khấu tổng đơn hàng giá gốc", new[] { "ListedTradeDiscountAmount" });
            if (this.TotalListedTaxableAmount != Math.Round(this.GetTotalListedAmount() - this.GetListedTradeDiscountAmount(), GlobalEnums.rndAmount, MidpointRounding.AwayFromZero)) yield return new ValidationResult("Lỗi tổng tiền hàng tính thuế giá gốc", new[] { "TotalListedTaxableAmount" });

            if (this.TotalListedVATAmount != this.GetTotalListedVATAmount()) yield return new ValidationResult("Lỗi tổng tiền thuế giá gốc", new[] { "TotalListedVATAmount" });
            if (this.TotalListedGrossAmount != this.GetTotalListedGrossAmount()) yield return new ValidationResult("Lỗi tổng tiền sau thuế giá gốc", new[] { "TotalListedGrossAmount" });
        }

        protected virtual decimal GetTotalListedAmount() { return this.DtoDetails().Select(o => o.ListedAmount).Sum(); }

        protected virtual decimal GetListedTradeDiscountAmount()
        {
            if (this.VATbyRow)
                return 0;
            else
                return Math.Round(this.GetTotalListedAmount() * this.TradeDiscountRate / 100, GlobalEnums.rndAmount, MidpointRounding.AwayFromZero);
        }

        protected virtual decimal GetTotalListedVATAmount()
        {
            if (this.VATbyRow)
                return this.DtoDetails().Select(o => o.ListedVATAmount).Sum();
            else
                return Math.Round(this.TotalListedTaxableAmount * this.VATPercent / 100, GlobalEnums.rndAmount, MidpointRounding.AwayFromZero);
        }

        protected virtual decimal GetTotalListedGrossAmount()
        {
            if (this.VATbyRow)
                return this.DtoDetails().Select(o => o.ListedGrossAmount).Sum();
            else
                return Math.Round(this.TotalListedTaxableAmount + this.GetTotalListedVATAmount(), GlobalEnums.rndAmount, MidpointRounding.AwayFromZero);
        }

    }

}
