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
    
    public partial class PendingDeliveryAdvice
    {
        public int DeliveryAdviceID { get; set; }
        public string DeliveryAdviceReference { get; set; }
        public System.DateTime DeliveryAdviceEntryDate { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAttentionName { get; set; }
        public string CustomerTelephone { get; set; }
        public string CustomerEntireTerritoryEntireName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerVATCode { get; set; }
        public int ReceiverID { get; set; }
        public string ReceiverCode { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverVATCode { get; set; }
        public string ReceiverAttentionName { get; set; }
        public string ReceiverTelephone { get; set; }
        public string ReceiverEntireTerritoryEntireName { get; set; }
        public string CustomerBillingAddress { get; set; }
        public string ReceiverBillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public int PaymentTermID { get; set; }
    }
}
