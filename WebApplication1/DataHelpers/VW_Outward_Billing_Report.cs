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
    
    public partial class VW_Outward_Billing_Report
    {
        public string BILLED_AIRLINE { get; set; }
        public string TICKET { get; set; }
        public string CPN { get; set; }
        public string CHECK_DIGIT { get; set; }
        public string SECTOR { get; set; }
        public string PRORATE_METHOD { get; set; }
        public Nullable<decimal> AMOUNT { get; set; }
        public Nullable<int> ISC__ { get; set; }
        public Nullable<decimal> ISC_AMOUNT { get; set; }
        public Nullable<decimal> NET { get; set; }
        public Nullable<System.DateTime> FLIGHT_DATE { get; set; }
        public decimal TOTAL_TAX { get; set; }
        public string FLIGHT { get; set; }
        public string CUR { get; set; }
        public string E_TKT_SAC { get; set; }
        public string INVOICENO { get; set; }
        public string BILLING_PERIOD { get; set; }
        public Nullable<decimal> BILLED_AMOUNT { get; set; }
        public string FARE_BASIS { get; set; }
        public string RBD { get; set; }
    }
}
