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
    
    public partial class VW_InwardRejectionList_Excluding_frUserChanges
    {
        public int BillingAirlineCode { get; set; }
        public string BillingMonth { get; set; }
        public Nullable<int> PeriodNo { get; set; }
        public string InvoiceNumber { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public int SerialNo { get; set; }
        public string Document_Number { get; set; }
        public Nullable<int> CpnNo { get; set; }
        public Nullable<int> CheckDigit { get; set; }
        public Nullable<decimal> OAL_Computation_Fare { get; set; }
        public Nullable<decimal> OWN_Computation_Fare { get; set; }
        public Nullable<decimal> Fare_Difference { get; set; }
        public Nullable<int> OAL_ISC_Percentage { get; set; }
        public decimal OWN_ISC_Percentage { get; set; }
        public Nullable<decimal> OAL_Computation_ISC { get; set; }
        public Nullable<decimal> OWN_Computation_ISC { get; set; }
        public Nullable<decimal> ISC_Difference { get; set; }
        public Nullable<decimal> OAL_Net { get; set; }
        public Nullable<decimal> OWN_Net { get; set; }
        public Nullable<decimal> Fare_Rejection { get; set; }
        public Nullable<double> OAL_Computation_TFC { get; set; }
        public Nullable<decimal> OWN_Computation_TFC { get; set; }
        public Nullable<double> TFC_Rejection { get; set; }
        public string Reason_For_Rejection { get; set; }
        public string Additional_Remarks { get; set; }
        public string Method { get; set; }
        public string IT_BT { get; set; }
        public string RBD { get; set; }
        public string FareBasis { get; set; }
        public string Endorsement_Box { get; set; }
        public string SPA_Applicability { get; set; }
        public System.Guid PrimeGuid { get; set; }
        public int ChargeCode { get; set; }
        public Nullable<byte> Processed { get; set; }
        public string SettlementMonthPeriod { get; set; }
        public string FromAirportCode { get; set; }
        public string ToAirportCode { get; set; }
    }
}
