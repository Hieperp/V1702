using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Sales;


namespace TotalDAL.Repositories.Sales
{
    public class SalesReturnRepository : GenericWithDetailRepository<SalesReturn, SalesReturnDetail>, ISalesReturnRepository
    {
        public SalesReturnRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "SalesReturnEditable", "SalesReturnApproved", null, "SalesReturnVoidable")
        {
        }
    }








    public class SalesReturnAPIRepository : GenericAPIRepository, ISalesReturnAPIRepository
    {
        public SalesReturnAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetSalesReturnIndexes")
        {
        }

        public IEnumerable<SalesReturnPendingGoodsIssue> GetGoodsIssues(int? locationID, int? customerID, int? receiverID, DateTime? fromDate, DateTime? toDate)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<SalesReturnPendingGoodsIssue> pendingGoodsIssues = base.TotalSalesPortalEntities.GetSalesReturnPendingGoodsIssues(locationID, customerID, receiverID, fromDate, toDate).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssues;
        }

        public IEnumerable<SalesReturnPendingGoodsIssueDetail> GetPendingGoodsIssueDetails(int? locationID, int? salesReturnID, int? goodsIssueID, int? customerID, int? receiverID, Nullable<decimal> vATPercent, DateTime? fromDate, DateTime? toDate, string goodsIssueDetailIDs, bool isReadonly)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<SalesReturnPendingGoodsIssueDetail> pendingGoodsIssueDetails = base.TotalSalesPortalEntities.GetSalesReturnPendingGoodsIssueDetails(locationID, salesReturnID, goodsIssueID, customerID, receiverID, vATPercent, fromDate, toDate, goodsIssueDetailIDs, isReadonly).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueDetails;
        }
    }


}
