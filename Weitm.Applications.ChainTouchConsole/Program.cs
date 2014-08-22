using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

using Weitm.Modules.CommonData.Demo;
using Weitm.Modules.ChainTouch.Models;
using Weitm.Modules.Membership;
using Weitm.Modules.Membership.Models;
using System.Threading;

using Weitm.Modules.BaiduMap.LbsCloud.Api;

namespace Weitm.Applications.ChainTouchConsole
{
    class Program
    {
        private static WeitmMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        static void Main(string[] args)
        {
            //LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);

            //BuildIndutrialParkProperty(new Guid("627B64C3-98BA-42F4-ABE9-C060F2F618D1"));
            //BuildDeveloperProperty(new Guid("FAD75A49-E40C-4FDA-B1FF-BBB32E09F996"));
            
            //RandomizeProperties();
            //GeoSearch.Nearby("ieRhhd5LwrP2xW1xZtgarD7U", 75115, "121.5150750000,31.3092870000", "", 3, 1000, "", "", "", 0, 50, "", "");
            RebultProperties();
        }

        static void RebultProperties()
        {
            var cities = GeoCommonCity.GetDemoCityData();
            var content = new WeitmChainTouchEntities();
            var properties = content.Properties;
            int index = 1;
            int count = properties.Count();
            var rnd = new Random();
            foreach (var property in properties)
            {
                Console.WriteLine("{0}/{1}...", index++, count);
                var city = cities.Where(p => 
                    p.Province == property.Province.Replace("\r","").Replace("\n","") &&
                    p.City == property.Municipal.Replace("\r", "").Replace("\n", "")).SingleOrDefault();
                if (city != null)
                {
                    property.NorthLatitude = city.NorthLatitude + rnd.NextDouble() * 0.1 - 0.05;
                    property.EastLongitude = city.EastLongitude + rnd.NextDouble() * 0.1 - 0.05;
                }
            }
            content.SaveChanges();
        }

        static void RandomizeProperties()
        {
            var content= new WeitmChainTouchEntities();
            var properties = content.Properties;
            int index = 1;
            int count = properties.Count();
            var rnd =  new Random();
            foreach(var property in properties)
            {
                Console.WriteLine("{0}/{1}...", index++, count);
                property.Rent = Convert.ToDecimal(rnd.NextDouble() * 1 + 0.5);
                property.TotalLandSize = Convert.ToDecimal(rnd.NextDouble() * 495000 + 5000);
                property.LeasableArea = Convert.ToDecimal(rnd.NextDouble() * 495000 + 5000);
                property.ManagementFee = Convert.ToDecimal("0.1");
            }
            content.SaveChanges();
        }

        static void BuildDeveloperProperty(Guid id)
        {
            var cities = GeoCommonCity.GetDemoCityData();
            int count = cities.Count();
            var rnd = new Random();
            int j = 1;

                //找几个地儿加进来
            int n = 20;
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("{0}/{1}", j++, count);
                var city = cities[rnd.Next(0, count - 1)];
                var property = new Property()
                {
                    Id = Guid.NewGuid(),
                    Address = city.Province + city.City + "附近",
                    LeasableArea = 0,
                    GrossFloorArea = 0,
                    FloorLoading = 0,
                    EastLongitude = city.EastLongitude + rnd.NextDouble() * 0.2 - 0.1,
                    NorthLatitude = city.NorthLatitude + rnd.NextDouble() * 0.2 - 0.1,
                    Province = city.Province,
                    Municipal = city.City,
                    District = "市中心",
                    CreateTime = DateTime.Now,
                    ContactIds = "",
                    CompleteDate = "",
                    ColumnSpaceWidth = 0,
                    AvailableDate = "",
                    ClearHeight = 0,
                    ColumnSpace = "",
                    ColumnSpaceHeight = 0,
                    ManagementFee = 0,
                    ManagementFeeIncludedInRent = false,
                    Name = city.Province + city.City + "物业",
                    NumberOfFloors = 0,
                    PowerCapacity = 0,
                    Rent = 0,
                    PropertyStructure = "",
                    SponsorId = id,
                    TotalLandSize = 0,
                    Type = rnd.NextDouble() > 0.5 ? "warehouse" : "workshop"
                };

                Property.Add(property);
            }
        }

        static void BuildIndutrialParkProperty(Guid industrialParkId)
        {
            var cities = GeoCommonCity.GetDemoCityData();
            int count = cities.Count();
            var rnd = new Random();
            int j = 1;

            var industrialPark = IndustrialPark.Get(industrialParkId);
                //找几个地儿加进来
            int n = 20;
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(j++);
                var property = new Property()
                {
                    Id = Guid.NewGuid(),
                    Address = industrialPark.Province + industrialPark.Municipal + "附近",
                    LeasableArea = 0,
                    GrossFloorArea = 0,
                    FloorLoading = 0,
                    EastLongitude = industrialPark.EastLongitude + rnd.NextDouble() * 0.2 - 0.1,
                    NorthLatitude = industrialPark.NorthLatitude + rnd.NextDouble() * 0.2 - 0.1,
                    Province = industrialPark.Province,
                    Municipal = industrialPark.Municipal,
                    District = "市中心",
                    CreateTime = DateTime.Now,
                    ContactIds = "",
                    CompleteDate = "",
                    ColumnSpaceWidth = 0,
                    AvailableDate = "",
                    ClearHeight = 0,
                    ColumnSpace = "",
                    ColumnSpaceHeight = 0,
                    ManagementFee = 0,
                    ManagementFeeIncludedInRent = false,
                    Name = industrialPark.Province + industrialPark.Municipal + "物业",
                    NumberOfFloors = 0,
                    PowerCapacity = 0,
                    Rent = 0,
                    PropertyStructure = "",
                    SponsorId = industrialPark.Id,
                    TotalLandSize = 0,
                    Type = rnd.NextDouble() > 0.5 ? "warehouse" : "workshop"
                };

                Property.Add(property);
            }
        }

        static void BuildIndutrialParkProperties()
        {
            var cities = GeoCommonCity.GetDemoCityData();
            int count = cities.Count();
            var rnd = new Random();
            int j = 1;

            foreach (var industrialPark in IndustrialPark.Get())
            {
                Console.WriteLine("{0}/{1}", j++, count);
                //找几个地儿加进来
                int n = 10;
                for (int i = 0; i < n; i++)
                {
                    var property = new Property()
                    {
                        Id = Guid.NewGuid(),
                        Address = industrialPark.Province + industrialPark.Municipal + "附近",
                        LeasableArea = 0,
                        GrossFloorArea = 0,
                        FloorLoading = 0,
                        EastLongitude = industrialPark.EastLongitude + rnd.NextDouble() * 20 - 10,
                        NorthLatitude = industrialPark.NorthLatitude + rnd.NextDouble() * 20 - 10,
                        Province = industrialPark.Province,
                        Municipal = industrialPark.Municipal,
                        District = "市中心",
                        CreateTime = DateTime.Now,
                        ContactIds = "",
                        CompleteDate = "",
                        ColumnSpaceWidth = 0,
                        AvailableDate = "",
                        ClearHeight = 0,
                        ColumnSpace = "",
                        ColumnSpaceHeight = 0,
                        ManagementFee = 0,
                        ManagementFeeIncludedInRent = false,
                        Name = industrialPark.Province + industrialPark.Municipal + "物业",
                        NumberOfFloors = 0,
                        PowerCapacity = 0,
                        Rent = 0,
                        PropertyStructure = "",
                        SponsorId = industrialPark.Id,
                        TotalLandSize = 0,
                        Type = rnd.NextDouble() > 0.5 ? "warehouse" : "workshop"
                    };

                    Property.Add(property);
                }
            }
        }
                
        static void BuildDeveloperProperties()
        {
            var cities = GeoCommonCity.GetDemoCityData();
            int count = cities.Count();
            var rnd = new Random();
            int j = 1;

            foreach (var developer in Developer.Get())
            {
                Console.WriteLine("{0}/{1}", j++, count);
                //找几个地儿加进来
                int n = 10;
                for(int i = 0;i<n;i++)
                {
                    var city = cities[rnd.Next(0,count-1)];
                    var property = new Property()
                    {
                        Id = Guid.NewGuid(),
                        Address = city.Province + city.City + "附近",
                        LeasableArea = 0,
                        GrossFloorArea = 0,
                        FloorLoading = 0,
                        EastLongitude = city.EastLongitude + rnd.NextDouble() * 20 - 10,
                        NorthLatitude = city.NorthLatitude + rnd.NextDouble() * 20 - 10,
                        Province = city.Province,
                        Municipal = city.City,
                        District = "市中心",
                        CreateTime = DateTime.Now,
                        ContactIds = "",
                        CompleteDate = "",
                        ColumnSpaceWidth = 0,
                        AvailableDate = "",
                        ClearHeight = 0,
                        ColumnSpace = "",
                        ColumnSpaceHeight = 0,
                        ManagementFee = 0,
                        ManagementFeeIncludedInRent = false,
                        Name = city.Province + city.City + "物业",
                        NumberOfFloors = 0,
                        PowerCapacity = 0,
                        Rent = 0,
                        PropertyStructure = "",
                        SponsorId = developer.Id,
                        TotalLandSize = 0,
                        Type = rnd.NextDouble() > 0.5 ? "warehouse" : "workshop"
                    };

                    Property.Add(property);
                }
            }
        }

        static void BuildIndustrialParks()
        {
            var cities = GeoCommonCity.GetDemoCityData();
            int i = 1;
            int count = cities.Count();
            foreach (var city in cities)
            {
                Console.WriteLine("{0}/{1}", i, count);
                string username = "industrialpark" + i.ToString("000") + "@chaintouch.com";
                string password = "chaintouch123";
                WebSecurity.CreateUserAndAccount(username, password,
                        new
                        {
                            Email = username,
                            NickName = username.Split('@')[0],
                            UserName = username
                        });
                Roles.AddUserToRole(username, "Users");
                Roles.AddUserToRole(username, "IndustrialParks");
                Guid userId = WebSecurity.GetUserId(username);

                var industrialPark = new IndustrialPark()
                {
                    Id = userId,
                    Name = city.Province + city.City + "工业园",
                    Address = city.Province + city.City + "市中心",
                    CreateTime = DateTime.Now.AddDays(-2),
                    EstablishedYear = 2000,
                    JurisdictionAreaSize = 0,
                    MncPresence = city.Province + city.City + "市中心",
                    Others = city.Province + city.City + "工业园",
                    PillarIndustry = city.Province + city.City + "工业园",
                    Province = city.Province,
                    Municipal = city.City,
                    District = "市中心",
                    EastLongitude = city.EastLongitude,
                    NorthLatitude = city.NorthLatitude
                };
                IndustrialPark.Add(industrialPark);
                i++;
            }
        }

        static void BuildDevelopers()
        {
            var cities = GeoCommonCity.GetDemoCityData();
            int i = 1;
            int count = cities.Count();
            foreach (var city in cities)
            {
                Console.WriteLine("{0}/{1}", i, count);
                string username = "developer" + i.ToString("000") + "@chaintouch.com";
                string password = "chaintouch123";
                WebSecurity.CreateUserAndAccount(username, password,
                        new
                        {
                            Email = username,
                            NickName = username.Split('@')[0],
                            UserName = username
                        });
                Roles.AddUserToRole(username, "Users");
                Roles.AddUserToRole(username, "Developers");
                Guid userId = WebSecurity.GetUserId(username);

                var developer = new Developer()
                {
                    Id = userId,
                    BriefIntroduction = city.Province + city.City + "开发商",
                    ChinaHeadquarterAddress = city.Province + city.City + "市中心",
                    CreateTime = DateTime.Now.AddDays(-2),
                    DevelopedGfaInChina = 0,
                    Name = city.Province + city.City + "开发商",
                    NumberOfProjectsInChina = 0,
                    PropertyType = "workshop and warehouse"
                };
                Developer.Add(developer);
                i++;
            }
        }
    }
}
