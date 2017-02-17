using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using TotalModel.Models;
using TotalCore.Repositories;



using TotalModel; //for Loading (09/07/2015) - let review and optimize Loading laster




namespace TotalDAL.Repositories
{
    public class GenericWithDetailRepository<TEntity, TEntityDetail> : GenericRepository<TEntity>, IGenericWithDetailRepository<TEntity, TEntityDetail>
        where TEntity : class, IAccessControlAttribute //IAccessControlAttribute: for Loading (09/07/2015) - let review and optimize Loading laster
        where TEntityDetail : class
    {
        private DbSet<TEntityDetail> modelDetailDbSet = null;

        public GenericWithDetailRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : this(totalSalesPortalEntities, null) { }

        public GenericWithDetailRepository(TotalSalesPortalEntities totalSalesPortalEntities, string functionNameEditable)
            : this(totalSalesPortalEntities, functionNameEditable, null) { }

        public GenericWithDetailRepository(TotalSalesPortalEntities totalSalesPortalEntities, string functionNameEditable, string functionNameApproved)
            : this(totalSalesPortalEntities, functionNameEditable, functionNameApproved, null) { }

        public GenericWithDetailRepository(TotalSalesPortalEntities totalSalesPortalEntities, string functionNameEditable, string functionNameApproved, string functionNameDeletable)
            : this(totalSalesPortalEntities, functionNameEditable, functionNameApproved, functionNameDeletable, null) { }

        public GenericWithDetailRepository(TotalSalesPortalEntities totalSalesPortalEntities, string functionNameEditable, string functionNameApproved, string functionNameDeletable, string functionNameVoidable)
            : base(totalSalesPortalEntities, functionNameEditable, functionNameApproved, functionNameDeletable, functionNameVoidable)
        {
            modelDetailDbSet = this.TotalSalesPortalEntities.Set<TEntityDetail>();
        }


        public virtual TEntityDetail RemoveDetail(TEntityDetail entityDetail)
        {
            return this.modelDetailDbSet.Remove(entityDetail);
        }

        public virtual IEnumerable<TEntityDetail> RemoveRangeDetail(IEnumerable<TEntityDetail> entityDetails)
        {
            return this.modelDetailDbSet.RemoveRange(entityDetails);
        }
    }
}
