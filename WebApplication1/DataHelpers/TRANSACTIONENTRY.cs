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
    
    public partial class TRANSACTIONENTRY
    {
        public long TRANSACTIONID { get; set; }
        public string TICKETNUMBER { get; set; }
        public string RELATEDDOCUMENTNUMBER { get; set; }
        public string COA_REF { get; set; }
        public string REF { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DATE { get; set; }
        public Nullable<decimal> AMOUNT { get; set; }
        public Nullable<decimal> Cr { get; set; }
        public Nullable<decimal> Dr { get; set; }
        public string Status { get; set; }
    }
}