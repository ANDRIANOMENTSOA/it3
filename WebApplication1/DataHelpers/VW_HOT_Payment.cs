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
    
    public partial class VW_HOT_Payment
    {
        public string ProcessingFileName { get; set; }
        public System.DateTime ProcessingDate { get; set; }
        public string TicketDocumentNumber { get; set; }
        public string DocumentNumber { get; set; }
        public Nullable<bool> FBSFlag { get; set; }
        public string TransactionCode { get; set; }
        public string CurrencyType2 { get; set; }
        public string Currency { get; set; }
        public string FormofPaymentAmount { get; set; }
        public Nullable<decimal> HOT_FormofPaymentAmount { get; set; }
        public Nullable<decimal> FOP_Amount { get; set; }
        public string CurrencyType1 { get; set; }
        public string RemittanceCurrency { get; set; }
        public string RemittanceAmount { get; set; }
        public Nullable<decimal> HOT_RemittanceAmount { get; set; }
        public Nullable<decimal> FOP_RemittanceAmount { get; set; }
    }
}