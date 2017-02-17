using System.Web;

namespace TotalPortal.Areas.Inventories.Controllers.Sessions
{
    public class GoodsIssueSession
    {
        public static string GetStorekeeper(HttpContextBase context)
        {
            if (context.Session["GoodsIssue-Storekeeper"] == null)
                return null;
            else
                return (string)context.Session["GoodsIssue-Storekeeper"];
        }

        public static void SetStorekeeper(HttpContextBase context, int storekeeperID, string storekeeperName)
        {
            context.Session["GoodsIssue-Storekeeper"] = storekeeperID.ToString() + "#@#" + storekeeperName;
        }
    }
}