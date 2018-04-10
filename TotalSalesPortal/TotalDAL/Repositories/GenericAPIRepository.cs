using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalCore.Repositories;
using TotalModel.Models;


namespace TotalDAL.Repositories
{
    public class GenericAPIRepository : BaseRepository, IGenericAPIRepository
    {
        private readonly string functionNameGetEntityIndexes;

        public GenericAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities, string functionNameGetEntityIndexes)
            : base(totalSalesPortalEntities)
        {
            this.functionNameGetEntityIndexes = functionNameGetEntityIndexes;
        }

        public virtual ICollection<TEntityIndex> GetEntityIndexes<TEntityIndex>(string aspUserID, DateTime fromDate, DateTime toDate)
        {
            return base.ExecuteFunction<TEntityIndex>(this.functionNameGetEntityIndexes, this.GetEntityIndexParameters(aspUserID, fromDate, toDate));
        }

        protected virtual ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {
            return new ObjectParameter[] { new ObjectParameter("AspUserID", aspUserID), new ObjectParameter("FromDate", fromDate), new ObjectParameter("ToDate", toDate) };
        }

    }
}
