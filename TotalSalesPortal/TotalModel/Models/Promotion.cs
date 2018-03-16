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
    
    public partial class Promotion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Promotion()
        {
            this.DeliveryAdvices = new HashSet<DeliveryAdvice>();
            this.SalesOrders = new HashSet<SalesOrder>();
            this.SalesReturns = new HashSet<SalesReturn>();
            this.PromotionCommodityCodeParts = new HashSet<PromotionCommodityCodePart>();
            this.DeliveryAdvices1 = new HashSet<DeliveryAdvice>();
            this.SalesOrders1 = new HashSet<SalesOrder>();
            this.SalesReturns1 = new HashSet<SalesReturn>();
            this.GoodsIssues = new HashSet<GoodsIssue>();
            this.AccountInvoices = new HashSet<AccountInvoice>();
        }
    
        public int PromotionID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Reference { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal ControlFreeQuantity { get; set; }
        public int LocationID { get; set; }
        public int CommodityBrandID { get; set; }
        public bool ApplyToAllCustomers { get; set; }
        public bool ApplyToAllCommodities { get; set; }
        public bool ApplyToTradeDiscount { get; set; }
        public string Remarks { get; set; }
        public bool InActive { get; set; }
        public bool Approved { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        public int UserID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public string Specs { get; set; }
    
        public virtual CommodityBrand CommodityBrand { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryAdvice> DeliveryAdvices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesReturn> SalesReturns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PromotionCommodityCodePart> PromotionCommodityCodeParts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryAdvice> DeliveryAdvices1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesOrder> SalesOrders1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesReturn> SalesReturns1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsIssue> GoodsIssues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountInvoice> AccountInvoices { get; set; }
    }
}
