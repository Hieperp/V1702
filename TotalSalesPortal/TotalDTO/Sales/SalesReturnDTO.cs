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
    public class SalesReturnDTO
    {
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
        [Display(Name = "Tổng số tiền chênh lệch tỷ giá")]
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
