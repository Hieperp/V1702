using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;

namespace TotalService.Inventories
{
    public class GoodsDeliveryService : GenericWithViewDetailService<GoodsDelivery, GoodsDeliveryDetail, GoodsDeliveryViewDetail, GoodsDeliveryDTO, GoodsDeliveryPrimitiveDTO, GoodsDeliveryDetailDTO>, IGoodsDeliveryService
    {
        public GoodsDeliveryService(IGoodsDeliveryRepository handlingUnitRepository)
            : base(handlingUnitRepository, "GoodsDeliveryPostSaveValidate", "GoodsDeliverySaveRelative", null, null, null, "GetGoodsDeliveryViewDetails")
        {
        }

        public override ICollection<GoodsDeliveryViewDetail> GetViewDetails(int goodsDeliveryID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("GoodsDeliveryID", goodsDeliveryID) };
            return this.GetViewDetails(parameters);
        }

    }
}
