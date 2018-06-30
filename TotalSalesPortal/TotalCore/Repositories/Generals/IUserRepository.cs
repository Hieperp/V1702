using System.Collections.Generic;

using TotalModel.Models;


namespace TotalCore.Repositories.Generals
{
    public interface IUserRepository
    {
    }

    public interface IUserAPIRepository : IGenericAPIRepository
    {
        IList<UserTree> GetUserTrees(int? id, int? activeOption);
    }
}
