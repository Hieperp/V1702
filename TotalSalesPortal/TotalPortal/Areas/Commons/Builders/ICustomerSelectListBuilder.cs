using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Commons.ViewModels;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface ICustomerSelectListBuilder : IViewModelSelectListBuilder<CustomerViewModel>
    {
    }

    public class CustomerSelectListBuilder : ICustomerSelectListBuilder
    {
        private readonly ICustomerTypeSelectListBuilder customerTypeSelectListBuilder;
        private readonly ICustomerTypeRepository customerTypeRepository;

        private readonly ICustomerCategorySelectListBuilder customerCategorySelectListBuilder;
        private readonly ICustomerCategoryRepository customerCategoryRepository;

        private readonly ITerritorySelectListBuilder territorySelectListBuilder;
        private readonly ITerritoryRepository territoryRepository;

        private readonly IPriceCategorySelectListBuilder priceCategorySelectListBuilder;
        private readonly IPriceCategoryRepository priceCategoryRepository;

        private readonly IPaymentTermSelectListBuilder paymentTermSelectListBuilder;
        private readonly IPaymentTermRepository paymentTermRepository;

        public CustomerSelectListBuilder(ICustomerTypeSelectListBuilder customerTypeSelectListBuilder, ICustomerTypeRepository customerTypeRepository, ICustomerCategorySelectListBuilder customerCategorySelectListBuilder, ICustomerCategoryRepository customerCategoryRepository, ITerritorySelectListBuilder territorySelectListBuilder, ITerritoryRepository territoryRepository, IPriceCategorySelectListBuilder priceCategorySelectListBuilder, IPriceCategoryRepository priceCategoryRepository, IPaymentTermSelectListBuilder paymentTermSelectListBuilder, IPaymentTermRepository paymentTermRepository)
        {
            this.customerTypeSelectListBuilder = customerTypeSelectListBuilder;
            this.customerTypeRepository = customerTypeRepository;

            this.customerCategorySelectListBuilder = customerCategorySelectListBuilder;
            this.customerCategoryRepository = customerCategoryRepository;

            this.territorySelectListBuilder = territorySelectListBuilder;
            this.territoryRepository = territoryRepository;

            this.priceCategorySelectListBuilder = priceCategorySelectListBuilder;
            this.priceCategoryRepository = priceCategoryRepository;

            this.paymentTermSelectListBuilder = paymentTermSelectListBuilder;
            this.paymentTermRepository = paymentTermRepository;
        }

        public virtual void BuildSelectLists(CustomerViewModel customerViewModel)
        {
            customerViewModel.CustomerTypeSelectList = this.customerTypeSelectListBuilder.BuildSelectListItemsForCustomerTypes(this.customerTypeRepository.GetAllCustomerTypes());
            customerViewModel.CustomerCategorySelectList = this.customerCategorySelectListBuilder.BuildSelectListItemsForCustomerCategorys(this.customerCategoryRepository.GetAllCustomerCategories());
            customerViewModel.TerritorySelectList = this.territorySelectListBuilder.BuildSelectListItemsForTerritorys(this.territoryRepository.GetAllTerritories());
            customerViewModel.PriceCategorySelectList = this.priceCategorySelectListBuilder.BuildSelectListItemsForPriceCategorys(this.priceCategoryRepository.GetAllPriceCategories());
            customerViewModel.PaymentTermSelectList = this.paymentTermSelectListBuilder.BuildSelectListItemsForPaymentTerms(this.paymentTermRepository.GetAllPaymentTerms());
        }
    }
}