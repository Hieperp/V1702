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
        
        IList<UserAccessControl> GetUserAccessControls(int? userID, int? nmvnTaskID);
        int SaveUserAccessControls(int? accessControlID, int? accessLevel, bool? approvalPermitted, bool? unApprovalPermitted, bool? voidablePermitted, bool? unVoidablePermitted, bool? showDiscount);

        IList<UserReportControl> GetUserReportControls(int? userID);
        int SaveUserReportControls(int? reportControlID, bool? enabled);

        IList<UserTree> GetUserTrees(int? id, int? activeOption);
    }
}
