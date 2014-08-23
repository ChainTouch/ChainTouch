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
                    name = p.Name,
                    rent = p.Rent, 
                    area = p.LeasableArea, 
                    size = p.TotalLandSize,
                    cost = p.ManagementFee
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

            double lat = Convert.ToDouble(Request.Form["current-lat"].ToString());
            double lon = Convert.ToDouble(Request.Form["current-lon"].ToString());
            int radius = Convert.ToInt32(Request.Form["Radius"].ToString());

            var service = new PropertyGeoService();
            var properties = service.Nearby(lat, lon, radius);

            #region Search Filters
            //Province:any
            if(Request.Form["Province"]!=null)
            {
                string provinceCode = Request.Form["Province"].ToString();
                if (provinceCode != "any" && provinceCode !="loading...")
                {
                    string provinceName = GeoProvince.Get(Convert.ToInt32(provinceCode)).ProvinceName;
                    properties = properties.Where(p => p.Province == provinceName).ToList();
                }
            }


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
            if (Request.Form["Structure"] != null)
            {
                string structure = Request.Form["Structure"].ToString();
                if (structure != "any")
                {
                    properties = properties.Where(p => p.PropertyStructure == structure).ToList();
                }
            }

            //Size:any
            if (Request.Form["Size"] != null)
            {
                string size = Request.Form["Size"].ToString();
                switch (size)
                {
                    case "1000-":
                        properties = properties.Where(p => p.TotalLandSize <= 1000).ToList();
                        break;
                    case "1000-1500":
                        properties = properties.Where(p => p.TotalLandSize >= 1000 && p.TotalLandSize <= 1500).ToList();
                        break;
                    case "1500-2000":
                        properties = properties.Where(p => p.TotalLandSize >= 1500 && p.TotalLandSize <= 2000 ).ToList();
                        break;
                    case "2000+":
                        properties = properties.Where(p => p.TotalLandSize >= 2000).ToList();
                        break;
                }
            }

            //RentalDown:
            if (Request.Form["RentalDown"] != null)
            {
                string rentDown = Request.Form["RentalDown"].ToString();
                if(rentDown!="")
                {
                    decimal rentDownNumber = Convert.ToDecimal(rentDown);
                    properties = properties.Where(p => p.Rent >= rentDownNumber).ToList();
                }
            }
            //RentalUp:
            if (Request.Form["RentalUp"] != null)
            {
                string rentUp = Request.Form["RentalUp"].ToString();
                if (rentUp != "")
                {
                    decimal rentUpNumber = Convert.ToDecimal(rentUp);
                    properties = properties.Where(p => p.Rent <= rentUpNumber).ToList();
                }
            }
            //CostDown:
            if (Request.Form["CostDown"] != null)
            {
                string costDown = Request.Form["CostDown"].ToString();
                if (costDown != "")
                {
                    decimal costDownNumber = Convert.ToDecimal(costDown);
                    properties = properties.Where(p => p.ManagementFee >= costDownNumber).ToList();
                }
            }
            //CostUp:
            if (Request.Form["CostUp"] != null)
            {
                string costUp = Request.Form["CostUp"].ToString();
                if (costUp != "")
                {
                    decimal costUpNumber = Convert.ToDecimal(costUp);
                    properties = properties.Where(p => p.ManagementFee <= costUpNumber).ToList();
                }
            }

            #endregion

            #region Sorting
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
                    name = p.Name,
                    rent = p.Rent, 
                    area = p.LeasableArea, 
                    size = p.TotalLandSize,
                    cost = p.ManagementFee
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
            //string region = "";
            //var properties = service.Local(region);

            //使用本地解决方案
            var properties = Property.Get();

            #region Search Filters
            //Province:any
            if (Request.Form["Province"] != null)
            {
                string provinceCode = Request.Form["Province"].ToString();
                if (provinceCode != "any" && provinceCode != "loading...")
                {
                    string provinceName = GeoProvince.Get(Convert.ToInt32(provinceCode)).ProvinceName;
                    properties = properties.Where(p => p.Province == provinceName).ToList();
                }
            }

            ////City:any
            if (Request.Form["City"] != null)
            {
                string cityCode = Request.Form["City"].ToString();
                if (cityCode != "any" && cityCode != "loading...")
                {
                    string cityName = GeoCity.Get(Convert.ToInt32(cityCode)).CityName;
                    properties = properties.Where(p => p.Municipal == cityName).ToList();
                }
            }

            //Disctrict:any
            if (Request.Form["Disctrict"] != null)
            {
                string areaCode = Request.Form["Disctrict"].ToString();
                if (areaCode != "any" && areaCode != "loading...")
                {
                    string areaName = GeoArea.Get(Convert.ToInt32(areaCode)).AreaName;
                    properties = properties.Where(p => p.District == areaName).ToList();
                }
            }
            
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
            if (Request.Form["Structure"] != null)
            {
                string structure = Request.Form["Structure"].ToString();
                if (structure != "any")
                {
                    properties = properties.Where(p => p.PropertyStructure == structure).ToList();
                }
            }

            //Size:any
            if (Request.Form["Size"] != null)
            {
                string size = Request.Form["Size"].ToString();
                switch (size)
                {
                    case "1000-":
                        properties = properties.Where(p => p.TotalLandSize <= 1000).ToList();
                        break;
                    case "1000-1500":
                        properties = properties.Where(p => p.TotalLandSize >= 1000 && p.TotalLandSize <= 1500).ToList();
                        break;
                    case "1500-2000":
                        properties = properties.Where(p => p.TotalLandSize >= 1500 && p.TotalLandSize <= 2000).ToList();
                        break;
                    case "2000+":
                        properties = properties.Where(p => p.TotalLandSize >= 2000).ToList();
                        break;
                }
            }

            //RentalDown:
            if (Request.Form["RentalDown"] != null)
            {
                string rentDown = Request.Form["RentalDown"].ToString();
                if (rentDown != "")
                {
                    decimal rentDownNumber = Convert.ToDecimal(rentDown);
                    properties = properties.Where(p => p.Rent >= rentDownNumber).ToList();
                }
            }
            //RentalUp:
            if (Request.Form["RentalUp"] != null)
            {
                string rentUp = Request.Form["RentalUp"].ToString();
                if (rentUp != "")
                {
                    decimal rentUpNumber = Convert.ToDecimal(rentUp);
                    properties = properties.Where(p => p.Rent <= rentUpNumber).ToList();
                }
            }
            //CostDown:
            if (Request.Form["CostDown"] != null)
            {
                string costDown = Request.Form["CostDown"].ToString();
                if (costDown != "")
                {
                    decimal costDownNumber = Convert.ToDecimal(costDown);
                    properties = properties.Where(p => p.ManagementFee >= costDownNumber).ToList();
                }
            }
            //CostUp:
            if (Request.Form["CostUp"] != null)
            {
                string costUp = Request.Form["CostUp"].ToString();
                if (costUp != "")
                {
                    decimal costUpNumber = Convert.ToDecimal(costUp);
                    properties = properties.Where(p => p.ManagementFee <= costUpNumber).ToList();
                }
            }
            #endregion 

            #region Sorting
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
