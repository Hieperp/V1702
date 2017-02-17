using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TotalPortal.APIs
{
    public class GenericAPIsController : Controller
    {
        // GET: GenericApi
        public ActionResult Index()
        {
            return View();
        }
    }
}