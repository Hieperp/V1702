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
using TotalDTO.Helpers.Interfaces;

namespace TotalDTO.Sales
{
    public class DeliveryAdvicePrimitiveDTO : FreeQuantityDiscountVATAmountDTO<DeliveryAdviceDetailDTO>, IPrimitiveEntity, IPrimitiveDTO, IPriceCategory
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.DeliveryAdvice; } }

        public int GetID() { return this.DeliveryAdviceID; }
        public void SetID(int id) { this.DeliveryAdviceID = id; }

        public int DeliveryAdviceID { get; set; }

        public virtual int CustomerID { get; set; }
        public virtual int ReceiverID { get; set; }

        public virtual Nullable<int> WarehouseID { get; set; }

        [Required]
        [Display(Name = "Bảng giá")]
        public int PriceCategoryID { get; set; }
        [Display(Name = "Bảng giá")]
        public string PriceCategoryCode { get; set; }

        [Display(Name = "Phương thức TT")]
        public int PaymentTermID { get; set; }

        public bool HasSalesOrder { get; set; }
        public Nullable<int> SalesOrderID { get; set; }
        public string SalesOrderReference { get; set; }
        public string SalesOrderReferences { get; set; }
        public string SalesOrderCode { get; set; }
        public string SalesOrderCodes { get; set; }
        [Display(Name = "Phiếu đặt hàng")]
        public string SalesOrderReferenceNote { get { return this.SalesOrderID != null ? this.SalesOrderReference : (this.SalesOrderReferences != "" ? this.SalesOrderReferences : "Giao hàng tổng hợp của nhiều ĐH"); } }
        [Display(Name = "Số đơn hàng")]
        public string SalesOrderCodeNote { get { return this.SalesOrderID != null ? this.SalesOrderCode : (this.SalesOrderCodes != "" ? this.SalesOrderCodes : ""); } }
        [Display(Name = "Ngày đặt hàng")]
        public Nullable<System.DateTime> SalesOrderEntryDate { get; set; }

        [Display(Name = "Số đơn hàng")]
        [UIHint("Commons/SOCode")]
        public string Code { get; set; }
        public virtual Nullable<int> PromotionID { get; set; }
        [Display(Name = "Chứng từ khuyến mãi")]
        public string PromotionVouchers { get; set; }

        public virtual Nullable<int> TradePromotionID { get; set; }
        [Display(Name = "Chiết khấu tổng")]
        public string TradePromotionSpecs { get; set; }

        [Display(Name = "Ngày giao hàng")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        [Display(Name = "Địa chỉ giao hàng")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [UIHint("AutoCompletes/ShippingAddress")]
        public string ShippingAddress { get; set; }

        [Display(Name = "Tên người nhận hàng")]
        [UIHint("AutoCompletes/Addressee")]
        public string Addressee { get; set; }

        public virtual int SalespersonID { get; set; }

        public override void PerformPresaveRule()
        {
            this.Approved = true; this.ApprovedDate = this.EntryDate; //At DeliveryAdvice, Approve right after save. Surely, It can be UnApprove later for editing

            base.PerformPresaveRule();

            if (this.Addressee == null) { this.Addressee = ""; } this.Addressee = this.Addressee.Trim();

            string salesOrderReferences = ""; string salesOrderCodes = "";
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; e.ReceiverID = this.ReceiverID; e.PromotionID = this.PromotionID; e.SalespersonID = this.SalespersonID; if (this.HasSalesOrder && salesOrderReferences.IndexOf(e.SalesOrderReference) < 0) salesOrderReferences = salesOrderReferences + (salesOrderReferences != "" ? ", " : "") + e.SalesOrderReference; if (this.HasSalesOrder && salesOrderCodes.IndexOf(e.SalesOrderCode) < 0) salesOrderCodes = salesOrderCodes + (salesOrderCodes != "" ? ", " : "") + e.SalesOrderCode; });
            this.SalesOrderReferences = salesOrderReferences; this.SalesOrderCodes = salesOrderCodes != "" ? salesOrderCodes : null; if (this.HasSalesOrder) this.Code = this.SalesOrderCodes;
        }
    }


    public class DeliveryAdviceDTO : DeliveryAdvicePrimitiveDTO, IBaseDetailEntity<DeliveryAdviceDetailDTO>, ISearchCustomer
    {
        public DeliveryAdviceDTO()
        {
            this.DeliveryAdviceViewDetails = new List<DeliveryAdviceDetailDTO>();
        }

        public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override int ReceiverID { get { return (this.Receiver != null ? this.Receiver.CustomerID : 0); } }
        [Display(Name = "Đơn vị, người nhận hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Receiver { get; set; }

        public override Nullable<int> WarehouseID { get { return (this.Warehouse != null ? this.Warehouse.WarehouseID : null); } }
        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        public WarehouseBaseDTO Warehouse { get; set; }

        public override Nullable<int> PromotionID { get { return (this.Promotion != null ? this.Promotion.PromotionID : null); } }
        [UIHint("Commons/Promotion")]
        public PromotionBaseDTO Promotion { get; set; }

        public override int SalespersonID { get { return (this.Salesperson != null ? this.Salesperson.EmployeeID : 0); } }
        [Display(Name = "Nhân viên tiếp thị")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Salesperson { get; set; }

        public override Nullable<int> VoidTypeID { get { return (this.VoidType != null ? this.VoidType.VoidTypeID : null); } }
        [UIHint("AutoCompletes/VoidType")]
        public VoidTypeBaseDTO VoidType { get; set; }

        public List<DeliveryAdviceDetailDTO> DeliveryAdviceViewDetails { get; set; }
        public List<DeliveryAdviceDetailDTO> ViewDetails { get { return this.DeliveryAdviceViewDetails; } set { this.DeliveryAdviceViewDetails = value; } }

        public ICollection<DeliveryAdviceDetailDTO> GetDetails() { return this.DeliveryAdviceViewDetails; }

        protected override IEnumerable<DeliveryAdviceDetailDTO> DtoDetails() { return this.DeliveryAdviceViewDetails; }

        public override void PrepareVoidDetail(int? detailID)
        {
            this.ViewDetails.RemoveAll(w => w.DeliveryAdviceDetailID != detailID);
            if (this.ViewDetails.Count() > 0)
                this.VoidType = new VoidTypeBaseDTO() { VoidTypeID = this.ViewDetails[0].VoidTypeID, Code = this.ViewDetails[0].VoidTypeCode, Name = this.ViewDetails[0].VoidTypeName, VoidClassID = this.ViewDetails[0].VoidClassID };
            base.PrepareVoidDetail(detailID);
        }
    }









    public interface IDeliveryAdviceBoxDTO //This DTO is used to display related DeliveryAdvice data only
    {
        Nullable<int> DeliveryAdviceID { get; set; }
        [Display(Name = "Số phiếu ĐNGH")]
        string Reference { get; set; }
        [Display(Name = "Ngày ĐNGH")]
        DateTime? EntryDate { get; set; }
    }

    public class DeliveryAdviceBoxDTO : IDeliveryAdviceBoxDTO
    {
        public Nullable<int> DeliveryAdviceID { get; set; }
        public string Reference { get; set; }
        public DateTime? EntryDate { get; set; }
    }


}
