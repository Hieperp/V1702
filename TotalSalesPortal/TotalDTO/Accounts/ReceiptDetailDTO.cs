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
        [Display(Name = "Ngày PXK")]
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

        [Display(Name = "Bán hàng")]
        [UIHint("DecimalReadonly")]
        public decimal TotalGrossAmount { get; set; }

        [Display(Name = "Phải thu")]
        [UIHint("DecimalReadonly")]
        public Nullable<decimal> AmountDue { get; set; }

        [Display(Name = "CKTT")]
        public decimal CashDiscount { get; set; }

        [Display(Name = "CK khác")]
        public decimal OtherDiscount { get; set; }

        [Display(Name = "Thu khác")]
        public decimal FluctuationAmount { get; set; }
        //[UIHint("DecimalWithMinus")], [Display(Name = "Thu (+) hoặc CK khác (-)")]
        //HERE: WE MAY ALLOW FluctuationAmount TO ACCEPT: + OR - VALUE FOR FluctuationAmount. WITH THE CURRENT PROGRAMMING HERE, THE FluctuationAmount VALUE WILL BE CALCULATED AND UPDATED CORRECTLY. 
        //BUT: WHEN FluctuationAmount < 0: IT MEANS 'Chiết khấu khác' => THE VALUE MUST BE MINUS TO A PENDING INVOICE VALUE (BECAUSE IT MEANS 'Chiết khấu khác').
        //THE ORIGINAL OF FluctuationAmount IS 'Chênh lệch tỷ giá' => IT MAY BE A + OR - VALUE. WHEN IT < 0 => IT NEVER MINUS TO ANY PENDING INVOICE VALUE (BECAUSE IT MEANS 'Chênh lệch tỷ giá', IT MAY BE + OR -).
        //NOW: WE DON'T CHANGE THE PROGRAMMING TO KEEP THIS ORGINAL RULE FOR FluctuationAmount [NEVER MINUS TO ANY PENDING INVOICE VALUE]. LATER: IF WE REALY NEED A REAL FIELD FOR 'Chênh lệch tỷ giá' => WE CAN USE THIS FluctuationAmount WITH + OR - VALUE. IT WILL PLAY CORRECTLY. (ANE SURELY, WE MUST ADD ANOTHER FIELD FOR "Thu khác")
        //NOW: WE ADD OtherDiscount FOR "CK khác". IT PLAY THE SAME RULE AS CashDiscount
        

        [Display(Name = "Thu BH")]
        public decimal ReceiptAmount { get; set; }

        [GenericCompare(CompareToPropertyName = "AmountDue", OperatorName = GenericCompareOperator.LessThanOrEqual, ErrorMessage = "Số tiền cấn trừ không vượt số lượng phải thu")]
        public decimal ApplyAmount { get { return (this.ReceiptAmount + this.CashDiscount + this.OtherDiscount); } }

        [Display(Name = "Còn lại")]
        [UIHint("DecimalReadonly")] //Remains value after receipt
        public decimal AmountRemains { get; set; }
    }
}