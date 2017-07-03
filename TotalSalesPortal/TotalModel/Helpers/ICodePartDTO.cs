using System.ComponentModel.DataAnnotations;

namespace TotalModel.Helpers
{
    public interface ICodePartDTO
    {
        string CodePart { get; set; }
    }


    public class CodePartDTO : ICodePartDTO
    {
        [Display(Name = "Mã sản phẩm")]
        public string CodePart { get; set; }
    }
}
