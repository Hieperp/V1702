using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Accounts;
using TotalDAL.Helpers;

namespace TotalDAL.Repositories.Accounts
{
    public class ReceiptRepository : GenericWithDetailRepository<Receipt, ReceiptDetail>, IReceiptRepository
    {
        public ReceiptRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "ReceiptEditable")
        {
        }

        
    }


    public class ReceiptAPIRepository : GenericAPIRepository, IReceiptAPIRepository
    {
        public ReceiptAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetReceiptIndexes")
        {
        }

        public ICollection<GoodsIssueReceivable> GetGoodsIssueReceivables(int locationID)
        {
            return this.TotalSalesPortalEntities.GetGoodsIssueReceivables(locationID).ToList();
        }

        public ICollection<CustomerReceivable> GetCustomerReceivables(int locationID)
        {
            return this.TotalSalesPortalEntities.GetCustomerReceivables(locationID).ToList();
        }

        public ICollection<PendingCustomerCredit> GetPendingCustomerCredits(int locationID, int customerID)
        {
            return this.TotalSalesPortalEntities.GetPendingCustomerCredits(locationID, customerID).ToList();
        }        
    }
}
