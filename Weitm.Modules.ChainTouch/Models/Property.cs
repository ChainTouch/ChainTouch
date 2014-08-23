//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Weitm.Modules.ChainTouch.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Property
    {
        public Property()
        {
            this.Favorites = new HashSet<Favorite>();
        }
    
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> SponsorId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Province { get; set; }
        public string Municipal { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public Nullable<decimal> TotalLandSize { get; set; }
        public Nullable<decimal> GrossFloorArea { get; set; }
        public Nullable<decimal> LeasableArea { get; set; }
        public string AvailableDate { get; set; }
        public string CompleteDate { get; set; }
        public string PropertyStructure { get; set; }
        public Nullable<int> NumberOfFloors { get; set; }
        public Nullable<decimal> ClearHeight { get; set; }
        public string ColumnSpace { get; set; }
        public Nullable<decimal> ColumnSpaceWidth { get; set; }
        public Nullable<decimal> ColumnSpaceHeight { get; set; }
        public Nullable<decimal> PowerCapacity { get; set; }
        public Nullable<decimal> FloorLoading { get; set; }
        public Nullable<decimal> Rent { get; set; }
        public Nullable<decimal> ManagementFee { get; set; }
        public Nullable<bool> ManagementFeeIncludedInRent { get; set; }
        public string ContactIds { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<double> NorthLatitude { get; set; }
        public Nullable<double> EastLongitude { get; set; }
    
        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}