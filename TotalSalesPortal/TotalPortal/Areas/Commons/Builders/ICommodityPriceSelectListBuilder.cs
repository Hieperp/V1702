using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Commons.ViewModels;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface ICommodityPriceSelectListBuilder : IViewModelSelectListBuilder<CommodityPriceViewModel>
    {
    }

    public class CommodityPriceSelectListBuilder : ICommodityPriceSelectListBuilder
    {
        private readonly IPriceCategorySelectListBuilder priceCategorySelectListBuilder;
        private readonly IPriceCategoryRepository priceCategoryRepository;

        public CommodityPriceSelectListBuilder(IPriceCategorySelectListBuilder priceCategorySelectListBuilder, IPriceCategoryRepository priceCategoryRepository)
        {
            this.priceCategorySelectListBuilder = priceCategorySelectListBuilder;
            this.priceCategoryRepository = priceCategoryRepository;
        }

        public virtual void BuildSelectLists(CommodityPriceViewModel commodityPriceViewModel)
        {
            commodityPriceViewModel.PriceCategorySelectList = this.priceCategorySelectListBuilder.BuildSelectListItemsForPriceCategorys(this.priceCategoryRepository.GetAllPriceCategories());
        }
    }

}