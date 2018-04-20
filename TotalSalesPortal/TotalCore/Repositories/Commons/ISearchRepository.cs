using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface ISearchAPIRepository : IGenericAPIRepository
    {
        IList<WarehouseEntry> SearchWarehouseEntries(string aspUserID, DateTime fromDate, DateTime toDate, String codePartA, String codePartB);
    }
}
