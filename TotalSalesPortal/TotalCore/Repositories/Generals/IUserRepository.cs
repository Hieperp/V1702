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

        IList<UserTree> GetUserTrees(int? id, int? activeOption);
    }
}
