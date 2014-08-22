using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;

namespace Weitm.Modules.ChainTouch.Models
{
    public partial class IndustrialPark
    {
        #region 静态方法

        public static IndustrialPark Get(Guid id)
        {
            var content = new WeitmChainTouchEntities();
            return content.IndustrialParks
                .Where(p => p.Id == id)
                .SingleOrDefault();
        }

        public static List<IndustrialPark> Get()
        {
            var content = new WeitmChainTouchEntities();
            return content.IndustrialParks
                .OrderByDescending(p => p.CreateTime)
                .ToList();
        }

        public static void Add(IndustrialPark industrialPark)
        {
            var content = new WeitmChainTouchEntities();
            content.IndustrialParks.Add(industrialPark);
            content.SaveChanges();
        }

        public static void Remove(Guid id)
        {
            var content = new WeitmChainTouchEntities();
            var industrialPark = new IndustrialPark { Id = id };
            content.IndustrialParks.Attach(industrialPark);
            content.IndustrialParks.Remove(industrialPark);
            content.SaveChanges();
        }

        public static void Update(IndustrialPark industrialPark)
        {
            var content = new WeitmChainTouchEntities();
            content.Configuration.ValidateOnSaveEnabled = false;
            var item = new IndustrialPark()
            {
                Id = industrialPark.Id,
                Address = industrialPark.Address,
                District = industrialPark.District,
                EastLongitude = industrialPark.EastLongitude,
                EstablishedYear = industrialPark.EstablishedYear,
                JurisdictionAreaSize = industrialPark.JurisdictionAreaSize,
                MncPresence = industrialPark.MncPresence,
                Municipal = industrialPark.Municipal,
                Name = industrialPark.Name,
                NorthLatitude = industrialPark.NorthLatitude,
                Province = industrialPark.Province,
                PillarIndustry = industrialPark.PillarIndustry,
                Others = industrialPark.Others
            };
            DbEntityEntry<IndustrialPark> entry = content.Entry<IndustrialPark>(item);
            entry.State = System.Data.Entity.EntityState.Unchanged;
            entry.Property("Address").IsModified = true;
            entry.Property("District").IsModified = true;
            entry.Property("EastLongitude").IsModified = true;
            entry.Property("EstablishedYear").IsModified = true;
            entry.Property("JurisdictionAreaSize").IsModified = true;
            entry.Property("MncPresence").IsModified = true;
            entry.Property("Municipal").IsModified = true;
            entry.Property("Name").IsModified = true;
            entry.Property("NorthLatitude").IsModified = true;
            entry.Property("Province").IsModified = true;
            entry.Property("PillarIndustry").IsModified = true;
            entry.Property("Others").IsModified = true;
            content.SaveChanges();
        }
        #endregion

        #region 动态方法



        #endregion

        #region 对象属性

        public List<Property> Properties
        {
            get
            {
                return Property.FindBySponser(this.Id);
            }
        }

        public List<Contact> Contacts
        {
            get
            {
                return Contact.FindByTarget(this.Id);
            }
        }

        #endregion

    }
}
