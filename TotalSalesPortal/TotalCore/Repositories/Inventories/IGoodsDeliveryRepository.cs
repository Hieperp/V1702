using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IGoodsDeliveryRepository : IGenericWithDetailRepository<GoodsDelivery, GoodsDeliveryDetail>
    {
    }

    public interface IGoodsDeliveryAPIRepository : IGenericAPIRepository
    {
        IEnumerable<PendingHandlingUnitReceiver> GetReceivers(int? locationID);

        IEnumerable<PendingHandlingUnit> GetPendingHandlingUnits(int? goodsDeliveryID, int? receiverID, string handlingUnitIDs, bool isReadonly);
    }
}
