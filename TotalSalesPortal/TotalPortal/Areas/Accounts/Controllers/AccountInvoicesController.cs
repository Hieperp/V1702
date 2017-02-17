using System.Web.Mvc;

using TotalModel.Models;
using TotalDTO.Accounts;
using TotalCore.Services.Accounts;
using TotalPortal.Controllers;
using TotalPortal.Areas.Accounts.ViewModels;
using TotalPortal.Areas.Accounts.Builders;


namespace TotalPortal.Areas.Accounts.Controllers
{
    public class AccountInvoicesController : GenericViewDetailController<AccountInvoice, AccountInvoiceDetail, AccountInvoiceViewDetail, AccountInvoiceDTO, AccountInvoicePrimitiveDTO, AccountInvoiceDetailDTO, AccountInvoiceViewModel>
    {
        public AccountInvoicesController(IAccountInvoiceService accountInvoiceService, IAccountInvoiceViewModelSelectListBuilder accountInvoiceViewModelSelectListBuilder)
            : base(accountInvoiceService, accountInvoiceViewModelSelectListBuilder, true, true)
        {
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