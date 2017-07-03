using TotalModel.Models;
using TotalDTO.Commons;
using TotalCore.Repositories.Commons;
using TotalCore.Services.Commons;

namespace TotalService.Commons
{
    public class CommodityPriceService : GenericService<CommodityPrice, CommodityPriceDTO, CommodityPricePrimitiveDTO>, ICommodityPriceService
    {
        public CommodityPriceService(ICommodityPriceRepository commodityPriceRepository)
            : base(commodityPriceRepository)
        {
        }

    }
}
