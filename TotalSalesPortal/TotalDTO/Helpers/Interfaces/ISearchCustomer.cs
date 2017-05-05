using System.ComponentModel.DataAnnotations;

using TotalDTO.Commons;

namespace TotalDTO.Helpers.Interfaces
{
    public interface ISearchCustomer
    {
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        CustomerBaseDTO Customer { get; set; }

        [Display(Name = "Đơn vị, người nhận hàng")]
        [UIHint("Commons/CustomerBase")]
        CustomerBaseDTO Receiver { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [UIHint("Commons/ShippingAddress")]
        string ShippingAddress { get; set; }
    }
}
