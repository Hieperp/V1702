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
    
    public partial class PromotionCommodityCodePart
    {
        public int PromotionCommodityCodePartID { get; set; }
        public int PromotionID { get; set; }
        public int CommodityBrandID { get; set; }
        public string CodePartA { get; set; }
        public string CodePartB { get; set; }
        public string CodePartC { get; set; }
        public bool InActive { get; set; }
    
        public virtual Promotion Promotion { get; set; }
    }
}
