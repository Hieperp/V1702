using System;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Data.Entity;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories;





using TotalModel; //for Loading (09/07/2015) - let review and optimize Loading laster





namespace TotalDAL.Repositories
{
    public class GenericRepository<TEntity> : BaseRepository, IGenericRepository<TEntity>
        where TEntity : class, IAccessControlAttribute //IAccessControlAttribute: for Loading (09/07/2015) - let review and optimize Loading laster
    {
        private DbSet<TEntity> modelDbSet = null;

        private readonly string functionNameEditable;
        private readonly string functionNameApproved;
        private readonly string functionNameDeletable;
        private readonly string functionNameVoidable;

        public GenericRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : this(totalSalesPortalEntities, null) { }

        public GenericRepository(TotalSalesPortalEntities totalSalesPortalEntities, string functionNameEditable)
            : this(totalSalesPortalEntities, functionNameEditable, null) { }

        public GenericRepository(TotalSalesPortalEntities totalSalesPortalEntities, string functionNameEditable, string functionNameApproved)
            : this(totalSalesPortalEntities, functionNameEditable, functionNameApproved, null) { }

        public GenericRepository(TotalSalesPortalEntities totalSalesPortalEntities, string functionNameEditable, string functionNameApproved, string functionNameDeletable)
            : this(totalSalesPortalEntities, functionNameEditable, functionNameApproved, functionNameDeletable, null) { }

        public GenericRepository(TotalSalesPortalEntities totalSalesPortalEntities, string functionNameEditable, string functionNameApproved, string functionNameDeletable, string functionNameVoidable)
            : base(totalSalesPortalEntities)
        {
            modelDbSet = this.TotalSalesPortalEntities.Set<TEntity>();

            this.functionNameEditable = functionNameEditable;
            this.functionNameApproved = functionNameApproved;
            this.functionNameDeletable = functionNameDeletable;
            this.functionNameVoidable = functionNameVoidable;
        }



        public DbContextTransaction BeginTransaction()
        {
            return this.TotalSalesPortalEntities.Database.BeginTransaction();
        }








        public virtual IQueryable<TEntity> Loading(string aspUserID, GlobalEnums.NmvnTaskID nmvnTaskID)//for Loading (09/07/2015) - let review and optimize Loading laster
        {
            int userID = this.TotalSalesPortalEntities.AspNetUsers.Where(w => w.Id == aspUserID).FirstOrDefault().UserID;
            return this.modelDbSet.Where(w => this.TotalSalesPortalEntities.AccessControls.Where(acl => acl.UserID == userID && acl.NMVNTaskID == (int)nmvnTaskID && acl.AccessLevel > 0).Select(s => s.OrganizationalUnitID).Contains(w.OrganizationalUnitID));
        }







        public IQueryable<TEntity> GetAll()
        {
            return this.modelDbSet;
        }

        public TEntity GetByID(int id)
        {
            return this.modelDbSet.Find(id);
        }



        public TEntity GetEntity(params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntity<TEntity>(includes);
        }
        public TEntity GetEntity(bool proxyCreationEnabled, params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntity<TEntity>(proxyCreationEnabled, includes);
        }
        public TEntity GetEntity(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntity<TEntity>(predicate, includes);
        }
        public TEntity GetEntity(bool proxyCreationEnabled, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntity<TEntity>(proxyCreationEnabled, predicate, includes);
        }




        public ICollection<TEntity> GetEntities(params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntities<TEntity>(includes);
        }
        public ICollection<TEntity> GetEntities(bool proxyCreationEnabled, params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntities<TEntity>(proxyCreationEnabled, includes);
        }
        public ICollection<TEntity> GetEntities(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntities<TEntity>(predicate, includes);
        }
        public ICollection<TEntity> GetEntities(bool proxyCreationEnabled, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetEntities<TEntity>(proxyCreationEnabled, predicate, includes);
        }



        public DateTime GetEditLockedDate(int? locationID, GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            if (nmvnTaskID == GlobalEnums.NmvnTaskID.ServiceContract)
                return DateTime.Now.AddYears(-20);
            else
            {
                DateTime? lockedDate = this.TotalSalesPortalEntities.Locations.Where(w => w.LocationID == locationID).FirstOrDefault().LockedDate;
                if (lockedDate == null || lockedDate <= new DateTime(2016, 1, 1)) lockedDate = new DateTime(2016, 1, 1);

                return (DateTime)lockedDate;
            }
        }


        public GlobalEnums.AccessLevel GetAccessLevel(int? userID, GlobalEnums.NmvnTaskID nmvnTaskID, int? organizationalUnitID)
        {
            if (userID == null || userID == 0 || (int)nmvnTaskID == 0) return GlobalEnums.AccessLevel.Deny;

            int? accessLevel = this.TotalSalesPortalEntities.GetAccessLevel(userID, (int)nmvnTaskID, organizationalUnitID).Single();
            return accessLevel == null || accessLevel == (int)GlobalEnums.AccessLevel.Deny ? GlobalEnums.AccessLevel.Deny : (accessLevel == (int)GlobalEnums.AccessLevel.Readable ? GlobalEnums.AccessLevel.Readable : (accessLevel == (int)GlobalEnums.AccessLevel.Editable ? GlobalEnums.AccessLevel.Editable : GlobalEnums.AccessLevel.Deny));
        }


        public bool GetApprovalPermitted(int? userID, GlobalEnums.NmvnTaskID nmvnTaskID, int? organizationalUnitID)
        {
            if (userID == null || userID == 0 || (int)nmvnTaskID == 0) return false;

            bool? approvalPermitted = this.TotalSalesPortalEntities.GetApprovalPermitted(userID, (int)nmvnTaskID, organizationalUnitID).Single();
            return approvalPermitted == null ? false : (bool)approvalPermitted;
        }

        public bool GetUnApprovalPermitted(int? userID, GlobalEnums.NmvnTaskID nmvnTaskID, int? organizationalUnitID)
        {
            if (userID == null || userID == 0 || (int)nmvnTaskID == 0) return false;

            bool? unApprovalPermitted = this.TotalSalesPortalEntities.GetUnApprovalPermitted(userID, (int)nmvnTaskID, organizationalUnitID).Single();
            return unApprovalPermitted == null ? false : (bool)unApprovalPermitted;
        }

        public bool GetVoidablePermitted(int? userID, GlobalEnums.NmvnTaskID nmvnTaskID, int? organizationalUnitID)
        {
            if (userID == null || userID == 0 || (int)nmvnTaskID == 0) return false;

            bool? voidablePermitted = this.TotalSalesPortalEntities.GetVoidablePermitted(userID, (int)nmvnTaskID, organizationalUnitID).Single();
            return voidablePermitted == null ? false : (bool)voidablePermitted;
        }

        public bool GetUnVoidablePermitted(int? userID, GlobalEnums.NmvnTaskID nmvnTaskID, int? organizationalUnitID)
        {
            if (userID == null || userID == 0 || (int)nmvnTaskID == 0) return false;

            bool? unVoidablePermitted = this.TotalSalesPortalEntities.GetUnVoidablePermitted(userID, (int)nmvnTaskID, organizationalUnitID).Single();
            return unVoidablePermitted == null ? false : (bool)unVoidablePermitted;
        }

        public bool GetShowDiscount(int? userID, GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            if (userID == null || userID == 0 || (int)nmvnTaskID == 0) return false;

            bool? showDiscount = this.TotalSalesPortalEntities.GetShowDiscount(userID, (int)nmvnTaskID).Single();
            return showDiscount == null ? false : (bool)showDiscount;
        }

        public bool GetApproved(int id)
        {
            return this.CheckExisting(id, this.functionNameApproved);
        }

        public bool GetEditable(int id)
        {
            return !this.CheckExisting(id, this.functionNameEditable);
        }

        public bool GetDeletable(int id)
        {
            return !this.CheckExisting(id, this.functionNameDeletable);
        }


        public bool GetVoidable(int id)
        {
            return !this.CheckExisting(id, this.functionNameVoidable);
        }


        public bool CheckExisting(int id, string functionName)
        {
            return this.GetExisting(id, functionName) != null;
        }

        public string GetExisting(int id, string functionName)
        {
            if (id <= 0 || functionName == null || functionName == "")
                return null;
            else
            {
                ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("EntityID", id) };
                var foundEntityID = base.ExecuteFunction<string>(functionName, parameters);

                return foundEntityID.FirstOrDefault();
            }
        }


        public TEntity Add(TEntity entity)
        {
            return this.modelDbSet.Add(entity);
        }

        public TEntity Remove(TEntity entity)
        {
            return this.modelDbSet.Remove(entity);
        }

        public int SaveChanges()
        {
            return this.TotalSalesPortalEntities.SaveChanges();
        }


    }
}
