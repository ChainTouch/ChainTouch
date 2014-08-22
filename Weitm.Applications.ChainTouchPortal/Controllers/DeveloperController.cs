using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Weitm.Modules.ChainTouch.Models;

namespace Weitm.Applications.ChainTouchPortal.Controllers
{

    [Authorize]
    public class DeveloperController : Controller
    {
        public ActionResult EditProfile(Guid ? id)
        {
            var developer = Developer.Get(id == null ? Weitm.Modules.Membership.WebSecurity.GetUserId(User.Identity.Name) : (Guid)id);
            if(Request.HttpMethod=="POST")
            {
                //update
                Developer.Update(developer);
            }
            return View(developer);
        }

    }
}
