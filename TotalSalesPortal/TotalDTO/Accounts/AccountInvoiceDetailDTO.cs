using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalDTO.Helpers;

namespace TotalDTO.Accounts
{
    public class AccountInvoiceDetailDTO : FreeQuantityDiscountVATAmountDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.AccountInvoiceDetailID; }

        public int AccountInvoiceDetailID { get; set; }
        public int AccountInvoiceID { get; set; }

        public int CustomerID { get; set; }
        public int GoodsIssueID { get; set; }
        public int GoodsIssueDetailID { get; set; }

        [Display(Name = "PXK")]
        [UIHint("StringReadonly")]
        public string GoodsIssueReference { get; set; }
        [Display(Name = "Ngày XK")]
        [UIHint("DateTimeReadonly")]
        public System.DateTime GoodsIssueEntryDate { get; set; }

        [UIHint("StringReadonly")]
        public override string CommodityCode { get; set; }

        [Display(Name = "SL ĐH")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityRemains { get; set; }

        [Display(Name = "SL QT")]
        [UIHint("DecimalReadonly")]
        public decimal FreeQuantityRemains { get; set; }

        [UIHint("DecimalReadonly")]
        public override decimal DiscountPercent { get; set; }
        [UIHint("DecimalReadonly")]
        public override decimal UnitPrice { get; set; }
        [UIHint("DecimalReadonly")]
        public override decimal GrossPrice { get; set; }        
    }
}
