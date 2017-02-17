using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Inventories;
using TotalDAL.Helpers;

namespace TotalDAL.Repositories.Inventories
{
    public class GoodsDeliveryRepository : GenericWithDetailRepository<GoodsDelivery, GoodsDeliveryDetail>, IGoodsDeliveryRepository
    {
        public GoodsDeliveryRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities) { }
    }


    public class GoodsDeliveryAPIRepository : GenericAPIRepository, IGoodsDeliveryAPIRepository
    {
        public GoodsDeliveryAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetGoodsDeliveryIndexes")
        {
        }





        public IEnumerable<PendingHandlingUnitReceiver> GetReceivers(int? locationID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingHandlingUnitReceiver> pendingHandlingUnitReceivers = base.TotalSalesPortalEntities.GetPendingHandlingUnitReceivers(locationID).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingHandlingUnitReceivers;
        }

        public IEnumerable<PendingHandlingUnit> GetPendingHandlingUnits(int? goodsDeliveryID, int? receiverID, string handlingUnitIDs, bool isReadonly)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingHandlingUnit> pendingHandlingUnits = base.TotalSalesPortalEntities.GetPendingHandlingUnits(goodsDeliveryID, receiverID, handlingUnitIDs, isReadonly).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingHandlingUnits;
        }
    }

}
