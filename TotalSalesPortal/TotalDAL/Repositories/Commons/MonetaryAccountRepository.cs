using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class MonetaryAccountRepository : IMonetaryAccountRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public MonetaryAccountRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public IList<MonetaryAccount> GetAllMonetaryAccounts()
        {
            return this.totalSalesPortalEntities.MonetaryAccounts.ToList();
        }
    }
}

