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
    
    public partial class SalesDocumentStatu
    {
        public System.Guid RelatedDocumentGuid { get; set; }
        public int SequenceNumber { get; set; }
        public Nullable<System.Guid> UploadedRef { get; set; }
        public string AgentNumericCode { get; set; }
        public string AgentPlace { get; set; }
        public string IssuePlace { get; set; }
        public Nullable<System.DateTime> DateofIssue { get; set; }
        public string DocumentStatus { get; set; }
        public string TransactionCode { get; set; }
    }
}
