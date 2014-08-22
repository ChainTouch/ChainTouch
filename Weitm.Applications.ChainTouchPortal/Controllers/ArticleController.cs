using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Weitm.Modules.Article.Models;

namespace Weitm.Applications.ChainTouchPortal.Controllers
{
    public class ArticleController : Controller
    {
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var article = Article.Get(id);
            return View(article);
        }

    }
}
