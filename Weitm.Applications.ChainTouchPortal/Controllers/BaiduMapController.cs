using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Weitm.Applications.ChainTouchPortal.Controllers
{

    [Authorize]
    public class BaiduMapController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
