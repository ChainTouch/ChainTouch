using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Weitm.Modules.ChainTouch.Models;

namespace Weitm.Applications.ChainTouch.Controllers
{
    public class PropertyController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(Guid id)
        {
            var property = Property.Get(id);
            return View(property);
        }
        
        public ActionResult AjaxLbsResult(string lat, string lon)
        {
            string ak = "ieRhhd5LwrP2xW1xZtgarD7U";
            string geoTableId = "75115";

            string url = @"http://api.map.baidu.com/geosearch/v3/nearby?ak="
                + ak + "&geotable_id=" + geoTableId + "&location=" + lon + "," + lat + "&radius=1000";
            var client = new Weitm.Modules.BaiduMap.Utility.HttpClient();
            string html = client.HttpGet(url);
            //JObject obj = (JObject)JsonConvert.DeserializeObject(html);//http://www.cnblogs.com/mcgrady/archive/2013/06/08/3127781.html
            
            //    contents[0].property_id;
            return View(html);
        }

        public ActionResult Search()
        {
            return View();
        }
    }
}
