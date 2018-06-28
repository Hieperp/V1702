using System.Web.Mvc;

namespace TotalPortal.Areas.Generals
{
    public class GeneralsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Generals";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Generals_default",
                "Generals/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Generals_default_Two_Parameters",
                "Generals/{controller}/{action}/{id}/{detailId}",
                new { action = "Index", id = UrlParameter.Optional, detailId = UrlParameter.Optional }
            );

        }
    }
}