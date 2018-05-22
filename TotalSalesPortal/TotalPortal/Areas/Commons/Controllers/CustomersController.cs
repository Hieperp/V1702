using System.Net;
using System.Linq;
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
        private readonly IPromotionAPIRepository promotionAPIRepository;

        public CustomersController(ICustomerService customerService, ICustomerSelectListBuilder customerViewModelSelectListBuilder, IPromotionAPIRepository promotionAPIRepository)
            : base(customerService, customerViewModelSelectListBuilder)
        {
            this.customerService = customerService;
            this.promotionAPIRepository = promotionAPIRepository;
        }

        protected override bool GetShowDiscount(CustomerViewModel simpleViewModel)
        {
            return base.GetShowDiscount(simpleViewModel) || this.customerService.GetShowDiscount(simpleViewModel == null ? 0 : simpleViewModel.CustomerID);
        }

        public virtual ActionResult Promotion(int id)
        {
            CustomerViewModel customerViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable);
            if (customerViewModel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.Promotions = promotionAPIRepository.GetPromotionByCustomers(null, (int)GlobalEnums.ApplyToSalesVersusReturns.ApplyToAll, null).Select(pt => new SelectListItem { Text = pt.Specs + " => " + pt.EndDate.ToString(), Value = pt.PromotionID.ToString() }).ToList();

            return View(customerViewModel);
        }

    }
}

