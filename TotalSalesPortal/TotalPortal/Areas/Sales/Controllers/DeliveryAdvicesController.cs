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

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Parts);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Consumables);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }

        protected override bool GetShowDiscount(DeliveryAdviceViewModel simpleViewModel)
        {
            if (base.GetShowDiscount(simpleViewModel))
                return true;
            else
                return this.customerRepository.GetShowDiscount(simpleViewModel.CustomerID);
        }


        public virtual ActionResult GetSalesOrderDetails()
        {
            this.AddRequireJsOptions();
            return View();
        }



    }

}