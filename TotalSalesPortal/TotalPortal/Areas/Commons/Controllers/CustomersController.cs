using System.Web.Mvc;

using TotalModel.Models;

using TotalDTO.Commons;
using TotalCore.Services.Commons;

using TotalPortal.Controllers;
using TotalPortal.Areas.Commons.ViewModels;
using TotalPortal.Areas.Commons.Builders;
using TotalBase.Enums;
using System.Net;


namespace TotalPortal.Areas.Commons.Controllers
{
    public class CustomersController : GenericSimpleController<Customer, CustomerDTO, CustomerPrimitiveDTO, CustomerViewModel>
    {
        private ICustomerService customerService;
        public CustomersController(ICustomerService customerService, ICustomerSelectListBuilder customerViewModelSelectListBuilder)
            : base(customerService, customerViewModelSelectListBuilder)
        {
            this.customerService = customerService;
        }

        protected override bool GetShowDiscount(CustomerViewModel simpleViewModel)
        {
            return base.GetShowDiscount(simpleViewModel) || this.customerService.GetShowDiscount(simpleViewModel == null ? 0 : simpleViewModel.CustomerID);
        }

        public virtual ActionResult Promotion(int id)
        {
            CustomerViewModel customerViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable);
            if (customerViewModel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(customerViewModel);
        }

    }
}

