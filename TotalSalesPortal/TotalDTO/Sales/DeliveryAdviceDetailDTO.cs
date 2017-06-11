using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalDTO.Helpers;
using System.Collections.Generic;

namespace TotalDTO.Sales
{
    public class StockableDetailDTO : FreeQuantityDiscountVATAmountDetailDTO, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public Nullable<int> WarehouseID { get; set; }
        public int GetWarehouseID() { return (int)this.WarehouseID; } //Purpose: for IHelperWarehouseID only

        [Display(Name = "Kho")]
        [UIHint("StringReadonly")]
        public string WarehouseCode { get; set; }

        [Display(Name = "Tồn kho")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityAvailable { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.WarehouseID != null && this.QuantityAvailable < (this.Quantity + this.FreeQuantity)) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng tồn kho [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }

    public class SaleDetailDTO : StockableDetailDTO
    {
        public int DeliveryAdviceDetailID { get; set; }
        public int DeliveryAdviceID { get; set; }

        public int CustomerID { get; set; }
        public int ReceiverID { get; set; }
        public int SalespersonID { get; set; }


        [Display(Name = "Tồn đơn")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityRemains { get; set; }

        [Display(Name = "Tồn QT")]
        [UIHint("DecimalReadonly")]
        public decimal FreeQuantityRemains { get; set; }


        public Nullable<int> PromotionID { get; set; }

        public string VoidTypeCode { get; set; }
        [Display(Name = "Lý do")]
        [UIHint("AutoCompletes/VoidTypeBase")]
        public string VoidTypeName { get; set; }
        public Nullable<int> VoidClassID { get; set; }
    }

    public class SaleDetailVaDTO : SaleDetailDTO
    {
        [UIHint("Decimal")]
        public override decimal Quantity { get; set; }

        [UIHint("DecimalReadonly")]
        public override decimal UnitPrice { get; set; }
    }

    public class SaleDetailVa1DTO : SaleDetailVaDTO
    {
        [UIHint("DecimalReadonly")]
        public override decimal FreeQuantity { get; set; }
        [UIHint("DecimalReadonly")]
        public override decimal ListedPrice { get; set; }
        [UIHint("DecimalReadonly")]
        public override decimal DiscountPercent { get; set; }
    }






    public class DeliveryAdviceDetailDTO : SaleDetailVa1DTO, IPrimitiveEntity
    {
        public int GetID() { return this.DeliveryAdviceDetailID; }
        
        public Nullable<int> SalesOrderID { get; set; }
        public Nullable<int> SalesOrderDetailID { get; set; }

        [Display(Name = "Phiếu ĐH")]
        [UIHint("StringReadonly")]
        public string SalesOrderReference { get; set; }
        [Display(Name = "Số ĐH")]
        [UIHint("StringReadonly")]
        public string SalesOrderCode { get; set; }
        [Display(Name = "Ngày ĐH")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> SalesOrderEntryDate { get; set; }
        

        [UIHint("AutoCompletes/CommodityAvailable")]
        public override string CommodityCode { get; set; }
        

        public bool InActiveIssue { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.SalesOrderID > 0 && (this.Quantity > this.QuantityRemains || this.FreeQuantity > this.FreeQuantityRemains)) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng còn lại [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }
}
