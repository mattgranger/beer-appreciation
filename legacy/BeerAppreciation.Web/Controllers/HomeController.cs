using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BeerAppreciation.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            ViewBag.Title = "Beer Home";
            return PartialView();
        }
    }
}
