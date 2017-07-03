using TotalModel.Models;

using TotalDTO.Commons;
using TotalCore.Services.Commons;

using TotalPortal.Controllers;
using TotalPortal.Areas.Commons.ViewModels;
using TotalPortal.Areas.Commons.Builders;


namespace TotalPortal.Areas.Commons.Controllers
{
    public class CommodityPricesController : GenericSimpleController<CommodityPrice, CommodityPriceDTO, CommodityPricePrimitiveDTO, CommodityPriceViewModel>
    {
        public CommodityPricesController(ICommodityPriceService commodityPriceService, ICommodityPriceSelectListBuilder commodityPriceViewModelSelectListBuilder)
            : base(commodityPriceService, commodityPriceViewModelSelectListBuilder)
        {
        }
    }
}

