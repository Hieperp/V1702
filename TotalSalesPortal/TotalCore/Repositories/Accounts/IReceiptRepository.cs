using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Accounts
{
    public interface IReceiptRepository : IGenericWithDetailRepository<Receipt, ReceiptDetail>
    {
    }

    public interface IReceiptAPIRepository : IGenericAPIRepository
    {
        ICollection<GoodsIssueReceivable> GetGoodsIssueReceivables(int locationID);
        ICollection<CustomerReceivable> GetCustomerReceivables(int locationID);
        ICollection<PendingCustomerCredit> GetPendingCustomerCredits(int locationID, int customerID);
    }
}
