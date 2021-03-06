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
    
    public partial class InvoiceAnalytic
    {
        public long RecId { get; set; }
        public string TransmisionId { get; set; }
        public string InvoiceNo { get; set; }
        public string SellerOrganization_OrganizationDesignator { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string BillingPeriod { get; set; }
        public int LineItemNumber { get; set; }
        public int DetailNumber { get; set; }
        public string ChargeCode { get; set; }
        public string DocumentNumber { get; set; }
        public int CouponNumber { get; set; }
        public Nullable<int> CheckDigit { get; set; }
        public Nullable<decimal> FinalShare { get; set; }
        public Nullable<decimal> SpaAmount { get; set; }
        public Nullable<decimal> Variance { get; set; }
        public Nullable<decimal> YourGrossBillAmount { get; set; }
        public Nullable<decimal> YourTaxAmount { get; set; }
        public Nullable<decimal> YourISCPercentage { get; set; }
        public Nullable<decimal> YourISCAmount { get; set; }
        public Nullable<decimal> YourOtherCommision { get; set; }
        public Nullable<decimal> YourUATP { get; set; }
        public Nullable<decimal> YourHandlingFees { get; set; }
        public Nullable<decimal> YourVatAmount { get; set; }
        public Nullable<decimal> YourNetAmourt { get; set; }
        public Nullable<decimal> OurGrossBill { get; set; }
        public Nullable<decimal> OurTaxAmount { get; set; }
        public Nullable<decimal> OurISCPercentage { get; set; }
        public Nullable<decimal> OurISCAmount { get; set; }
        public Nullable<decimal> OurOtherCommission { get; set; }
        public Nullable<decimal> OurUATP { get; set; }
        public Nullable<decimal> OurHandlingFees { get; set; }
        public Nullable<decimal> OurVatAmount { get; set; }
        public Nullable<decimal> OurNetAmount { get; set; }
        public Nullable<decimal> DiffGrossAmount { get; set; }
        public Nullable<decimal> DiffTaxAmount { get; set; }
        public Nullable<decimal> DiffISCPercentage { get; set; }
        public Nullable<decimal> DiffISCAmount { get; set; }
        public Nullable<decimal> DiffOtherCommission { get; set; }
        public Nullable<decimal> DiffUATP { get; set; }
        public Nullable<decimal> DiffHandlingFees { get; set; }
        public Nullable<decimal> DiffVatAmount { get; set; }
        public Nullable<decimal> DiffNetAmount { get; set; }
        public string BillingStatus { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonDescription { get; set; }
        public string NextChargeCode { get; set; }
        public string NextBillingPeriod { get; set; }
        public Nullable<System.DateTime> Processdate { get; set; }
        public Nullable<int> XmlGenetared { get; set; }
        public string REMARKS { get; set; }
        public Nullable<System.Guid> RelatedDocumentGuid { get; set; }
        public Nullable<System.Guid> ProrateGuid { get; set; }
        public Nullable<decimal> DieFlown { get; set; }
        public Nullable<decimal> DieInterLine { get; set; }
        public Nullable<decimal> DieDiff { get; set; }
        public Nullable<decimal> YourOtherCommisionPercentage { get; set; }
        public Nullable<decimal> YourUATPPercentage { get; set; }
        public Nullable<decimal> YourHandlingFeesPercentage { get; set; }
        public Nullable<decimal> YourVatAmountPercentage { get; set; }
        public Nullable<decimal> OurOtherCommissionPercentage { get; set; }
        public Nullable<decimal> OurUATPPercentage { get; set; }
        public Nullable<decimal> OurHandlingFeesPercentage { get; set; }
        public Nullable<decimal> OurVatAmountPercentage { get; set; }
        public Nullable<decimal> DiffOtherCommissionPercentage { get; set; }
        public Nullable<decimal> DiffUATPPercentage { get; set; }
        public Nullable<decimal> DiffHandlingFeesPercentage { get; set; }
        public Nullable<decimal> DiffVatAmountPercentage { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Indicator { get; set; }
        public string USERACTIONINDICATOR { get; set; }
        public string USERACTION { get; set; }
        public string ReasonForAcceptance { get; set; }
        public string RejectionInvoiceNo { get; set; }
        public Nullable<System.DateTime> RejectionInvoiceDate { get; set; }
        public string RejectionBillingPeriod { get; set; }
        public string RejectionStatus { get; set; }
        public string RejectionMthOfClearance { get; set; }
        public string RejectionXmlFile { get; set; }
        public string SISValidation { get; set; }
        public Nullable<System.DateTime> SISPostedDate { get; set; }
        public Nullable<int> BILLINGMEMO { get; set; }
        public Nullable<int> DUPLICATE { get; set; }
        public string OriginalDuplicateInvoiceNo { get; set; }
        public string REJECTIONMEMONUMBER { get; set; }
        public string BreakdownSerialNumber { get; set; }
        public string BatchSeq { get; set; }
        public Nullable<int> NotinSystem { get; set; }
        public Nullable<int> Accounting { get; set; }
        public Nullable<System.DateTime> PostAccounting { get; set; }
        public Nullable<int> Archived { get; set; }
        public Nullable<System.DateTime> ArchivedDate { get; set; }
    }
}
