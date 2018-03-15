using System.Collections.Generic;

using TotalModel.Models;

using TotalDTO.Inventories;
using TotalCore.Repositories.Commons;
using TotalCore.Services.Inventories;

using TotalPortal.Controllers;

using TotalPortal.APIs.Sessions;

using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Inventories.Builders;
using TotalPortal.Areas.Inventories.Controllers.Sessions;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class GoodsIssuesController : GenericViewDetailController<GoodsIssue, GoodsIssueDetail, GoodsIssueViewDetail, GoodsIssueDTO, GoodsIssuePrimitiveDTO, GoodsIssueDetailDTO, GoodsIssueViewModel>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IGoodsIssueService goodsIssueService;

        public GoodsIssuesController(IGoodsIssueService goodsIssueService, IGoodsIssueViewModelSelectListBuilder goodsIssueViewModelSelectListBuilder, ICustomerRepository customerRepository)
            : base(goodsIssueService, goodsIssueViewModelSelectListBuilder, true)
        {
            this.goodsIssueService = goodsIssueService;
            this.customerRepository = customerRepository;
        }


        protected override ICollection<GoodsIssueViewDetail> GetEntityViewDetails(GoodsIssueViewModel goodsIssueViewModel)
        {
            ICollection<GoodsIssueViewDetail> goodsIssueViewDetails = this.goodsIssueService.GetGoodsIssueViewDetails(goodsIssueViewModel.GoodsIssueID, this.goodsIssueService.LocationID, goodsIssueViewModel.DeliveryAdviceID == null ? 0 : (int)goodsIssueViewModel.DeliveryAdviceID, goodsIssueViewModel.CustomerID, goodsIssueViewModel.ReceiverID, goodsIssueViewModel.WarehouseID == null ? 0 : (int)goodsIssueViewModel.WarehouseID, goodsIssueViewModel.ShippingAddress, goodsIssueViewModel.Addressee == null ? "" : goodsIssueViewModel.Addressee, goodsIssueViewModel.TradePromotionID, goodsIssueViewModel.VATPercent, false);

            return goodsIssueViewDetails;
        }

        protected override bool GetShowDiscount(GoodsIssueViewModel simpleViewModel)
        {
            return base.GetShowDiscount(simpleViewModel) || this.customerRepository.GetShowDiscount(simpleViewModel == null ? 0 : simpleViewModel.CustomerID);
        }

        protected override GoodsIssueViewModel InitViewModelByPrior(GoodsIssueViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByPrior(simpleViewModel);
            if (simpleViewModel.CustomerID > 0 && simpleViewModel.DeliveryAdviceID == null)
            {
                List<PendingDeliveryAdviceDescription> pendingDeliveryAdviceDescriptions = this.goodsIssueService.GetDescriptions(this.goodsIssueService.LocationID, simpleViewModel.CustomerID, simpleViewModel.ReceiverID, (simpleViewModel.WarehouseID != null ? (int)simpleViewModel.WarehouseID : 0), simpleViewModel.ShippingAddress, simpleViewModel.Addressee, simpleViewModel.TradePromotionID, simpleViewModel.VATPercent);

                string description = ""; string remarks = "";
                pendingDeliveryAdviceDescriptions.ForEach(e =>
                {
                    if (e.Description != null && e.Description != "")
                        description = description + (description != "" ? ", " : "") + e.Reference + ": " + e.Description;
                    if (e.Remarks != null && e.Remarks != "")
                        remarks = remarks + (remarks != "" ? ", " : "") + e.Reference + ": " + e.Remarks;
                });
                simpleViewModel.Description = description; simpleViewModel.Remarks = remarks;
            }

            return simpleViewModel;
        }


        protected override GoodsIssueViewModel InitViewModelByDefault(GoodsIssueViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);

            if (simpleViewModel.Storekeeper == null)
            {
                string storekeeperSession = GoodsIssueSession.GetStorekeeper(this.HttpContext);

                if (HomeSession.TryParseID(storekeeperSession) > 0)
                {
                    simpleViewModel.Storekeeper = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.Storekeeper.EmployeeID = (int)HomeSession.TryParseID(storekeeperSession);
                    simpleViewModel.Storekeeper.Name = HomeSession.TryParseName(storekeeperSession);
                }
            }

            return simpleViewModel;
        }

        protected override void BackupViewModelToSession(GoodsIssueViewModel simpleViewModel)
        {
            base.BackupViewModelToSession(simpleViewModel);
            GoodsIssueSession.SetStorekeeper(this.HttpContext, simpleViewModel.Storekeeper.EmployeeID, simpleViewModel.Storekeeper.Name);
        }
    }
}
