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
    
    public partial class VW_RefundedRelatedTicket
    {
        public System.Guid UploadGuid { get; set; }
        public string TransactionNumber { get; set; }
        public string RefundedTicket { get; set; }
        public string OriginalRelatedTicket { get; set; }
        public string RelatedTicketDocumentCouponNumberIdentifier { get; set; }
        public string SalesSource { get; set; }
        public Nullable<System.DateTime> RefundDate { get; set; }
        public Nullable<System.DateTime> DateOfIssueRelatedDocument { get; set; }
        public int IsConjunction { get; set; }
    }
}
