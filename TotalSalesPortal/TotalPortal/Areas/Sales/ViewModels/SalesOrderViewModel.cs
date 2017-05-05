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
    public class SalesOrderViewModel : SalesOrderDTO, IViewDetailViewModel<SalesOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IPaymentTermDropDownViewModel, IA02SimpleViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> PaymentTermSelectList { get; set; }
    }

}
