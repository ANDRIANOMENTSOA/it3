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
    
    public partial class AgentSalesLimit
    {
        public string AgentNumericCode { get; set; }
        public Nullable<System.DateTime> From { get; set; }
        public Nullable<long> Warning { get; set; }
        public Nullable<long> SalesLimit { get; set; }
        public Nullable<System.DateTime> WarningDate { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> AmountCollectedConverted { get; set; }
    }
}
