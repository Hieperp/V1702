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
    
    public partial class PendingGoodsIssue
    {
        public int GoodsIssueID { get; set; }
        public string GoodsIssueReference { get; set; }
        public System.DateTime GoodsIssueEntryDate { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string GoodsIssueReceiverCode { get; set; }
        public string GoodsIssueReceiverName { get; set; }
        public int CustomerID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerVATCode { get; set; }
        public string CustomerAttentionName { get; set; }
        public string CustomerTelephone { get; set; }
        public string CustomerEntireTerritoryEntireName { get; set; }
        public string CustomerBillingAddress { get; set; }
        public int PaymentTermID { get; set; }
    }
}
