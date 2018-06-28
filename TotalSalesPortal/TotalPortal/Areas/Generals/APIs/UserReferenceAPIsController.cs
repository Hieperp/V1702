using System.Web.Mvc;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Generals;

namespace TotalPortal.Areas.Generals.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class UserReferenceAPIsController : Controller
    {
        private readonly IUserReferenceAPIRepository userReferenceAPIRepository;

        public UserReferenceAPIsController(IUserReferenceAPIRepository userReferenceAPIRepository)
        {
            this.userReferenceAPIRepository = userReferenceAPIRepository;
        }

        public JsonResult GetUserTrees(int? id)
        {
            IList<UserTree> userTrees = this.userReferenceAPIRepository.GetUserTrees(id, (int)GlobalEnums.ActiveOption.Both);
            return Json(userTrees, JsonRequestBehavior.AllowGet);
        }
    }
}