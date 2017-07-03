using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class CommodityPriceRepository : GenericRepository<CommodityPrice>, ICommodityPriceRepository
    {
        public CommodityPriceRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities)
        {
        }
    }


    public class CommodityPriceAPIRepository : GenericAPIRepository, ICommodityPriceAPIRepository
    {
        public CommodityPriceAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetCommodityPriceIndexes")
        {
        }
    }
}