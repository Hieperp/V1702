using System.Web;

namespace TotalPortal.Areas.Accounts.Controllers.Sessions
{
    public class ReceiptSession
    {
        public static string GetCashier(HttpContextBase context)
        {
            if (context.Session["GoodsIssue-Cashier"] == null)
                return null;
            else
                return (string)context.Session["GoodsIssue-Cashier"];
        }

        public static void SetCashier(HttpContextBase context, int CashierID, string CashierName)
        {
            context.Session["GoodsIssue-Cashier"] = CashierID.ToString() + "#@#" + CashierName;
        }
    }
}