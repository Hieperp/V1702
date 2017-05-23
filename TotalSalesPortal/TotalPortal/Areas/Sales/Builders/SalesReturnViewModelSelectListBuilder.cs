using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Sales.ViewModels;


namespace TotalPortal.Areas.Sales.Builders
{
    public interface ISalesReturnViewModelSelectListBuilder : IViewModelSelectListBuilder<SalesReturnViewModel>
    {
    }

    public class SalesReturnViewModelSelectListBuilder : A02ViewModelSelectListBuilder<SalesReturnViewModel>, ISalesReturnViewModelSelectListBuilder
    {
        public SalesReturnViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IPaymentTermSelectListBuilder paymentTermSelectListBuilder, IPaymentTermRepository paymentTermRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, paymentTermSelectListBuilder, paymentTermRepository)
        {
        }
    }

}