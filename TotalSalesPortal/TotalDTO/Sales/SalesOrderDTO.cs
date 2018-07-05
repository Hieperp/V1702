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
    public class SalesOrderPrimitiveDTO : FreeQuantityDiscountVATAmountDTO<SalesOrderDetailDTO>, IPrimitiveEntity, IPrimitiveDTO, IPriceCategory
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.SalesOrder; } }

        public int GetID() { return this.SalesOrderID; }
        public void SetID(int id) { this.SalesOrderID = id; }

        public int SalesOrderID { get; set; }

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

        public Nullable<int> QuotationID { get; set; }
        [Display(Name = "Phiếu báo giá")]
        public string QuotationReference { get; set; }
        [Display(Name = "Ngày báo giá")]
        public Nullable<System.DateTime> QuotationEntryDate { get; set; }

        [Display(Name = "Số đơn hàng")]
        [Required(ErrorMessage = "Vui lòng nhập số đơn hàng")]
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
            //this.Approved = true; this.ApprovedDate = this.EntryDate; //At SalesOrder, Approve right after save. Surely, It can be UnApprove later for editing
            base.PerformPresaveRule();

            if (this.Addressee == null) { this.Addressee = ""; } this.Addressee = this.Addressee.Trim(); 
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; e.ReceiverID = this.ReceiverID; e.PromotionID = this.PromotionID; e.SalespersonID = this.SalespersonID; });
        }
    }


    public class SalesOrderDTO : SalesOrderPrimitiveDTO, IBaseDetailEntity<SalesOrderDetailDTO>, ISearchCustomer
    {
        public SalesOrderDTO()
        {
            this.SalesOrderViewDetails = new List<SalesOrderDetailDTO>();
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

        public List<SalesOrderDetailDTO> SalesOrderViewDetails { get; set; }
        public List<SalesOrderDetailDTO> ViewDetails { get { return this.SalesOrderViewDetails; } set { this.SalesOrderViewDetails = value; } }

        public ICollection<SalesOrderDetailDTO> GetDetails() { return this.SalesOrderViewDetails; }

        protected override IEnumerable<SalesOrderDetailDTO> DtoDetails() { return this.SalesOrderViewDetails; }

        public override void PrepareVoidDetail(int? detailID)
        {
            this.ViewDetails.RemoveAll(w => w.SalesOrderDetailID != detailID);
            if (this.ViewDetails.Count() > 0)
                this.VoidType = new VoidTypeBaseDTO() { VoidTypeID = this.ViewDetails[0].VoidTypeID, Code = this.ViewDetails[0].VoidTypeCode, Name = this.ViewDetails[0].VoidTypeName, VoidClassID = this.ViewDetails[0].VoidClassID };
            base.PrepareVoidDetail(detailID);
        }
    }









    public interface ISalesOrderBoxDTO //This DTO is used to display related SalesOrder data only
    {
        Nullable<int> SalesOrderID { get; set; }
        [Display(Name = "Số phiếu ĐNGH")]
        string Reference { get; set; }
        [Display(Name = "Ngày ĐNGH")]
        DateTime? EntryDate { get; set; }
    }

    public class SalesOrderBoxDTO : ISalesOrderBoxDTO
    {
        public Nullable<int> SalesOrderID { get; set; }
        public string Reference { get; set; }
        public DateTime? EntryDate { get; set; }
    }


}
