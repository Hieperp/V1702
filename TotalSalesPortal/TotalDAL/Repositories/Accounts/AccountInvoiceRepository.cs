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

        public IEnumerable<PendingGoodsIssueConsumer> GetConsumers(int? locationID, int? accountInvoiceID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsIssueConsumer> pendingGoodsIssueConsumers = base.TotalSalesPortalEntities.GetPendingGoodsIssueConsumers(locationID, accountInvoiceID).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueConsumers;
        }

        public IEnumerable<PendingGoodsIssue> GetGoodsIssues(int? locationID, int? accountInvoiceID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsIssue> pendingGoodsIssues = base.TotalSalesPortalEntities.GetPendingGoodsIssues(locationID, accountInvoiceID).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssues;
        }


        public IEnumerable<PendingGoodsIssueDetail> GetPendingGoodsIssueDetails(int? accountInvoiceID, int? goodsIssueID, int? customerID, int? commodityTypeID, string aspUserID, int? locationID, DateTime fromDate, DateTime toDate, string goodsIssueDetailIDs, bool isReadonly)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsIssueDetail> pendingGoodsIssueDetails = base.TotalSalesPortalEntities.GetPendingGoodsIssueDetails(accountInvoiceID, locationID, goodsIssueID, customerID, commodityTypeID, aspUserID, fromDate, toDate, goodsIssueDetailIDs, isReadonly).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueDetails;
        }
    }

}
