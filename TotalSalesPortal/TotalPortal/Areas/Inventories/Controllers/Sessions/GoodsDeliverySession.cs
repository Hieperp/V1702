using System.Web;

namespace TotalPortal.Areas.Inventories.Controllers.Sessions
{
    public class GoodsDeliverySession
    {
        public static string GetDriver(HttpContextBase context)
        {
            if (context.Session["GoodsDelivery-Driver"] == null)
                return null;
            else
                return (string)context.Session["GoodsDelivery-Driver"];
        }

        public static void SetDriver(HttpContextBase context, int driverID, string driverName)
        {
            context.Session["GoodsDelivery-Driver"] = driverID.ToString() + "#@#" + driverName;
        }

        public static string GetCollector(HttpContextBase context)
        {
            if (context.Session["GoodsDelivery-Collector"] == null)
                return null;
            else
                return (string)context.Session["GoodsDelivery-Collector"];
        }

        public static void SetCollector(HttpContextBase context, int collectorID, string collectorName)
        {
            context.Session["GoodsDelivery-Collector"] = collectorID.ToString() + "#@#" + collectorName;
        }
    }
}