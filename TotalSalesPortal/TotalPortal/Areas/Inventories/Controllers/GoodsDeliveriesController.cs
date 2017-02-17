using System.Web.Mvc;

using TotalModel.Models;

using TotalDTO.Inventories;
using TotalCore.Services.Inventories;

using TotalPortal.Controllers;
using TotalPortal.APIs.Sessions;
using TotalPortal.Areas.Inventories.Controllers.Sessions;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Inventories.Builders;


namespace TotalPortal.Areas.Inventories.Controllers
{
    public class GoodsDeliveriesController : GenericViewDetailController<GoodsDelivery, GoodsDeliveryDetail, GoodsDeliveryViewDetail, GoodsDeliveryDTO, GoodsDeliveryPrimitiveDTO, GoodsDeliveryDetailDTO, GoodsDeliveryViewModel>
    {
        public GoodsDeliveriesController(IGoodsDeliveryService goodsDeliveryService, IGoodsDeliveryViewModelSelectListBuilder goodsDeliveryViewModelSelectListBuilder)
            : base(goodsDeliveryService, goodsDeliveryViewModelSelectListBuilder, true)
        {
        }


        protected override GoodsDeliveryViewModel InitViewModelByDefault(GoodsDeliveryViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);

            if (simpleViewModel.Driver == null)
            {
                string driverSession = GoodsDeliverySession.GetDriver(this.HttpContext);

                if (HomeSession.TryParseID(driverSession) > 0)
                {
                    simpleViewModel.Driver = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.Driver.EmployeeID = (int)HomeSession.TryParseID(driverSession);
                    simpleViewModel.Driver.Name = HomeSession.TryParseName(driverSession);
                }
            }

            if (simpleViewModel.Collector == null)
            {
                string collectorSession = GoodsDeliverySession.GetCollector(this.HttpContext);

                if (HomeSession.TryParseID(collectorSession) > 0)
                {
                    simpleViewModel.Collector = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.Collector.EmployeeID = (int)HomeSession.TryParseID(collectorSession);
                    simpleViewModel.Collector.Name = HomeSession.TryParseName(collectorSession);
                }
            }

            return simpleViewModel;
        }

        protected override void BackupViewModelToSession(GoodsDeliveryViewModel simpleViewModel)
        {
            base.BackupViewModelToSession(simpleViewModel);
            GoodsDeliverySession.SetDriver(this.HttpContext, simpleViewModel.Driver.EmployeeID, simpleViewModel.Driver.Name);
            GoodsDeliverySession.SetCollector(this.HttpContext, simpleViewModel.Collector.EmployeeID, simpleViewModel.Collector.Name);
        }
    
        public virtual ActionResult GetPendingHandlingUnits()
        {
            return View();
        }
    }
}