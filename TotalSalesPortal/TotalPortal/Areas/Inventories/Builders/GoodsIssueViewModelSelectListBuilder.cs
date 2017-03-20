using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;


namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IGoodsIssueViewModelSelectListBuilder : IViewModelSelectListBuilder<GoodsIssueViewModel>
    {
    }

    public class GoodsIssueViewModelSelectListBuilder : A02ViewModelSelectListBuilder<GoodsIssueViewModel>, IGoodsIssueViewModelSelectListBuilder
    {
        public GoodsIssueViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IPaymentTermSelectListBuilder paymentTermSelectListBuilder, IPaymentTermRepository paymentTermRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, paymentTermSelectListBuilder, paymentTermRepository)
        {
        }
    }
}