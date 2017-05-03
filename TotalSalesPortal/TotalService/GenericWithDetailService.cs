using System;
using System.Linq;
using System.Data.Entity.Core.Objects;

using AutoMapper;

using TotalBase;
using TotalModel;
using TotalDTO;
using TotalCore.Repositories;
using TotalCore.Services;


namespace TotalService
{
    public class GenericWithDetailService<TEntity, TEntityDetail, TDto, TPrimitiveDto, TDtoDetail> : GenericService<TEntity, TDto, TPrimitiveDto>, IGenericWithDetailService<TEntity, TEntityDetail, TDto, TPrimitiveDto, TDtoDetail>

        where TEntity : class, IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<TEntityDetail>, new()
        where TEntityDetail : class, IPrimitiveEntity, new()
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {

        private readonly IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository;

        private readonly string functionNameToggleVoidDetail;

        public GenericWithDetailService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository)
            : this(genericWithDetailRepository, null)
        {
        }

        public GenericWithDetailService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository, string functionNamePostSaveValidate)
            : this(genericWithDetailRepository, functionNamePostSaveValidate, null)
        {
        }

        public GenericWithDetailService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository, string functionNamePostSaveValidate, string functionNameSaveRelative)
            : this(genericWithDetailRepository, functionNamePostSaveValidate, functionNameSaveRelative, null)
        {
        }

        public GenericWithDetailService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository, string functionNamePostSaveValidate, string functionNameSaveRelative, string functionNameToggleApproved)
            : this(genericWithDetailRepository, functionNamePostSaveValidate, functionNameSaveRelative, functionNameToggleApproved, null)
        {
        }

        public GenericWithDetailService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository, string functionNamePostSaveValidate, string functionNameSaveRelative, string functionNameToggleApproved, string functionNameToggleVoid)
            : this(genericWithDetailRepository, functionNamePostSaveValidate, functionNameSaveRelative, functionNameToggleApproved, functionNameToggleVoid, null)
        {
        }

        public GenericWithDetailService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository, string functionNamePostSaveValidate, string functionNameSaveRelative, string functionNameToggleApproved, string functionNameToggleVoid, string functionNameToggleVoidDetail)
            : base(genericWithDetailRepository, functionNamePostSaveValidate, functionNameSaveRelative, functionNameToggleApproved, functionNameToggleVoid)
        {
            this.genericWithDetailRepository = genericWithDetailRepository;

            this.functionNameToggleVoidDetail = functionNameToggleVoidDetail;
        }

        protected IGenericWithDetailRepository<TEntity, TEntityDetail> GenericWithDetailRepository { get { return this.genericWithDetailRepository; } }

        protected override TEntity SaveMaster(TDto dto)
        {
            TEntity entity = base.SaveMaster(dto);

            this.SaveDetail(dto, entity);

            return entity;
        }

        protected virtual void SaveDetail(TDto dto, TEntity entity)
        {
            if (dto.GetID() > 0) //Edit existing ModelClass
                this.UndoDetail(dto, entity, false);

            this.UpdateDetail(dto, entity);
        }

        protected virtual void UpdateDetail(TDto dto, TEntity entity)
        {
            if (dto.GetDetails() != null && dto.GetDetails().Count > 0)
                dto.GetDetails().Each(detailDTO =>
                {
                    TEntityDetail entityDetail;

                    if (detailDTO.GetID() <= 0 || (entityDetail = entity.GetDetails().First(detailModel => detailModel.GetID() == detailDTO.GetID())) == null)
                    {
                        entityDetail = new TEntityDetail();
                        entity.GetDetails().Add(entityDetail);
                    }

                    Mapper.Map<TDtoDetail, TEntityDetail>(detailDTO, entityDetail);
                });
        }

        protected virtual void UndoDetail(TDto dto, TEntity entity, bool isDelete)
        {
            //Remove saved detail entity which is not in cusrrent dto details collection (The 'saved detail entity' is the entity which is saved in database, The dto details collection: is the new detail collection)
            if (entity.GetID() > 0 && entity.GetDetails().Count > 0)
                if (isDelete || dto.GetDetails() == null || dto.GetDetails().Count == 0)
                    this.genericWithDetailRepository.RemoveRangeDetail(entity.GetDetails());
                else
                    entity.GetDetails().ToList()//Have to use .ToList(): to convert enumerable to List before do remove. To correct this error: Collection was modified; enumeration operation may not execute. 
                            .Where(detailModel => !dto.GetDetails().Any(detailDTO => detailDTO.GetID() == detailModel.GetID()))
                            .Each(deleted => this.genericWithDetailRepository.RemoveDetail(deleted)); //remove deleted details
        }


        protected override void DeleteMaster(TDto dto, TEntity entity)
        {
            base.DeleteMaster(dto, entity);
            this.UndoDetail(dto, entity, true);
        }











        public override bool ToggleVoidDetail(TDto dto, int detailID, bool inActivePartial, int voidTypeID)
        {
            using (var dbContextTransaction = this.genericWithDetailRepository.BeginTransaction())
            {
                try
                {
                    if ((!inActivePartial && !this.Voidable(dto)) || (inActivePartial && !this.UnVoidable(dto))) throw new System.ArgumentException("Lỗi " + (inActivePartial ? "hủy " : "") + "duyệt dữ liệu", "Bạn không có quyền hoặc dữ liệu này đã bị khóa.");

                    this.ToggleVoidDetailMe(dto, detailID, inActivePartial, voidTypeID);

                    this.genericWithDetailRepository.SaveChanges();

                    dbContextTransaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw ex;
                }
            }
        }


        protected virtual void ToggleVoidDetailMe(TDto dto, int detailID, bool inActivePartial, int voidTypeID)
        {
            if (this.functionNameToggleVoidDetail != null && this.functionNameToggleVoidDetail != "")
            {
                ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("EntityID", dto.GetID()), new ObjectParameter("EntityDetailID", detailID), new ObjectParameter("InActivePartial", !inActivePartial), new ObjectParameter("VoidTypeID", voidTypeID) };
                if (this.genericWithDetailRepository.ExecuteFunction(this.functionNameToggleVoidDetail, parameters) < 2) throw new System.ArgumentException("Lỗi", "Chứng từ không tồn tại hoặc đã " + (inActivePartial ? "phục hồi lệnh" : "") + "hủy"); 
            }
            else
                throw new System.ArgumentException("Lỗi", "Hệ thống không cho phép thực hiện tác vụ này.");
        }







    }
}
