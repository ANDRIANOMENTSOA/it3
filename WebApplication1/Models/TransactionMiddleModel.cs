using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TransactionMiddleModel
    {
        public string CouponNumber { get; set; }
        public string StopOverCode { get; set; }
        public string Carrier { get; set; }
        public string FlightNumber { get; set; }
        public string GFPA { get; set; }
        public string FlightDepartureDate { get; set; }
        public string FlightDepartureTime { get; set; }
        public string CouponStatus { get; set; }
        public string FareBasisTicketDesignator { get; set; }
        public string NotValidBefore { get; set; }
        public string NotValidAfter { get; set; }
        public string FreeBaggageAllowance { get; set; }
        public string FlightBookingStatus { get; set; }
        public string UsedClassofService { get; set; }
        public string UsageSector { get; set; }
        public string UsageAirline { get; set; }
        public string UsageFlightNumber { get; set; }
        public string UsageDate { get; set; }
        public string FrequentFlyerReference { get; set; }
        public string RelatedDocumentNumber { get; set; }
        public Int32 isConjonction { get; set; }
    }
    
}
