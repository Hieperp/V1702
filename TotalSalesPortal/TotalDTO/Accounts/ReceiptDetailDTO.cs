using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;

namespace TotalDTO.Accounts
{
    public class ReceiptDetailDTO : BaseModel, IPrimitiveEntity
    {
        public int GetID() { return this.ReceiptDetailID; }

        public int ReceiptDetailID { get; set; }
        public int ReceiptID { get; set; }

        public int GoodsIssueID { get; set; }
        [Display(Name = "Ngày giao hàng")]
        [UIHint("DateTimeReadonly")]
        public System.DateTime GoodsIssueEntryDate { get; set; }
        [Display(Name = "Số phiếu")]
        [UIHint("StringReadonly")]
        public string GoodsIssueReference { get; set; }

        public int CustomerID { get; set; }
        [Display(Name = "Khách hàng")]
        [UIHint("StringReadonly")]
        public string CustomerName { get; set; }
        [Display(Name = "Khách hàng")]
        [UIHint("StringReadonly")]
        public string CustomerOfficialName { get; set; }

        public int CashierID { get; set; }

        [Display(Name = "Diễn giải")]
        [UIHint("StringReadonly")]
        public string Description { get; set; }

        [Display(Name = "Diễn giải")]
        [UIHint("StringReadonly")]
        public string ReceiverDescription { get; set; }

        [Display(Name = "Số tiền bán hàng")]
        [UIHint("DecimalReadonly")]
        public decimal TotalGrossAmount { get; set; }

        [Display(Name = "Số tiền phải thu")]
        [UIHint("DecimalReadonly")]
        public Nullable<decimal> AmountDue { get; set; }

        [Display(Name = "CK thanh toán")]
        public decimal CashDiscount { get; set; }

        [Display(Name = "Thu (+) hoặc CK khác (-)")]
        public decimal FluctuationAmount { get; set; }

        [Display(Name = "Số tiền thu")]
        public decimal ReceiptAmount { get; set; }

        [GenericCompare(CompareToPropertyName = "AmountDue", OperatorName = GenericCompareOperator.LessThanOrEqual, ErrorMessage = "Số tiền cấn trừ không vượt số lượng phải thu")]
        public decimal ApplyAmount { get { return (this.ReceiptAmount + this.CashDiscount); } }
    }
}