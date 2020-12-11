using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        //GET : /Home/Error
        /// <summary>
        /// This window is for showing Generic Errors!
        /// </summary>
        public ActionResult Error()
        {
            //TempData["ErrorMessage"] should be set
            return View();
        }
    }
}
