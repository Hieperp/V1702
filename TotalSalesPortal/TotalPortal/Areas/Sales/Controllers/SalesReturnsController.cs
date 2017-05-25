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
    public class SalesReturnsController : GenericViewDetailController<SalesReturn, SalesReturnDetail, SalesReturnViewDetail, SalesReturnDTO, SalesReturnPrimitiveDTO, SalesReturnDetailDTO, SalesReturnViewModel>
    {
        private readonly ICustomerRepository customerRepository;

        public SalesReturnsController(ISalesReturnService salesReturnService, ISalesReturnViewModelSelectListBuilder salesReturnViewModelSelectListBuilder, ICustomerRepository customerRepository)
            : base(salesReturnService, salesReturnViewModelSelectListBuilder, true)
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


            //USING THE SAME WarehouseTaskID WITH DeliveryAdvice. LATER, IF NEED NEW WarehouseTaskID FOR SalesReturn: JUST MODIFY HERE ONLY
            StringBuilder warehouseTaskIDList = new StringBuilder();
            warehouseTaskIDList.Append((int)GlobalEnums.WarehouseTaskID.DeliveryAdvice);

            ViewBag.WarehouseTaskID = (int)GlobalEnums.WarehouseTaskID.DeliveryAdvice;
            ViewBag.WarehouseTaskIDList = warehouseTaskIDList.ToString();
        }

        protected override bool GetShowDiscount(SalesReturnViewModel simpleViewModel)
        {
            return base.GetShowDiscount(simpleViewModel) || this.customerRepository.GetShowDiscount(simpleViewModel == null ? 0 : simpleViewModel.CustomerID);
        }


        public virtual ActionResult GetPendingGoodsIssueDetails()
        {
            this.AddRequireJsOptions();
            return View();
        }



    }

}