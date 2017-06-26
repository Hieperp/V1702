using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class PriceCategoryRepository : IPriceCategoryRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public PriceCategoryRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public IList<PriceCategory> GetAllPriceCategories()
        {
            return this.totalSalesPortalEntities.PriceCategories.ToList();
        }
    }
}

