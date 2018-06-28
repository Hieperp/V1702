using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Generals;


namespace TotalDAL.Repositories.Generals
{
    public class UserReferenceRepository : IUserReferenceRepository //GenericWithDetailRepository<UserReference, UserReferenceDetail>, 
    {
        //public UserReferenceRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        //    : base(totalSalesPortalEntities, "UserReferenceEditable", "UserReferenceApproved", null, "UserReferenceVoidable")
        //{
        //}
    }








    public class UserReferenceAPIRepository : GenericAPIRepository, IUserReferenceAPIRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public UserReferenceAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetUserIndexes")
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public IList<UserTree> GetUserTrees(int? id, int? activeOption)
        {
            return this.TotalSalesPortalEntities.GetUserTrees(id, activeOption).ToList();
        }

    }

}
