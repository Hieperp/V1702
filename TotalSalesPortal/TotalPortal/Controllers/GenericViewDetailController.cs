using System;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;

using AutoMapper;

using TotalBase.Enums;
using TotalModel;
using TotalDTO;
using TotalCore.Services;

using TotalPortal.Builders;
using TotalPortal.ViewModels.Helpers;


namespace TotalPortal.Controllers
{
    public abstract class GenericViewDetailController<TEntity, TEntityDetail, TEntityViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericSimpleController<TEntity, TDto, TPrimitiveDto, TViewDetailViewModel>

        where TEntity : class, IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<TEntityDetail>, new()
        where TEntityDetail : class, IPrimitiveEntity, new()
        where TEntityViewDetail : class
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, new()
    {
        private readonly IGenericWithViewDetailService<TEntity, TEntityDetail, TEntityViewDetail, TDto, TPrimitiveDto, TDtoDetail> genericWithViewDetailService;

        public GenericViewDetailController(IGenericWithViewDetailService<TEntity, TEntityDetail, TEntityViewDetail, TDto, TPrimitiveDto, TDtoDetail> genericWithViewDetailService, IViewModelSelectListBuilder<TViewDetailViewModel> viewModelSelectListBuilder)
            : this(genericWithViewDetailService, viewModelSelectListBuilder, false, true)
        { }

        public GenericViewDetailController(IGenericWithViewDetailService<TEntity, TEntityDetail, TEntityViewDetail, TDto, TPrimitiveDto, TDtoDetail> genericWithViewDetailService, IViewModelSelectListBuilder<TViewDetailViewModel> viewModelSelectListBuilder, bool isCreateWizard)
            : this(genericWithViewDetailService, viewModelSelectListBuilder, isCreateWizard, false)
        { }

        public GenericViewDetailController(IGenericWithViewDetailService<TEntity, TEntityDetail, TEntityViewDetail, TDto, TPrimitiveDto, TDtoDetail> genericWithViewDetailService, IViewModelSelectListBuilder<TViewDetailViewModel> viewModelSelectListBuilder, bool isCreateWizard, bool isSimpleCreate)
            : base(genericWithViewDetailService, viewModelSelectListBuilder, isCreateWizard, isSimpleCreate)
        {
            this.genericWithViewDetailService = genericWithViewDetailService;
        }





        protected override TViewDetailViewModel DecorateViewModel(TViewDetailViewModel viewDetailViewModel)
        {
            viewDetailViewModel = base.DecorateViewModel(viewDetailViewModel);
            return this.GetViewDetails(viewDetailViewModel);
        }

        protected virtual TViewDetailViewModel GetViewDetails(TViewDetailViewModel viewDetailViewModel)
        {
            ICollection<TEntityViewDetail> entityViewDetails = this.GetEntityViewDetails(viewDetailViewModel);
            Mapper.Map<ICollection<TEntityViewDetail>, ICollection<TDtoDetail>>(entityViewDetails, viewDetailViewModel.ViewDetails);

            return viewDetailViewModel;
        }


        protected virtual ICollection<TEntityViewDetail> GetEntityViewDetails(TViewDetailViewModel viewDetailViewModel)
        {
            ICollection<TEntityViewDetail> entityViewDetails = this.genericWithViewDetailService.GetViewDetails(viewDetailViewModel.GetID());

            return entityViewDetails;
        }

    }
}