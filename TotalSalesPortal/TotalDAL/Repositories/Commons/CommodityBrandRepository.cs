using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class CommodityBrandRepository : ICommodityBrandRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public CommodityBrandRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public IList<CommodityBrand> GetAllCommodityBrands()
        {
            return this.totalSalesPortalEntities.CommodityBrands.ToList();
        }

    }
}
