﻿using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Inventories;
using TotalDAL.Helpers;

namespace TotalDAL.Repositories.Inventories
{
    public class GoodsIssueRepository : GenericWithDetailRepository<GoodsIssue, GoodsIssueDetail>, IGoodsIssueRepository
    {
        public GoodsIssueRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GoodsIssueEditable", "GoodsIssueApproved")
        {
        }
    }


    public class GoodsIssueAPIRepository : GenericAPIRepository, IGoodsIssueAPIRepository
    {
        public GoodsIssueAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetGoodsIssueIndexes")
        {
        }

        public ICollection<PendingDeliveryAdvice> GetDeliveryAdvices(int locationID)
        {
            return this.TotalSalesPortalEntities.GetPendingDeliveryAdvices(locationID).ToList();
        }

        public ICollection<PendingDeliveryAdviceCustomer> GetCustomers(int locationID)
        {
            return this.TotalSalesPortalEntities.GetPendingDeliveryAdviceCustomers(locationID).ToList();
        }
    }
}
