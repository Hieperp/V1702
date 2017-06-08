using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Accounts.ViewModels;

namespace TotalPortal.Areas.Accounts.Builders
{
    public interface ICreditNoteViewModelSelectListBuilder : IViewModelSelectListBuilder<CreditNoteViewModel>
    {
    }

    public class CreditNoteViewModelSelectListBuilder : A02ViewModelSelectListBuilder<CreditNoteViewModel>, ICreditNoteViewModelSelectListBuilder
    {
        public CreditNoteViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IPaymentTermSelectListBuilder paymentTermSelectListBuilder, IPaymentTermRepository paymentTermRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, paymentTermSelectListBuilder, paymentTermRepository)
        {
        }
    }

}