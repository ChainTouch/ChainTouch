﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Weitm.Applications.ChainTouchPortal.Controllers
{
    public class AccountsController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}
