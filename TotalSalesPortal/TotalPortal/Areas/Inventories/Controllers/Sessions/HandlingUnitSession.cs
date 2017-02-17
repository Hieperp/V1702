using System.Web;

namespace TotalPortal.Areas.Inventories.Controllers.Sessions
{
    public class HandlingUnitSession
    {
        public static string GetPackagingStaff(HttpContextBase context)
        {
            if (context.Session["HandlingUnit-PackagingStaff"] == null)
                return null;
            else
                return (string)context.Session["HandlingUnit-PackagingStaff"];
        }

        public static void SetPackagingStaff(HttpContextBase context, int packagingStaffID, string packagingStaffName)
        {
            context.Session["HandlingUnit-PackagingStaff"] = packagingStaffID.ToString() + "#@#" + packagingStaffName;
        }
    }
}