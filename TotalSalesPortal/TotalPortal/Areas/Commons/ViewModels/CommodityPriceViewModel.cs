using System.Web.Mvc;
using System.Collections.Generic;

using TotalDTO.Commons;
using TotalPortal.Areas.Commons.ViewModels.Helpers;
using TotalPortal.ViewModels.Helpers;


namespace TotalPortal.Areas.Commons.ViewModels
{
    public class CommodityPriceViewModel : CommodityPriceDTO, ISimpleViewModel, IPriceCategoryDropDownViewModel
    {
        public IEnumerable<SelectListItem> PriceCategorySelectList { get; set; }
    }
}