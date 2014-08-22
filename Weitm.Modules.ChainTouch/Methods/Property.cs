using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;

namespace Weitm.Modules.ChainTouch.Models
{
    public partial class Property
    {
        #region 静态方法

        public static Property Get(Guid id)
        {
            var content = new WeitmChainTouchEntities();
            return content.Properties
                .Where(p => p.Id == id)
                .SingleOrDefault();
        }

        public static List<Property> Get()
        {
            var content = new WeitmChainTouchEntities();
            return content.Properties
                .OrderByDescending(p => p.CreateTime)
                .ToList();
        }

        public static List<Property> FindBySponser(Guid sponserId)
        {
            var content = new WeitmChainTouchEntities();
            return content.Properties
                .Where(p=>p.SponsorId == sponserId)
                .OrderByDescending(p => p.CreateTime)
                .ToList();

        }

        public static int CountBySponser(Guid sponserId)
        {
            var content = new WeitmChainTouchEntities();
            return content.Properties
                .Where(p => p.SponsorId == sponserId)
                .Count();

        }

        public static void Add(Property property)
        {
            var content = new WeitmChainTouchEntities();
            content.Properties.Add(property);
            content.SaveChanges();
        }

        public static void Remove(Guid id)
        {
            var content = new WeitmChainTouchEntities();
            var property = new Property { Id = id };
            content.Properties.Attach(property);
            content.Properties.Remove(property);
            content.SaveChanges();
        }

        public static void Update(Property property)
        {
            var content = new WeitmChainTouchEntities();
            content.Configuration.ValidateOnSaveEnabled = false;
            var item = new Property()
            {
                Id = property.Id,
                Address = property.Address,
                ColumnSpace = property.ColumnSpace,
                AvailableDate = property.AvailableDate,
                ClearHeight = property.ClearHeight,
                ColumnSpaceHeight = property.ColumnSpaceHeight,
                ColumnSpaceWidth = property.ColumnSpaceWidth,
                CompleteDate = property.CompleteDate,
                ContactIds = property.ContactIds,
                CreateTime = property.CreateTime,
                District = property.District,
                FloorLoading = property.FloorLoading,
                GrossFloorArea = property.GrossFloorArea,
                LeasableArea = property.LeasableArea,
                ManagementFee = property.ManagementFee,
                ManagementFeeIncludedInRent = property.ManagementFeeIncludedInRent,
                Municipal = property.Municipal,
                Name = property.Name,
                NumberOfFloors = property.NumberOfFloors,
                PowerCapacity = property.PowerCapacity,
                PropertyStructure = property.PropertyStructure,
                Province = property.Province,
                Rent = property.Rent,
                SponsorId = property.SponsorId,
                TotalLandSize = property.TotalLandSize,
                Type = property.Type,
                EastLongitude = property.EastLongitude,
                NorthLatitude = property.NorthLatitude
            };
            DbEntityEntry<Property> entry = content.Entry<Property>(item);
            entry.State = System.Data.Entity.EntityState.Unchanged;
            entry.Property("Address").IsModified = true;
            entry.Property("ColumnSpace").IsModified = true;
            entry.Property("AvailableDate").IsModified = true;
            entry.Property("ClearHeight").IsModified = true;
            entry.Property("ColumnSpaceHeight").IsModified = true;
            entry.Property("ColumnSpaceWidth").IsModified = true;
            entry.Property("CompleteDate").IsModified = true;
            entry.Property("ContactIds").IsModified = true;
            entry.Property("CreateTime").IsModified = true;
            entry.Property("District").IsModified = true;
            entry.Property("FloorLoading").IsModified = true;
            entry.Property("GrossFloorArea").IsModified = true;
            entry.Property("LeasableArea").IsModified = true;
            entry.Property("ManagementFee").IsModified = true;
            entry.Property("ManagementFeeIncludedInRent").IsModified = true;
            entry.Property("Municipal").IsModified = true;
            entry.Property("NumberOfFloors").IsModified = true;
            entry.Property("PowerCapacity").IsModified = true;
            entry.Property("PropertyStructure").IsModified = true;
            entry.Property("Province").IsModified = true;
            entry.Property("Rent").IsModified = true;
            entry.Property("SponsorId").IsModified = true;
            entry.Property("TotalLandSize").IsModified = true;
            entry.Property("Type").IsModified = true;
            entry.Property("EastLongitude").IsModified = true;
            entry.Property("NorthLatitude").IsModified = true;
            content.SaveChanges();
        }

        public static int Count()
        {
            var content = new WeitmChainTouchEntities();
            return content.Properties.Count();
        }

        #endregion

        #region 动态方法



        #endregion

        #region 对象属性

        public List<Picture> Pictures
        {
            get
            {
                return Picture.FindByTarget(this.Id);
            }
        }

        public List<Contact> Contacts
        {
            get
            {
                var contactIds = this.ContactIds
                    .Split('|')
                    .Where(p => p.Length > 0)
                    .Select(p => new Guid(p));
                var contacts = new List<Contact>();
                foreach (var contactId in contactIds)
                {
                    contacts.Add(Contact.Get(contactId));
                }
                return contacts;
            }
        }

        #endregion
    }
}
