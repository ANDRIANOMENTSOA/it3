//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.DataHelpers
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserSession
    {
        public System.Guid SessionGuid { get; set; }
        public System.DateTime SessionTime { get; set; }
        public string AttemptType { get; set; }
        public string LoginFrom { get; set; }
        public string UsrId { get; set; }
        public string MachineName { get; set; }
        public string IpAddress { get; set; }
    
        public virtual User User { get; set; }
    }
}
