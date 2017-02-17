using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;

using AutoMapper;

using TotalBase.Enums;
using TotalModel;
using TotalModel.Models;
using TotalDTO;
using TotalCore.Repositories;
using TotalCore.Services;


namespace TotalService
{
    public enum SaveRelativeOption
    {
        Undo = -1,
        Update = 1
    }

    public class GenericService<TEntity, TDto, TPrimitiveDto> : BaseService, IGenericService<TEntity, TDto, TPrimitiveDto>

        where TEntity : class, IPrimitiveEntity, IBaseEntity, new()
        where TDto : class, TPrimitiveDto
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
    {

        private readonly IGenericRepository<TEntity> genericRepository;


        private readonly string functionNamePostSaveValidate;
        private readonly string functionNameSaveRelative;
        private readonly string functionNameToggleApproved;
        private readonly string functionNameToggleVoid;

        private readonly GlobalEnums.NmvnTaskID nmvnTaskID;

        public GenericService(IGenericRepository<TEntity> genericRepository)
            : this(genericRepository, null)
        { }

        public GenericService(IGenericRepository<TEntity> genericRepository, string functionNamePostSaveValidate)
            : this(genericRepository, functionNamePostSaveValidate, null)
        { }

        public GenericService(IGenericRepository<TEntity> genericRepository, string functionNamePostSaveValidate, string functionNameSaveRelative)
            : this(genericRepository, functionNamePostSaveValidate, functionNameSaveRelative, null)
        { }

        public GenericService(IGenericRepository<TEntity> genericRepository, string functionNamePostSaveValidate, string functionNameSaveRelative, string functionNameToggleApproved)
            : this(genericRepository, functionNamePostSaveValidate, functionNameSaveRelative, functionNameToggleApproved, null)
        { }

        public GenericService(IGenericRepository<TEntity> genericRepository, string functionNamePostSaveValidate, string functionNameSaveRelative, string functionNameToggleApproved, string functionNameToggleVoid)
            : base(genericRepository)
        {
            this.genericRepository = genericRepository;

            this.functionNamePostSaveValidate = functionNamePostSaveValidate;
            this.functionNameSaveRelative = functionNameSaveRelative;
            this.functionNameToggleApproved = functionNameToggleApproved;
            this.functionNameToggleVoid = functionNameToggleVoid;

            this.nmvnTaskID = (new TPrimitiveDto()).NMVNTaskID;
        }


        public virtual TEntity GetByID(int id)
        {
            return this.genericRepository.GetByID(id);
        }

        public override GlobalEnums.NmvnTaskID NmvnTaskID
        {
            get { return this.nmvnTaskID; }
        }




        public virtual bool GlobalLocked(TDto dto)
        {
            return (dto.EntryDate <= this.genericRepository.GetEditLockedDate(this.LocationID, this.nmvnTaskID));
        }

        public override GlobalEnums.AccessLevel GetAccessLevel(int? organizationalUnitID)
        {
            return this.genericRepository.GetAccessLevel(this.UserID, this.nmvnTaskID, organizationalUnitID);
        }

        public override bool GetApprovalPermitted(int? organizationalUnitID)
        {
            return this.genericRepository.GetApprovalPermitted(this.UserID, this.nmvnTaskID, organizationalUnitID);
        }

        public override bool GetUnApprovalPermitted(int? organizationalUnitID)
        {
            return this.genericRepository.GetUnApprovalPermitted(this.UserID, this.nmvnTaskID, organizationalUnitID);
        }

        public override bool GetVoidablePermitted(int? organizationalUnitID)
        {
            return this.genericRepository.GetVoidablePermitted(this.UserID, this.nmvnTaskID, organizationalUnitID);
        }

        public override bool GetUnVoidablePermitted(int? organizationalUnitID)
        {
            return this.genericRepository.GetUnVoidablePermitted(this.UserID, this.nmvnTaskID, organizationalUnitID);
        }

        public override bool GetShowDiscount()
        {
            return this.genericRepository.GetShowDiscount(this.UserID, this.nmvnTaskID);
        }

        public virtual bool Approvable(TDto dto)
        {
            if (this.GlobalLocked(dto)) return false;
            if (dto.Approved || !this.GetApprovalPermitted(dto.OrganizationalUnitID)) return false;

            return this.genericRepository.GetEditable(dto.GetID());
        }

        public virtual bool UnApprovable(TDto dto)
        {
            if (this.GlobalLocked(dto)) return false;
            if (!dto.Approved || !this.GetUnApprovalPermitted(dto.OrganizationalUnitID)) return false;

            return this.genericRepository.GetEditable(dto.GetID());
        }

        public virtual bool Voidable(TDto dto)
        {
            if (this.GlobalLocked(dto)) return false;
            if (!this.GetVoidablePermitted(dto.OrganizationalUnitID)) return false;

            return this.genericRepository.GetVoidable(dto.GetID());
        }

        public virtual bool UnVoidable(TDto dto)
        {
            if (this.GlobalLocked(dto)) return false;
            if (!this.GetUnVoidablePermitted(dto.OrganizationalUnitID)) return false;

            return this.genericRepository.GetVoidable(dto.GetID());
        }

        public virtual bool Editable(TDto dto)
        {
            if (this.GlobalLocked(dto)) return false;
            if (this.GetAccessLevel(dto.OrganizationalUnitID) != GlobalEnums.AccessLevel.Editable) return false;

            if (this.genericRepository.GetApproved(dto.GetID())) return false;

            return this.genericRepository.GetEditable(dto.GetID());
        }

        public virtual bool Deletable(TDto dto)
        {
            if (!this.Editable(dto)) return false;
            return this.genericRepository.GetDeletable(dto.GetID());
        }


        protected virtual bool TryValidateModel(TDto dto)
        {
            StringBuilder invalidMessage = new StringBuilder();
            this.TryValidateModel(dto, ref invalidMessage);

            if (invalidMessage.ToString().Length > 0) throw new System.ArgumentException("Lỗi dữ liệu", invalidMessage.ToString());

            return true;
        }

        protected virtual bool TryValidateModel(TDto dto, ref StringBuilder invalidMessage)
        {
            //if (dto.EntryDate < new DateTime(2015, 7, 1) || dto.EntryDate > DateTime.Today.AddDays(2)) invalidMessage.Append(" Ngày không hợp lệ;");
            return true;
        }

        public virtual bool Save(TDto dto)
        {
            return this.Save(dto, false);
        }

        /// <summary>
        /// This is a protected method, to be accessible ONLY within its class and by derived class instances
        /// To use this, just call it from the Derived Classes
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="useExistingTransaction"></param>
        /// <returns></returns>
        protected virtual bool Save(TDto dto, bool useExistingTransaction)
        {
            TEntity entity;
            if (useExistingTransaction)
                entity = this.SaveThis(dto);
            else
                using (var dbContextTransaction = this.genericRepository.BeginTransaction())
                {
                    try
                    {
                        entity = this.SaveThis(dto);

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                    }
                }

            dto.SetID(entity.GetID());
            return true;
        }

        public virtual bool ToggleApproved(TDto dto)
        {
            using (var dbContextTransaction = this.genericRepository.BeginTransaction())
            {
                try
                {
                    if ((!dto.Approved && !this.Approvable(dto)) || (dto.Approved && !this.UnApprovable(dto))) throw new System.ArgumentException("Lỗi " + (dto.Approved ? "hủy " : "") + "duyệt dữ liệu", "Bạn không có quyền hoặc dữ liệu này đã bị khóa.");
                                        
                    this.ToggleApprovedMe(dto);

                    this.genericRepository.SaveChanges();

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

        public virtual bool ToggleVoid(TDto dto)
        {
            using (var dbContextTransaction = this.genericRepository.BeginTransaction())
            {
                try
                {
                    if ((!dto.InActive && !this.Voidable(dto)) || (dto.InActive && !this.UnVoidable(dto))) throw new System.ArgumentException("Lỗi " + (dto.InActive ? "hủy " : "") + "duyệt dữ liệu", "Bạn không có quyền hoặc dữ liệu này đã bị khóa.");

                    this.ToggleVoidMe(dto);

                    this.genericRepository.SaveChanges();

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

        public virtual bool ToggleVoidDetail(TDto dto, int detailID, bool inActivePartial, int voidTypeID)
        {
            return false;
        }

        public virtual bool Delete(int id)
        {
            if (id <= 0) return false;

            using (var dbContextTransaction = this.genericRepository.BeginTransaction())
            {
                try
                {
                    TEntity entity = this.genericRepository.GetByID(id);
                    TDto dto = Mapper.Map<TDto>(entity);

                    if (!this.TryValidateModel(dto)) throw new System.ArgumentException("Lỗi xóa dữ liệu", "Dữ liệu này không hợp lệ.");
                    if (!this.Deletable(dto)) throw new System.ArgumentException("Lỗi xóa dữ liệu", "Dữ liệu này không thể xóa được.");

                    this.DeleteMe(dto, entity);

                    this.genericRepository.SaveChanges();

                    this.PostSaveValidate(entity);

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


        public virtual bool Alter(TDto dto)
        {
            using (var dbContextTransaction = this.genericRepository.BeginTransaction())
            {
                try
                {
                    if (this.GlobalLocked(dto)) throw new System.ArgumentException("Lỗi điều chỉnh dữ liệu", "Dữ liệu này đã bị khóa.");
                    if (this.GetAccessLevel(dto.OrganizationalUnitID) != GlobalEnums.AccessLevel.Editable) throw new System.ArgumentException("Lỗi điều chỉnh dữ liệu", "Bạn không có quyền điều chỉnh dữ liệu này.");

                    this.AlterMe(dto);

                    this.genericRepository.SaveChanges();

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
       


        public virtual void PreSaveRoutines(TDto dto)
        {
            if (dto.PreparedPersonID <= 0) throw new System.ArgumentException("Lỗi lưu dữ liệu", "Vui lòng chọn người lập.");

            OrganizationalUnitUser organizationalUnitUser = this.genericRepository.GetEntity<OrganizationalUnitUser>(w => w.UserID == dto.PreparedPersonID && !w.InActive, i => i.OrganizationalUnit);
            if (organizationalUnitUser != null)
            {
                //Hien tai, cac van de security da duoc xem xet OK (tuc la: this.UserID duoc quyen lap du lieu cho dto.PreparedPersonID hay khong can cu vao phan quyen trong table AccessControls, va viec kiem tra phan quyen nay da duoc thuc hien day du khi Save.New va Save.Edit)
                //Dieu do co nghia la this.UserID duoc quyen modify du lieu cho bat ky dto.PreparedPersonID nao (KE CA this.UserID.LocationID <> dto.PreparedPersonID.LocationID), mien la duoc phan quyen trong table AccessControls
                //TUY NHIEN, can phai luu y la: CAC MODULE LIEN QUAN DEN Warehouse. CAC MODULE NAY CHO PHEP Get Data TU Warehouse CO LocationID = this.LocationID (TUC LA LocationID CUA this.UserID) (VI DU: LAY SO LIEU TON KHO CUA 1 MAT HANG CO Warehouse.LocationID = this.LocationID, ...)
                //VI LY DO TREN, TRONG table AccessControls, KHI PHAN QUYEN CAN LUU Y: CHI PHAN QUYEN Editable CHO OrganizationalUnit.LocationID CUNG this.LocationID (*)
                //VA DE CHO AN TOAN, NEU CO KHAI BAO NHAM LAN TRONG table AccessControls THI SE BI PHAT HIEN KHI SAVE, TAI DAY BO SUNG CAU LENH KIEM TRA this.LocationID != organizationalUnitUser.OrganizationalUnit.LocationID DE DAM BAO YEU CAU NEU TREN (TUC YEU CAU DA DUOC DANH DAU * NGAY BEN TREN)
                //CAU LENH NAY: CHI NHAM MUC DICH KIEM TRA NHU VAY MA THOI
                //SAU NAY, NEU CAN PHAI BO CAU LENH NAY DE PHAN QUYEN LINH DONG HON, TUC LA CO THE CAN CU TRONG table AccessControls MA THOI (CHANG HAN, this.UserID duoc quyen modify du lieu cho bat ky dto.PreparedPersonID nao, KE CA this.LocationID <> dto.PreparedPersonID.LocationID) 
                //KHI DO, CAU LENH NAY HOAN TOAN CO THE BO DI, VA KHI DO, CAC MODULE LIEN QUAN DEN Warehouse SE DUOC KIEM SOAT BANG 1 CACH THUC KHAC, DE DAM BAO RANG: DOI VOI MODULE LIEN QUAN DEN Warehouse THI this.LocationID = organizationalUnitUser.OrganizationalUnit.LocationID
                //if (this.LocationID != organizationalUnitUser.OrganizationalUnit.LocationID) throw new System.ArgumentException("Lỗi lưu dữ liệu", "Vui lòng chọn lại người lập hợp lệ.");




                dto.UserID = this.UserID;
                dto.OrganizationalUnitID = organizationalUnitUser.OrganizationalUnitID;
                dto.LocationID = organizationalUnitUser.OrganizationalUnit.LocationID;
            }
            else throw new System.ArgumentException("Lỗi lưu dữ liệu", "Vui lòng chọn người lập.");
        }

        private TEntity SaveThis(TDto dto)
        {
            this.PreSaveRoutines(dto);

            if (!this.TryValidateModel(dto)) throw new System.ArgumentException("Lỗi lưu dữ liệu", "Dữ liệu không hợp lệ.");
            if (!this.Editable(dto)) throw new System.ArgumentException("Lỗi lưu dữ liệu", "Bạn không có quyền lưu lại dữ liệu này.");

            dto.PerformPresaveRule();

            TEntity entity = this.SaveMe(dto);

            this.PostSaveValidate(entity);

            return entity;
        }

        protected virtual TEntity SaveMe(TDto dto)
        {
            TEntity entity = this.SaveMaster(dto);

            if (this.genericRepository.IsDirty())
                entity.EditedDate = DateTime.Now;

            this.genericRepository.SaveChanges();

            this.SaveRelative(entity, SaveRelativeOption.Update);

            return entity;
        }

        protected virtual TEntity SaveMaster(TDto dto)
        {
            TEntity entity;

            if (dto.GetID() > 0) //Edit existing Domain Model
            {
                entity = this.genericRepository.GetByID(dto.GetID());
                if (entity == null) throw new System.ArgumentException("", "Không tìm thấy dữ liệu. Dữ liệu cần điều chỉnh có thể đã bị xóa.");

                if (this.GetAccessLevel(entity.OrganizationalUnitID) != GlobalEnums.AccessLevel.Editable) throw new System.ArgumentException("", "Lưu ý: Bạn không có quyền điều chỉnh dữ liệu của người khác.");

                this.SaveRelative(entity, SaveRelativeOption.Undo);
            }
            else//New Domain Model
            {
                entity = new TEntity();
                entity.CreatedDate = DateTime.Now;

                this.genericRepository.Add(entity);
            }

            //Convert from DTOModel to Domain Model
            Mapper.Map<TPrimitiveDto, TEntity>((TPrimitiveDto)dto, entity);

            return entity;
        }


        protected virtual void ToggleApprovedMe(TDto dto)
        {
            if (this.functionNameToggleApproved != null && this.functionNameToggleApproved != "")
            {
                ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("EntityID", dto.GetID()), new ObjectParameter("Approved", !dto.Approved) };
                if (this.genericRepository.ExecuteFunction(this.functionNameToggleApproved, parameters) < 1) throw new System.ArgumentException("Lỗi", "Chứng từ không tồn tại hoặc đã " + (dto.Approved ? "hủy" : "") + "duyệt");
            }
            else
                throw new System.ArgumentException("Lỗi", "Hệ thống không cho phép thực hiện tác vụ này.");
        }

        protected virtual void ToggleVoidMe(TDto dto)
        {
            if (this.functionNameToggleVoid != null && this.functionNameToggleVoid != "")
            {
                ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("EntityID", dto.GetID()), new ObjectParameter("InActive", !dto.InActive), new ObjectParameter("VoidTypeID", dto.VoidTypeID) };
                if (this.genericRepository.ExecuteFunction(this.functionNameToggleVoid, parameters) < 1) throw new System.ArgumentException("Lỗi", "Chứng từ không tồn tại hoặc đã " + (dto.InActive ? "phục hồi lệnh" : "") + "hủy");
            }
            else
                throw new System.ArgumentException("Lỗi", "Hệ thống không cho phép thực hiện tác vụ này.");
        }


        protected virtual void DeleteMe(TDto dto, TEntity entity)
        {
            this.DeleteMaster(dto, entity);
            this.genericRepository.Remove(entity);
        }


        protected virtual void DeleteMaster(TDto dto, TEntity entity)
        {
            this.SaveRelative(entity, SaveRelativeOption.Undo);
        }


        protected virtual void AlterMe(TDto dto)
        {
            throw new System.ArgumentException("Lỗi điều chỉnh dữ liệu", "Hệ thống không cho phép điều chỉnh dữ liệu này.");
        }


        protected virtual void PostSaveValidate(TEntity entity)
        {
            string foundInvalid = this.genericRepository.GetExisting(entity.GetID(), this.functionNamePostSaveValidate);
            if (foundInvalid != null)
                throw new Exception("Vui lòng kiểm tra: " + foundInvalid);
        }

        protected virtual void SaveRelative(TEntity entity, SaveRelativeOption saveRelativeOption)
        {
            if (this.functionNameSaveRelative != null && this.functionNameSaveRelative != "")
            {
                ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("EntityID", entity.GetID()), new ObjectParameter("SaveRelativeOption", (int)saveRelativeOption) };
                this.genericRepository.ExecuteFunction(this.functionNameSaveRelative, parameters);
            }
        }

    }
}
