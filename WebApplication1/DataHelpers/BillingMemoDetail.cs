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
    
    public partial class BillingMemoDetail
    {
        public long RecId { get; set; }
        public Nullable<long> DataTransmissionHDRID { get; set; }
        public Nullable<long> DataInvoicesHRDID { get; set; }
        public Nullable<long> DataLineItemDetailsID { get; set; }
        public Nullable<int> DetailNumber { get; set; }
        public Nullable<int> LineItemNumber { get; set; }
        public string TransmissionId { get; set; }
        public string InvoiceNo { get; set; }
        public string SellerOrganization_OrganizationDesignator { get; set; }
        public string TicketNo { get; set; }
        public Nullable<int> CouponNo { get; set; }
        public string BillingMemoNumber { get; set; }
        public Nullable<int> RejectionStage { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonDescription { get; set; }
        public string CorrespondenceRefNumber { get; set; }
        public string YourInvoiceNumber { get; set; }
        public Nullable<System.DateTime> YourInvoiceBillingDate { get; set; }
        public string YourRejectionMemoNumber { get; set; }
        public string AttachmentIndicatorOriginal { get; set; }
        public string AttachmentIndicatorValidated { get; set; }
        public Nullable<int> NumberOfAttachments { get; set; }
    }
}
