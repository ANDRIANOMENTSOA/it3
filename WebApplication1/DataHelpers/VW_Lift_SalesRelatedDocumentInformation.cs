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
    
    public partial class VW_Lift_SalesRelatedDocumentInformation
    {
        public Nullable<System.Guid> RelatedDocumentGuid { get; set; }
        public Nullable<System.Guid> HdrGuid { get; set; }
        public string DocumentNumber { get; set; }
        public Nullable<long> CheckDigit { get; set; }
        public int IsConjunction { get; set; }
        public Nullable<bool> IsReissue { get; set; }
        public string RelatedDocumentNumber { get; set; }
        public Nullable<long> RelatedTicketCheckDigit { get; set; }
        public Nullable<int> CouponIndicator { get; set; }
        public string TransactionCode { get; set; }
        public Nullable<int> TransmissionControlNumber { get; set; }
        public Nullable<int> SalesUploadGuid { get; set; }
        public System.Guid LiftUploadGuid { get; set; }
    }
}
