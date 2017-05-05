using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Sales.ViewModels;


namespace TotalPortal.Areas.Sales.Builders
{
    public interface ISalesOrderViewModelSelectListBuilder : IViewModelSelectListBuilder<SalesOrderViewModel>
    {
    }

    public class SalesOrderViewModelSelectListBuilder : A02ViewModelSelectListBuilder<SalesOrderViewModel>, ISalesOrderViewModelSelectListBuilder
    {
        public SalesOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IPaymentTermSelectListBuilder paymentTermSelectListBuilder, IPaymentTermRepository paymentTermRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, paymentTermSelectListBuilder, paymentTermRepository)
        {
        }
    }

}