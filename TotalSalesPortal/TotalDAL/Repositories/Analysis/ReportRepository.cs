using TotalModel.Models;
using TotalCore.Repositories.Analysis;

namespace TotalDAL.Repositories.Analysis
{
    public class ReportAPIRepository : GenericAPIRepository, IReportAPIRepository
    {
        public ReportAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetReportIndexes")
        {
        }
    }
}
