using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Weitm.Modules.ChainTouch.Models;

namespace Weitm.Applications.ChainTouchPortal.Controllers
{

    [Authorize]
    public class IndustrialParkController : Controller
    {
        public ActionResult EditProfile(Guid ? id)
        {
            var industrialPark = IndustrialPark.Get(id == null ? Weitm.Modules.Membership.WebSecurity.GetUserId(User.Identity.Name) : (Guid)id);
            return View(industrialPark);
        }

    }
}
