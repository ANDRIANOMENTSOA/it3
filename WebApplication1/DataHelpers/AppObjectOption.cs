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
    
    public partial class AppObjectOption
    {
        public System.Guid ObjGuid { get; set; }
        public System.Guid OptGuid { get; set; }
        public string OptName { get; set; }
        public string OptDescription { get; set; }
    
        public virtual AppObject AppObject { get; set; }
    }
}
