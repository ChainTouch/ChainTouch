using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Weitm.Modules.ChainTouch.Models;

namespace Weitm.Applications.ChainTouchPortal.Controllers
{

    [Authorize]
    public class ManagedPropertyController : Controller
    {
        public ActionResult Edit(Guid id)
        {
            var property = Property.Get(id);
            if(Request.HttpMethod=="POST")
            {
                //update
                Property.Update(property);
            }
            return View(property);
        }

    }
}
