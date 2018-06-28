using System.Collections.Generic;

using TotalModel.Models;


namespace TotalCore.Repositories.Generals
{
    public interface IUserReferenceRepository
    {
    }

    public interface IUserReferenceAPIRepository : IGenericAPIRepository
    {
        IList<UserTree> GetUserTrees(int? id, int? activeOption);
    }
}
