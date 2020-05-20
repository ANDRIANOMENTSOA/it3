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
    
    public partial class INADHeader
    {
        public string NewDocumentNumber { get; set; }
        public Nullable<System.DateTime> NewDateOfIssue { get; set; }
        public string NewFareCalculationArea { get; set; }
        public string PassengerName { get; set; }
        public string OriginalDocumentNumber { get; set; }
        public Nullable<System.DateTime> OriginalDateOfIssue { get; set; }
        public string OriginalFareCalculationArea { get; set; }
        public string INADPoint { get; set; }
        public string FinalInboundCarrier { get; set; }
        public string ParticipatingInInboundCarriage1 { get; set; }
        public string ParticipatingInInboundCarriage2 { get; set; }
        public string ParticipatingInInboundCarriage3 { get; set; }
        public string ParticipatingInInboundCarriage4 { get; set; }
        public string ParticipatingInInboundCarriage5 { get; set; }
        public string BillingPeriod { get; set; }
        public string UncollectedFareCurrency { get; set; }
        public Nullable<decimal> UncollectedFareAmount { get; set; }
        public Nullable<decimal> UncollectedFareAmountInUSD { get; set; }
        public string UncollectedNonTransportationCostCurrency { get; set; }
        public Nullable<decimal> UncollectedNonTransportationCostAmount { get; set; }
        public Nullable<decimal> UncollectedNonTransportationCostAmountInUSD { get; set; }
        public Nullable<decimal> ISCPercentage { get; set; }
        public string OutboundCarrier { get; set; }
        public string ParticipatingInOutboundCarriage1 { get; set; }
        public string ParticipatingInOutboundCarriage2 { get; set; }
        public string ParticipatingInOutboundCarriage3 { get; set; }
        public string ParticipatingInOutboundCarriage4 { get; set; }
        public string ParticipatingInOutboundCarriage5 { get; set; }
        public string HandlingAirline { get; set; }
        public Nullable<bool> InstructionsIssued { get; set; }
    }
}
