using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Analysis;

namespace TotalDAL.Repositories.Analysis
{
    public class ReportRepository : IReportRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public ReportRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public List<Report> GetReports()
        {
            return this.totalSalesPortalEntities.Reports.ToList();
        }

    }
}
