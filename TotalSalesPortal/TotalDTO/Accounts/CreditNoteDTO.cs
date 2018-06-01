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

namespace TotalDTO.Accounts
{
    public class CreditNotePrimitiveDTO : VATAmountDTO<CreditNoteDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.CreditNote; } }

        public int GetID() { return this.CreditNoteID; }
        public void SetID(int id) { this.CreditNoteID = id; }

        public int CreditNoteID { get; set; }

        public virtual int CustomerID { get; set; }

        [Display(Name = "Chứng từ chiết khấu")]
        public string MemoCode { get; set; }
        [Display(Name = "Ngày chứng từ")]
        public Nullable<System.DateTime> MemoDate { get; set; }

        public virtual Nullable<int> PromotionID { get; set; }
        [Display(Name = "Chứng từ khuyến mãi")]
        public string PromotionVouchers { get; set; }

        [Display(Name = "Phương thức TT")]
        public int PaymentTermID { get; set; }

        public virtual int SalespersonID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; e.PromotionID = this.PromotionID; e.SalespersonID = this.SalespersonID; });
        }
    }


    public class CreditNoteDTO : CreditNotePrimitiveDTO, IBaseDetailEntity<CreditNoteDetailDTO>
    {
        public CreditNoteDTO()
        {
            this.CreditNoteDetails = new List<CreditNoteDetailDTO>();
        }

        public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> PromotionID { get { return (this.Promotion != null ? this.Promotion.PromotionID : null); } }
        [UIHint("Commons/Promotion")]
        public PromotionBaseDTO Promotion { get; set; }

        public override int SalespersonID { get { return (this.Salesperson != null ? this.Salesperson.EmployeeID : 0); } }
        [Display(Name = "Nhân viên tiếp thị")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Salesperson { get; set; }


        public List<CreditNoteDetailDTO> CreditNoteDetails { get; set; }

        public ICollection<CreditNoteDetailDTO> GetDetails() { return this.CreditNoteDetails; }

        protected override IEnumerable<CreditNoteDetailDTO> DtoDetails() { return this.CreditNoteDetails; }
    }











    public interface ICreditNoteBoxDTO //This DTO is used to display related CreditNote data only
    {
        Nullable<int> CreditNoteID { get; set; }
        [Display(Name = "Số phiếu thu")]
        string Reference { get; set; }
        [Display(Name = "Ngày thu tiền")]
        DateTime? EntryDate { get; set; }


        [Display(Name = "Số tiền chiết khấu")]
        Nullable<decimal> TotalCreditAmount { get; set; }
        [Display(Name = "Tổng số tiền cấn trừ công nợ")]
        Nullable<decimal> TotalReceiptAmount { get; set; }
        [Display(Name = "Tổng thu (+) hoặc CK khác (-)")]
        Nullable<decimal> TotalFluctuationAmount { get; set; }

        [Display(Name = "Số tiền đã cấn trừ")]
        Nullable<decimal> TotalCreditAmountApplied { get; }
        [Display(Name = "Số tiền thanh toán chưa cấn trừ")]
        Nullable<decimal> TotalCreditAmountPending { get; }
    }

    public class CreditNoteBoxDTO : ICreditNoteBoxDTO
    {
        public Nullable<int> CreditNoteID { get; set; }
        public string Reference { get; set; }
        public DateTime? EntryDate { get; set; }


        public Nullable<decimal> TotalCreditAmount { get; set; }
        public Nullable<decimal> TotalReceiptAmount { get; set; }
        public Nullable<decimal> TotalFluctuationAmount { get; set; }

        public Nullable<decimal> TotalCreditAmountApplied { get { return this.TotalReceiptAmount + this.TotalFluctuationAmount; } }
        public Nullable<decimal> TotalCreditAmountPending { get { return this.TotalCreditAmount - this.TotalCreditAmountApplied; } }
    }

}
