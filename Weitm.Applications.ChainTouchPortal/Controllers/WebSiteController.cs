using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Weitm.Applications.ChainTouchPortal.Controllers
{

    [Authorize]
    public class WebSiteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TracePageView()
        {
            return View();
        }
        public ActionResult TracePropertySearch()
        {
            return View();
        }
    }
}
