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
    
    public partial class HandlingUnitIndex
    {
        public int HandlingUnitID { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public string Reference { get; set; }
        public string LocationCode { get; set; }
        public string Description { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal RealWeight { get; set; }
        public string GoodsIssueReferences { get; set; }
        public string CustomerName { get; set; }
        public string ReceiverName { get; set; }
        public string ShippingAddress { get; set; }
        public int Identification { get; set; }
        public int CountIdentification { get; set; }
        public string PackingMaterialCode { get; set; }
        public decimal WeightDifference { get; set; }
        public int LotNo { get; set; }
        public Nullable<int> GoodsIssueID { get; set; }
        public string Addressee { get; set; }
        public string Code { get; set; }
    }
}
