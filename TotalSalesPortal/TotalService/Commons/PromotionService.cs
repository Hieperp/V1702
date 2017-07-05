using TotalModel.Models;
using TotalDTO.Commons;
using TotalCore.Repositories.Commons;
using TotalCore.Services.Commons;

namespace TotalService.Commons
{
    public class PromotionService : GenericWithDetailService<Promotion, PromotionCommodityCodePart, PromotionDTO, PromotionPrimitiveDTO, PromotionCommodityCodePartDTO>, IPromotionService
    {
        public PromotionService(IPromotionRepository promotionRepository)
            : base(promotionRepository, null, "PromotionSaveRelative", "PromotionToggleApproved", "PromotionToggleVoid")
        {
        }
    }

}
