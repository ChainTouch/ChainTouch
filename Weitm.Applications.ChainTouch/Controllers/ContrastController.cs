using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;

using Weitm.Modules.Membership;
using Weitm.Modules.ChainTouch.Models;

namespace Weitm.Applications.ChainTouch.Controllers
{
    [Authorize]
    public class ContrastController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Output()
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            var properties = Favorite.FindPropertiesByUserId(userId);

            string fileName = Guid.NewGuid().ToString() + ".csv";
            string downloadUrl = string.Format(@"~/UploadFiles/{0}", fileName);
            string filePath = Server.MapPath(downloadUrl);
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

            int i = 1;

            #region 打印表头
            StreamWriter sw = new StreamWriter(
                new FileStream(filePath, FileMode.CreateNew),
                Encoding.GetEncoding("GB2312"));
            StringBuilder column = new StringBuilder();
            column.Append("#").Append(",");
            column.Append("Id").Append(",");
            column.Append("Name").Append(",");
            column.Append("Type").Append(",");
            column.Append("Province").Append(",");
            column.Append("Municipal").Append(",");
            column.Append("District").Append(",");
            column.Append("Address").Append(",");
            column.Append("NorthLatitude").Append(",");
            column.Append("EastLongitude").Append(",");
            column.Append("TotalLandSize").Append(",");
            column.Append("GrossFloorArea").Append(",");
            column.Append("LeasableArea").Append(",");
            column.Append("AvailableDate").Append(",");
            column.Append("CompleteDate").Append(",");
            column.Append("PropertyStructure").Append(",");
            column.Append("NumberOfFloors").Append(",");
            column.Append("ClearHeight").Append(",");
            column.Append("ColumnSpace").Append(",");
            column.Append("ColumnSpaceWidth").Append(",");
            column.Append("ColumnSpaceHeight").Append(",");
            column.Append("PowerCapacity").Append(",");
            column.Append("FloorLoading").Append(",");
            column.Append("Rent").Append(",");
            column.Append("ManagementFee").Append(",");
            column.Append("ManagementFeeIncludedInRent").Append(",");
            column.Append("CreateTime");
            sw.WriteLine(column);

            #endregion

            foreach (var property in properties)
            {
                StringBuilder row = new StringBuilder();
                row.Append(i++).Append(",");
                row.Append(property.Id).Append(",");
                row.Append(property.Name).Append(",");
                row.Append(property.Type).Append(",");
                row.Append(property.Province).Append(",");
                row.Append(property.Municipal).Append(",");
                row.Append(property.District).Append(",");
                row.Append(property.Address).Append(",");
                row.Append(property.NorthLatitude).Append(",");
                row.Append(property.EastLongitude).Append(",");
                row.Append(property.TotalLandSize).Append(",");
                row.Append(property.GrossFloorArea).Append(",");
                row.Append(property.LeasableArea).Append(",");
                row.Append(property.AvailableDate).Append(",");
                row.Append(property.CompleteDate).Append(",");
                row.Append(property.PropertyStructure).Append(",");
                row.Append(property.NumberOfFloors).Append(",");
                row.Append(property.ClearHeight).Append(",");
                row.Append(property.ColumnSpace).Append(",");
                row.Append(property.ColumnSpaceWidth).Append(",");
                row.Append(property.ColumnSpaceHeight).Append(",");
                row.Append(property.PowerCapacity).Append(",");
                row.Append(property.FloorLoading).Append(",");
                row.Append(property.Rent).Append(",");
                row.Append(property.ManagementFee).Append(",");
                row.Append(property.ManagementFeeIncludedInRent).Append(",");
                row.Append(property.CreateTime);

                sw.WriteLine(row);
            }

            sw.Close();
            return Redirect(downloadUrl);
        }

        public ActionResult Add(Guid id)
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            Favorite.Insert(id, userId);
            return RedirectToAction("Index");
        }

        public ActionResult Remove(Guid id)
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            Favorite.Remove(id, userId);
            return RedirectToAction("Index");
        }
    }
}
