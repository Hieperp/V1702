﻿using System;
using System.Collections.Generic;
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
        [Display(Name = "ĐH")]
        [UIHint("StringReadonly")]
        public string GoodsIssueCode { get; set; }

        [UIHint("StringReadonly")]
        public override string CommodityCode { get; set; }

        [Display(Name = "SLXK")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityRemains { get; set; }
        [Display(Name = "XKQT")]
        [UIHint("DecimalReadonly")]
        public decimal FreeQuantityRemains { get; set; }

        [UIHint("Decimal")]
        public override decimal Quantity { get; set; }
        [UIHint("Decimal")]
        public override decimal FreeQuantity { get; set; }

        [UIHint("DecimalReadonly")]
        public override decimal ListedPrice { get; set; }
        [UIHint("Decimal")]
        public override decimal DiscountPercent { get; set; }
        [UIHint("DecimalReadonly")]
        public override decimal UnitPrice { get; set; }


        public Nullable<System.DateTime> VATInvoiceDate { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.Quantity > this.QuantityRemains || this.FreeQuantity > this.FreeQuantityRemains) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng còn lại [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }
}
