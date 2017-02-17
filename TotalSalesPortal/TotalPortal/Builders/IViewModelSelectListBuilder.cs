using TotalPortal.ViewModels.Helpers;

namespace TotalPortal.Builders
{
    public interface IViewModelSelectListBuilder<TBaseViewModel>
        where TBaseViewModel: ISimpleViewModel
    {
        void BuildSelectLists(TBaseViewModel baseViewModel);
    }
}
