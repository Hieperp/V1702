using System.Collections.Generic;

using TotalBase.Enums;
using TotalModel;
using TotalDTO;


namespace TotalPortal.ViewModels.Helpers
{
    public interface ISimpleViewModel : IPrimitiveEntity, IPrimitiveDTO
    {
        bool PrintAfterClosedSubmit { get; }
        GlobalEnums.SubmitTypeOption SubmitTypeOption { get; set; }
    }

    public interface IViewDetailViewModel<TDtoDetail> : ISimpleViewModel
        where TDtoDetail : class, IPrimitiveEntity
    {
        List<TDtoDetail> ViewDetails { get; set; } //This ViewDetails is designed to use by Mapper in GenericViewDetailController only
    }

}
