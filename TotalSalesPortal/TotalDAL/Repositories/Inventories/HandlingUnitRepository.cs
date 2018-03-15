using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Inventories;
using TotalDAL.Helpers;

namespace TotalDAL.Repositories.Inventories
{
    public class HandlingUnitRepository : GenericWithDetailRepository<HandlingUnit, HandlingUnitDetail>, IHandlingUnitRepository
    {
        public HandlingUnitRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "HandlingUnitEditable") { }
    }


    public class HandlingUnitAPIRepository : GenericAPIRepository, IHandlingUnitAPIRepository
    {
        public HandlingUnitAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetHandlingUnitIndexes")
        {
        }

        public IEnumerable<HandlingUnitPendingCustomer> GetCustomers(int? locationID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<HandlingUnitPendingCustomer> pendingGoodsIssueCustomers = base.TotalSalesPortalEntities.GetHandlingUnitPendingCustomers(locationID).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueCustomers;
        }

        public IEnumerable<HandlingUnitPendingGoodsIssue> GetGoodsIssues(int? locationID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<HandlingUnitPendingGoodsIssue> pendingGoodsIssues = base.TotalSalesPortalEntities.GetHandlingUnitPendingGoodsIssues(locationID).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssues;
        }


        public IEnumerable<HandlingUnitPendingGoodsIssueDetail> GetPendingGoodsIssueDetails(int? locationID, int? handlingUnitID, int? goodsIssueID, int? customerID, int? receiverID, string shippingAddress, string addressee, string goodsIssueDetailIDs, bool isReadonly)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<HandlingUnitPendingGoodsIssueDetail> pendingGoodsIssueDetails = base.TotalSalesPortalEntities.GetHandlingUnitPendingGoodsIssueDetails(locationID, handlingUnitID, goodsIssueID, customerID, receiverID, shippingAddress, addressee, goodsIssueDetailIDs, isReadonly).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueDetails;
        }
    }

}
