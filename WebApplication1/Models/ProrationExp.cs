using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ProrationExp
    {
        public String DateProcess { get; set; }
        public String RelatedDocumentGuid { get; set; }
        public String DocumentNumber { get; set; }
        public String FareCalculationArea { get; set; }
        public String CouponNumber { get; set; }
        public String CouponStatus { get; set; }
        public String Fare { get; set; }
        public String EquivalentFare { get; set; }
        public String ErrorMsg { get; set; }

    }
}