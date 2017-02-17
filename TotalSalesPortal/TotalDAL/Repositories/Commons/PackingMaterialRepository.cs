using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class PackingMaterialRepository : IPackingMaterialRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public PackingMaterialRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public IList<PackingMaterial> GetAllPackingMaterials()
        {
            return this.totalSalesPortalEntities.PackingMaterials.ToList();
        }
    }
}

