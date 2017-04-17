namespace TotalPortal.ViewModels.Helpers
{
    public class PrintViewModel
    {
        public int Id { get; set; }

        public string ServerName { get { return "SERVERCTY"; } }
        public string CatalogName { get { return "TotalSalesPortal"; } }

        public string ReportFolder { get { return "TotalSalesPortal"; } }
        public string ReportPath { get; set; }

        public int PrintOptionID { get; set; }
    }
}