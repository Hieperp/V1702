using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalBase.Enums;
using TotalDTO.Helpers.Interfaces;


namespace TotalDTO.Commons
{
    public class PromotionPrimitiveDTO : BaseWithDetailDTO<PromotionCommodityCodePartDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Promotion; } }

        public int GetID() { return this.PromotionID; }
        public void SetID(int id) { this.PromotionID = id; }

        public int PromotionID { get; set; }

        [Display(Name = "Mã chương trình")]
        [Required(ErrorMessage = "Vui lòng nhập mã chương trình")]
        public string Code { get; set; }

        [Display(Name = "Diễn giải")]
        public string Name { get; set; }

        [Display(Name = "Nhãn hàng")]
        [Required(ErrorMessage = "Vui lòng nhập nhãn hàng")]
        public int CommodityBrandID { get; set; }
        [Display(Name = "Nhãn hàng")]
        [Required(ErrorMessage = "Vui lòng nhập nhãn hàng")]
        public string CommodityBrandName { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        [Required(ErrorMessage = "Vui lòng nhập ngày bắt đầu")]
        public Nullable<System.DateTime> StartDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        [Required(ErrorMessage = "Vui lòng nhập ngày kết thúc")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Display(Name = "Tỷ lệ % chiết khấu")]
        public decimal DiscountPercent { get; set; }

        [Display(Name = "SL mua/ 1 quà tặng")]
        [Range(0, 999999, ErrorMessage = "Số lượng mua hàng phải >= 0")]
        public decimal ControlFreeQuantity { get; set; }

        [Display(Name = "Áp dụng cho bán hàng")]
        public bool ApplyToSales { get; set; }
        [Display(Name = "Áp dụng cho hàng trả lại")]
        public bool ApplyToReturns { get; set; }

        public bool ApplyToAllCustomers { get; set; }
        [Display(Name = "Áp dụng tất cả mặt hàng")]
        public bool ApplyToAllCommodities { get; set; }
        [Display(Name = "Chiết khấu tổng (chiết khấu 1 dòng)")]
        public bool ApplyToTradeDiscount { get; set; }

        public override int PreparedPersonID { get { return 1; } }

        public string Specs { get { return this.Code + " [" + this.DiscountPercent.ToString("N1") + "%" + (this.ControlFreeQuantity > 0 ? ", " + this.ControlFreeQuantity.ToString("N0") + "/1QT" : "") + "] "; } }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }
            if (this.EndDate <= DateTime.Now.AddHours(1)) yield return new ValidationResult("Ngày giờ kết thúc phải còn ít nhất 60 phút kể từ bây giờ");
            if (!(this.DiscountPercent == -1 || this.DiscountPercent >= 0)) yield return new ValidationResult("Tỷ lệ chiết khấu >= 0 hoặc = -1", new[] { "DiscountPercent" });
        }

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





    public class CommodityCodePartABC
    {
        [Display(Name = "Mã sản phẩm")]
        [UIHint("AutoCompletes/CodePart")]
        public CodePartDTO CodePartA { get; set; }

        [Display(Name = "Mã bông")]
        [UIHint("AutoCompletes/CodePart")]
        public CodePartDTO CodePartB { get; set; }

        [Display(Name = "Nguyên liệu")]
        [UIHint("AutoCompletes/CodePart")]
        public CodePartDTO CodePartC { get; set; }
    }

    public class PromotionBaseDTO : BaseDTO
    {
        public Nullable<int> PromotionID { get; set; }
        [Display(Name = "Chương trình khuyến mãi")]
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        [Display(Name = "Ngày kết thúc")]
        public Nullable<System.DateTime> EndDate { get; set; }
        [Display(Name = "Tỷ lệ chiết khấu")]
        public Nullable<decimal> DiscountPercent { get; set; }
        [Display(Name = "Tặng quà")]
        public Nullable<decimal> ControlFreeQuantity { get; set; }
        public bool ApplyToAllCustomers { get; set; }
        public bool ApplyToAllCommodities { get; set; }
    }
}
