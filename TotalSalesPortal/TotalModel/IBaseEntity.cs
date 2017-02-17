
namespace TotalModel
{
    public interface IBaseEntity : IAccessControlAttribute
    {
        System.DateTime CreatedDate { get; set; }
        System.DateTime EditedDate { get; set; }
    }
}
