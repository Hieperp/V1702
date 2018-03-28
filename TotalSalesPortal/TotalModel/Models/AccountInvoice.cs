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
    using System.Collections.Generic;
    
    public partial class AccountInvoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccountInvoice()
        {
            this.AccountInvoiceDetails = new HashSet<AccountInvoiceDetail>();
        }
    
        public int AccountInvoiceID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Reference { get; set; }
        public string Code { get; set; }
        public string VATInvoiceNo { get; set; }
        public System.DateTime VATInvoiceDate { get; set; }
        public string VATInvoiceSeries { get; set; }
        public string CustomerPO { get; set; }
        public int CustomerID { get; set; }
        public Nullable<int> ConsumerID { get; set; }
        public Nullable<int> ReceiverID { get; set; }
        public Nullable<int> GoodsIssueID { get; set; }
        public int GoodsIssueFirstID { get; set; }
        public string GoodsIssueReferences { get; set; }
        public Nullable<int> TradePromotionID { get; set; }
        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }
        public int ApproverID { get; set; }
        public int PaymentTermID { get; set; }
        public bool VATbyRow { get; set; }
        public decimal VATPercent { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalFreeQuantity { get; set; }
        public decimal TotalListedAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TradeDiscountRate { get; set; }
        public decimal ListedTradeDiscountAmount { get; set; }
        public decimal TradeDiscountAmount { get; set; }
        public decimal TotalListedTaxableAmount { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public decimal TotalListedVATAmount { get; set; }
        public decimal TotalVATAmount { get; set; }
        public decimal TotalListedGrossAmount { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public decimal AverageDiscountPercent { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
        public bool Approved { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public bool InActive { get; set; }
        public bool InActivePartial { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        public decimal SumListedGrossAmount { get; set; }
        public decimal SumGrossAmount { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountInvoiceDetail> AccountInvoiceDetails { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Customer Customer1 { get; set; }
        public virtual Customer Customer2 { get; set; }
        public virtual GoodsIssue GoodsIssue { get; set; }
        public virtual Location Location { get; set; }
        public virtual PaymentTerm PaymentTerm { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}
