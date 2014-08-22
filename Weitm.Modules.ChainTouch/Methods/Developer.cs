using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;

namespace Weitm.Modules.ChainTouch.Models
{
    public partial class Developer
    {
        #region 静态方法

        public static Developer Get(Guid id)
        {
            var content = new WeitmChainTouchEntities();
            return content.Developers
                .Where(p => p.Id == id)
                .SingleOrDefault();
        }

        public static List<Developer> Get()
        {
            var content = new WeitmChainTouchEntities();
            return content.Developers
                .OrderByDescending(p => p.CreateTime)
                .ToList();
        }

        public static void Add(Developer developer)
        {
            var content = new WeitmChainTouchEntities();
            content.Developers.Add(developer);
            content.SaveChanges();
        }

        public static void Remove(Guid id)
        {
            var content = new WeitmChainTouchEntities();
            var developer = new Developer { Id = id };
            content.Developers.Attach(developer);
            content.Developers.Remove(developer);
            content.SaveChanges();
        }

        public static void Update(Developer developer)
        {
            var content = new WeitmChainTouchEntities();
            content.Configuration.ValidateOnSaveEnabled = false;
            var item = new Developer()
            {
                Id = developer.Id,
                BriefIntroduction = developer.BriefIntroduction,
                ChinaHeadquarterAddress = developer.ChinaHeadquarterAddress,
                CreateTime = developer.CreateTime,
                DevelopedGfaInChina = developer.DevelopedGfaInChina,
                Name = developer.Name,
                NumberOfProjectsInChina = developer.NumberOfProjectsInChina,
                PropertyType = developer.PropertyType,
            };
            DbEntityEntry<Developer> entry = content.Entry<Developer>(item);
            entry.State = System.Data.Entity.EntityState.Unchanged;
            entry.Property("BriefIntroduction").IsModified = true;
            entry.Property("ChinaHeadquarterAddress").IsModified = true;
            entry.Property("CreateTime").IsModified = true;
            entry.Property("DevelopedGfaInChina").IsModified = true;
            entry.Property("Name").IsModified = true;
            entry.Property("NumberOfProjectsInChina").IsModified = true;
            entry.Property("PropertyType").IsModified = true;
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
