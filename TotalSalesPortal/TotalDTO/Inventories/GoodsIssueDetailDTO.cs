using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Sales;

namespace TotalDTO.Inventories
{
    public class GoodsIssueDetailDTO : SaleDetailVa1DTO, IPrimitiveEntity
    {
        public int GetID() { return this.GoodsIssueDetailID; }

        public int GoodsIssueDetailID { get; set; }
        public int GoodsIssueID { get; set; }

        public int StorekeeperID { get; set; }

        [Display(Name = "Phiếu ĐN")]
        [UIHint("StringReadonly")]
        public string DeliveryAdviceReference { get; set; }
        [Display(Name = "Đơn hàng")]
        public string DeliveryAdviceCode { get; set; }

        [UIHint("StringReadonly")]
        public override string CommodityCode { get; set; }        

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.Quantity > this.QuantityRemains || this.FreeQuantity > this.FreeQuantityRemains) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng còn lại [" + this.CommodityName + "]", new[] { "Quantity" });
            if ((this.Quantity != this.QuantityRemains || this.FreeQuantity != this.FreeQuantityRemains) && this.VoidTypeID == null) yield return new ValidationResult("Vui lòng chọn lý do không xuất kho [" + this.CommodityName + "]", new[] { "VoidTypeName" });
        }

    }
}