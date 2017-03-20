using System.Web.Mvc;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalDTO.Inventories;
using TotalPortal.Builders;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Commons.ViewModels.Helpers;

namespace TotalPortal.Areas.Inventories.ViewModels
{
    public class HandlingUnitViewModel : HandlingUnitDTO, IViewDetailViewModel<HandlingUnitDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IPackingMaterialDropDownViewModel, IA01SimpleViewModel
    {
        public IEnumerable<SelectListItem> PackingMaterialSelectList { get; set; }
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

}