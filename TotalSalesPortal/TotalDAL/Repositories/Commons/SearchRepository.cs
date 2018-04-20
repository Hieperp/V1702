using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class SearchAPIRepository : GenericAPIRepository, ISearchAPIRepository
    {
        public SearchAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "SearchIndexes")
        {
        }

        public IList<WarehouseEntry> SearchWarehouseEntries(string aspUserID, DateTime fromDate, DateTime toDate, String codePartA, String codePartB)
        {
            List<WarehouseEntry> warehouseEntry = this.TotalSalesPortalEntities.SearchWarehouseEntries(aspUserID, fromDate, toDate, codePartA, codePartB).ToList();

            return warehouseEntry;
        }
    }

}
