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
    
    public partial class JournalEntry
    {
        public string Period { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string COACredit { get; set; }
        public string COADebit { get; set; }
        public string AccountDetails { get; set; }
        public string PaxReferences { get; set; }
        public string RelatedDocumentNumber { get; set; }
        public string Currency { get; set; }
        public string DocumentAmountType { get; set; }
        public string OtherAmountCode { get; set; }
        public Nullable<decimal> Value { get; set; }
    }
}
