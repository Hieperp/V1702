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

namespace TotalDTO.Accounts
{
    public class CreditNoteDTO
    {
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
        [Display(Name = "Tổng số tiền chênh lệch tỷ giá")]
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
