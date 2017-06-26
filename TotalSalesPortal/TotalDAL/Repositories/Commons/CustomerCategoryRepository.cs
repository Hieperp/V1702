using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class CustomerCategoryRepository : ICustomerCategoryRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public CustomerCategoryRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public IList<CustomerCategory> GetAllCustomerCategories()
        {
            return this.totalSalesPortalEntities.CustomerCategories.ToList();
        }
    }
}

