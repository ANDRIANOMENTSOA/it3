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
    
    public partial class CouponsNotToBeProrated_Result
    {
        public string DocumentNumber { get; set; }
        public string RelatedDocumentNumber { get; set; }
        public int CouponNumber { get; set; }
        public string BillingType { get; set; }
        public string Carrier { get; set; }
        public string OriginAirportCityCode { get; set; }
        public string DestinationAirportCityCode { get; set; }
        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }
        public decimal SpecialProrateAgreement { get; set; }
        public decimal FinalShare { get; set; }
        public string OutwardBillingPeriod { get; set; }
        public Nullable<int> InwardBillingPeriod { get; set; }
        public System.Guid HdrGuid { get; set; }
        public System.Guid RelatedDocumentGuid { get; set; }
    }
}