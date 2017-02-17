using TotalCore.Repositories.Commons;
using TotalCore.Repositories.Accounts;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Accounts.ViewModels;


namespace TotalPortal.Areas.Accounts.Builders
{
    public interface IReceiptViewModelSelectListBuilder : IViewModelSelectListBuilder<ReceiptViewModel>
    {

    }


    public class ReceiptViewModelSelectListBuilder : IReceiptViewModelSelectListBuilder
    {
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public ReceiptViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
        {
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(ReceiptViewModel receiptViewModel)
        {
            receiptViewModel.AspNetUserSelectList = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), receiptViewModel.UserID);
        }
    }


}