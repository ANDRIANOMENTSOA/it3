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
    
    public partial class GetProration
    {
        public string DocumentNumber { get; set; }
        public Nullable<System.DateTime> DateofIssue { get; set; }
        public string SectorOrigin { get; set; }
        public string SectorDestination { get; set; }
        public string SectorCarrier { get; set; }
        public string SourceType { get; set; }
        public string Currency { get; set; }
        public decimal ProrateFactor { get; set; }
        public decimal ProrateValue { get; set; }
        public decimal StraightRateProrate { get; set; }
        public decimal SpecialProrateAgreement { get; set; }
        public Nullable<decimal> Surcharge { get; set; }
        public decimal FinalShare { get; set; }
        public Nullable<decimal> TaxesFeesCharges { get; set; }
        public decimal BaseAmount { get; set; }
        public Nullable<decimal> IscPercent { get; set; }
        public Nullable<decimal> IscAmount { get; set; }
        public Nullable<decimal> HandlingFeeAmt { get; set; }
        public Nullable<decimal> OtherCommissions { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public Nullable<decimal> UatpAmount { get; set; }
    }
}
