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
    
    public partial class FileUploadStatu
    {
        public System.Guid UploadGuid { get; set; }
        public string ProcessingFileName { get; set; }
        public string FilePath { get; set; }
        public System.DateTime ProcessingDate { get; set; }
        public string ProcessingStatus { get; set; }
        public Nullable<long> RecordCount { get; set; }
        public Nullable<long> CouponCount { get; set; }
        public Nullable<long> RelatedCount { get; set; }
        public Nullable<long> PaymentCount { get; set; }
        public Nullable<long> OtherAmountCount { get; set; }
        public Nullable<long> RemittanceCount { get; set; }
        public Nullable<long> BillingAnalysisCount { get; set; }
        public string ProcessingFileType { get; set; }
        public string UserID { get; set; }
        public Nullable<int> MaintenanceFlag { get; set; }
        public string UploadedFileID { get; set; }
        public Nullable<bool> ProratedFlag { get; set; }
        public Nullable<int> FareAuditFlag { get; set; }
        public Nullable<int> TFCAuditFlag { get; set; }
        public Nullable<System.DateTime> BatchID { get; set; }
    }
}