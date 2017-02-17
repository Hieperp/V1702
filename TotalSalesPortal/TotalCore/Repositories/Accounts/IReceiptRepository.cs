using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Accounts
{
    public interface IReceiptRepository : IGenericWithDetailRepository<Receipt, ReceiptDetail>
    {
    }

    public interface IReceiptAPIRepository : IGenericAPIRepository
    {
        ICollection<GoodsIssueReceivable> GetGoodsIssueReceivables(int locationID, int? receiptID, string goodsIssueReference);
        ICollection<CustomerReceivable> GetCustomerReceivables(int locationID, int? receiptID, string customerName);
    }
}
