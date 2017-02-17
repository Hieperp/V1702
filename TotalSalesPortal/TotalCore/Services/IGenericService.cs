using TotalBase.Enums;
using TotalModel;
using TotalDTO;


namespace TotalCore.Services
{
    public interface IGenericService<TEntity, TDto, TPrimitiveDto>: IBaseService

        where TEntity : class, IPrimitiveEntity, IBaseEntity, new()
        where TDto : class, TPrimitiveDto
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
    {
        TEntity GetByID(int id);

        bool GlobalLocked(TDto dto);
        bool Editable(TDto dto);
        bool Approvable(TDto dto);
        bool UnApprovable(TDto dto);
        bool Voidable(TDto dto);
        bool UnVoidable(TDto dto);
        bool Deletable(TDto dto);

        bool Save(TDto dto);
        
        bool ToggleApproved(TDto dto);
        bool ToggleVoid(TDto dto);
        bool ToggleVoidDetail(TDto dto, int detailID, bool inActivePartial, int voidTypeID);

        bool Delete(int id);

        bool Alter(TDto dto);

        void PreSaveRoutines(TDto dto);
    }
}
