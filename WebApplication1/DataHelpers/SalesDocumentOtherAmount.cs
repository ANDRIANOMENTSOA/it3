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
    
    public partial class SalesDocumentOtherAmount
    {
        public System.Guid RelatedDocumentGuid { get; set; }
        public int SequenceNumber { get; set; }
        public string DocumentAmountType { get; set; }
        public Nullable<System.DateTime> DateofIssue { get; set; }
        public string CurrencyType { get; set; }
        public string TransactionCode { get; set; }
        public string OtherAmountCode { get; set; }
        public Nullable<decimal> OtherAmount { get; set; }
        public Nullable<decimal> OtherAmountRate { get; set; }
        public Nullable<decimal> TicketDocumentAmount { get; set; }
        public Nullable<decimal> CommissionableAmount { get; set; }
        public Nullable<decimal> AmountEnteredbyAgent { get; set; }
        public Nullable<decimal> AmountPaidbyCustomer { get; set; }
        public Nullable<decimal> LateReportingPenalty { get; set; }
        public Nullable<decimal> NetFareAmount { get; set; }
        public string StatisticalCode { get; set; }
        public string HdrGuidRef { get; set; }
        public string DocumentNumber { get; set; }
        public string RelatedDocumentNumber { get; set; }
        public long FileSequence { get; set; }
    
        public virtual SalesRelatedDocumentInformation SalesRelatedDocumentInformation { get; set; }
    }
}
