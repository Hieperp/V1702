using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using TotalModel.Models;
using TotalCore.Repositories.Sessions;

using TotalPortal.Models;
using TotalPortal.APIs.Sessions;
using TotalPortal.ViewModels.Sessions;


namespace TotalPortal.APIs.Sessions
{
    //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class MenuAPIsController : Controller
    {
        private readonly IModuleRepository moduleRepository;
        private readonly IModuleDetailRepository moduleDetailRepository;

        public MenuAPIsController(IModuleRepository moduleRepository, IModuleDetailRepository moduleDetailRepository)
        {
            this.moduleRepository = moduleRepository;
            this.moduleDetailRepository = moduleDetailRepository;
        }


        public ActionResult TaskMenu(int? moduleID)
        {
            if (moduleID == null)
            {
                moduleID = MenuSession.GetModuleID(this.HttpContext);
            }
            else
            {
                MenuSession.SetModuleID(this.HttpContext, (int)moduleID);
            }


            int taskID = MenuSession.GetTaskID(this.HttpContext);


            ViewBag.TaskID = taskID;


            //var moduleDetail = moduleDetailRepository.GetModuleDetailByID((int)moduleID);
            var moduleDetail = moduleDetailRepository.GetAllModuleDetails().ToList().Where(w => (w.ModuleID == moduleID || (w.ModuleID == 2 && moduleID == 0)) && w.InActive == 0).OrderBy(o => o.SerialID);
            return PartialView(moduleDetail);
        }

        [ChildActionOnly]
        //[OutputCache(NoStore = true, Location = OutputCacheLocation.Server, Duration = 100)]
        public ActionResult MainMenu()
        {
            string moduleName = MenuSession.GetModuleName(this.HttpContext);
            string taskName = MenuSession.GetTaskName(this.HttpContext);
            string taskController = MenuSession.GetTaskController(this.HttpContext);
            ViewBag.ModuleName = moduleName;
            ViewBag.TaskName = taskName;
            ViewBag.TaskController = taskController;

            ViewBag.GlobalFromDate = HomeSession.GetGlobalFromDate(this.HttpContext);
            ViewBag.GlobalToDate = HomeSession.GetGlobalToDate(this.HttpContext);



            //BEGIN: Cho nay: sau nay can phai bo di, vi lam nhu the nay khong hay ho gi ca. Thay vao do, se thua ke tu base controller -> de lay userid, locationid, location official name
            var Db = new ApplicationDbContext();

            string aspUserID = User.Identity.GetUserId();
            int userID = Db.Users.Where(w => w.Id == aspUserID).FirstOrDefault().UserID;
            ViewBag.LocationName = this.moduleRepository.GetLocationName(userID);
            //BEGIN: Cho nay: sau nay can phai bo di, vi lam nhu the nay khong hay ho gi ca. Thay vao do, se thua ke tu base controller -> de lay userid, locationid, location official name



            var moduleMaster = moduleRepository.GetAllModules().OrderByDescending(p => p.SerialID);

            return PartialView(moduleMaster);
        }

        public ActionResult SetTask(int? taskID, string taskName, string taskController)
        {
            if (taskID == null)
            {
                return Json(new { Success = 0 });
            }

            int moduleID = MenuSession.GetModuleID(this.HttpContext);
            Module module = moduleRepository.GetModuleByID((int)moduleID);

            MenuSession.SetModuleName(this.HttpContext, module.Description);

            MenuSession.SetTaskID(this.HttpContext, (int)taskID);
            MenuSession.SetTaskName(this.HttpContext, taskName);
            MenuSession.SetTaskController(this.HttpContext, taskController);

            return Json(new { Success = 1 });
        }
    }
}