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
    
    public partial class Maintenance
    {
        public string Operation { get; set; }
        public int Sequence { get; set; }
        public string StoredProcedure { get; set; }
        public Nullable<System.DateTime> Started { get; set; }
        public Nullable<System.DateTime> Completed { get; set; }
        public string TimeTaken { get; set; }
        public Nullable<int> Recs { get; set; }
        public Nullable<int> ReturnValue { get; set; }
        public string ErrMessage { get; set; }
        public System.DateTime BatchID { get; set; }
    }
}