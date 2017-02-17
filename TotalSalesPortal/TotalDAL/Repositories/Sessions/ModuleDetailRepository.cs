using TotalCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TotalModel.Models;
using TotalCore.Repositories.Sessions;

namespace TotalDAL.Repositories.Sessions
{    
    public class ModuleDetailRepository : IModuleDetailRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public ModuleDetailRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public IQueryable<ModuleDetail> GetAllModuleDetails()
        {
            return this.totalSalesPortalEntities.ModuleDetails;
        }

        public IQueryable<ModuleDetail> GetModuleDetailByModuleID(int moduleID)
        {
            return this.totalSalesPortalEntities.ModuleDetails.Where(x => x.ModuleID == moduleID && x.InActive == 0);
        }

        public ModuleDetail GetModuleDetailByID(int taskID)
        {
            return this.totalSalesPortalEntities.ModuleDetails.SingleOrDefault(x => x.TaskID == taskID);
        }

        public void AddModuleDetail(ModuleDetail moduleDetail)
        {
            this.totalSalesPortalEntities.ModuleDetails.Add(moduleDetail);
        }

        public void Add(ModuleDetail moduleDetail)
        {
            this.totalSalesPortalEntities.ModuleDetails.Add(moduleDetail);
        }

        public void Remove(ModuleDetail moduleDetail)
        {
            this.totalSalesPortalEntities.ModuleDetails.Remove(moduleDetail);
        }

        public void SaveChanges()
        {
            this.totalSalesPortalEntities.SaveChanges();
        }

    }
}
