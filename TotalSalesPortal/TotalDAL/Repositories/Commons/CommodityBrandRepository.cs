using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class CommodityBrandAPIRepository : GenericAPIRepository, ICommodityBrandAPIRepository
    {
        public CommodityBrandAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetCommodityBrandIndexes")
        {
        }

        public IList<CommodityBrand> GetAllCommodityBrands()
        {
            return base.TotalSalesPortalEntities.CommodityBrands.ToList();
        }

    }
}
