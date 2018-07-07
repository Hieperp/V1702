using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using TotalCore.Repositories.Sales;
using TotalPortal.Models;
using TotalPortal.ViewModels.Home;
using TotalPortal.APIs.Sessions;
using TotalModel.Models;
using System.Data.Entity.Core.Objects;
using TotalCore.Repositories;

namespace TotalPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBaseRepository baseRepository;
        public HomeController(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This software is licensed to Vinh Cơ Company Limited.";
            ViewBag.SystemInfos = "";// this.baseRepository.GetSystemInfos();
            ViewBag.SecureSystemInfos = "[@@" + this.baseRepository.GetSystemInfos(true) + "@@]";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Print()
        {
            return View();
        }


        public ActionResult Searches()
        {
            //ViewBag.SelectedEntityID = id == null ? -1 : (int)id;
            //ViewBag.ShowDiscount = this.GenericService.GetShowDiscount();

            OptionViewModel optionViewModel = new OptionViewModel { GlobalFromDate = HomeSession.GetGlobalFromDate(this.HttpContext), GlobalToDate = HomeSession.GetGlobalToDate(this.HttpContext) };

            return View(optionViewModel);
        }




        public ActionResult UserGuide()
        {
            if (User.Identity.IsAuthenticated)
            {
                string aspUserID = User.Identity.GetUserId();

                var Db = new ApplicationDbContext();

                var userID = Db.Users.Where(w => w.Id == aspUserID).FirstOrDefault().UserID;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Options()
        {
            OptionViewModel optionViewModel = new OptionViewModel { GlobalFromDate = HomeSession.GetGlobalFromDate(this.HttpContext), GlobalToDate = HomeSession.GetGlobalToDate(this.HttpContext) };

            return View(optionViewModel);
        }

        [HttpPost]
        public ActionResult Options(OptionViewModel optionViewModel)
        {
            HomeSession.SetGlobalFromDate(this.HttpContext, optionViewModel.GlobalFromDate);
            HomeSession.SetGlobalToDate(this.HttpContext, optionViewModel.GlobalToDate);

            return View("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult LockedDate()
        {
            List<Location> Locations = this.baseRepository.GetEntities<Location>().ToList();

            return View(Locations);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult LockedDate(int locationID, DateTime lockedDate)
        {
            int x = locationID;
            DateTime d = lockedDate;
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("AspUserID", User.Identity.GetUserId()), new ObjectParameter("LocationID", locationID), new ObjectParameter("LockedDate", lockedDate) };
            this.baseRepository.ExecuteFunction("UpdateLockedDate", parameters);

            return Json(new { Success = true });
        }


    }
}