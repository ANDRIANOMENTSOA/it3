using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TicketHistoryPaymentModel
    {
        public string CouponNumber { get; set; }
        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }
        public string Carrier { get; set; }
        public string ReservationBookingDesignator { get; set; }
        public string FareBasisTicketDesignator { get; set; }
        public string FlightNumber { get; set; }
        public string FlightDepartureDate { get; set; }
        public string CouponStatus { get; set; }
        public string FinalShare { get; set; }
    }
}