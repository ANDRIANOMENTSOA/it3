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
    
    public partial class SalesDocumentDetailListing_Result
    {
        public Nullable<System.DateTime> SaleDate { get; set; }
        public string AgentNumericCode { get; set; }
        public string AgentName { get; set; }
        public string TicketingAgentID { get; set; }
        public string SignCode { get; set; }
        public string TourCode { get; set; }
        public string DocumentNumber { get; set; }
        public string FareCurrency { get; set; }
        public Nullable<decimal> Fare { get; set; }
        public Nullable<decimal> EquivalentFare { get; set; }
        public string TotalCurrency { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string TaxCurrency { get; set; }
        public Nullable<decimal> TotalTax { get; set; }
        public string Tax1Code { get; set; }
        public Nullable<decimal> Tax1Amount { get; set; }
        public string Tax2Code { get; set; }
        public Nullable<decimal> Tax2Amount { get; set; }
        public string Tax3Code { get; set; }
        public Nullable<decimal> Tax3Amount { get; set; }
        public Nullable<decimal> Commission { get; set; }
        public Nullable<decimal> TaxOnCommission { get; set; }
        public Nullable<decimal> NET_Total { get; set; }
        public Nullable<decimal> AmountCollectedMinusCommission { get; set; }
        public Nullable<System.DateTime> RefundDate { get; set; }
        public Nullable<decimal> TotalRefundAmount { get; set; }
        public string FOP { get; set; }
        public string PassengerName { get; set; }
        public string AgentSignCode { get; set; }
        public string BookingReference { get; set; }
        public string TransactionCode { get; set; }
        public string ExchangeADC { get; set; }
        public string Exchange { get; set; }
        public Nullable<bool> IsReissue { get; set; }
        public string OriginalDocumentNumber { get; set; }
    }
}