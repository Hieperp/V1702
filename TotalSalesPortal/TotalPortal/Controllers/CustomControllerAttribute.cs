using System.Web.Mvc;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity;

using TotalBase.Enums;
using TotalPortal.Models;

namespace TotalPortal.Controllers
{
    public class GenericSimpleAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var Db = new ApplicationDbContext();

            BaseController baseController = filterContext.Controller as BaseController;

            string aspUserID = filterContext.HttpContext.User.Identity.GetUserId();

            baseController.BaseService.UserID = Db.Users.Where(w => w.Id == aspUserID).FirstOrDefault().UserID;

            base.OnAuthorization(filterContext);
        }
    }



    public class AccessLevelAuthorizeAttribute : AuthorizeAttribute
    {
        private BaseController baseController;

        private GlobalEnums.AccessLevel accessLevel;

        public AccessLevelAuthorizeAttribute()
            : this(GlobalEnums.AccessLevel.Editable)
        { }

        public AccessLevelAuthorizeAttribute(GlobalEnums.AccessLevel accessLevel)
        {
            this.accessLevel = accessLevel;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            this.baseController = filterContext.Controller as BaseController;

            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if (!authorized) return false;

            return this.baseController.BaseService.GetAccessLevel() >= this.accessLevel;
        }
    }





    public class OnResultExecutingFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);

            if (filterContext.Result is ViewResult)
            {
                var controller = filterContext.Controller as BaseController;
                controller.AddRequireJsOptions();
            }
        }
    }








    public abstract class ModelStateTempDataTransfer : ActionFilterAttribute
    {
        protected static readonly string Key = typeof(ModelStateTempDataTransfer).FullName;
    }

    public class ExportModelStateToTempData : ModelStateTempDataTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Only export when ModelState is not valid
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                //Export if we are redirecting
                if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
                {
                    filterContext.Controller.TempData[Key] = filterContext.Controller.ViewData.ModelState;
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }

    public class ImportModelStateFromTempData : ModelStateTempDataTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ModelStateDictionary modelState = filterContext.Controller.TempData[Key] as ModelStateDictionary;

            if (modelState != null)
            {
                //Only Import if we are viewing
                if (filterContext.Result is ViewResult)
                {
                    filterContext.Controller.ViewData.ModelState.Merge(modelState);
                }
                else
                {
                    //Otherwise remove it.
                    filterContext.Controller.TempData.Remove(Key);
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }




}