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
    
    public partial class Contact
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> TargetId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Department { get; set; }
        public string Title { get; set; }
        public string MobilePhone { get; set; }
        public string Landline { get; set; }
        public string EmailAddress { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
