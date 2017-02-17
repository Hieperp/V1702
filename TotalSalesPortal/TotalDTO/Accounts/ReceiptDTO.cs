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

namespace TotalDTO.Accounts
{
    public class ReceiptPrimitiveDTO : BaseWithDetailDTO<ReceiptDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Receipt; } }

        public int GetID() { return this.ReceiptID; }
        public void SetID(int id) { this.ReceiptID = id; }

        public int ReceiptID { get; set; }

        public virtual int CustomerID { get; set; }

        public Nullable<int> GoodsIssueID { get; set; }
        [Display(Name = "Đơn đặt hàng")]
        public string GoodsIssueReference { get; set; }
        [Display(Name = "Ngày đặt hàng")]
        public Nullable<System.DateTime> GoodsIssueEntryDate { get; set; }

        [Display(Name = "Ngày ghi sổ cái")]
        public Nullable<System.DateTime> PostDate { get; set; }

        public virtual int CashierID { get; set; }

        [Display(Name = "Tổng cấn trừ hóa đơn")]
        public decimal TotalReceiptAmount { get { return this.GetReceiptAmount(); } } //PHAI XEM XET LAI: NHAM MUC DICH HOAN THIEN UN-APPLY AMOUNT { get; set; }
        [Display(Name = "Tổng CK")]
        public decimal TotalCashDiscount { get; set; }
        [Display(Name = "Tổng CLTG")]
        public decimal TotalFluctuationAmount { get; set; }
        [Display(Name = "Tổng thanh toán")]
        public decimal TotalDepositAmount { get { return this.TotalReceiptAmount; } } //PHAI XEM XET LAI: NHAM MUC DICH HOAN THIEN UN-APPLY AMOUNT { get; set; }

        protected virtual decimal GetReceiptAmount() { return this.DtoDetails().Select(o => o.ReceiptAmount).Sum(); }
        protected virtual decimal GetCashDiscount() { return this.DtoDetails().Select(o => o.CashDiscount).Sum(); }
        protected virtual decimal GetFluctuationAmount() { return this.DtoDetails().Select(o => o.FluctuationAmount).Sum(); }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.TotalReceiptAmount != this.GetReceiptAmount()) yield return new ValidationResult("Lỗi tổng số tiền cấn trừ", new[] { "TotalReceiptAmount" });
            if (this.TotalCashDiscount != this.GetCashDiscount()) yield return new ValidationResult("Lỗi tổng số tiền ck thanh toán", new[] { "TotalCashDiscount" });
            if (this.TotalFluctuationAmount != this.GetFluctuationAmount()) yield return new ValidationResult("Lỗi tổng tiền chênh lệch tỷ giá", new[] { "TotalFluctuationAmount" });
        }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; });
        }
    }


    public class ReceiptDTO : ReceiptPrimitiveDTO, IBaseDetailEntity<ReceiptDetailDTO>
    {
        public ReceiptDTO()
        {
            this.ReceiptViewDetails = new List<ReceiptDetailDTO>();
        }

        public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override int CashierID { get { return (this.Cashier != null ? this.Cashier.EmployeeID : 0); } }
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Cashier { get; set; }

        public List<ReceiptDetailDTO> ReceiptViewDetails { get; set; }
        public List<ReceiptDetailDTO> ViewDetails { get { return this.ReceiptViewDetails; } set { this.ReceiptViewDetails = value; } }

        public ICollection<ReceiptDetailDTO> GetDetails() { return this.ReceiptViewDetails; }

        protected override IEnumerable<ReceiptDetailDTO> DtoDetails() { return this.ReceiptViewDetails; }
    }


}
