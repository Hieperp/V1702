using System.Linq;

using TotalModel.Models;

namespace TotalCore.Repositories.Sessions
{
    public interface IModuleDetailRepository
    {
        IQueryable<ModuleDetail> GetAllModuleDetails();
        IQueryable<ModuleDetail> GetModuleDetailByModuleID(int moduleID);
        ModuleDetail GetModuleDetailByID(int moduleDetailID);

        void AddModuleDetail(ModuleDetail moduleDetail);

        void Add(ModuleDetail moduleDetail);

        void Remove(ModuleDetail recordToDelete);
        void SaveChanges();
    }
}
