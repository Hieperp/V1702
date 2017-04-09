using System.Web.Mvc;
using System.Collections.Generic;

using TotalDTO.Accounts;
using TotalPortal.Builders;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Commons.ViewModels.Helpers;

namespace TotalPortal.Areas.Accounts.ViewModels
{
    public class ReceiptViewModel : ReceiptDTO, IViewDetailViewModel<ReceiptDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IMonetaryAccountDropDownViewModel, IA01SimpleViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> MonetaryAccountSelectList { get; set; }
    }

}