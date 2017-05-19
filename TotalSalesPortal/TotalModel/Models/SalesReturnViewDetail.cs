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
    
    public partial class SalesReturnViewDetail
    {
        public int SalesReturnDetailID { get; set; }
        public int SalesReturnID { get; set; }
        public Nullable<int> GoodsIssueID { get; set; }
        public Nullable<int> GoodsIssueDetailID { get; set; }
        public string GoodsIssueReference { get; set; }
        public Nullable<System.DateTime> GoodsIssueEntryDate { get; set; }
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public int CommodityTypeID { get; set; }
        public int CalculatingTypeID { get; set; }
        public Nullable<decimal> QuantityRemains { get; set; }
        public Nullable<decimal> FreeQuantityRemains { get; set; }
        public decimal Quantity { get; set; }
        public decimal ControlFreeQuantity { get; set; }
        public decimal FreeQuantity { get; set; }
        public decimal ListedPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VATPercent { get; set; }
        public decimal ListedGrossPrice { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal ListedAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal ListedVATAmount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal ListedGrossAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public Nullable<bool> IsBonus { get; set; }
        public bool InActivePartial { get; set; }
        public Nullable<System.DateTime> InActivePartialDate { get; set; }
        public string Remarks { get; set; }
    }
}
