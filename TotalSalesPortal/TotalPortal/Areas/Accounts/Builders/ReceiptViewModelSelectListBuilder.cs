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


    public class ReceiptViewModelSelectListBuilder : A01ViewModelSelectListBuilder<ReceiptViewModel>, IReceiptViewModelSelectListBuilder
    {
        private readonly IMonetaryAccountSelectListBuilder monetaryAccountSelectListBuilder;
        private readonly IMonetaryAccountRepository monetaryAccountRepository;

        public ReceiptViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IMonetaryAccountSelectListBuilder monetaryAccountSelectListBuilder, IMonetaryAccountRepository monetaryAccountRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.monetaryAccountSelectListBuilder = monetaryAccountSelectListBuilder;
            this.monetaryAccountRepository = monetaryAccountRepository;
        }

        public void BuildSelectLists(ReceiptViewModel receiptViewModel)
        {
            base.BuildSelectLists(receiptViewModel);
            receiptViewModel.MonetaryAccountSelectList = this.monetaryAccountSelectListBuilder.BuildSelectListItemsForMonetaryAccounts(this.monetaryAccountRepository.GetAllMonetaryAccounts());
        }
    }


}