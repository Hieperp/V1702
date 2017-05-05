using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalDTO.Helpers;
using System.Collections.Generic;

namespace TotalDTO.Sales
{
    public class SalesOrderDetailDTO : SaleDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.SalesOrderDetailID; }

        public int SalesOrderDetailID { get; set; }
        public int SalesOrderID { get; set; }

        public Nullable<int> QuotationDetailID { get; set; }
        public Nullable<int> PromotionID { get; set; }

        [UIHint("AutoCompletes/CommodityAvailable")]
        public override string CommodityCode { get; set; }


        [UIHint("Decimal")]
        public override decimal FreeQuantity { get; set; }
        [UIHint("Decimal")]
        public override decimal ListedPrice { get; set; }
        [UIHint("Decimal")]
        public override decimal DiscountPercent { get; set; }


        public bool InActiveIssue { get; set; }
    }
}
