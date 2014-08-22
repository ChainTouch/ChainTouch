using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Weitm.Applications.ChainTouchPortal.Controllers
{

    [Authorize]
    public class CloudController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
