using System.Web.Mvc;

namespace TotalPortal.Areas.Purchases
{
    public class PurchasesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Purchases";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Purchases_default",
                "Purchases/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Purchases_default_Two_Parameters",
                "Purchases/{controller}/{action}/{id}/{detailId}",
                new { action = "Index", id = UrlParameter.Optional, detailId = UrlParameter.Optional }
            );
        }
    }
}