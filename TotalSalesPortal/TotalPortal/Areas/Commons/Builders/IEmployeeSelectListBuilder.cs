using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Commons.ViewModels;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface IEmployeeSelectListBuilder : IViewModelSelectListBuilder<EmployeeViewModel>
    {
    }

    public class EmployeeSelectListBuilder : IEmployeeSelectListBuilder
    {
        public virtual void BuildSelectLists(EmployeeViewModel employeeViewModel)
        {
        }
    }

}