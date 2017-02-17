using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;
using TotalDTO.Helpers;
using TotalDTO.Commons;

namespace TotalDTO.Inventories
{
    public class GoodsDeliveryPrimitiveDTO : QuantityDTO<GoodsDeliveryDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.GoodsDelivery; } }

        public int GetID() { return this.GoodsDeliveryID; }
        public void SetID(int id) { this.GoodsDeliveryID = id; }

        public int GoodsDeliveryID { get; set; }

        public virtual Nullable<int> ReceiverID { get; set; }


        [Display(Name = "Biển số xe")]
        [UIHint("DropDowns/Vehicle")]
        public int VehicleID { get; set; }

        public virtual int DriverID { get; set; }
        public virtual int CollectorID { get; set; }

        public override decimal TotalQuantity { get { return this.DtoDetails().Select(o => o.Quantity).Sum(); } }
        public decimal TotalWeight { get { return this.DtoDetails().Select(o => o.Weight).Sum(); } }
        public decimal TotalRealWeight { get { return this.DtoDetails().Select(o => o.RealWeight).Sum(); } }
    }

    public class GoodsDeliveryDTO : GoodsDeliveryPrimitiveDTO, IBaseDetailEntity<GoodsDeliveryDetailDTO>
    {
        public GoodsDeliveryDTO()
        {
            this.GoodsDeliveryViewDetails = new List<GoodsDeliveryDetailDTO>();
        }

        public override Nullable<int> ReceiverID { get { return (this.Receiver != null ? (this.Receiver.CustomerID > 0 ? (Nullable<int>)this.Receiver.CustomerID : null) : null); } }
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Receiver { get; set; }

        public override int DriverID { get { return (this.Driver != null ? this.Driver.EmployeeID : 0); } }
        [Display(Name = "Tài xế")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Driver { get; set; }

        public override int CollectorID { get { return (this.Collector != null ? this.Collector.EmployeeID : 0); } }
        [Display(Name = "Nhân viên giao hàng")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Collector { get; set; }

        public List<GoodsDeliveryDetailDTO> GoodsDeliveryViewDetails { get; set; }
        public List<GoodsDeliveryDetailDTO> ViewDetails { get { return this.GoodsDeliveryViewDetails; } set { this.GoodsDeliveryViewDetails = value; } }

        public ICollection<GoodsDeliveryDetailDTO> GetDetails() { return this.GoodsDeliveryViewDetails; }

        protected override IEnumerable<GoodsDeliveryDetailDTO> DtoDetails() { return this.GoodsDeliveryViewDetails; }
    }

}
