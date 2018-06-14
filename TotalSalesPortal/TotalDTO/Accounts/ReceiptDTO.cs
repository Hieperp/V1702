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
using TotalDTO.Inventories;


namespace TotalDTO.Accounts
{
    public class ReceiptPrimitiveDTO : BaseWithDetailDTO<ReceiptDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Receipt; } }

        public int GetID() { return this.ReceiptID; }
        public void SetID(int id) { this.ReceiptID = id; }

        public int ReceiptID { get; set; }

        public virtual int CustomerID { get; set; }
        public virtual Nullable<int> GoodsIssueID { get; set; }

        public string GoodsIssueReferences { get; set; }

        public int ReceiptTypeID { get; set; }
        [Display(Name = "Phương thức TT")]
        public Nullable<int> MonetaryAccountID { get; set; }

        public virtual Nullable<int> AdvanceReceiptID { get; set; }
        public virtual Nullable<int> SalesReturnID { get; set; }
        public virtual Nullable<int> CreditNoteID { get; set; }

        [Display(Name = "Ngày ghi sổ cái")] //This properties is now not shown in view (not used)
        public Nullable<System.DateTime> PostDate { get; set; }

        public virtual int CashierID { get; set; }

        [Display(Name = "Số tiền thanh toán")]
        public decimal TotalDepositAmount { get; set; }

        [Display(Name = "Tổng số tiền cấn trừ công nợ")]
        public decimal TotalReceiptAmount { get; set; }
        [Display(Name = "Tổng chiết khấu thanh toán")]
        public decimal TotalCashDiscount { get; set; }
        [Display(Name = "Tổng chiết khấu khác")]
        public decimal TotalOtherDiscount { get; set; }
        [Display(Name = "Tổng thu khác")]
        public decimal TotalFluctuationAmount { get; set; }

        protected virtual decimal GetReceiptAmount() { return this.DtoDetails().Select(o => o.ReceiptAmount).Sum(); }
        protected virtual decimal GetCashDiscount() { return this.DtoDetails().Select(o => o.CashDiscount).Sum(); }
        protected virtual decimal GetOtherDiscount() { return this.DtoDetails().Select(o => o.OtherDiscount).Sum(); }
        protected virtual decimal GetFluctuationAmount() { return this.DtoDetails().Select(o => o.FluctuationAmount).Sum(); }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.TotalReceiptAmount != this.GetReceiptAmount()) yield return new ValidationResult("Lỗi tổng số tiền cấn trừ công nợ", new[] { "TotalReceiptAmount" });
            if (this.TotalCashDiscount != this.GetCashDiscount()) yield return new ValidationResult("Lỗi tổng số tiền chiết khấu thanh toán", new[] { "TotalCashDiscount" });
            if (this.TotalOtherDiscount != this.GetOtherDiscount()) yield return new ValidationResult("Lỗi tổng số tiền chiết khấu khác", new[] { "TotalOtherDiscount" });
            if (this.TotalFluctuationAmount != this.GetFluctuationAmount()) yield return new ValidationResult("Lỗi tổng số tiền thu khác (-)", new[] { "TotalFluctuationAmount" });
        }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            string goodsIssueReferences = ""; int i = 0;
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; if ((e.CashDiscount != 0 || e.OtherDiscount != 0 || e.ReceiptAmount != 0) && i <= 6) goodsIssueReferences = goodsIssueReferences + (goodsIssueReferences != "" ? ", " : "") + (i++ < 6 ? e.GoodsIssueReference : "..."); });
            this.GoodsIssueReferences = goodsIssueReferences;
        }
    }


    public class ReceiptDTO : ReceiptPrimitiveDTO, IBaseDetailEntity<ReceiptDetailDTO>
    {
        public ReceiptDTO()
        {
            this.ReceiptViewDetails = new List<ReceiptDetailDTO>();
        }


        public List<ReceiptDetailDTO> ReceiptViewDetails { get; set; }
        public List<ReceiptDetailDTO> ViewDetails { get { return this.ReceiptViewDetails; } set { this.ReceiptViewDetails = value; } }

        public ICollection<ReceiptDetailDTO> GetDetails() { return this.ReceiptViewDetails; }

        protected override IEnumerable<ReceiptDetailDTO> DtoDetails() { return this.ReceiptViewDetails; }




        public decimal TotalReceiptAmountSaved { get; set; }
        public decimal TotalFluctuationAmountSaved { get; set; }

        public string CreditTypeName { get { return (this.AdvanceReceipt != null && this.AdvanceReceipt.ReceiptID > 0 ? "trả trước" : (this.SalesReturn != null && this.SalesReturn.SalesReturnID > 0 ? "trả hàng" : (this.CreditNote != null && this.CreditNote.CreditNoteID > 0 ? "chiết khấu" : null))); } }
        public string CreditTypeReference { get { return (this.AdvanceReceipt != null && this.AdvanceReceipt.ReceiptID > 0 ? this.AdvanceReceipt.Reference : (this.SalesReturn != null && this.SalesReturn.SalesReturnID > 0 ? this.SalesReturn.Reference : (this.CreditNote != null && this.CreditNote.CreditNoteID > 0 ? this.CreditNote.Reference : null))); } }
        public System.DateTime CreditTypeDate { get { return ((System.DateTime)(this.AdvanceReceipt != null && this.AdvanceReceipt.ReceiptID > 0 ? this.AdvanceReceipt.EntryDate : (this.SalesReturn != null && this.SalesReturn.SalesReturnID > 0 ? this.SalesReturn.EntryDate : (this.CreditNote != null && this.CreditNote.CreditNoteID > 0 ? this.CreditNote.EntryDate : DateTime.Today)))); } }

        public string CreditAmountLabel { get { return (this.AdvanceReceipt != null && this.AdvanceReceipt.ReceiptID > 0 ? "Số tiền trả trước" : (this.SalesReturn != null && this.SalesReturn.SalesReturnID > 0 ? "Số tiền trả hàng" : (this.CreditNote != null && this.CreditNote.CreditNoteID > 0 ? "Số tiền chiết khấu" : null))); } }
        public Nullable<decimal> CreditAmount { get { return (this.AdvanceReceipt != null && this.AdvanceReceipt.ReceiptID > 0 ? this.AdvanceReceipt.TotalDepositAmount : (this.SalesReturn != null && this.SalesReturn.SalesReturnID > 0 ? this.SalesReturn.TotalGrossAmount : (this.CreditNote != null && this.CreditNote.CreditNoteID > 0 ? this.CreditNote.TotalCreditAmount : null))); } }
        [Display(Name = "Số tiền đã cấn trừ trước đó")]
        public Nullable<decimal> CreditAmountApplied { get { return -this.TotalReceiptAmountSaved - this.TotalFluctuationAmountSaved + (this.AdvanceReceipt != null && this.AdvanceReceipt.ReceiptID > 0 ? this.AdvanceReceipt.TotalDepositAmountApplied : (this.SalesReturn != null && this.SalesReturn.SalesReturnID > 0 ? this.SalesReturn.TotalGrossAmountApplied : (this.CreditNote != null && this.CreditNote.CreditNoteID > 0 ? this.CreditNote.TotalCreditAmountApplied : null))); } }
        public string CreditAmountPendingLabel { get { return (this.AdvanceReceipt != null && this.AdvanceReceipt.ReceiptID > 0 ? "Số tiền trả trước còn lại" : (this.SalesReturn != null && this.SalesReturn.SalesReturnID > 0 ? "Số tiền trả hàng còn lại" : (this.CreditNote != null && this.CreditNote.CreditNoteID > 0 ? "Số tiền chiết khấu còn lại" : null))); } }
        public Nullable<decimal> CreditAmountPending { get { return this.TotalReceiptAmountSaved + this.TotalFluctuationAmountSaved + (this.AdvanceReceipt != null && this.AdvanceReceipt.ReceiptID > 0 ? this.AdvanceReceipt.TotalDepositAmountPending : (this.SalesReturn != null && this.SalesReturn.SalesReturnID > 0 ? this.SalesReturn.TotalGrossAmountPending : (this.CreditNote != null && this.CreditNote.CreditNoteID > 0 ? this.CreditNote.TotalCreditAmountPending : null))); } }


        public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> GoodsIssueID { get { return (this.GoodsIssue != null ? this.GoodsIssue.GoodsIssueID : null); } }
        [UIHint("Commons/GoodsIssueBox")]
        public GoodsIssueBoxDTO GoodsIssue { get; set; }


        public override Nullable<int> AdvanceReceiptID { get { return (this.AdvanceReceipt != null ? this.AdvanceReceipt.ReceiptID : null); } }
        [UIHint("Commons/ReceiptBox")]
        public ReceiptBoxDTO AdvanceReceipt { get; set; }

        public override Nullable<int> SalesReturnID { get { return (this.SalesReturn != null ? this.SalesReturn.SalesReturnID : null); } }
        [UIHint("Commons/SalesReturnBox")]
        public SalesReturnBoxDTO SalesReturn { get; set; }

        public override Nullable<int> CreditNoteID { get { return (this.CreditNote != null ? this.CreditNote.CreditNoteID : null); } }
        [UIHint("Commons/CreditNoteBox")]
        public CreditNoteBoxDTO CreditNote { get; set; }


        public override int CashierID { get { return (this.Cashier != null ? this.Cashier.EmployeeID : 0); } }
        [Display(Name = "Nhân viên thu tiền")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Cashier { get; set; }


        [Display(Name = "Số tiền chuyển kỳ sau")]
        public decimal TotalAmountDifference { get { return this.GetAmountDifference(); } }

        protected decimal GetAmountDifference() { return (this.ReceiptTypeID == GlobalReceiptTypeID.ReceiveMoney ? this.TotalDepositAmount : (decimal)(this.CreditAmountPending != null ? this.CreditAmountPending : 0)) - this.TotalReceiptAmount - this.TotalFluctuationAmount; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.TotalAmountDifference < 0 || this.TotalAmountDifference != this.GetAmountDifference()) yield return new ValidationResult("Lỗi tổng số tiền cấn trừ", new[] { "TotalAmountDifference" });
        }
    }







    public interface IReceiptBoxDTO //This DTO is used to display related Receipt data only
    {
        Nullable<int> ReceiptID { get; set; }
        [Display(Name = "Số phiếu thu")]
        string Reference { get; set; }
        [Display(Name = "Ngày thu tiền")]
        DateTime? EntryDate { get; set; }


        [Display(Name = "Tổng thanh toán")]
        Nullable<decimal> TotalDepositAmount { get; set; }
        [Display(Name = "Tổng số tiền cấn trừ công nợ")]
        Nullable<decimal> TotalReceiptAmount { get; set; }
        [Display(Name = "Tổng thu (+) hoặc CK khác (-)")]
        Nullable<decimal> TotalFluctuationAmount { get; set; }

        [Display(Name = "Số tiền đã cấn trừ")]
        Nullable<decimal> TotalDepositAmountApplied { get; }
        [Display(Name = "Số tiền thanh toán chưa cấn trừ")]
        Nullable<decimal> TotalDepositAmountPending { get; }
    }

    public class ReceiptBoxDTO : IReceiptBoxDTO
    {
        public Nullable<int> ReceiptID { get; set; }
        public string Reference { get; set; }
        public DateTime? EntryDate { get; set; }


        public Nullable<decimal> TotalDepositAmount { get; set; }
        public Nullable<decimal> TotalReceiptAmount { get; set; }
        public Nullable<decimal> TotalFluctuationAmount { get; set; }

        public Nullable<decimal> TotalDepositAmountApplied { get { return this.TotalReceiptAmount + this.TotalFluctuationAmount; } }
        public Nullable<decimal> TotalDepositAmountPending { get { return this.TotalDepositAmount - this.TotalDepositAmountApplied; } }
    }

}
