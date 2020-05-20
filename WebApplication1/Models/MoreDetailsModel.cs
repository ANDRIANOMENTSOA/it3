using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MoreDetailsModel
    {
        public String DocumentNumber { get; set; }
        public String CheckDigit { get; set; }

        public String AgentNumericCode { get; set; }
        public String DateofIssue { get; set; }
        public String SaleDate { get; set; }
        public String PassengerName { get; set; }
        public String PassengerSpecificData { get; set; }
        public String FareCalculationArea { get; set; }
        public String EndosRestriction { get; set; }
        public String OriginalIssueInformation { get; set; }
        public String OriginalIssueDocumentNumber { get; set; }
        public String OriginalIssueCity { get; set; }
        public String OriginalIssueDate { get; set; }
        public String OriginalIssueAgentNumericCode { get; set; }
        public String InConnectionWithDocumentNumber { get; set; }
        public String Fare { get; set; }
        public String EquivalentFare { get; set; }
        public String Tax1 { get; set; }
        public String Tax2 { get; set; }
        public String Tax3 { get; set; }
        public String TotalAmount { get; set; }
        public String SequenceNumber { get; set; }
        public String ITBT { get; set; }
        public String ExchangeADC { get; set; }
        public String DocumentType { get; set; }
        public String TicketingModeIndicator { get; set; }
        public String FareCalculationModeIndicator { get; set; }
        public String BookingAgentIdentification { get; set; }
        public String BookingAgencyLocationNumber { get; set; }
        public String BookingEntityOutletType { get; set; }
        public String BookingReference { get; set; }
        public String AuditStatusIndicator { get; set; }
        public String ClientIdentification { get; set; }
        public String CommercialAgreementReference { get; set; }
        public String CustomerFileReference { get; set; }
        public String DataInputStatusIndicator { get; set; }
        public String FareCalculationPricingIndicator { get; set; }
        public String FormatIdentifier { get; set; }
        public String TourCode { get; set; }
        public String NetReportingIndicator { get; set; }
        public String NeutralTicketingSystemIdentifier { get; set; }
        public String ReportingSystemIdentifier { get; set; }
        public String ServicingAirlineSystemProviderIdentifier { get; set; }
        public String TrueOriginDestinationCityCodes { get; set; }
        public String SettlementAuthorizationCode { get; set; }
        public String VendorIdentification { get; set; }
        public String VendorISOCountryCode { get; set; }
        public String TransactionCode { get; set; }
        public String SalesDataAvailable { get; set; }
        public String AccountingStatus { get; set; }
        public String ProratedFlag { get; set; }
        public String FBSFlag { get; set; }
        public String DocumentAirlineID { get; set; }
        public String TransactionGroup { get; set; }
        public String PaxSPC { get; set; }
        public String PaxType { get; set; }
        public String SalesPeriod { get; set; }
        public String PrevMOS { get; set; }
        public String TotalTax { get; set; }
        public String BookingAgentID { get; set; }
        public String TicketingAgentID { get; set; }
        public String IsReissue { get; set; }
        public String DataSource { get; set; }
        public String ProcessingFileType { get; set; }
        public String ProcessingDate { get; set; }
        public String OwnTicket { get; set; }
        public String OwnCarrier { get; set; }
        public String OwnISOCountry { get; set; }
        public String OwnAirline { get; set; }
        public String HostCurrency { get; set; }
        public String AmountCollected { get; set; }
        public String TaxCollected { get; set; }
        public String SurchargeCollected { get; set; }
        public String CommissionCollected { get; set; }
        public String BilateralEndorsement { get; set; }
        public String InvoluntaryReroute { get; set; }
        public String BspIdentifier { get; set; }
        public String IsoCountryCode { get; set; }
        public String USDRatePayCur { get; set; }
        public String USDRateHostCur { get; set; }
        public String PMPPeriod { get; set; }
        public String TaxOnCommissionCollected { get; set; }
        public String SignCode { get; set; }
    }
}