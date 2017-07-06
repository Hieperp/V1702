using System;
using System.Net;
using System.Linq;
using System.Web.Mvc;

using TotalBase.Enums;
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
        private readonly ICommodityBrandRepository commodityBrandRepository;
        private readonly ICustomerCategoryRepository customerCategoryRepository;

        public PromotionsController(IPromotionService promotionService, IPromotionViewModelSelectListBuilder promotionViewModelSelectListBuilder, ICommodityBrandRepository commodityBrandRepository, ICustomerCategoryRepository customerCategoryRepository)
            : base(promotionService, promotionViewModelSelectListBuilder, true)
        {
            this.commodityBrandRepository = commodityBrandRepository;
            this.customerCategoryRepository = customerCategoryRepository;
        }


        protected override PromotionViewModel InitViewModelByDefault(PromotionViewModel simpleViewModel)
        {
            ViewBag.CommodityBrands = commodityBrandRepository.GetAllCommodityBrands().Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CommodityBrandID.ToString() }).ToList();

            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);
            simpleViewModel.DiscountPercent = -1;
            
            return simpleViewModel;
        }

        public virtual ActionResult GetCommodityCodeParts()
        {
            this.AddRequireJsOptions();
            return View(new CommodityCodePartABC());
        }

        public virtual ActionResult PromotionCustomerCategories(int id)
        {
            PromotionViewModel promotionViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable);
            if (promotionViewModel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.CustomerCategories = customerCategoryRepository.GetAllCustomerCategories().Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CustomerCategoryID.ToString() }).ToList();

            return View(promotionViewModel);
        }

    }

}