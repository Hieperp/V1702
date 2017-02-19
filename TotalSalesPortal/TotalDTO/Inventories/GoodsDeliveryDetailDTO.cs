using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;

namespace TotalDTO.Inventories
{
    public class GoodsDeliveryDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.GoodsDeliveryDetailID; }

        public int GoodsDeliveryDetailID { get; set; }
        public int GoodsDeliveryID { get; set; }

        public int HandlingUnitID { get; set; }

        public override int CommodityID { get { return 1; } }
        public override string CommodityName { get { return "#"; } }
        public override int CommodityTypeID { get { return 1; } }


        public int CustomerID { get; set; }
        [Display(Name = "Mã khách hàng")]
        [UIHint("StringReadonly")]
        public string CustomerCode { get; set; }
        [Display(Name = "Tên khách hàng")]
        [UIHint("StringReadonly")]
        public string CustomerName { get; set; }

        public int ReceiverID { get; set; }
        [Display(Name = "Mã đơn vị nhận")]
        [UIHint("StringReadonly")]
        public string ReceiverCode { get; set; }
        [Display(Name = "Tên đơn vị nhận")]
        [UIHint("StringReadonly")]
        public string ReceiverName { get; set; }
        [Display(Name = "Địa chỉ giao hàng")]
        [UIHint("StringReadonly")]
        public string ShippingAddress { get; set; }

        [Display(Name = "Giao hàng")]
        [UIHint("StringReadonly")]
        public string ReceiverDescription { get { return (this.CustomerID == this.ReceiverID ? "" : this.ReceiverName + ", ") + this.ShippingAddress; } }
        
        [Display(Name = "PXK")]
        [UIHint("StringReadonly")]
        public string GoodsIssueReferences { get; set; }
        [Display(Name = "STT")]
        [UIHint("StringReadonly")]
        public string HandlingUnitIdentification { get; set; }

        [Display(Name = "Loại")]
        [UIHint("StringReadonly")]
        public string PrintedLabel { get; set; }

        [UIHint("DecimalReadonly")]
        public override decimal Quantity { get; set; }

        [Display(Name = "TL")]
        [UIHint("DecimalReadonly")]
        public decimal Weight { get; set; }
        [Display(Name = "TLTT")]
        [UIHint("DecimalReadonly")]
        public decimal RealWeight { get; set; }
    }
}
