using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class CustomerTypeRepository : ICustomerTypeRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public CustomerTypeRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public IList<CustomerType> GetAllCustomerTypes()
        {
            return this.totalSalesPortalEntities.CustomerTypes.ToList();
        }
    }
}

