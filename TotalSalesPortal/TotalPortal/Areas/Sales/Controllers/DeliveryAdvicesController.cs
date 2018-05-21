using System.Net;
using System.Web.Mvc;
using System.Text;

using AutoMapper;
using RequireJsNet;

using TotalBase.Enums;

using TotalModel.Models;

using TotalCore.Services.Sales;
using TotalCore.Repositories.Commons;

using TotalDTO.Sales;

using TotalPortal.Controllers;
using TotalPortal.Areas.Sales.ViewModels;
using TotalPortal.Areas.Sales.Builders;



namespace TotalPortal.Areas.Sales.Controllers
{
    public class DeliveryAdvicesController : GenericViewDetailController<DeliveryAdvice, DeliveryAdviceDetail, DeliveryAdviceViewDetail, DeliveryAdviceDTO, DeliveryAdvicePrimitiveDTO, DeliveryAdviceDetailDTO, DeliveryAdviceViewModel>
    {
        private readonly ICustomerRepository customerRepository;

        public DeliveryAdvicesController(IDeliveryAdviceService deliveryAdviceService, IDeliveryAdviceViewModelSelectListBuilder deliveryAdviceViewModelSelectListBuilder, ICustomerRepository customerRepository)
            : base(deliveryAdviceService, deliveryAdviceViewModelSelectListBuilder, true)
        {
            this.customerRepository = customerRepository;
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            RequireJsOptions.Add("ApplyToSalesVersusReturns", (int)GlobalEnums.ApplyToSalesVersusReturns.ApplyToSales, RequireJsOptionsScope.Page);

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Parts);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Consumables);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);


            StringBuilder warehouseTaskIDList = new StringBuilder();
            warehouseTaskIDList.Append((int)GlobalEnums.WarehouseTaskID.DeliveryAdvice);

            ViewBag.WarehouseTaskID = (int)GlobalEnums.WarehouseTaskID.DeliveryAdvice;
            ViewBag.WarehouseTaskIDList = warehouseTaskIDList.ToString();
        }

        protected override bool GetShowDiscount(DeliveryAdviceViewModel simpleViewModel)
        {
            return base.GetShowDiscount(simpleViewModel) || this.customerRepository.GetShowDiscount(simpleViewModel == null ? 0 : simpleViewModel.CustomerID);
        }

        protected override bool GetShowListedPrice(DeliveryAdviceViewModel simpleViewModel)
        {
            return base.BaseService.GetShowListedPrice(simpleViewModel.PriceCategoryID);
        }

        protected override bool GetShowListedGrossPrice(DeliveryAdviceViewModel simpleViewModel)
        {
            return base.BaseService.GetShowListedGrossPrice(simpleViewModel.PriceCategoryID);
        }

        public virtual ActionResult GetPendingSalesOrderDetails()
        {
            this.AddRequireJsOptions();
            return View();
        }



    }

}