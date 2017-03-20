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
    
    public partial class CustomerCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomerCategory()
        {
            this.CustomerCategories1 = new HashSet<CustomerCategory>();
            this.Customers = new HashSet<Customer>();
        }
    
        public int CustomerCategoryID { get; set; }
        public string Name { get; set; }
        public Nullable<int> AncestorID { get; set; }
        public string Remarks { get; set; }
        public int PriceCategoryID { get; set; }
        public string Code { get; set; }
        public bool ShowDiscount { get; set; }
        public int PaymentTermID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerCategory> CustomerCategories1 { get; set; }
        public virtual CustomerCategory CustomerCategory1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual PaymentTerm PaymentTerm { get; set; }
    }
}
