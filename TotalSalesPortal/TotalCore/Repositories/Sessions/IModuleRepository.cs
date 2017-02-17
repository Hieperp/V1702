using System.Linq;

using TotalModel.Models;

namespace TotalCore.Repositories.Sessions
{
    public interface IModuleRepository
    {
        IQueryable<Module> GetAllModules();

        Module GetModuleByID(int moduleID);
        string GetLocationName(int userID);
        int GetLocationID(int userID);

        void SaveChanges();
        void Add(Module module);
        void Delete(Module module);
    }
}
