using System.Web.Mvc;

namespace TotalPortal.Areas.Accounts
{
    public class AccountsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Accounts";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Accounts_default",
                "Accounts/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Accounts_default_Two_Parameters",
                "Accounts/{controller}/{action}/{id}/{detailId}",
                new { action = "Index", id = UrlParameter.Optional, detailId = UrlParameter.Optional }
            );

        }
    }
}