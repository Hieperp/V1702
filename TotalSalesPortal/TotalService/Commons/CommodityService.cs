using TotalModel.Models;
using TotalDTO.Commons;
using TotalCore.Repositories.Commons;
using TotalCore.Services.Commons;

namespace TotalService.Commons
{
    public class CommodityService : GenericService<Commodity, CommodityDTO, CommodityPrimitiveDTO>, ICommodityService
    {
        public CommodityService(ICommodityRepository commodityRepository)
            : base(commodityRepository)
        {
        }

    }
}