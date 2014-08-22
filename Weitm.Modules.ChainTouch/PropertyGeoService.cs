using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Weitm.Modules.ChainTouch.Models;
using Weitm.Modules.BaiduMap.LbsCloud.Api;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Weitm.Modules.ChainTouch
{
    public class PropertyGeoService
    {
        public string ak{get;set;}
        public int geotable_id{get;set;}

        public PropertyGeoService()
        {
            this.ak = "ieRhhd5LwrP2xW1xZtgarD7U";
            this.geotable_id = 75115;

        }
        
        public PropertyGeoService(string ak, int geotable_id)
        {
            this.ak = ak;
            this.geotable_id = geotable_id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat">北纬：默认大学路</param>
        /// <param name="lon">东经：默认大学路</param>
        /// <param name="radius">搜索半径：默认1公里</param>
        /// <returns></returns>
        public List<Property> Nearby(double lat = 31.3092870000, double lon = 121.5150750000, int radius = 1000)
        {
            JObject items = GeoSearch.Nearby(this.ak, this.geotable_id,
                string.Format("{0},{1}", lon, lat),
                "", 3, radius, "", "", "", 0, 50, "", "");
            JArray contents = (JArray)items["contents"];
            var result = contents.Select(p => 
                Property.Get(
                    new Guid(
                        p["property_id"].ToString()
                    )
                )
            ).ToList();
            return result;
        }

        public List<Property> Local(string region)
        {
            JObject items = GeoSearch.Local(this.ak, this.geotable_id,
                "",3,region, "", "", "", 0, 50, "", "");
            JArray contents = (JArray)items["contents"];
            var result = contents.Select(p =>
                Property.Get(
                    new Guid(
                        p["property_id"].ToString()
                    )
                )
            ).ToList();
            return result;
        }
    }
}
