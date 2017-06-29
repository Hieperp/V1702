using System.Net;
using System.Web.Mvc;

using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Commons;

using TotalCore.Repositories.Commons;
using TotalCore.Services.Commons;

using TotalPortal.Controllers;
using TotalPortal.Areas.Commons.ViewModels;
using TotalPortal.Areas.Commons.Builders;



namespace TotalPortal.Areas.Commons.Controllers
{
    public class CustomersController : GenericSimpleController<Customer, CustomerDTO, CustomerPrimitiveDTO, CustomerViewModel>
    {
        private ICustomerService customerService;
        private readonly IPromotionRepository promotionRepository;

        public CustomersController(ICustomerService customerService, ICustomerSelectListBuilder customerViewModelSelectListBuilder, IPromotionRepository promotionRepository)
            : base(customerService, customerViewModelSelectListBuilder)
        {
            this.customerService = customerService;
            this.promotionRepository = promotionRepository;
        }

        protected override bool GetShowDiscount(CustomerViewModel simpleViewModel)
        {
            return base.GetShowDiscount(simpleViewModel) || this.customerService.GetShowDiscount(simpleViewModel == null ? 0 : simpleViewModel.CustomerID);
        }

        public virtual ActionResult Promotion(int id)
        {
            CustomerViewModel customerViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable);
            if (customerViewModel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.Promotions = promotionRepository.GetPromotionByCustomers(null);

            return View(customerViewModel);
        }

    }
}

