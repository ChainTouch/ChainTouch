using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Weitm.Modules.ChainTouch.Models;

namespace Weitm.Applications.ChainTouch.Controllers
{
    public class IndustrialParkController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(Guid id)
        {
            return View(IndustrialPark.Get(id));
        }
    }
}
