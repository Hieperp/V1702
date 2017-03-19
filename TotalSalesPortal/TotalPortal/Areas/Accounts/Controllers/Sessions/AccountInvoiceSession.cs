using System.Web;

namespace TotalPortal.Areas.Accounts.Controllers.Sessions
{
    public class AccountInvoiceSession
    {
        public static string GetVATInvoiceSeries(HttpContextBase context)
        {
            if (context.Session["AccountInvoice-VATInvoiceSeries"] == null)
                return null;
            else
                return (string)context.Session["AccountInvoice-VATInvoiceSeries"];
        }

        public static void SetVATInvoiceSeries(HttpContextBase context, string VATInvoiceSeries)
        {
            context.Session["AccountInvoice-VATInvoiceSeries"] = VATInvoiceSeries;
        }
    }
}