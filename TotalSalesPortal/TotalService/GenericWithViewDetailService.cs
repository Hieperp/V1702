using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel;
using TotalDTO;
using TotalCore.Repositories;
using TotalCore.Services;

namespace TotalService
{
    public abstract class GenericWithViewDetailService<TEntity, TEntityDetail, TEntityViewDetail, TDto, TPrimitiveDto, TDtoDetail> : GenericWithDetailService<TEntity, TEntityDetail, TDto, TPrimitiveDto, TDtoDetail>, IGenericWithViewDetailService<TEntity, TEntityDetail, TEntityViewDetail, TDto, TPrimitiveDto, TDtoDetail>

        where TEntity : class, IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<TEntityDetail>, new()
        where TEntityDetail : class, IPrimitiveEntity, new()
        where TEntityViewDetail : class
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        private readonly string functionNameGetViewDetails;

        public GenericWithViewDetailService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository)
            : this(genericWithDetailRepository, null)
        {
        }

        public GenericWithViewDetailService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository, string functionNamePostSaveValidate)
            : this(genericWithDetailRepository, functionNamePostSaveValidate, null)
        {
        }

        public GenericWithViewDetailService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository, string functionNamePostSaveValidate, string functionNameSaveRelative)
            : this(genericWithDetailRepository, functionNamePostSaveValidate, functionNameSaveRelative, null)
        {
        }

        public GenericWithViewDetailService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository, string functionNamePostSaveValidate, string functionNameSaveRelative, string functionNameToggleApproved)
            : this(genericWithDetailRepository, functionNamePostSaveValidate, functionNameSaveRelative, functionNameToggleApproved, null)
        {
        }

        public GenericWithViewDetailService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository, string functionNamePostSaveValidate, string functionNameSaveRelative, string functionNameToggleApproved, string functionNameToggleVoid)
            : this(genericWithDetailRepository, functionNamePostSaveValidate, functionNameSaveRelative, functionNameToggleApproved, functionNameToggleVoid, null)
        {
        }

        public GenericWithViewDetailService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository, string functionNamePostSaveValidate, string functionNameSaveRelative, string functionNameToggleApproved, string functionNameToggleVoid, string functionNameToggleVoidDetail)
            : this(genericWithDetailRepository, functionNamePostSaveValidate, functionNameSaveRelative, functionNameToggleApproved, functionNameToggleVoid, functionNameToggleVoidDetail, null)
        {
        }

        public GenericWithViewDetailService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository, string functionNamePostSaveValidate, string functionNameSaveRelative, string functionNameToggleApproved, string functionNameToggleVoid, string functionNameToggleVoidDetail, string functionNameGetViewDetails)
            : base(genericWithDetailRepository, functionNamePostSaveValidate, functionNameSaveRelative, functionNameToggleApproved, functionNameToggleVoid, functionNameToggleVoidDetail)
        {
            this.functionNameGetViewDetails = functionNameGetViewDetails;
        }

        public abstract ICollection<TEntityViewDetail> GetViewDetails(int id);

        public virtual ICollection<TEntityViewDetail> GetViewDetails(params ObjectParameter[] parameters)
        {
            return base.GenericWithDetailRepository.ExecuteFunction<TEntityViewDetail>(this.functionNameGetViewDetails, parameters);
        }
    }
}
