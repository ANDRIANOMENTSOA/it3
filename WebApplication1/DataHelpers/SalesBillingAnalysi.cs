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
    
    public partial class SalesBillingAnalysi
    {
        public System.Guid UploadedRef { get; set; }
        public int SequenceNumber { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string ProcessingDateIdentifier { get; set; }
        public string ProcessingCycleIdentifier { get; set; }
        public Nullable<decimal> GrossValue { get; set; }
        public Nullable<decimal> TotalRemittance { get; set; }
        public Nullable<decimal> TotalCommission { get; set; }
        public Nullable<decimal> TotalTax { get; set; }
        public Nullable<decimal> TotalPenalty { get; set; }
        public Nullable<decimal> TotalTaxOnCommission { get; set; }
        public string Currency { get; set; }
    }
}