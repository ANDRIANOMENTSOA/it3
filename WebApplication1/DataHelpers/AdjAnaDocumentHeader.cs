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
    
    public partial class AdjAnaDocumentHeader
    {
        public System.Guid AdjAnaGuid { get; set; }
        public Nullable<System.Guid> HdrGuid { get; set; }
        public string FareCalculationArea { get; set; }
        public string Period { get; set; }
        public Nullable<decimal> EquivalentFare { get; set; }
        public Nullable<decimal> Fare { get; set; }
        public Nullable<System.DateTime> DateofIssue { get; set; }
        public string FareCurrency { get; set; }
        public string TotalCurrency { get; set; }
        public Nullable<System.DateTime> TimeUpdated { get; set; }
        public string DocumentNumber { get; set; }
    }
}
