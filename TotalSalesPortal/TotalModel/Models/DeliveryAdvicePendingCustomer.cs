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
    
    public partial class DeliveryAdvicePendingCustomer
    {
        public int CustomerID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerVATCode { get; set; }
        public string CustomerAttentionName { get; set; }
        public string CustomerTelephone { get; set; }
        public string CustomerBillingAddress { get; set; }
        public string CustomerEntireTerritoryEntireName { get; set; }
        public int ReceiverID { get; set; }
        public string ReceiverCode { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverVATCode { get; set; }
        public string ReceiverAttentionName { get; set; }
        public string ReceiverTelephone { get; set; }
        public string ReceiverBillingAddress { get; set; }
        public string ReceiverEntireTerritoryEntireName { get; set; }
        public string ShippingAddress { get; set; }
        public int PaymentTermID { get; set; }
        public int SalespersonID { get; set; }
        public string SalespersonName { get; set; }
        public int PriceCategoryID { get; set; }
        public string PriceCategoryCode { get; set; }
        public decimal VATPercent { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string CustomerOfficialName { get; set; }
        public Nullable<System.DateTime> CustomerBirthday { get; set; }
        public string CustomerShippingAddress { get; set; }
        public int CustomerTerritoryID { get; set; }
        public int CustomerSalespersonID { get; set; }
        public string CustomerSalespersonName { get; set; }
        public int CustomerPriceCategoryID { get; set; }
        public string CustomerPriceCategoryCode { get; set; }
        public string ReceiverOfficialName { get; set; }
        public Nullable<System.DateTime> ReceiverBirthday { get; set; }
        public string ReceiverShippingAddress { get; set; }
        public int ReceiverTerritoryID { get; set; }
        public int ReceiverSalespersonID { get; set; }
        public string ReceiverSalespersonName { get; set; }
        public int ReceiverPriceCategoryID { get; set; }
        public string ReceiverPriceCategoryCode { get; set; }
    }
}
