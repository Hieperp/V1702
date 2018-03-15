using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IHandlingUnitRepository : IGenericWithDetailRepository<HandlingUnit, HandlingUnitDetail>
    {
    }

    public interface IHandlingUnitAPIRepository : IGenericAPIRepository
    {
        IEnumerable<HandlingUnitPendingCustomer> GetCustomers(int? locationID);
        IEnumerable<HandlingUnitPendingGoodsIssue> GetGoodsIssues(int? locationID);

        IEnumerable<HandlingUnitPendingGoodsIssueDetail> GetPendingGoodsIssueDetails(int? locationID, int? handlingUnitID, int? goodsIssueID, int? customerID, int? receiverID, string shippingAddress, string addressee, string goodsIssueDetailIDs, bool isReadonly);
    }
}
