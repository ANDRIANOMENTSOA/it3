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
    
    public partial class SP_InterlineApplicableTax_Result
    {
        public string RelatedDocumentNumber { get; set; }
        public int CouponNumber { get; set; }
        public Nullable<System.DateTime> DomesticInternational { get; set; }
        public string TaxCode { get; set; }
        public string TaxCurrency { get; set; }
        public string TaxAmount { get; set; }
        public Nullable<decimal> TaxAmountUSD { get; set; }
        public Nullable<decimal> USDRate { get; set; }
        public string CheckDigit { get; set; }
    }
}
