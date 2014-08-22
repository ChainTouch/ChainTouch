using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Weitm.Modules.Article.Models;
using Weitm.Modules.WebSite.Models;

namespace Weitm.Applications.ChainTouch.Controllers
{
    public class MarketNewsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Show(Guid id)
        {
            var article = Article.Get(id);
            article.DoHit();
            Visitor.Record(id,  Request);
            return View(article);
        }

    }
}
