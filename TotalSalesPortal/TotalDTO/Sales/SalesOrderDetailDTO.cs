using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalDTO.Helpers;
using System.Collections.Generic;

namespace TotalDTO.Sales
{
    public class SalesOrderDetailDTO : SaleDetailVaDTO, IPrimitiveEntity
    {
        public int GetID() { return this.SalesOrderDetailID; }

        public int SalesOrderDetailID { get; set; }
        public int SalesOrderID { get; set; }

        public Nullable<int> QuotationDetailID { get; set; }        

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

            if (this.QuotationDetailID > 0 && (this.Quantity > this.QuantityRemains || this.FreeQuantity > this.FreeQuantityRemains)) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng còn lại [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }
}
