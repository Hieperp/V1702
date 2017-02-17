using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Accounts.ViewModels;


namespace TotalPortal.Areas.Accounts.Builders
{
    public interface IAccountInvoiceViewModelSelectListBuilder : IViewModelSelectListBuilder<AccountInvoiceViewModel>
    {
    }

    public class AccountInvoiceViewModelSelectListBuilder : IAccountInvoiceViewModelSelectListBuilder
    {
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public AccountInvoiceViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
        {
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(AccountInvoiceViewModel accountInvoiceViewModel)
        {
            accountInvoiceViewModel.AspNetUserSelectList = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), accountInvoiceViewModel.UserID);
        }

    }

}