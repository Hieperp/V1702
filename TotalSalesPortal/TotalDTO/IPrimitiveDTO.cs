using TotalBase.Enums;

namespace TotalDTO
{
    public interface IPrimitiveDTO
    {
        GlobalEnums.NmvnTaskID NMVNTaskID { get; }
        void SetID(int id);
    }
}
