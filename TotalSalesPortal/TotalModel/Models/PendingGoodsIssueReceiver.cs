//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TotalModel.Models
{
    using System;
    
    public partial class PendingGoodsIssueReceiver
    {
        public int ReceiverID { get; set; }
        public string ReceiverCode { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverVATCode { get; set; }
        public string ReceiverAttentionName { get; set; }
        public string ReceiverTelephone { get; set; }
        public string ReceiverBillingAddress { get; set; }
        public string ReceiverEntireTerritoryEntireName { get; set; }
        public int PaymentTermID { get; set; }
        public decimal VATPercent { get; set; }
        public int CustomerID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public Nullable<int> TradePromotionID { get; set; }
        public string TradePromotionSpecs { get; set; }
        public decimal TradeDiscountRate { get; set; }
    }
}
