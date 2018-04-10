using System.Web.Mvc;
using System.Collections.Generic;

using TotalDTO.Commons;
using TotalPortal.ViewModels.Helpers;

using TotalPortal.Areas.Commons.ViewModels.Helpers;

namespace TotalPortal.Areas.Commons.ViewModels
{
    public class CustomerViewModel : CustomerDTO, ISimpleViewModel, ICustomerTypeDropDownViewModel, ICustomerCategoryDropDownViewModel, ITerritoryDropDownViewModel, IPriceCategoryDropDownViewModel, IPaymentTermDropDownViewModel
    {
        public IEnumerable<SelectListItem> CustomerTypeSelectList { get; set; }
        public IEnumerable<SelectListItem> CustomerCategorySelectList { get; set; }
        public IEnumerable<SelectListItem> TerritorySelectList { get; set; }
        public IEnumerable<SelectListItem> PriceCategorySelectList { get; set; }
        public IEnumerable<SelectListItem> PaymentTermSelectList { get; set; }
    }
}