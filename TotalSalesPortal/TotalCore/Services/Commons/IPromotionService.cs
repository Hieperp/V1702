using TotalModel.Models;
using TotalDTO.Commons;
using TotalCore.Services.Helpers;

namespace TotalCore.Services.Commons
{
    public interface IPromotionService : IGenericWithDetailService<Promotion, PromotionCommodityCodePart, PromotionDTO, PromotionPrimitiveDTO, PromotionCommodityCodePartDTO>
    {
    }
}
