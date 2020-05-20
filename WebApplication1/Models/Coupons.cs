using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Coupons
    {
        public String RelatedDocumentGuid { get; set; }
        public String HdrGuid { get; set; }
        public String DocumentNumber { get; set; }
        public String CheckDigit { get; set; }
        public String IsConjunction { get; set; }
        public String IsReissue { get; set; }
        public String RelatedDocumentNumber { get; set; }
        public String RelatedTicketCheckDigit { get; set; }
        public String CouponIndicator { get; set; }
        public String TransactionCode { get; set; }
        public String TransmissionControlNumber { get; set; }
        public String SalesUploadGuid { get; set; }
        public String LiftUploadGuid { get; set; }
        public String SequenceNumber { get; set; }

        public String CouponNumber { get; set; }
        public String OriginAirportCityCode { get; set; }
        public String DestinationAirportCityCode { get; set; }
        public String Carrier { get; set; }
        public String StopOverCode { get; set; }
        public String FareBasisTicketDesignator { get; set; }
        public String FrequentFlyerReference { get; set; }
        public String FlightNumber { get; set; }
        public String FlightDepartureDate { get; set; }
        public String FlightDepartureTime { get; set; }
        public String NotValidBefore { get; set; }
        public String NotValidAfter { get; set; }
        public String ReservationBookingDesignator { get; set; }
        public String FreeBaggageAllowance { get; set; }
        public String FlightBookingStatus { get; set; }
        public String OriginAirportCityCode1 { get; set; }
        public String StopoverCode1 { get; set; }
        public String OriginAirportCityCode2 { get; set; }
        public String StopoverCode2 { get; set; }
        public String SegmentIdentifier { get; set; }
        public String UsageAirline { get; set; }
        public String UsageDate { get; set; }
        public String UsageFlightNumber { get; set; }
        public String UsageOriginCode { get; set; }
        public String UsageDestinationCode { get; set; }
        public String UsedClassofService { get; set; }
        public String SettlementAuthorizationCode { get; set; }
        public String CouponStatus { get; set; }
        public String AccountingStatus { get; set; }
        public String AccountingOnEstimate { get; set; }
        public String BillingStatus { get; set; }
        public String HdrGuidRef { get; set; }


        public String BillingType { get; set; }
        public String DomesticInternational { get; set; }
        public String OriginCity { get; set; }
        public String DestinationCity { get; set; }
        public String OriginCountry { get; set; }
        public String DestinationCountry { get; set; }
        public String IsOAL { get; set; }


    }
}