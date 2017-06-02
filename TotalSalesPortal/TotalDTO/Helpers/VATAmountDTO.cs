using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalBase.Enums;

namespace TotalDTO.Helpers
{
    public interface IVATAmountDTO : IAmountDTO
    {
        bool VATbyRow { get; set; }

        decimal TradeDiscountRate { get; set; }
        decimal TradeDiscountAmount { get; set; }
        decimal TotalTaxableAmount { get; set; }

        decimal VATPercent { get; set; }
        decimal TotalVATAmount { get; set; }
        decimal TotalGrossAmount { get; set; }
    }

    public abstract class VATAmountDTO<TVATAmountDetailDTO> : AmountDTO<TVATAmountDetailDTO>, IVATAmountDTO
        where TVATAmountDetailDTO : class, IVATAmountDetailDTO
    {
        protected VATAmountDTO() { this.VATbyRow = GlobalEnums.VATbyRow; this.VATPercent = GlobalEnums.VATPercent; } //Later, if we need program to allow user to change VATPercent of D.A => We can do it by setup two things: 1) Declare a default VATPercent and init VATPercent by default here. 2) Do workarround to change VATPercent in View: 2.a) Have an action to change VATPercent 2.b) Clear all detail of D.A (because: each detail row init its VATPercent by getting VATPercent from its master data: See this js statement: [ currentDataSourceRow.set("VATPercent", currentDataSourceRow.VATbyRow === true ? dataItem.VATPercent : $("#VATPercent").val()) ]  )

        public bool VATbyRow { get; set; }

        public decimal TradeDiscountRate { get; set; }
        public decimal TradeDiscountAmount { get; set; }
        public decimal TotalTaxableAmount { get; set; }

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

            if (this.TradeDiscountAmount != this.GetTradeDiscountAmount()) yield return new ValidationResult("Lỗi chiết khấu tổng đơn hàng", new[] { "TradeDiscountAmount" });
            if (this.TotalTaxableAmount != Math.Round(this.GetTotalAmount() - this.GetTradeDiscountAmount(), GlobalEnums.rndAmount, MidpointRounding.AwayFromZero)) yield return new ValidationResult("Lỗi tổng tiền hàng tính thuế", new[] { "TotalTaxableAmount" });

            if (this.TotalVATAmount != this.GetTotalVATAmount()) yield return new ValidationResult("Lỗi tổng tiền thuế", new[] { "TotalVATAmount" });
            if (this.TotalGrossAmount != this.GetTotalGrossAmount()) yield return new ValidationResult("Lỗi tổng tiền sau thuế", new[] { "TotalGrossAmount" });
        }



        protected virtual decimal GetTradeDiscountAmount()
        {
            if (this.VATbyRow)
                return 0;
            else
                return Math.Round(this.GetTotalAmount() * this.TradeDiscountRate / 100, GlobalEnums.rndAmount, MidpointRounding.AwayFromZero);
        }

        protected virtual decimal GetTotalVATAmount()
        {
            if (this.VATbyRow)
                return this.DtoDetails().Select(o => o.VATAmount).Sum();
            else
                return Math.Round(this.TotalTaxableAmount * this.VATPercent / 100, GlobalEnums.rndAmount, MidpointRounding.AwayFromZero);
        }

        protected virtual decimal GetTotalGrossAmount()
        {
            if (this.VATbyRow)
                return this.DtoDetails().Select(o => o.GrossAmount).Sum();
            else
                return Math.Round(this.TotalTaxableAmount + this.GetTotalVATAmount(), GlobalEnums.rndAmount, MidpointRounding.AwayFromZero);
        }
    }
}
