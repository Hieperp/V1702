using System;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalDTO.Sales;
using TotalPortal.Builders;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Commons.ViewModels.Helpers;

namespace TotalPortal.Areas.Sales.ViewModels
{
    public class SalesReturnViewModel : SalesReturnDTO, IViewDetailViewModel<SalesReturnDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

}
