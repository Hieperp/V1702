using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;
using TotalModel.Helpers;
using TotalDTO.Helpers;

namespace TotalDTO.Inventories
{
    public class HandlingUnitDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.HandlingUnitDetailID; }

        public int HandlingUnitDetailID { get; set; }
        public int HandlingUnitID { get; set; }

        public int CustomerID { get; set; }
        public int ReceiverID { get; set; }
        public int GoodsIssueID { get; set; }
        public int GoodsIssueDetailID { get; set; }

        [Display(Name = "Phiếu XK")]
        [UIHint("StringReadonly")]
        public string GoodsIssueReference { get; set; }
        [Display(Name = "Ngày, giờ XK")]
        [UIHint("DateTimeReadonly")]
        public System.DateTime GoodsIssueEntryDate { get; set; }
        [Display(Name = "Đơn hàng")]
        public string GoodsIssueCode { get; set; }

        [UIHint("StringReadonly")]
        public override string CommodityCode { get; set; }

        [Display(Name = "SL XK")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityRemains { get; set; }

        [UIHint("Decimal")]
        public override decimal Quantity { get; set; }

        [Display(Name = "TL chuẩn (g)")]
        [UIHint("DecimalReadonly")]
        public virtual decimal UnitWeight { get; set; }

        [Display(Name = "Trọng lượng")]
        [UIHint("DecimalReadonly")]
        public decimal Weight { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (Math.Round(this.Quantity * this.UnitWeight / 1000, GlobalEnums.rndWeight, MidpointRounding.AwayFromZero) != this.Weight) yield return new ValidationResult("Lỗi trọng lượng", new[] { "TotalWeight" });
            if (this.Quantity > this.QuantityRemains) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng còn lại [" + this.CommodityName + "]", new[] { "Quantity" });
        }

    }
}
