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
using TotalDTO.Sales;

namespace TotalDTO.Inventories
{
    public class GoodsIssuePrimitiveDTO : FreeQuantityDiscountVATAmountDTO<GoodsIssueDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.GoodsIssue; } }

        public int GetID() { return this.GoodsIssueID; }
        public void SetID(int id) { this.GoodsIssueID = id; }

        public int GoodsIssueID { get; set; }

        public virtual int CustomerID { get; set; }
        public virtual int ReceiverID { get; set; }

        public virtual Nullable<int> WarehouseID { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [UIHint("AutoCompletes/ShippingAddress")]
        public string ShippingAddress { get; set; }

        [Display(Name = "Tên người nhận hàng")]
        [UIHint("AutoCompletes/Addressee")]
        public string Addressee { get; set; }

        public virtual Nullable<int> DeliveryAdviceID { get; set; }

        public string DeliveryAdviceReferences { get; set; }
        [Display(Name = "Số đơn hàng")]
        [UIHint("Commons/SOCode")]
        public string Code { get; set; }

        public virtual Nullable<int> TradePromotionID { get; set; }
        [Display(Name = "Chiết khấu tổng")]
        public string TradePromotionSpecs { get; set; }

        [Display(Name = "Ngày giao hàng")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }

        public virtual int StorekeeperID { get; set; }

        [Display(Name = "Phương thức TT")]
        [UIHint("DropDowns/PaymentTerm")]
        public int PaymentTermID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            if (this.Addressee == null) { this.Addressee = ""; } this.Addressee = this.Addressee.Trim();

            string deliveryAdviceReferences = ""; string deliveryAdviceCodes = "";
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; e.ReceiverID = this.ReceiverID; e.StorekeeperID = this.StorekeeperID; if (deliveryAdviceReferences.IndexOf(e.DeliveryAdviceReference) < 0) deliveryAdviceReferences = deliveryAdviceReferences + (deliveryAdviceReferences != "" ? ", " : "") + e.DeliveryAdviceReference; if (e.DeliveryAdviceCode != null && deliveryAdviceCodes.IndexOf(e.DeliveryAdviceCode) < 0) deliveryAdviceCodes = deliveryAdviceCodes + (deliveryAdviceCodes != "" ? ", " : "") + e.DeliveryAdviceCode; });
            this.DeliveryAdviceReferences = deliveryAdviceReferences; this.Code = deliveryAdviceCodes != "" ? deliveryAdviceCodes : null;
        }
    }


    public class GoodsIssueDTO : GoodsIssuePrimitiveDTO, IBaseDetailEntity<GoodsIssueDetailDTO>
    {
        public GoodsIssueDTO()
        {
            this.GoodsIssueViewDetails = new List<GoodsIssueDetailDTO>();
        }

        public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override int ReceiverID { get { return (this.Receiver != null ? this.Receiver.CustomerID : 0); } }
        [Display(Name = "Đơn vị, người nhận hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Receiver { get; set; }

        public override Nullable<int> DeliveryAdviceID { get { return (this.DeliveryAdvice != null ? (Nullable<int>)this.DeliveryAdvice.DeliveryAdviceID : null); } }
        [UIHint("Commons/DeliveryAdviceBox")]
        public DeliveryAdviceBoxDTO DeliveryAdvice { get; set; }

        public override Nullable<int> WarehouseID { get { return (this.Warehouse != null ? this.Warehouse.WarehouseID : null); } }
        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        public WarehouseBaseDTO Warehouse { get; set; }

        public override int StorekeeperID { get { return (this.Storekeeper != null ? this.Storekeeper.EmployeeID : 0); } }
        [Display(Name = "Nhân viên kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Storekeeper { get; set; }

        public List<GoodsIssueDetailDTO> GoodsIssueViewDetails { get; set; }
        public List<GoodsIssueDetailDTO> ViewDetails { get { return this.GoodsIssueViewDetails; } set { this.GoodsIssueViewDetails = value; } }

        public ICollection<GoodsIssueDetailDTO> GetDetails() { return this.GoodsIssueViewDetails; }

        protected override IEnumerable<GoodsIssueDetailDTO> DtoDetails() { return this.GoodsIssueViewDetails; }
    }








    public interface IGoodsIssueBoxDTO //This DTO is used to display related GoodsIssue data only
    {
        Nullable<int> GoodsIssueID { get; set; }
        [Display(Name = "Số phiếu xuất kho")]
        string Reference { get; set; }
        [Display(Name = "Ngày xuất kho")]
        DateTime? EntryDate { get; set; }

        [Display(Name = "Mã khách hàng")]
        string CustomerCode { get; set; }
        [Display(Name = "Tên khách hàng")]
        string CustomerName { get; set; }

        [Display(Name = "Mã đơn vị, người nhận hàng")]
        string ReceiverCode { get; set; }
        [Display(Name = "Tên đơn vị, người nhận hàng")]
        string ReceiverName { get; set; }
    }

    public class GoodsIssueBoxDTO : IGoodsIssueBoxDTO
    {
        public Nullable<int> GoodsIssueID { get; set; }
        public string Reference { get; set; }
        public DateTime? EntryDate { get; set; }

        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }

        public string ReceiverCode { get; set; }
        public string ReceiverName { get; set; }
    }



}



