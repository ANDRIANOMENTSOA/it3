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
    
    public partial class SalesTransactionRemittance_Bot93_Result
    {
        public System.Guid UploadedRef { get; set; }
        public Nullable<int> SequenceNumber { get; set; }
        public string TransactionCode { get; set; }
        public Nullable<System.DateTime> RemittanceDate { get; set; }
        public string AgentNumericCode { get; set; }
        public Nullable<decimal> GrossValue { get; set; }
        public Nullable<decimal> TotalRemittance { get; set; }
        public Nullable<decimal> TotalCommission { get; set; }
        public Nullable<decimal> TotalTax { get; set; }
        public Nullable<decimal> TotalPenalty { get; set; }
        public Nullable<decimal> TotalTaxonCommission { get; set; }
        public string Currency { get; set; }
    }
}