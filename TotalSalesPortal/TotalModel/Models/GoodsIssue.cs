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
    
    public partial class GoodsIssue
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GoodsIssue()
        {
            this.AccountInvoices = new HashSet<AccountInvoice>();
            this.GoodsIssueDetails = new HashSet<GoodsIssueDetail>();
            this.HandlingUnits = new HashSet<HandlingUnit>();
            this.ReceiptDetails = new HashSet<ReceiptDetail>();
            this.Receipts = new HashSet<Receipt>();
        }
    
        public int GoodsIssueID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Reference { get; set; }
        public Nullable<int> DeliveryAdviceID { get; set; }
        public int CustomerID { get; set; }
        public int ReceiverID { get; set; }
        public string ShippingAddress { get; set; }
        public int StorekeeperID { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }
        public int ApproverID { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalFreeQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalVATAmount { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public decimal AverageDiscountPercent { get; set; }
        public decimal TotalReceiptAmount { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
        public bool Approved { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public bool InActive { get; set; }
        public bool InActivePartial { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        public decimal TotalListedAmount { get; set; }
        public decimal TotalListedVATAmount { get; set; }
        public decimal TotalListedGrossAmount { get; set; }
        public string DeliveryAdviceReferences { get; set; }
        public int PaymentTermID { get; set; }
        public decimal TotalCashDiscount { get; set; }
        public decimal VATPercent { get; set; }
        public int WarehouseID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountInvoice> AccountInvoices { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Customer Customer1 { get; set; }
        public virtual DeliveryAdvice DeliveryAdvice { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsIssueDetail> GoodsIssueDetails { get; set; }
        public virtual Employee Employee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HandlingUnit> HandlingUnits { get; set; }
        public virtual PaymentTerm PaymentTerm { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
