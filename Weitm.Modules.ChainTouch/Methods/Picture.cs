using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;

namespace Weitm.Modules.ChainTouch.Models
{
    public partial class Picture
    {
        #region 静态方法

        public static Picture Get(Guid id)
        {
            var content = new WeitmChainTouchEntities();
            return content.Pictures
                .Where(p => p.Id == id)
                .SingleOrDefault();
        }

        public static List<Picture> FindByTarget(Guid targetId)
        {
            var content = new WeitmChainTouchEntities();
            return content.Pictures
                .Where(p => p.TargetId == targetId)
                .OrderByDescending(p => p.CreateTime)
                .ToList();
        }

        public static void Add(Picture picture)
        {
            var content = new WeitmChainTouchEntities();
            content.Pictures.Add(picture);
            content.SaveChanges();
        }

        public static void Remove(Guid id)
        {
            var content = new WeitmChainTouchEntities();
            var picture = new Picture { Id = id };
            content.Pictures.Attach(picture);
            content.Pictures.Remove(picture);
            content.SaveChanges();
        }

        public static void Update(Picture picture)
        {
            var content = new WeitmChainTouchEntities();
            content.Configuration.ValidateOnSaveEnabled = false;
            var item = new Picture()
            {
                Id = picture.Id,
                TargetId = picture.TargetId,
                CreateTime = picture.CreateTime,
                ImageUrl = picture.ImageUrl
            };
            DbEntityEntry<Picture> entry = content.Entry<Picture>(item);
            entry.State = System.Data.Entity.EntityState.Unchanged;
            entry.Property("TargetId").IsModified = true;
            entry.Property("CreateTime").IsModified = true;
            entry.Property("ImageUrl").IsModified = true;
            content.SaveChanges();
        }
        #endregion

        #region 动态方法

        #endregion

        #region 对象属性


        #endregion
    }
}
