using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;


using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Accounts;



namespace TotalDAL.Repositories.Accounts
{
    public class AccountInvoiceRepository : GenericWithDetailRepository<AccountInvoice, AccountInvoiceDetail>, IAccountInvoiceRepository
    {
        public AccountInvoiceRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities) { }
    }


    public class AccountInvoiceAPIRepository : GenericAPIRepository, IAccountInvoiceAPIRepository
    {
        public AccountInvoiceAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetAccountInvoiceIndexes")
        {
        }

        public IEnumerable<PendingGoodsIssue> GetGoodsIssues(int? locationID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsIssue> pendingGoodsIssues = base.TotalSalesPortalEntities.GetPendingGoodsIssues(locationID).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssues;
        }

        public IEnumerable<PendingGoodsIssueConsumer> GetConsumers(int? locationID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsIssueConsumer> pendingGoodsIssueConsumers = base.TotalSalesPortalEntities.GetPendingGoodsIssueConsumers(locationID).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueConsumers;
        }

        public IEnumerable<PendingGoodsIssueReceiver> GetReceivers(int? locationID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsIssueReceiver> pendingGoodsIssueReceivers = base.TotalSalesPortalEntities.GetPendingGoodsIssueReceivers(locationID).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueReceivers;
        }





        public IEnumerable<PendingGoodsIssueDetail> GetPendingGoodsIssueDetails(int? accountInvoiceID, int? goodsIssueID, int? customerID, int? receiverID, int? commodityTypeID, string aspUserID, int? locationID, DateTime fromDate, DateTime toDate, string goodsIssueDetailIDs, bool isReadonly)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsIssueDetail> pendingGoodsIssueDetails = base.TotalSalesPortalEntities.GetPendingGoodsIssueDetails(accountInvoiceID, locationID, goodsIssueID, customerID, receiverID, commodityTypeID, aspUserID, fromDate, toDate, goodsIssueDetailIDs, isReadonly).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueDetails;
        }
    }

}
