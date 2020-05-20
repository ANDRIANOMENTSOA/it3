using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class SerctorValueControlModels
    {
        public string OrigCity { get; set; }
        public string DestCity { get; set; }
        public string PrimeCode { get; set; }
        public string RBD { get; set; }
        public string MinSectorVal { get; set; }
        public string MaxSectorVal { get; set; }
        //public string Validated { get; set; }
        public string DateofIssue { get; set; }
        public string AgentNumericCode { get; set; }
        public string DocumentNumber { get; set; }
        public string RelatedDocumentNumber { get; set; }
        public string CouponNumber { get; set; }
        public string OriginAirportCityCode { get; set; }
        public string DestinationAirportCityCode { get; set; }
        public string ReservationBookingDesignator { get; set; }
        public string FinalShare { get; set; }
        public object OriginCity { get; internal set; }
        public object DestinationCity { get; internal set; }
        public bool Validated { get; set; }
    }
}
