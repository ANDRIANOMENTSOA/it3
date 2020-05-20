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
    
    public partial class VW_Lift_SalesDocumentHeader
    {
        public Nullable<System.Guid> HdrGuid { get; set; }
        public string DocumentNumber { get; set; }
        public string CheckDigit { get; set; }
        public System.Guid UploadedRef { get; set; }
        public string AgentNumericCode { get; set; }
        public Nullable<System.DateTime> DateofIssue { get; set; }
        public Nullable<System.DateTime> SaleDate { get; set; }
        public string PassengerName { get; set; }
        public string PassengerSpecificData { get; set; }
        public string FareCalculationArea { get; set; }
        public string EndosRestriction { get; set; }
        public string OriginalIssueInformation { get; set; }
        public string OriginalIssueDocumentNumber { get; set; }
        public string OriginalIssueCity { get; set; }
        public Nullable<System.DateTime> OriginalIssueDate { get; set; }
        public string OriginalIssueAgentNumericCode { get; set; }
        public string InConnectionWithDocumentNumber { get; set; }
        public decimal Fare { get; set; }
        public string FareCurrency { get; set; }
        public decimal EquivalentFare { get; set; }
        public string Tax1Currency { get; set; }
        public string Tax2Currency { get; set; }
        public string Tax3Currency { get; set; }
        public string Tax1Code { get; set; }
        public string Tax2Code { get; set; }
        public string Tax3Code { get; set; }
        public decimal Tax1Amount { get; set; }
        public decimal Tax2Amount { get; set; }
        public decimal Tax3Amount { get; set; }
        public string TotalCurrency { get; set; }
        public decimal TotalAmount { get; set; }
        public string SequenceNumber { get; set; }
        public string ITBT { get; set; }
        public string ExchangeADC { get; set; }
        public string DocumentType { get; set; }
        public string TicketingModeIndicator { get; set; }
        public string FareCalculationModeIndicator { get; set; }
        public string BookingAgentIdentification { get; set; }
        public string BookingAgencyLocationNumber { get; set; }
        public string BookingEntityOutletType { get; set; }
        public string BookingReference { get; set; }
        public string AuditStatusIndicator { get; set; }
        public string ClientIdentification { get; set; }
        public string CommercialAgreementReference { get; set; }
        public string CustomerFileReference { get; set; }
        public string DataInputStatusIndicator { get; set; }
        public string FareCalculationPricingIndicator { get; set; }
        public string FormatIdentifier { get; set; }
        public string TourCode { get; set; }
        public string NetReportingIndicator { get; set; }
        public string NeutralTicketingSystemIdentifier { get; set; }
        public string ReportingSystemIdentifier { get; set; }
        public string ServicingAirlineSystemProviderIdentifier { get; set; }
        public string TrueOriginDestinationCityCodes { get; set; }
        public string SettlementAuthorizationCode { get; set; }
        public string VendorIdentification { get; set; }
        public string VendorISOCountryCode { get; set; }
        public string TransactionCode { get; set; }
        public Nullable<bool> SalesDataAvailable { get; set; }
        public string AccountingStatus { get; set; }
        public Nullable<bool> ProratedFlag { get; set; }
        public Nullable<bool> FBSFlag { get; set; }
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
        public Nullable<bool> IsReissue { get; set; }
        public string DataSource { get; set; }
        public string ProcessingFileType { get; set; }
        public System.DateTime ProcessingDate { get; set; }
        public string OwnTicket { get; set; }
        public string OwnCarrier { get; set; }
        public string OwnISOCountry { get; set; }
        public string OwnAirline { get; set; }
        public string HostCurrency { get; set; }
        public Nullable<decimal> AmountCollected { get; set; }
        public string AmountCollectedCurrency { get; set; }
        public Nullable<decimal> TaxCollected { get; set; }
        public string TaxCollectedCurrency { get; set; }
        public Nullable<decimal> SurchargeCollected { get; set; }
        public string SurchargeCollectedCurrency { get; set; }
        public Nullable<decimal> CommissionCollected { get; set; }
        public string CommissionCollectedCurrency { get; set; }
        public string BilateralEndorsement { get; set; }
        public string InvoluntaryReroute { get; set; }
        public string BspIdentifier { get; set; }
        public string IsoCountryCode { get; set; }
        public Nullable<decimal> USDRatePayCur { get; set; }
        public Nullable<decimal> USDRateHostCur { get; set; }
        public string PMPPeriod { get; set; }
    }
}
