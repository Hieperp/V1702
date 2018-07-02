using System.Linq;
using System.Web.Mvc;

using System.Collections.Generic;

using Microsoft.AspNet.Identity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalBase.Enums;
using TotalDTO.Commons;
using TotalModel.Models;

using TotalCore.Repositories.Generals;
using TotalPortal.APIs.Sessions;

namespace TotalPortal.Areas.Generals.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class UserAPIsController : Controller
    {
        private readonly IUserAPIRepository userAPIRepository;

        public UserAPIsController(IUserAPIRepository userAPIRepository)
        {
            this.userAPIRepository = userAPIRepository;
        }

        public JsonResult GetUserIndexes([DataSourceRequest] DataSourceRequest request, int? activeOption)
        {
            this.userAPIRepository.RepositoryBag["ActiveOption"] = 0;
            ICollection<UserIndex> userIndexes = this.userAPIRepository.GetEntityIndexes<UserIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = userIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaskIndexes([DataSourceRequest] DataSourceRequest request)
        {
            IList<TaskIndex> taskIndexes = this.userAPIRepository.GetTaskIndexes();
            DataSourceResult response = taskIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserAccessControls([DataSourceRequest] DataSourceRequest request, int? userID, int? nmvnTaskID)
        {
            IList<UserAccessControl> userAccessControls = this.userAPIRepository.GetUserAccessControls(userID, nmvnTaskID);
            DataSourceResult response = userAccessControls.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserTrees(int? id)
        {
            IList<UserTree> userTrees = this.userAPIRepository.GetUserTrees(id, (int)GlobalEnums.ActiveOption.Both);
            return Json(userTrees, JsonRequestBehavior.AllowGet);
        }
    }
}