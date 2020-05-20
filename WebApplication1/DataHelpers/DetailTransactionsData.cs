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
    
    public partial class DetailTransactionsData
    {
        public System.Guid HdrGuid { get; set; }
        public string DocumentNumber { get; set; }
        public Nullable<System.Guid> UploadedRef { get; set; }
        public string AgentNumericCode { get; set; }
        public Nullable<System.DateTime> DateofIssue { get; set; }
        public Nullable<System.DateTime> SaleDate { get; set; }
        public string PassengerName { get; set; }
        public Nullable<decimal> Fare { get; set; }
        public string FareCurrency { get; set; }
        public Nullable<decimal> EquivalentFare { get; set; }
        public string TotalCurrency { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string BookingReference { get; set; }
        public string TransactionCode1 { get; set; }
        public string DocumentType { get; set; }
        public bool SalesDataAvailable { get; set; }
        public string AccountingStatus { get; set; }
        public bool ProratedFlag { get; set; }
        public bool FBSFlag { get; set; }
        public Nullable<System.Guid> RelatedDocumentGuid { get; set; }
        public string RelatedDocumentNumber { get; set; }
        public string CouponIndicator { get; set; }
        public string TransactionCode2 { get; set; }
        public string AccountNumber { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Currency { get; set; }
        public string FormofPaymentType { get; set; }
        public string CustomerFileReference { get; set; }
        public string TransactionCode3 { get; set; }
        public string TransactionCode { get; set; }
        public string DocumentAmountType { get; set; }
        public string CurrencyType { get; set; }
        public string OtherAmountCode { get; set; }
        public Nullable<decimal> OtherAmount { get; set; }
        public Nullable<decimal> OtherAmountRate { get; set; }
        public Nullable<decimal> TicketDocumentAmount { get; set; }
        public Nullable<decimal> CommissionableAmount { get; set; }
    }
}