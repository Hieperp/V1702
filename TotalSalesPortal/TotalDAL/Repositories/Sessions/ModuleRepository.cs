using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

using TotalModel.Models;
using TotalCore.Repositories.Sessions;

namespace TotalDAL.Repositories.Sessions
{   
    public class ModuleRepository : IModuleRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public ModuleRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
            this.totalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
        }

        public IQueryable<Module> GetAllModules()
        {
            return this.totalSalesPortalEntities.Modules.Where(w => w.InActive == 0);
        }

        public Module GetModuleByID(int moduleID)
        {
            var module = this.totalSalesPortalEntities.Modules.SingleOrDefault(x => x.ModuleID == moduleID);
            return module;
        }

        //Cai nay su dung tam thoi, cho cai menu ma thoi. Cach lam nay amatuer qua!!!!
        public string GetLocationName(int userID)
        {
            var organizationalUnitUser = this.totalSalesPortalEntities.OrganizationalUnitUsers.Where(w => w.UserID == userID && !w.InActive).Include(i => i.OrganizationalUnit.Location).First();
            return organizationalUnitUser.OrganizationalUnit.Location.OfficialName;
        }
        public int GetLocationID(int userID)
        {
            var organizationalUnitUser = this.totalSalesPortalEntities.OrganizationalUnitUsers.Where(w => w.UserID == userID && !w.InActive).Include(i => i.OrganizationalUnit.Location).First();
            return organizationalUnitUser.OrganizationalUnit.Location.LocationID;
        }






        public void SaveChanges()
        {
            this.totalSalesPortalEntities.SaveChanges();
        }

        public void Add(Module module)
        {
            this.totalSalesPortalEntities.Modules.Add(module);
        }

        public void Delete(Module module)
        {
            this.totalSalesPortalEntities.Modules.Remove(module);
        }
    }
}
