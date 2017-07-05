using System;
using System.Linq;
using System.Web.Mvc;

using TotalModel.Models;

using TotalCore.Repositories.Commons;
using TotalCore.Services.Commons;

using TotalDTO.Commons;

using TotalPortal.Controllers;
using TotalPortal.Areas.Commons.ViewModels;
using TotalPortal.Areas.Commons.Builders;


namespace TotalPortal.Areas.Commons.Controllers
{
    public class PromotionsController : GenericSimpleController<Promotion, PromotionDTO, PromotionPrimitiveDTO, PromotionViewModel>
    {
        private readonly ICommodityBrandAPIRepository commodityBrandAPIRepository;

        public PromotionsController(IPromotionService promotionService, IPromotionViewModelSelectListBuilder promotionViewModelSelectListBuilder, ICommodityBrandAPIRepository commodityBrandAPIRepository)
            : base(promotionService, promotionViewModelSelectListBuilder, true)
        {
            this.commodityBrandAPIRepository = commodityBrandAPIRepository;
        }


        protected override PromotionViewModel InitViewModelByDefault(PromotionViewModel simpleViewModel)
        {
            ViewBag.CommodityBrands = commodityBrandAPIRepository.GetAllCommodityBrands().Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CommodityBrandID.ToString() }).ToList();

            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);
            simpleViewModel.DiscountPercent = -1;
            
            return simpleViewModel;
        }

        public virtual ActionResult GetCommodityCodeParts()
        {
            this.AddRequireJsOptions();
            return View();
        }

    }

}