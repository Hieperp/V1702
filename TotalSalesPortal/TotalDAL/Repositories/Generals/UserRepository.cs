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
            ObjectParameter[] objectParameters = new ObjectParameter[] { new ObjectParameter("ActiveOption", this.RepositoryBag.ContainsKey("ActiveOption") && this.RepositoryBag["ActiveOption"] != null ? this.RepositoryBag["ActiveOption"] : false), baseParameters[0], baseParameters[1], baseParameters[2] };

            this.RepositoryBag.Remove("ActiveOption");

            return objectParameters;
        }

        public IList<TaskIndex> GetTaskIndexes()
        {
            return this.TotalSalesPortalEntities.GetTaskIndexes().ToList();
        }

        public int UserRegister(int? userID, int? organizationalUnitID, int? sameOUAccessLevel, int? sameLocationAccessLevel, int? otherOUAccessLevel)
        {
            return this.totalSalesPortalEntities.UserRegister(userID, organizationalUnitID, sameOUAccessLevel, sameLocationAccessLevel, otherOUAccessLevel);
        }

        public int UserUnregister(int? userID, string userName, string organizationalUnitName)
        {
            return this.totalSalesPortalEntities.UserUnregister(userID, userName, organizationalUnitName);
        }

        public int UserToggleVoid(int? userID, bool? inActive)
        {
            return this.totalSalesPortalEntities.UserToggleVoid(userID, inActive);
        }

        public bool UserEditable(int? userID)
        {
            return this.totalSalesPortalEntities.UserEditable(userID).FirstOrDefault() == null;
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

        public IList<LocationOrganizationalUnit> GetLocationOrganizationalUnits(int? nothing)
        {
            return this.TotalSalesPortalEntities.GetLocationOrganizationalUnits(nothing).ToList();
        }
        

    }

}
