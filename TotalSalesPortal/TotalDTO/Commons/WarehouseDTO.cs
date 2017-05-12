using System;
using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Commons
{
    public interface IWarehouseBaseDTO
    {
        Nullable<int> WarehouseID { get; set; }
        string Code { get; set; }
        [Display(Name = "Lý do hủy hoặc chờ giao")]
        [UIHint("AutoCompletes/WarehouseBase")]
        [Required(ErrorMessage = "Vui lòng nhập lý do hủy hoặc chờ giao")]
        string Name { get; set; }
    }

    public class WarehouseBaseDTO : BaseDTO, IWarehouseBaseDTO
    {
        public Nullable<int> WarehouseID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class WarehouseDTO
    {
    }
}
