using System.Collections.Generic;

using TotalModel.Models;
using TotalDTO.Inventories;

namespace TotalCore.Services.Inventories
{
    public interface IGoodsDeliveryService : IGenericWithViewDetailService<GoodsDelivery, GoodsDeliveryDetail, GoodsDeliveryViewDetail, GoodsDeliveryDTO, GoodsDeliveryPrimitiveDTO, GoodsDeliveryDetailDTO>
    {
    }
}
