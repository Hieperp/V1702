using System.Web.Mvc;

using TotalModel.Models;

using TotalDTO.Accounts;
using TotalCore.Repositories.Commons;
using TotalCore.Services.Accounts;

using TotalPortal.Controllers;
using TotalPortal.Areas.Accounts.ViewModels;
using TotalPortal.Areas.Accounts.Builders;


namespace TotalPortal.Areas.Accounts.Controllers
{
    public class AccountInvoicesController : GenericViewDetailController<AccountInvoice, AccountInvoiceDetail, AccountInvoiceViewDetail, AccountInvoiceDTO, AccountInvoicePrimitiveDTO, AccountInvoiceDetailDTO, AccountInvoiceViewModel>
    {
        private readonly ICustomerRepository customerRepository;

        public AccountInvoicesController(IAccountInvoiceService accountInvoiceService, IAccountInvoiceViewModelSelectListBuilder accountInvoiceViewModelSelectListBuilder, ICustomerRepository customerRepository)
            : base(accountInvoiceService, accountInvoiceViewModelSelectListBuilder, true, true)
        {
            this.customerRepository = customerRepository;
        }

        protected override bool GetShowDiscount(AccountInvoiceViewModel simpleViewModel)
        {
            return base.GetShowDiscount(simpleViewModel) || this.customerRepository.GetShowDiscount(simpleViewModel == null ? 0 : simpleViewModel.CustomerID);
        }

        public ActionResult PrintDetail(int? id)
        {
            return View(InitPrintViewModel(id));
        }

        public virtual ActionResult GetPendingGoodsIssueDetails()
        {
            return View();
        }
    }  
}