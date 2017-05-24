using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

using TotalDTO.Accounts;
using TotalCore.Services.Accounts;

using TotalPortal.APIs.Sessions;

using TotalPortal.Controllers;
using TotalPortal.Areas.Accounts.ViewModels;
using TotalPortal.Areas.Accounts.Builders;
using TotalPortal.Areas.Accounts.Controllers.Sessions;


namespace TotalPortal.Areas.Accounts.Controllers
{
    public class ReceiptsController : GenericViewDetailController<Receipt, ReceiptDetail, ReceiptViewDetail, ReceiptDTO, ReceiptPrimitiveDTO, ReceiptDetailDTO, ReceiptViewModel>
    {
        private readonly IReceiptService receiptService;

        public ReceiptsController(IReceiptService receiptService, IReceiptViewModelSelectListBuilder receiptViewModelSelectListBuilder)
            : base(receiptService, receiptViewModelSelectListBuilder, true)
        {
            this.receiptService = receiptService;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult CreateReceipt(ReceiptViewModel simpleViewModel)
        {
            ModelState.Clear();

            return View(simpleViewModel);
        }


        protected override ICollection<ReceiptViewDetail> GetEntityViewDetails(ReceiptViewModel receiptViewModel)
        {
            ICollection<ReceiptViewDetail> receiptViewDetails = this.receiptService.GetReceiptViewDetails(receiptViewModel.ReceiptID, this.receiptService.LocationID, receiptViewModel.GoodsIssueID == null ? 0 : (int)receiptViewModel.GoodsIssueID, receiptViewModel.CustomerID, false);

            return receiptViewDetails;
        }



        protected override ReceiptViewModel InitViewModelByDefault(ReceiptViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);

            if (simpleViewModel.Cashier == null)
            {
                string CashierSession = ReceiptSession.GetCashier(this.HttpContext);

                if (HomeSession.TryParseID(CashierSession) > 0)
                {
                    simpleViewModel.Cashier = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.Cashier.EmployeeID = (int)HomeSession.TryParseID(CashierSession);
                    simpleViewModel.Cashier.Name = HomeSession.TryParseName(CashierSession);
                }
            }

            return simpleViewModel;
        }

        protected override void BackupViewModelToSession(ReceiptViewModel simpleViewModel)
        {
            base.BackupViewModelToSession(simpleViewModel);
            ReceiptSession.SetCashier(this.HttpContext, simpleViewModel.Cashier.EmployeeID, simpleViewModel.Cashier.Name);
        }


    }
}
