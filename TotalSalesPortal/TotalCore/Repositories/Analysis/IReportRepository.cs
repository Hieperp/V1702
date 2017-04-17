using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Analysis
{
    public interface IReportRepository
    {
        List<Report> GetReports();
    }
}
