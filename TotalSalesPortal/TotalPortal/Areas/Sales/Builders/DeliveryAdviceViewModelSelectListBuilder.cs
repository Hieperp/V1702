using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Sales.ViewModels;


namespace TotalPortal.Areas.Sales.Builders
{
    public interface IDeliveryAdviceViewModelSelectListBuilder : IViewModelSelectListBuilder<DeliveryAdviceViewModel>
    {
    }

    public class DeliveryAdviceViewModelSelectListBuilder : IDeliveryAdviceViewModelSelectListBuilder
    {
        private readonly IPaymentTermSelectListBuilder paymentTermSelectListBuilder;
        private readonly IPaymentTermRepository paymentTermRepository;
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public DeliveryAdviceViewModelSelectListBuilder(IPaymentTermSelectListBuilder paymentTermSelectListBuilder, IPaymentTermRepository paymentTermRepository, IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
        {
            this.paymentTermSelectListBuilder = paymentTermSelectListBuilder;
            this.paymentTermRepository = paymentTermRepository;
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(DeliveryAdviceViewModel deliveryAdviceViewModel)
        {
            deliveryAdviceViewModel.PaymentTermSelectList = paymentTermSelectListBuilder.BuildSelectListItemsForPaymentTerms(paymentTermRepository.GetAllPaymentTerms());
            deliveryAdviceViewModel.AspNetUserSelectList = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), deliveryAdviceViewModel.UserID);
        }

    }

}