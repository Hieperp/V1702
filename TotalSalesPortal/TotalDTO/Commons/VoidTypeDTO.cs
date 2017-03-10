using System;
using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Commons
{
    public interface IVoidTypeBaseDTO
    {
        Nullable<int> VoidTypeID { get; set; }
        string Code { get; set; }
        [Display(Name = "Lý do hủy hoặc chờ giao")]
        [UIHint("AutoCompletes/VoidTypeBase")]
        [Required(ErrorMessage = "Vui lòng nhập lý do hủy hoặc chờ giao")]
        string Name { get; set; }
        Nullable<int> VoidClassID { get; set; }
    }

    public class VoidTypeBaseDTO : BaseDTO, IVoidTypeBaseDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<int> VoidClassID { get; set; }
    }

    public class VoidTypeDTO
    {
    }
}
