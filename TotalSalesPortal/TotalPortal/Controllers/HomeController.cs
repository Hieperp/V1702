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

        //public HomeController(IDeliveryAdviceRepository deliveryAdviceRepository)
        //{
        //}

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

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


        public ActionResult UserReferences()
        {
            this.AddUserAndRoles1();

            return View();
        }

        public JsonResult GetUserTrees(int? id)
        {
            return null;

            //IList<UserTree> userTrees = this.userReferenceAPIRepository.GetUserTrees(id, (int)GlobalEnums.ActiveOption.Both);
            //return Json(userTrees, JsonRequestBehavior.AllowGet);
        }



        #region AddUserAndRoles: Run only one
        private bool AddUserAndRoles1()
        {
            return true;

            bool success = false;
            var idManager = new IdentityManager();


            success = idManager.CreateRole("Admin");
            if (!success == true) return success; //JUST NEED Admin ONLY

            success = idManager.AddUserToRole("dade9d9f-2a76-4d5d-9322-f4d47a12e50a", "Admin");
            if (!success) return success; //ADD ROLE Admin TO hieperp@gmail.com
            


            return true; //AT VCP: JUST RUN AS ABOVE










            success = idManager.CreateRole("Admin");

            if (!success == true) return success;



            success = idManager.CreateRole("CanEdit");

            if (!success == true) return success;



            success = idManager.CreateRole("User");

            if (!success) return success;








            success = idManager.AddUserToRole("11bf0c11-cc6d-4d78-8d24-c226c11cf89d", "Admin");

            if (!success) return success;



            success = idManager.AddUserToRole("11bf0c11-cc6d-4d78-8d24-c226c11cf89d", "CanEdit");

            if (!success) return success;



            success = idManager.AddUserToRole("11bf0c11-cc6d-4d78-8d24-c226c11cf89d", "User");

            if (!success) return success;



            return success;

        }


        private bool AddUserAndRoles()
        {

            bool success = false;



            var idManager = new IdentityManager();


            success = idManager.CreateRole("Admin");

            if (!success == true) return success;



            success = idManager.CreateRole("CanEdit");

            if (!success == true) return success;



            success = idManager.CreateRole("User");

            if (!success) return success;





            var newUser = new ApplicationUser()

            {
                Email = "tanthanhhotel@gmail.com",

                FirstName = "Tan Thanh",

                LastName = "Admin"

            };



            // Be careful here - you  will need to use a password which will 

            // be valid under the password rules for the application, 

            // or the process will abort:

            success = idManager.CreateUser(newUser, "TanThanh@014");

            if (!success) return success;



            success = idManager.AddUserToRole(newUser.Id, "Admin");

            if (!success) return success;



            success = idManager.AddUserToRole(newUser.Id, "CanEdit");

            if (!success) return success;



            success = idManager.AddUserToRole(newUser.Id, "User");

            if (!success) return success;



            return success;

        }

        #endregion AddUserAndRoles: Run only one
    }
}