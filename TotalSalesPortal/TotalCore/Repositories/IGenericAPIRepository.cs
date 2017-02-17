using System;
using System.Collections.Generic;

using TotalBase.Enums;

namespace TotalCore.Repositories
{
    public interface IGenericAPIRepository : IBaseRepository
    {
        ICollection<TEntityIndex> GetEntityIndexes<TEntityIndex>(string aspUserID, DateTime fromDate, DateTime toDate);
    }
}
