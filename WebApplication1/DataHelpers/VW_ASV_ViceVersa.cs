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
    
    public partial class VW_ASV_ViceVersa
    {
        public int SPANumber { get; set; }
        public Nullable<long> ASVNumber { get; set; }
        public string BillingEntity { get; set; }
        public string Trip { get; set; }
        public string DomesticInternational { get; set; }
        public string FIMIVL { get; set; }
        public string ViceVersa { get; set; }
        public string Currency { get; set; }
        public string SectorOrigin { get; set; }
        public string SectorDestination { get; set; }
        public Nullable<decimal> Gross { get; set; }
        public Nullable<decimal> Net { get; set; }
        public string RBD { get; set; }
        public string PublishedUnpublished { get; set; }
        public string FareBasisPrimeCode { get; set; }
        public string FareCategory { get; set; }
        public string FareBasisPrimeCodeSpecific { get; set; }
        public string FareAndPaxTypeCode { get; set; }
        public string ASVExclusiveOfTFCs { get; set; }
        public Nullable<int> MPAPercentage { get; set; }
        public string BaseAmountRequired { get; set; }
        public string ApplicabilityDateFlag { get; set; }
        public string IATASeason { get; set; }
        public Nullable<System.DateTime> SeasonalityFrom { get; set; }
        public Nullable<System.DateTime> SeasonalityTo { get; set; }
        public Nullable<System.DateTime> IssuedOnAfter { get; set; }
        public Nullable<System.DateTime> TravelOnBefore { get; set; }
        public Nullable<int> ASVExceptionNumber { get; set; }
        public Nullable<System.DateTime> IssueDateValidityFrom { get; set; }
        public Nullable<System.DateTime> IssueDateValidityTo { get; set; }
        public Nullable<System.DateTime> FlightDateValidityFrom { get; set; }
        public Nullable<System.DateTime> FlightDateValidityTo { get; set; }
        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }
    }
}
