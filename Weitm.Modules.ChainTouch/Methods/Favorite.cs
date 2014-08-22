using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weitm.Modules.ChainTouch.Models
{
    public partial class Favorite
    {
        public static List<Property> FindPropertiesByUserId(Guid userId)
        {
            var content = new WeitmChainTouchEntities();
            return content.Favorites.Where(p => p.UserId == userId).Select(p => p.Property).ToList();
        }

        public static List<Guid> FindUserIdsByPropertyId(Guid propertyId)
        {
            var content = new WeitmChainTouchEntities();
            return content.Favorites.Where(p => p.PropertyId == propertyId).Select(p => p.UserId).ToList();
        }

        public static List<Favorite> FindByUserId(Guid userId)
        {
            var content = new WeitmChainTouchEntities();
            return content.Favorites.Where(p => p.UserId == userId).ToList();
        }

        public static List<Favorite> FindByPropertyId(Guid propertyId)
        {
            var content = new WeitmChainTouchEntities();
            return content.Favorites.Where(p => p.PropertyId == propertyId).ToList();
        }

        public static bool Has(Guid propertyId, Guid userId)
        {
            var content = new WeitmChainTouchEntities();
            return content.Favorites.Where(p => p.PropertyId == propertyId && p.UserId == userId).Count() > 0;
        }

        public static void Insert(Guid propertyId, Guid userId)
        {
            if (!Has(propertyId, userId))
            {
                var content = new WeitmChainTouchEntities();
                content.Favorites.Add(new Favorite()
                {
                    PropertyId = propertyId,
                    UserId = userId,
                    CreateTime = DateTime.Now
                });
                content.SaveChanges();
            }
        }

        public static void Remove(Guid propertyId, Guid userId)
        {
            var content = new WeitmChainTouchEntities();
            var favorite = content.Favorites.Where(p=>p.PropertyId==propertyId && p.UserId == userId).SingleOrDefault();
            if (favorite != null)
            {
                content.Favorites.Remove(favorite);
                content.SaveChanges();
            }
        }
    }
}
