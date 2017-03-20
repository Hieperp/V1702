using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Accounts.ViewModels;


namespace TotalPortal.Areas.Accounts.Builders
{
    public interface IAccountInvoiceViewModelSelectListBuilder : IViewModelSelectListBuilder<AccountInvoiceViewModel>
    {
    }

    public class AccountInvoiceViewModelSelectListBuilder : A02ViewModelSelectListBuilder<AccountInvoiceViewModel>, IAccountInvoiceViewModelSelectListBuilder
    {
        public AccountInvoiceViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IPaymentTermSelectListBuilder paymentTermSelectListBuilder, IPaymentTermRepository paymentTermRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, paymentTermSelectListBuilder, paymentTermRepository)
        {
        }
    }

}