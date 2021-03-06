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
    
    public partial class GetIbDetails_Result
    {
        public System.Guid HistoryGuid { get; set; }
        public Nullable<System.Guid> PrimeGuid { get; set; }
        public Nullable<System.Guid> RjGuid { get; set; }
        public Nullable<System.Guid> HistoryTaxGuid { get; set; }
        public long SellerID { get; set; }
        public string AirportFrom { get; set; }
        public string AirportTo { get; set; }
        public int ChargeCode { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string YourInvoiceNumber { get; set; }
        public string YourInvoiceDate { get; set; }
        public string ReasonCode { get; set; }
        public long RejectionMemoNumber { get; set; }
        public string IssuingAirline { get; set; }
        public int CouponNumber { get; set; }
        public string DocumentNumber { get; set; }
        public string CheckDigit { get; set; }
        public Nullable<decimal> GrossBilled { get; set; }
        public Nullable<decimal> GrossAccepted { get; set; }
        public Nullable<decimal> GrossDifference { get; set; }
        public Nullable<decimal> TotalNetAmount { get; set; }
        public Nullable<decimal> ISCAllowed { get; set; }
        public Nullable<decimal> ISCAccepted { get; set; }
        public Nullable<decimal> ISCDifference { get; set; }
        public Nullable<decimal> AddOnChargePercentage { get; set; }
        public string ProrateSlipBreakdown { get; set; }
        public Nullable<int> Processed { get; set; }
        public string TaxCode { get; set; }
    }
}
