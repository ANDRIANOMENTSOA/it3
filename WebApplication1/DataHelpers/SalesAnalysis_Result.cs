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
    
    public partial class SalesAnalysis_Result
    {
        public string Currency { get; set; }
        public Nullable<System.DateTime> RemittanceDate { get; set; }
        public string TransactionCode { get; set; }
        public Nullable<decimal> TotalFareOrEquivalentFarePaid { get; set; }
        public Nullable<decimal> TotalRemittance { get; set; }
        public Nullable<decimal> TotalCommission { get; set; }
        public Nullable<decimal> TotalTFCs { get; set; }
        public Nullable<decimal> TotalPenalty { get; set; }
        public Nullable<decimal> TotalTaxOnCommission { get; set; }
    }
}