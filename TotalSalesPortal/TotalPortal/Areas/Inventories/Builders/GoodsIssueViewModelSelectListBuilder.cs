using TotalCore.Repositories.Commons;
using TotalCore.Repositories.Inventories;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;


namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IGoodsIssueViewModelSelectListBuilder : IViewModelSelectListBuilder<GoodsIssueViewModel>
    {

    }


    public class GoodsIssueViewModelSelectListBuilder : IGoodsIssueViewModelSelectListBuilder
    {
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public GoodsIssueViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
        {
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(GoodsIssueViewModel goodsIssueViewModel)
        {
            goodsIssueViewModel.AspNetUserSelectList = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), goodsIssueViewModel.UserID);
        }
    }


}