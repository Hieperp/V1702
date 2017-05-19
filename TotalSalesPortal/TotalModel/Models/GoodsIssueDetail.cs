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
    
    public partial class GoodsIssueDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GoodsIssueDetail()
        {
            this.AccountInvoiceDetails = new HashSet<AccountInvoiceDetail>();
            this.HandlingUnitDetails = new HashSet<HandlingUnitDetail>();
            this.SalesReturnDetails = new HashSet<SalesReturnDetail>();
        }
    
        public int GoodsIssueDetailID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public int LocationID { get; set; }
        public int GoodsIssueID { get; set; }
        public Nullable<int> VoidTypeID { get; set; }
        public int CustomerID { get; set; }
        public int ReceiverID { get; set; }
        public int DeliveryAdviceID { get; set; }
        public int DeliveryAdviceDetailID { get; set; }
        public Nullable<int> AccountInvoiceID { get; set; }
        public int CommodityID { get; set; }
        public int CommodityTypeID { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public int StorekeeperID { get; set; }
        public decimal Quantity { get; set; }
        public decimal ControlFreeQuantity { get; set; }
        public decimal FreeQuantity { get; set; }
        public decimal QuantityInvoice { get; set; }
        public decimal FreeQuantityInvoice { get; set; }
        public decimal QuantityHandlingUnit { get; set; }
        public decimal ListedPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VATPercent { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public Nullable<bool> IsBonus { get; set; }
        public string Remarks { get; set; }
        public bool Approved { get; set; }
        public bool InActive { get; set; }
        public bool InActivePartial { get; set; }
        public Nullable<System.DateTime> InActivePartialDate { get; set; }
        public int CalculatingTypeID { get; set; }
        public decimal ListedGrossPrice { get; set; }
        public decimal ListedAmount { get; set; }
        public decimal ListedVATAmount { get; set; }
        public decimal ListedGrossAmount { get; set; }
        public bool VATbyRow { get; set; }
        public decimal QuantityReturned { get; set; }
        public decimal FreeQuantityReturned { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountInvoiceDetail> AccountInvoiceDetails { get; set; }
        public virtual DeliveryAdviceDetail DeliveryAdviceDetail { get; set; }
        public virtual GoodsIssue GoodsIssue { get; set; }
        public virtual VoidType VoidType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HandlingUnitDetail> HandlingUnitDetails { get; set; }
        public virtual Commodity Commodity { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesReturnDetail> SalesReturnDetails { get; set; }
    }
}
