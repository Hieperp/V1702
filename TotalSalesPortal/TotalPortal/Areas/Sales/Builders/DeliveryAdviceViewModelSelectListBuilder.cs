using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Sales.ViewModels;


namespace TotalPortal.Areas.Sales.Builders
{
    public interface IDeliveryAdviceViewModelSelectListBuilder : IViewModelSelectListBuilder<DeliveryAdviceViewModel>
    {
    }

    public class DeliveryAdviceViewModelSelectListBuilder : A02ViewModelSelectListBuilder<DeliveryAdviceViewModel>, IDeliveryAdviceViewModelSelectListBuilder
    {
        public DeliveryAdviceViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IPaymentTermSelectListBuilder paymentTermSelectListBuilder, IPaymentTermRepository paymentTermRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, paymentTermSelectListBuilder, paymentTermRepository)
        {
        }
    }

}