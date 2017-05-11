using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Helpers.Interfaces
{
    public interface IWarehouse
    {
        [Display(Name = "Bảng giá")]
        int WarehouseID { get; set; }
        [Display(Name = "Bảng giá")]
        string WarehouseCode { get; set; }
    }
}