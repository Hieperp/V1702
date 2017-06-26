using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Commons.ViewModels;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface ICustomerViewModelSelectListBuilder : IViewModelSelectListBuilder<CustomerViewModel>
    {
    }

    public class CustomerViewModelSelectListBuilder : ICustomerViewModelSelectListBuilder
    {
        public virtual void BuildSelectLists(CustomerViewModel customerViewModel)
        {
        }
    }

}