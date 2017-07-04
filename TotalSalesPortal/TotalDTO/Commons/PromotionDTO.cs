using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;
using TotalDTO.Helpers.Interfaces;

namespace TotalDTO.Commons
{
    public class PromotionPrimitiveDTO : BaseWithDetailDTO<PromotionCommodityCodePartDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Promotion; } }

        public int GetID() { return (int)this.PromotionID; }
        public void SetID(int id) { this.PromotionID = id; }

        public Nullable<int> PromotionID { get; set; }

        [Display(Name = "Mã chương trình")]
        public string Code { get; set; }
        [Display(Name = "Tên chương trình khuyến mãi")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Nhãn hàng")]
        public int CommodityBrandID { get; set; }
        [Display(Name = "Nhãn hàng")]
        public string CommodityBrandName { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public Nullable<System.DateTime> StartDate { get; set; }
        [Display(Name = "Ngày kết thúc")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Display(Name = "Tỷ lệ chiết khấu")]
        public Nullable<decimal> DiscountPercent { get; set; }
        [Display(Name = "Số lượng mua hàng được tặng sản phẩm cùng loại")]
        public Nullable<decimal> ControlFreeQuantity { get; set; }

        public bool ApplyToAllCustomers { get; set; }
        public bool ApplyToAllCommodities { get; set; }
        public bool ApplyToTradeDiscount { get; set; }

        public override int PreparedPersonID { get { return 1; } }

        
        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.DtoDetails().ToList().ForEach(e => { e.CommodityBrandID = this.CommodityBrandID; });
        }
    }


    public class PromotionDTO : PromotionPrimitiveDTO, IBaseDetailEntity<PromotionCommodityCodePartDTO>
    {
        public PromotionDTO()
        {
            this.PromotionCommodityCodeParts = new List<PromotionCommodityCodePartDTO>();
        }

        public List<PromotionCommodityCodePartDTO> PromotionCommodityCodeParts { get; set; }
        public List<PromotionCommodityCodePartDTO> ViewDetails { get { return this.PromotionCommodityCodeParts; } set { this.PromotionCommodityCodeParts = value; } }

        public ICollection<PromotionCommodityCodePartDTO> GetDetails() { return this.PromotionCommodityCodeParts; }

        protected override IEnumerable<PromotionCommodityCodePartDTO> DtoDetails() { return this.PromotionCommodityCodeParts; }
    }


}
