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
    
    public partial class SysLog
    {
        public long LogId { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public string UserName { get; set; }
        public string Module { get; set; }
        public string SubModule { get; set; }
        public string ProgramName { get; set; }
        public string FunctionName { get; set; }
        public string FunctionMessage { get; set; }
        public Nullable<int> ErrorNo { get; set; }
        public string SysMessage { get; set; }
        public Nullable<int> SysErrorNo { get; set; }
        public string Action { get; set; }
        public Nullable<System.DateTime> DateAction { get; set; }
        public string ActionedBy { get; set; }
        public Nullable<int> ActionStatus { get; set; }
        public Nullable<int> LevelAccess { get; set; }
    }
}
