using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class TerritoryRepository : ITerritoryRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public TerritoryRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public IList<Territory> GetAllTerritories()
        {
            return this.totalSalesPortalEntities.Territories.ToList();
        }
    }
}

