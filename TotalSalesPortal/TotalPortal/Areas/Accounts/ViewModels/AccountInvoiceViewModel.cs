using System.Web.Mvc;
using System.Collections.Generic;

using TotalDTO.Accounts;
using TotalPortal.Builders;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Commons.ViewModels.Helpers;

namespace TotalPortal.Areas.Accounts.ViewModels
{
    public class AccountInvoiceViewModel : AccountInvoiceDTO, IViewDetailViewModel<AccountInvoiceDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IPaymentTermDropDownViewModel, IA02SimpleViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> PaymentTermSelectList { get; set; }
    }

}