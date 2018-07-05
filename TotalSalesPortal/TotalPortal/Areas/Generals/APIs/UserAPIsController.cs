using System;
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

        public JsonResult GetUserIndexes([DataSourceRequest] DataSourceRequest request, bool showInActiveUsers)
        {
            this.userAPIRepository.RepositoryBag["ActiveOption"] = showInActiveUsers ? -1 : 0;
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


        [HttpPost]
        public JsonResult UserToggleVoid(int? userID, bool? inActive)
        {
            try
            {
                this.userAPIRepository.UserToggleVoid(userID, inActive);
                return Json(new { SaveResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { SaveResult = "Lỗi. Vui lòng đóng phần mềm và thử lại. " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetUserAccessControls([DataSourceRequest] DataSourceRequest request, int? userID, int? nmvnTaskID)
        {
            IList<UserAccessControl> userAccessControls = this.userAPIRepository.GetUserAccessControls(userID, nmvnTaskID);
            DataSourceResult response = userAccessControls.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveUserAccessControls(int? accessControlID, string accessLevelName, bool? approvalPermitted, bool? unApprovalPermitted, bool? voidablePermitted, bool? unVoidablePermitted, bool? showDiscount)
        {
            try
            {
                int accessLevel = (int)(accessLevelName == "ReadOnly" ? GlobalEnums.AccessLevel.Readable : (accessLevelName == "Editable" ? GlobalEnums.AccessLevel.Editable : GlobalEnums.AccessLevel.Deny));

                this.userAPIRepository.SaveUserAccessControls(accessControlID, accessLevel, approvalPermitted, unApprovalPermitted, voidablePermitted, unVoidablePermitted, showDiscount);
                return Json(new { SaveResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { SaveResult = "Lỗi. Vui lòng đóng phần mềm và thử lại. " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult GetUserReportControls([DataSourceRequest] DataSourceRequest request, int? userID)
        {
            IList<UserReportControl> userReportControls = this.userAPIRepository.GetUserReportControls(userID);
            DataSourceResult response = userReportControls.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveUserReportControls(int? reportControlID, bool? enabled)
        {
            try
            {
                this.userAPIRepository.SaveUserReportControls(reportControlID, enabled);
                return Json(new { SaveResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { SaveResult = "Lỗi. Vui lòng đóng phần mềm và thử lại. " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}