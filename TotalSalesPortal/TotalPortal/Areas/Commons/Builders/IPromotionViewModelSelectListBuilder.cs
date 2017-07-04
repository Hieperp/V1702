using TotalPortal.Builders;
using TotalPortal.Areas.Commons.ViewModels;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface IPromotionViewModelSelectListBuilder : IViewModelSelectListBuilder<PromotionViewModel>
    {
    }

    public class PromotionViewModelSelectListBuilder : IPromotionViewModelSelectListBuilder
    {
        public virtual void BuildSelectLists(PromotionViewModel promotionViewModel)
        { }
    }

}