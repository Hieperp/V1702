using System.ComponentModel.DataAnnotations;

using TotalModel;

namespace TotalDTO.Helpers
{
    public interface IQuantityDetailDTO : IBaseModel
    {
        int CommodityID { get; set; }
        string CommodityCode { get; set; }
        string CommodityName { get; set; }
        int CommodityTypeID { get; set; }

        decimal Quantity { get; set; }
    }

    public abstract class QuantityDetailDTO : BaseModel, IQuantityDetailDTO, IBaseModel
    {
        public virtual int CommodityID { get; set; }
        
        //[UIHint("StringReadonly")]  Note: Must set later for any derived class, event for readonly attribute. Don't know why can not override this attribute when needed only
        [Display(Name = "Mặt hàng")]
        [Required(ErrorMessage = "Vui lòng chọn mặt hàng")]        
        public virtual string CommodityCode { get; set; }

        [Display(Name = "Mã hàng")]
        [UIHint("StringReadonly")]
        public virtual string CommodityName { get; set; }

        [Range(1, 99999999999, ErrorMessage = "Lỗi bắt buộc phải có id loại hàng hóa")]
        [Required(ErrorMessage = "Lỗi bắt buộc phải có loại hàng hóa")]
        public virtual int CommodityTypeID { get; set; }


        [Display(Name = "SL")]        
        [Range(0, 99999999999, ErrorMessage = "Số lượng không hợp lệ")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public virtual decimal Quantity { get; set; }
    }
}
