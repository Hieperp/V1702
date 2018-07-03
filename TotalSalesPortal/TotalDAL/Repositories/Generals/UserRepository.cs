using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Generals;


namespace TotalDAL.Repositories.Generals
{
    public class UserRepository : IUserRepository //GenericWithDetailRepository<UserReference, UserReferenceDetail>, 
    {
        //public UserRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        //    : base(totalSalesPortalEntities, "UserReferenceEditable", "UserReferenceApproved", null, "UserReferenceVoidable")
        //{
        //}
    }








    public class UserAPIRepository : GenericAPIRepository, IUserAPIRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public UserAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetUserIndexes")
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {

            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { new ObjectParameter("ActiveOption", 0), baseParameters[0], baseParameters[1], baseParameters[2] };

            this.RepositoryBag.Remove("ActiveOption");

            return objectParameters;
        }

        public IList<TaskIndex> GetTaskIndexes()
        {
            return this.TotalSalesPortalEntities.GetTaskIndexes().ToList();
        }


        public IList<UserAccessControl> GetUserAccessControls(int? userID, int? nmvnTaskID)
        {
            return this.TotalSalesPortalEntities.GetUserAccessControls(userID, nmvnTaskID).ToList();
        }

        public int SaveUserAccessControls(int? accessControlID, int? accessLevel, bool? approvalPermitted, bool? unApprovalPermitted, bool? voidablePermitted, bool? unVoidablePermitted, bool? showDiscount)
        {
            return this.TotalSalesPortalEntities.SaveUserAccessControls(accessControlID, accessLevel, approvalPermitted, unApprovalPermitted, voidablePermitted, unVoidablePermitted, showDiscount);
        }


        public IList<UserReportControl> GetUserReportControls(int? userID)
        {
            return this.TotalSalesPortalEntities.GetUserReportControls(userID).ToList();
        }

        public int SaveUserReportControls(int? reportControlID, bool? enabled)
        {
            return this.TotalSalesPortalEntities.SaveUserReportControls(reportControlID, enabled);
        }


        public IList<UserTree> GetUserTrees(int? id, int? activeOption)
        {
            return this.TotalSalesPortalEntities.GetUserTrees(id, activeOption).ToList();
        }

    }

}
