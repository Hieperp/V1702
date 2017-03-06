﻿using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IGoodsIssueRepository : IGenericWithDetailRepository<GoodsIssue, GoodsIssueDetail>
    {
        List<PendingDeliveryAdviceDescription> GetDescriptions(int locationID, int customerID, int receiverID, string shippingAddress);
    }

    public interface IGoodsIssueAPIRepository : IGenericAPIRepository
    {
        ICollection<PendingDeliveryAdvice> GetDeliveryAdvices(int locationID);
        ICollection<PendingDeliveryAdviceCustomer> GetCustomers(int locationID);
    }
}
