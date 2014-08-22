using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Weitm.Modules.Membership;
using Weitm.Modules.ChainTouch;
using Weitm.Modules.ChainTouch.Models;
using Weitm.Modules.CommonData.Models;
using Newtonsoft.Json;

namespace Weitm.Applications.ChainTouch.Controllers
{
    public class ApiDataController : Controller
    {
        public string Test()
        {
            return "Hello World!!!";
        }

        #region 加入对比

        public string AddFavorite(Guid id)
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            Favorite.Insert(id, userId);
            return "success";
        }

        public string RemoveFavorite(Guid id)
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            Favorite.Remove(id, userId);
            return "success";
        }

        #endregion

        #region 地图搜索

        public string Nearby()
        {
            var properties = SolveNearBy();
            return JsonConvert.SerializeObject(
                properties.Select(p => new
                {
                    id = p.Id,
                    lat = p.NorthLatitude,
                    lon = p.EastLongitude,
                    name = p.Name
                }                
            ));
        }

        public ActionResult NearbyHtml()
        {
            var properties = SolveNearBy();
            return View("BuildHtml", properties);
        }

        public List<Property> SolveNearBy()
        {
            //double lat, double lon, int radius;
            var service = new PropertyGeoService();
            var properties = service.Nearby();
            //var properties = service.Nearby(lat, lon, radius);

            //Province:any
            if(Request.Form["Province"]!=null)
            {
                string provinceCode = Request.Form["Province"].ToString();
                if (provinceCode != "any")
                {
                    string provinceName = GeoProvince.Get(Convert.ToInt32(provinceCode)).ProvinceName;
                    properties = properties.Where(p => p.Province == provinceName).ToList();
                }
            }
            //District:
            //Radius:any
            //Type:any
            //Structure:any
            //Size:any
            //RentalDown:
            //RentalUp:
            //CostDown:
            //CostUp:
            //sort-rent
            if (Request.Form["sort-rent"] != null)
            {
                string sortrent = Request.Form["sort-rent"].ToString();
                if (sortrent == "asc")
                {
                    properties = properties.OrderBy(p => p.Rent).ToList();
                }
                else if (sortrent == "desc")
                {
                    properties = properties.OrderByDescending(p => p.Rent).ToList();
                }
            }
            //sort-area
            if (Request.Form["sort-area"] != null)
            {
                string sortarea = Request.Form["sort-area"].ToString();
                if (sortarea == "asc")
                {
                    properties = properties.OrderBy(p => p.LeasableArea).ToList();
                }
                else if (sortarea == "desc")
                {
                    properties = properties.OrderByDescending(p => p.LeasableArea).ToList();
                }
            }
            //sort-size
            if (Request.Form["sort-size"] != null)
            {
                string sortsize = Request.Form["sort-size"].ToString();
                if (sortsize == "asc")
                {
                    properties = properties.OrderBy(p => p.TotalLandSize).ToList();
                }
                else if (sortsize == "desc")
                {
                    properties = properties.OrderByDescending(p => p.TotalLandSize).ToList();
                }
            }
            //sort-cost
            if (Request.Form["sort-cost"] != null)
            {
                string sortcost = Request.Form["sort-cost"].ToString();
                if (sortcost == "asc")
                {
                    properties = properties.OrderBy(p => p.ManagementFee).ToList();
                }
                else if (sortcost == "desc")
                {
                    properties = properties.OrderByDescending(p => p.ManagementFee).ToList();
                }
            }

            return properties;
        }

        public string Local()
        {
            var properties = SolveLocal();
            return JsonConvert.SerializeObject(
                properties.Select(p => new
                {
                    id = p.Id,
                    lat = p.NorthLatitude,
                    lon = p.EastLongitude,
                    name = p.Name
                }
            ));
        }

        public ActionResult LocalHtml()
        {
            var properties = SolveLocal();
            return View("BuildHtml", properties);
        }

        private List<Property> SolveLocal()
        {
            //var service = new PropertyGeoService();
            //使用本地解决方案
            //string region = "";
            //var properties = service.Local(region);

            var properties = Property.Get();

            //Province:any
            if (Request.Form["Province"] != null)
            {
                string provinceCode = Request.Form["Province"].ToString();
                if (provinceCode != "any")
                {
                    string provinceName = GeoProvince.Get(Convert.ToInt32(provinceCode)).ProvinceName;
                    properties = properties.Where(p => p.Province == provinceName).ToList();
                }
            }


            ////City:any
            //if (Request.Form["City"] != null)
            //{
            //    string cityCode = Request.Form["City"].ToString();
            //    if (cityCode != "any")
            //    {
            //        string cityName = GeoCity.Get(Convert.ToInt32(cityCode)).CityName;
            //        properties = properties.Where(p => p.Municipal == cityName).ToList();
            //    }
            //}

            //Disctrict:any

            //Type:any

            if (Request.Form["Type"] != null)
            {
                string type = Request.Form["Type"].ToString();
                if (type != "any")
                {
                    properties = properties.Where(p => p.Type == type).ToList();
                }
            }

            //Structure:any
            //Size:any
            //RentalDown:
            //RentalUp:
            //CostDown:
            //CostUp:

            #region 处理排序
            //sort-rent
            if (Request.Form["sort-rent"] != null)
            {
                string sortrent = Request.Form["sort-rent"].ToString();
                if (sortrent == "asc")
                {
                    properties = properties.OrderBy(p => p.Rent).ToList();
                }
                else if (sortrent == "desc")
                {
                    properties = properties.OrderByDescending(p => p.Rent).ToList();
                }
            }
            //sort-area
            if (Request.Form["sort-area"] != null)
            {
                string sortarea = Request.Form["sort-area"].ToString();
                if (sortarea == "asc")
                {
                    properties = properties.OrderBy(p => p.LeasableArea).ToList();
                }
                else if (sortarea == "desc")
                {
                    properties = properties.OrderByDescending(p => p.LeasableArea).ToList();
                }
            }
            //sort-size
            if (Request.Form["sort-size"] != null)
            {
                string sortsize = Request.Form["sort-size"].ToString();
                if (sortsize == "asc")
                {
                    properties = properties.OrderBy(p => p.TotalLandSize).ToList();
                }
                else if (sortsize == "desc")
                {
                    properties = properties.OrderByDescending(p => p.TotalLandSize).ToList();
                }
            }
            //sort-cost
            if (Request.Form["sort-cost"] != null)
            {
                string sortcost = Request.Form["sort-cost"].ToString();
                if (sortcost == "asc")
                {
                    properties = properties.OrderBy(p => p.ManagementFee).ToList();
                }
                else if (sortcost == "desc")
                {
                    properties = properties.OrderByDescending(p => p.ManagementFee).ToList();
                }
            }
            #endregion

            return properties.Take(20).ToList();
        }

        #endregion

        #region 地理信息
        public string Provinces(int ? id)
        {
            if (id == null)
            {
                return JsonConvert.SerializeObject(
                    GeoProvince.Get().Select(p => new
                    {
                        text = p.ProvinceName,
                        value = p.ProvinceCode
                    }).ToList()
                );
            }
            else
            {
                return JsonConvert.SerializeObject(
                    GeoProvince.Get((int)id).GeoCities.Select(p => new
                    {
                        text = p.CityName,
                        value = p.CityCode
                    }).ToList()
                );
            }
        }

        public string Cities(int id)
        {
            var province = GeoProvince.Get(id);
            var cities = province.GeoCities;
            return JsonConvert.SerializeObject(
                cities.Select(p => new
                {
                    text = p.CityName,
                    value = p.CityCode
                }).ToList()
            );
        }

        public string Areas(int id)
        {
            return JsonConvert.SerializeObject(
                GeoCity.Get(id).GeoAreas.Select(p => new
                {
                    text = p.AreaName,
                    value = p.AreaCode
                }).ToList()
            );
        }
        #endregion
    }
}
