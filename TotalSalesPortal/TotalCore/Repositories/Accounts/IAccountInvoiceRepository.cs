using System;
using System.Linq;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalModel.Models;


namespace TotalCore.Repositories.Accounts
{
    public interface IAccountInvoiceRepository : IGenericWithDetailRepository<AccountInvoice, AccountInvoiceDetail>
    {
    }

    public interface IAccountInvoiceAPIRepository : IGenericAPIRepository
    {
        IEnumerable<PendingGoodsIssueConsumer> GetConsumers(int? locationID);
        IEnumerable<PendingGoodsIssueReceiver> GetReceivers(int? locationID);
        IEnumerable<PendingGoodsIssue> GetGoodsIssues(int? locationID);

        IEnumerable<PendingGoodsIssueDetail> GetPendingGoodsIssueDetails(int? accountInvoiceID, int? goodsIssueID, int? customerID, int? receiverID, int? tradePromotionID, decimal? vatPercent, int? commodityTypeID, string aspUserID, int? locationID, DateTime fromDate, DateTime toDate, string goodsIssueDetailIDs, bool isReadonly);
    }
}
