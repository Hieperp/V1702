using System.Collections.Generic;

using TotalModel.Models;


namespace TotalCore.Repositories.Generals
{
    public interface IUserRepository
    {
    }

    public interface IUserAPIRepository : IGenericAPIRepository
    {
        IList<TaskIndex> GetTaskIndexes();

        int UserRegister(int? userID, int? organizationalUnitID, int? sameOUAccessLevel, int? sameLocationAccessLevel, int? otherOUAccessLevel);
        int UserUnregister(int? userID, string userName, string organizationalUnitName);
        int UserToggleVoid(int? userID, bool? inActive);

        bool UserEditable(int? userID);

        IList<UserAccessControl> GetUserAccessControls(int? userID, int? nmvnTaskID);
        int SaveUserAccessControls(int? accessControlID, int? accessLevel, bool? approvalPermitted, bool? unApprovalPermitted, bool? voidablePermitted, bool? unVoidablePermitted, bool? showDiscount);

        IList<UserReportControl> GetUserReportControls(int? userID);
        int SaveUserReportControls(int? reportControlID, bool? enabled);

        IList<LocationOrganizationalUnit> GetLocationOrganizationalUnits(int? nothing);        
    }
}
