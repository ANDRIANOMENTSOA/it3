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
    
    public partial class VW_InwardBilling_TaxDetail
    {
        public System.Guid PrimeGuid { get; set; }
        public string InvoiceNumber { get; set; }
        public int SerialNo { get; set; }
        public string RelatedDocumentNumber { get; set; }
        public Nullable<int> CouponNUmber { get; set; }
        public string TaxCode { get; set; }
        public string TaxCurrency { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public string BillingMonth { get; set; }
        public string PrevMOC { get; set; }
        public Nullable<decimal> USDRate { get; set; }
        public Nullable<decimal> TaxAmountUSD { get; set; }
        public string SalesPeriod { get; set; }
        public string PrevMOS { get; set; }
        public Nullable<decimal> USDRate_PrevMOS { get; set; }
        public Nullable<decimal> TaxAmountUSD_PrevMOS { get; set; }
    }
}
