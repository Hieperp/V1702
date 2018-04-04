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
    
    public partial class DeliveryAdvice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryAdvice()
        {
            this.DeliveryAdviceDetails = new HashSet<DeliveryAdviceDetail>();
            this.GoodsIssues = new HashSet<GoodsIssue>();
        }
    
        public int DeliveryAdviceID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Reference { get; set; }
        public Nullable<int> SalesOrderID { get; set; }
        public int CustomerID { get; set; }
        public int ReceiverID { get; set; }
        public int PriceCategoryID { get; set; }
        public Nullable<int> PromotionID { get; set; }
        public string PromotionVouchers { get; set; }
        public int SalespersonID { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string ShippingAddress { get; set; }
        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }
        public int ApproverID { get; set; }
        public int PaymentTermID { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalQuantityIssue { get; set; }
        public decimal TotalFreeQuantity { get; set; }
        public decimal TotalFreeQuantityIssue { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalVATAmount { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public decimal AverageDiscountPercent { get; set; }
        public decimal ReceiptAmount { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
        public bool Approved { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<int> VoidTypeID { get; set; }
        public bool InActive { get; set; }
        public bool InActivePartial { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        public decimal TotalListedAmount { get; set; }
        public decimal TotalListedVATAmount { get; set; }
        public decimal TotalListedGrossAmount { get; set; }
        public decimal VATPercent { get; set; }
        public bool HasSalesOrder { get; set; }
        public string SalesOrderReferences { get; set; }
        public string SalesOrderCodes { get; set; }
        public int WarehouseID { get; set; }
        public bool VATbyRow { get; set; }
        public decimal TradeDiscountRate { get; set; }
        public decimal ListedTradeDiscountAmount { get; set; }
        public decimal TradeDiscountAmount { get; set; }
        public decimal TotalListedTaxableAmount { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public Nullable<int> TradePromotionID { get; set; }
        public string Code { get; set; }
        public string Addressee { get; set; }
        public decimal SumListedGrossAmount { get; set; }
        public decimal SumGrossAmount { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Customer Customer1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryAdviceDetail> DeliveryAdviceDetails { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Location Location { get; set; }
        public virtual PaymentTerm PaymentTerm { get; set; }
        public virtual VoidType VoidType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsIssue> GoodsIssues { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Promotion Promotion { get; set; }
        public virtual Promotion Promotion1 { get; set; }
        public virtual PriceCategory PriceCategory { get; set; }
    }
}
