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
using TotalDTO.Inventories;
using TotalDTO.Helpers.Interfaces;

namespace TotalDTO.Sales
{

    public class SalesReturnPrimitiveDTO : FreeQuantityDiscountVATAmountDTO<SalesReturnDetailDTO>, IPrimitiveEntity, IPrimitiveDTO, IPriceCategory
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.SalesReturn; } }

        public int GetID() { return this.SalesReturnID; }
        public void SetID(int id) { this.SalesReturnID = id; }

        public int SalesReturnID { get; set; }

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

        public bool HasGoodsIssue { get; set; }

        public virtual Nullable<int> GoodsIssueID { get; set; }
        public string GoodsIssueReferences { get; set; }
        
        [Display(Name = "Chứng từ trả hàng")]
        [UIHint("Commons/SOCode")]
        public string Code { get; set; }
        public virtual Nullable<int> PromotionID { get; set; }

        public virtual Nullable<int> TradePromotionID { get; set; }
        [Display(Name = "Chiết khấu tổng")]
        public string TradePromotionSpecs { get; set; }

        [Display(Name = "Ngày nhận hàng")]
        public Nullable<System.DateTime> ReceivedDate { get; set; }
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
            base.PerformPresaveRule();

            if (this.Addressee == null) { this.Addressee = ""; } this.Addressee = this.Addressee.Trim();

            string goodsIssueReferences = "";
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; e.ReceiverID = this.ReceiverID; e.WarehouseID = this.WarehouseID; e.PromotionID = this.PromotionID; e.SalespersonID = this.SalespersonID; if (this.HasGoodsIssue && goodsIssueReferences.IndexOf(e.GoodsIssueReference) < 0) goodsIssueReferences = goodsIssueReferences + (goodsIssueReferences != "" ? ", " : "") + e.GoodsIssueReference; });
            this.GoodsIssueReferences = goodsIssueReferences;
        }
    }


    public class SalesReturnDTO : SalesReturnPrimitiveDTO, IBaseDetailEntity<SalesReturnDetailDTO>, ISearchCustomer
    {
        public SalesReturnDTO()
        {
            this.SalesReturnViewDetails = new List<SalesReturnDetailDTO>();
        }

        public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override int ReceiverID { get { return (this.Receiver != null ? this.Receiver.CustomerID : 0); } }
        [Display(Name = "Đơn vị, người nhận hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Receiver { get; set; }

        public override Nullable<int> GoodsIssueID { get { return (this.GoodsIssue != null ? (Nullable<int>)this.GoodsIssue.GoodsIssueID : null); } }
        [UIHint("Commons/GoodsIssueBox")]
        public GoodsIssueBoxDTO GoodsIssue { get; set; }

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
        
        public List<SalesReturnDetailDTO> SalesReturnViewDetails { get; set; }
        public List<SalesReturnDetailDTO> ViewDetails { get { return this.SalesReturnViewDetails; } set { this.SalesReturnViewDetails = value; } }

        public ICollection<SalesReturnDetailDTO> GetDetails() { return this.SalesReturnViewDetails; }

        protected override IEnumerable<SalesReturnDetailDTO> DtoDetails() { return this.SalesReturnViewDetails; }
        
    }









    public interface ISalesReturnBoxDTO //This DTO is used to display related SalesReturn data only
    {
        Nullable<int> SalesReturnID { get; set; }
        [Display(Name = "Số phiếu thu")]
        string Reference { get; set; }
        [Display(Name = "Ngày thu tiền")]
        DateTime? EntryDate { get; set; }


        [Display(Name = "Số tiền trả hàng")]
        Nullable<decimal> TotalGrossAmount { get; set; }
        [Display(Name = "Tổng số tiền cấn trừ công nợ")]
        Nullable<decimal> TotalReceiptAmount { get; set; }
        [Display(Name = "Tổng thu (+) hoặc CK khác (-)")]
        Nullable<decimal> TotalFluctuationAmount { get; set; }

        [Display(Name = "Số tiền đã cấn trừ")]
        Nullable<decimal> TotalGrossAmountApplied { get; }
        [Display(Name = "Số tiền thanh toán chưa cấn trừ")]
        Nullable<decimal> TotalGrossAmountPending { get; }
    }

    public class SalesReturnBoxDTO : ISalesReturnBoxDTO
    {
        public Nullable<int> SalesReturnID { get; set; }
        public string Reference { get; set; }
        public DateTime? EntryDate { get; set; }


        public Nullable<decimal> TotalGrossAmount { get; set; }
        public Nullable<decimal> TotalReceiptAmount { get; set; }
        public Nullable<decimal> TotalFluctuationAmount { get; set; }

        public Nullable<decimal> TotalGrossAmountApplied { get { return this.TotalReceiptAmount + this.TotalFluctuationAmount; } }
        public Nullable<decimal> TotalGrossAmountPending { get { return this.TotalGrossAmount - this.TotalGrossAmountApplied; } }
    }

}
