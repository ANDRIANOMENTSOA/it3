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
    
    public partial class FIMDetail
    {
        public System.Guid RelatedGuid { get; set; }
        public string FIMNO { get; set; }
        public Nullable<int> SeqNo { get; set; }
        public string PassengerName { get; set; }
        public Nullable<int> CouponNo { get; set; }
        public string AirlineCode { get; set; }
        public string DocumentNumber { get; set; }
        public Nullable<int> CheckDigit { get; set; }
        public string DocumentType { get; set; }
        public string FareBasis_PaxTypeCode { get; set; }
        public string NewFlightNo1 { get; set; }
        public string NewFlightNo2 { get; set; }
        public Nullable<decimal> Billed { get; set; }
        public Nullable<decimal> Accepted { get; set; }
        public Nullable<decimal> Difference { get; set; }
        public Nullable<int> ExcessBaggageWeight { get; set; }
        public Nullable<int> ExcessBaggagePiece { get; set; }
    }
}
