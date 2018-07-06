using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Analysis
{
    public class Report
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public Report(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetReportIndexes();
        }


        private void GetReportIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Reports.ReportID, Reports.ReportUniqueID, Reports.ReportGroupID, Reports.ReportGroupName, Reports.ReportName, Reports.ReportURL, Reports.ReportTypeID, Reports.SerialID, Reports.Remarks " + "\r\n";
            queryString = queryString + "       FROM        AspNetUsers " + "\r\n";
            queryString = queryString + "                   INNER JOIN ReportControls ON AspNetUsers.Id = @AspUserID AND ReportControls.Enabled = 1 AND AspNetUsers.UserID = ReportControls.UserID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Reports ON ReportControls.ReportID = Reports.ReportID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetReportIndexes", queryString);
        }
        
    }
}
