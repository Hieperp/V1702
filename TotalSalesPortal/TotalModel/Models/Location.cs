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
    
    public partial class Location
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            this.OrganizationalUnits = new HashSet<OrganizationalUnit>();
            this.Employees = new HashSet<Employee>();
            this.AccountInvoices = new HashSet<AccountInvoice>();
            this.GoodsDeliveries = new HashSet<GoodsDelivery>();
            this.DeliveryAdvices = new HashSet<DeliveryAdvice>();
            this.HandlingUnits = new HashSet<HandlingUnit>();
            this.SalesOrders = new HashSet<SalesOrder>();
            this.Warehouses = new HashSet<Warehouse>();
            this.SalesReturns = new HashSet<SalesReturn>();
            this.CreditNotes = new HashSet<CreditNote>();
        }
    
        public int LocationID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string OfficialName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Facsimile { get; set; }
        public string Remarks { get; set; }
        public System.DateTime LockedDate { get; set; }
        public string AspUserID { get; set; }
        public System.DateTime EditedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationalUnit> OrganizationalUnits { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountInvoice> AccountInvoices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsDelivery> GoodsDeliveries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryAdvice> DeliveryAdvices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HandlingUnit> HandlingUnits { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Warehouse> Warehouses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesReturn> SalesReturns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CreditNote> CreditNotes { get; set; }
    }
}
