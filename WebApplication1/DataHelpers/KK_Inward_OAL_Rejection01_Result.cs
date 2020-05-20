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
    
    public partial class KK_Inward_OAL_Rejection01_Result
    {
        public Nullable<long> RecId { get; set; }
        public Nullable<long> DataTransmissionHDRID { get; set; }
        public Nullable<long> DataInvoicesHRDID { get; set; }
        public Nullable<long> DataLineItemDetailsID { get; set; }
        public Nullable<long> DataRejMemoDetails { get; set; }
        public Nullable<int> DetailNumber { get; set; }
        public Nullable<int> LineItemNumber { get; set; }
        public string TransmissionId { get; set; }
        public string InvoiceNo { get; set; }
        public string SellerOrganization_OrganizationDesignator { get; set; }
        public string TicketNo { get; set; }
        public Nullable<int> CouponNo { get; set; }
        public Nullable<int> BreakdownSerialNumber { get; set; }
        public string TicketIssuingAirline { get; set; }
        public Nullable<int> CouponNumber { get; set; }
        public string TicketDocNumber { get; set; }
        public Nullable<int> CheckDigit { get; set; }
        public Nullable<decimal> ChargeAmountName___GrossBilled { get; set; }
        public Nullable<decimal> ChargeAmountName___GrossAccepted { get; set; }
        public Nullable<decimal> ChargeAmountName___GrossDifference { get; set; }
        public string TaxType { get; set; }
        public Nullable<decimal> TaxAmountName___Billed { get; set; }
        public Nullable<decimal> TaxAmountName___Accepted { get; set; }
        public Nullable<decimal> TaxAmountName___Difference { get; set; }
        public string AttachmentIndicatorOriginal { get; set; }
        public string AttachmentIndicatorValidated { get; set; }
        public Nullable<int> NumberOfAttachments { get; set; }
        public Nullable<decimal> TotalNetAmount { get; set; }
        public string FromAirportCode { get; set; }
        public string ToAirportCode { get; set; }
        public string SettlementAuthorizationCode { get; set; }
        public string ReasonCode { get; set; }
        public string ProrateSlipBreakdown { get; set; }
        public string ISValidationFlag { get; set; }
        public string RejectionMemoNumber { get; set; }
    }
}