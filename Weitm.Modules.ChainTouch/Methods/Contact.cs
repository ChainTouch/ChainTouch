using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;

namespace Weitm.Modules.ChainTouch.Models
{
    public partial class Contact
    {
        #region 静态方法

        public static Contact Get(Guid id)
        {
            var content = new WeitmChainTouchEntities();
            return content.Contacts
                .Where(p => p.Id == id)
                .SingleOrDefault();
        }

        public static List<Contact> Get()
        {
            var content = new WeitmChainTouchEntities();
            return content.Contacts
                .OrderByDescending(p => p.CreateTime)
                .ToList();
        }

        public static List<Contact> FindByTarget(Guid targetId)
        {
            var content = new WeitmChainTouchEntities();
            return content.Contacts
                .Where(p=>p.TargetId == targetId)
                .OrderByDescending(p => p.CreateTime)
                .ToList();

        }

        public static void Add(Contact contact)
        {
            var content = new WeitmChainTouchEntities();
            content.Contacts.Add(contact);
            content.SaveChanges();
        }

        public static void Remove(Guid id)
        {
            var content = new WeitmChainTouchEntities();
            var contact = new Contact { Id = id };
            content.Contacts.Attach(contact);
            content.Contacts.Remove(contact);
            content.SaveChanges();
        }

        public static void Update(Contact contact)
        {
            var content = new WeitmChainTouchEntities();
            content.Configuration.ValidateOnSaveEnabled = false;
            var item = new Contact()
            {
                Id = contact.Id,
                Title = contact.Title,
                CreateTime = contact.CreateTime,
                Department = contact.Department,
                EmailAddress = contact.EmailAddress,
                FirstName = contact.FirstName,
                Landline = contact.Landline,
                LastName = contact.LastName,
                MobilePhone = contact.MobilePhone,
                TargetId = contact.TargetId
            };
            DbEntityEntry<Contact> entry = content.Entry<Contact>(item);
            entry.State = System.Data.Entity.EntityState.Unchanged;
            entry.Property("Title").IsModified = true;
            entry.Property("CreateTime").IsModified = true;
            entry.Property("Department").IsModified = true;
            entry.Property("EmailAddress").IsModified = true;
            entry.Property("FirstName").IsModified = true;
            entry.Property("Landline").IsModified = true;
            entry.Property("LastName").IsModified = true;
            entry.Property("MobilePhone").IsModified = true;
            entry.Property("TargetId").IsModified = true;
            content.SaveChanges();
        }

        #endregion

        #region 动态方法

        #endregion

        #region 对象属性

        #endregion
    }
}
