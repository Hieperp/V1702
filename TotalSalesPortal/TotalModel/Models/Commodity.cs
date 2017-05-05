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
    
    public partial class Commodity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Commodity()
        {
            this.AccountInvoiceDetails = new HashSet<AccountInvoiceDetail>();
            this.DeliveryAdviceDetails = new HashSet<DeliveryAdviceDetail>();
            this.GoodsIssueDetails = new HashSet<GoodsIssueDetail>();
            this.SalesOrderDetails = new HashSet<SalesOrderDetail>();
        }
    
        public int CommodityID { get; set; }
        public string Code { get; set; }
        public string OfficialCode { get; set; }
        public string CodePartA { get; set; }
        public string CodePartB { get; set; }
        public string CodePartC { get; set; }
        public string CodePartD { get; set; }
        public string Name { get; set; }
        public string OfficialName { get; set; }
        public string OriginalName { get; set; }
        public Nullable<int> PreviousCommodityID { get; set; }
        public int CommodityCategoryID { get; set; }
        public int CommodityTypeID { get; set; }
        public int SupplierID { get; set; }
        public Nullable<int> PiecePerPack { get; set; }
        public Nullable<int> QuantityAlert { get; set; }
        public decimal ListedPrice { get; set; }
        public decimal GrossPrice { get; set; }
        public string PurchaseUnit { get; set; }
        public string SalesUnit { get; set; }
        public string Packing { get; set; }
        public string Origin { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<double> LeadTime { get; set; }
        public string HSCode { get; set; }
        public bool IsRegularCheckUps { get; set; }
        public Nullable<bool> Discontinue { get; set; }
        public string Specifycation { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> InActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountInvoiceDetail> AccountInvoiceDetails { get; set; }
        public virtual CommodityCategory CommodityCategory { get; set; }
        public virtual CommodityType CommodityType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryAdviceDetail> DeliveryAdviceDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsIssueDetail> GoodsIssueDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
    }
}
