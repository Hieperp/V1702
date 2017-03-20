using System.Web.Mvc;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalDTO.Inventories;
using TotalPortal.Builders;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Commons.ViewModels.Helpers;

namespace TotalPortal.Areas.Inventories.ViewModels
{
    public class GoodsDeliveryViewModel : GoodsDeliveryDTO, IViewDetailViewModel<GoodsDeliveryDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IVehicleDropDownViewModel, IA01SimpleViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }

        public IEnumerable<SelectListItem> VehicleSelectList { get; set; }
    }

}