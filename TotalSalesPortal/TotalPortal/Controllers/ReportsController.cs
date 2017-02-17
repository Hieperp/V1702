using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using TotalModel.Models;
using RequireJsNet;
using System.Net;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Models;
using TotalCore.Helpers;
using TotalCore.Repositories.Sessions;
using TotalPortal.APIs.Sessions;
using Microsoft.Reporting.WebForms;

namespace TotalPortal.Controllers
{
    public class ReportsController : CoreController
    {
        private readonly IModuleRepository moduleRepository;

        public ReportsController(IModuleRepository moduleRepository)
        {
            this.moduleRepository = moduleRepository;
        }

        public ActionResult Index()
        {
            MenuSession.SetModuleID(this.HttpContext, 9);                

            //RequireJsOptions.Add("LocationID", this.baseService.LocationID, RequireJsOptionsScope.Page);
            RequireJsOptions.Add("NmvnModuleID", 9, RequireJsOptionsScope.Page);
            RequireJsOptions.Add("NmvnTaskID", 0, RequireJsOptionsScope.Page);

            return View();
        }



        public ActionResult Open(int? id)
        {
            

            return View();
        }



    }
}