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
    
    public partial class VW_SalesHeader
    {
        public System.Guid HdrGuid { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentType { get; set; }
        public string AgentNumericCode { get; set; }
        public Nullable<System.DateTime> SaleDate { get; set; }
        public Nullable<System.DateTime> DateofIssue { get; set; }
        public string EndosRestriction { get; set; }
        public string FareCalculationArea { get; set; }
        public string FareCurrency { get; set; }
        public Nullable<decimal> Fare { get; set; }
        public Nullable<decimal> EquivalentFare { get; set; }
        public string TotalCurrency { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string Tax1Code { get; set; }
        public Nullable<decimal> Tax1Amount { get; set; }
        public string Tax2Code { get; set; }
        public Nullable<decimal> Tax2Amount { get; set; }
        public string Tax3Code { get; set; }
        public Nullable<decimal> Tax3Amount { get; set; }
        public string PassengerName { get; set; }
        public bool SalesDataAvailable { get; set; }
        public bool ProratedFlag { get; set; }
        public bool FBSFlag { get; set; }
        public string ITBT { get; set; }
        public string ExchangeADC { get; set; }
        public string FareCalculationModeIndicator { get; set; }
        public string FareCalculationPricingIndicator { get; set; }
        public string AccountingStatus { get; set; }
        public Nullable<decimal> VATPercentage { get; set; }
        public string TourCode { get; set; }
        public string TransactionCode { get; set; }
        public string BookingAgentIdentification { get; set; }
        public string BookingReference { get; set; }
        public string TrueOriginDestinationCityCodes { get; set; }
        public Nullable<System.Guid> UploadedRef { get; set; }
        public string BspIdentifier { get; set; }
        public string Bsp_IsoCountryCode { get; set; }
        public string DocumentAirlineID { get; set; }
        public string TransactionGroup { get; set; }
        public string PaxSPC { get; set; }
        public string PaxType { get; set; }
        public string SalesPeriod { get; set; }
        public string PrevMOS { get; set; }
        public Nullable<decimal> TotalTax { get; set; }
        public string TaxCurrency { get; set; }
        public string PayCurrency { get; set; }
        public string BookingAgentID { get; set; }
        public string TicketingAgentID { get; set; }
        public string SignCode { get; set; }
        public Nullable<bool> IsReissue { get; set; }
        public string InConnectionWithDocumentNumber { get; set; }
        public string IsExchange { get; set; }
        public string ProcessingFileType { get; set; }
        public Nullable<System.DateTime> ProcessingDate { get; set; }
        public string OwnTicket { get; set; }
        public string OwnCarrier { get; set; }
        public string OwnISOCountry { get; set; }
        public string OwnAirline { get; set; }
        public Nullable<int> AirlineName { get; set; }
        public string HostCurrency { get; set; }
        public string PMP_Period { get; set; }
    }
}
