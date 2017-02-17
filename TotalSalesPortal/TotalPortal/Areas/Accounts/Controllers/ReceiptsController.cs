using System.Collections.Generic;

using TotalModel.Models;

using TotalDTO.Accounts;
using TotalCore.Services.Accounts;

using TotalPortal.Controllers;
using TotalPortal.Areas.Accounts.ViewModels;
using TotalPortal.Areas.Accounts.Builders;


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


        protected override ICollection<ReceiptViewDetail> GetEntityViewDetails(ReceiptViewModel receiptViewModel)
        {
            ICollection<ReceiptViewDetail> receiptViewDetails = this.receiptService.GetReceiptViewDetails(receiptViewModel.ReceiptID, receiptViewModel.GoodsIssueID == null ? 0 : (int)receiptViewModel.GoodsIssueID, receiptViewModel.CustomerID, false);

            return receiptViewDetails;
        }


    }
}
