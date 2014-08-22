using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Weitm.Applications.ChainTouch.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("ListView");
        }

        public ActionResult ListView()
        {
            return View();
        }

        public ActionResult GridView()
        {
            return View();
        }

        public ActionResult MapView()
        {
            return View();
        }

        public ActionResult Advanced()
        {
            return View();
        }
    }
}