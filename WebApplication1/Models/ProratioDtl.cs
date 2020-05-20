using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ProratioDtl
    {
        public String ProrateGuid { get; set; }
        public String HeaderGuid { get; set; }
        public String ProrationFlag { get; set; }
        public String SectorOrigin { get; set; }
        public String SectorDestination { get; set; }
        public String SectorCarrier { get; set; }
        public String SourceType { get; set; }
        public String Currency { get; set; }
        public String ProrateFactor { get; set; }
        public String ProrateValue { get; set; }
        public String StraightRateProrate { get; set; }
        public String SpecialProrateAgreement { get; set; }
        public String Diffentials { get; set; }
        public String Surcharge { get; set; }
        public String YQ { get; set; }
        public String FinalShare { get; set; }
        public String TaxesFeesCharges { get; set; }
        public String BaseAmount { get; set; }
        public String IscPercent { get; set; }
        public String IscAmount { get; set; }
        public String HandlingFeeAmt { get; set; }
        public String OtherCommissions { get; set; }
        public String TaxAmount { get; set; }
        public String CouponNumber { get; set; }
        public String UatpAmount { get; set; }
        public String FareComponent { get; set; }
        public String DocumentNumber { get; set; }
        public String Roe { get; set; }
        public String RelatedDocumentGuid { get; set; }
        public String CodeShareValue { get; set; }


    }
}