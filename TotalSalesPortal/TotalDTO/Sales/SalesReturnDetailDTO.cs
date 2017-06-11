using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalDTO.Helpers;
using System.Collections.Generic;

namespace TotalDTO.Sales
{
    public class SalesReturnDetailDTO : SaleDetailVaDTO, IPrimitiveEntity
    {
        public int GetID() { return this.SalesReturnDetailID; }

        public int SalesReturnDetailID { get; set; }
        public int SalesReturnID { get; set; }

        public Nullable<int> GoodsIssueID { get; set; }
        public Nullable<int> GoodsIssueDetailID { get; set; }

        [Display(Name = "Phiếu XK")]
        [UIHint("StringReadonly")]
        public string GoodsIssueReference { get; set; }
        [Display(Name = "Ngày XK")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> GoodsIssueEntryDate { get; set; }
        

        [UIHint("AutoCompletes/CommodityAvailable")]
        public override string CommodityCode { get; set; }


        [UIHint("Decimal")]
        public override decimal FreeQuantity { get; set; }
        [UIHint("Decimal")]
        public override decimal ListedPrice { get; set; }
        [UIHint("Decimal")]
        public override decimal DiscountPercent { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.GoodsIssueID > 0 && (this.Quantity > this.QuantityRemains || this.FreeQuantity > this.FreeQuantityRemains)) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng còn lại [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }
}
