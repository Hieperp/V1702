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
    
    public partial class SalesReturnPendingGoodsIssueDetail
    {
        public int GoodsIssueID { get; set; }
        public int GoodsIssueDetailID { get; set; }
        public string GoodsIssueReference { get; set; }
        public System.DateTime GoodsIssueEntryDate { get; set; }
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public int CommodityTypeID { get; set; }
        public string GoodsIssueWarehouseCode { get; set; }
        public int CalculatingTypeID { get; set; }
        public bool VATbyRow { get; set; }
        public Nullable<decimal> QuantityRemains { get; set; }
        public Nullable<decimal> FreeQuantityRemains { get; set; }
        public int Quantity { get; set; }
        public decimal ControlFreeQuantity { get; set; }
        public int FreeQuantity { get; set; }
        public decimal ListedPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VATPercent { get; set; }
        public decimal ListedGrossPrice { get; set; }
        public decimal GrossPrice { get; set; }
        public int ListedAmount { get; set; }
        public int Amount { get; set; }
        public int ListedVATAmount { get; set; }
        public int VATAmount { get; set; }
        public int ListedGrossAmount { get; set; }
        public int GrossAmount { get; set; }
        public Nullable<bool> IsBonus { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsSelected { get; set; }
        public string DeliveryAdviceReferences { get; set; }
        public decimal TradeDiscountRate { get; set; }
    }
}
