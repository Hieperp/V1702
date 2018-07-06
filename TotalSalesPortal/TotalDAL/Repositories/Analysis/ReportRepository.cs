using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Analysis;

namespace TotalDAL.Repositories.Analysis
{
    public class ReportAPIRepository : IReportAPIRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public ReportAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public List<Report> GetReports()
        {
            return this.totalSalesPortalEntities.Reports.ToList();
        }

    }
}
