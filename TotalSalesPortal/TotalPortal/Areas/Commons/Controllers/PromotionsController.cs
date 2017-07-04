using System.Web.Mvc;

using TotalModel.Models;

using TotalCore.Services.Commons;

using TotalDTO.Commons;

using TotalPortal.Controllers;
using TotalPortal.Areas.Commons.ViewModels;
using TotalPortal.Areas.Commons.Builders;


namespace TotalPortal.Areas.Commons.Controllers
{
    public class PromotionsController : GenericSimpleController<Promotion, PromotionDTO, PromotionPrimitiveDTO, PromotionViewModel>
    {
        public PromotionsController(IPromotionService promotionService, IPromotionViewModelSelectListBuilder promotionViewModelSelectListBuilder)
            : base(promotionService, promotionViewModelSelectListBuilder, true)
        {
        }

        public virtual ActionResult GetCommodityCodeParts()
        {
            this.AddRequireJsOptions();
            return View();
        }

    }

}