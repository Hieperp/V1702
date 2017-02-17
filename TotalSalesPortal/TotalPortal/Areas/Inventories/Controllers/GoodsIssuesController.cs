using System.Collections.Generic;

using TotalModel.Models;

using TotalDTO.Inventories;
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
        private readonly IGoodsIssueService goodsIssueService;

        public GoodsIssuesController(IGoodsIssueService goodsIssueService, IGoodsIssueViewModelSelectListBuilder goodsIssueViewModelSelectListBuilder)
            : base(goodsIssueService, goodsIssueViewModelSelectListBuilder, true)
        {
            this.goodsIssueService = goodsIssueService;
        }


        protected override ICollection<GoodsIssueViewDetail> GetEntityViewDetails(GoodsIssueViewModel goodsIssueViewModel)
        {
            ICollection<GoodsIssueViewDetail> goodsIssueViewDetails = this.goodsIssueService.GetGoodsIssueViewDetails(goodsIssueViewModel.GoodsIssueID, this.goodsIssueService.LocationID, goodsIssueViewModel.DeliveryAdviceID == null ? 0 : (int)goodsIssueViewModel.DeliveryAdviceID, goodsIssueViewModel.CustomerID, goodsIssueViewModel.ReceiverID, goodsIssueViewModel.ShippingAddress, false);

            return goodsIssueViewDetails;
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
