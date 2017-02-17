using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TotalPortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default_Two_Parameters",
                url: "{area}/{controller}/{action}/{id}/{detailId}",
                defaults: new { area = "Inventories", controller = "GoodsIssues", action = "Index", id = UrlParameter.Optional, detailId = UrlParameter.Optional }
            );

        }
    }
}
