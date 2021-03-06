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
    
    public partial class RefundManagement
    {
        public int SequenceNo { get; set; }
        public System.Guid HdrGuid { get; set; }
        public string PassengerName { get; set; }
        public Nullable<System.DateTime> DateOfRefund { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentType { get; set; }
        public Nullable<System.DateTime> DateOfIssue { get; set; }
        public string RefundType { get; set; }
        public string OriginalRouting { get; set; }
        public string ActualFlownRouting { get; set; }
        public string OriginalFareCurrencyPaid { get; set; }
        public Nullable<decimal> OriginalFarePaid { get; set; }
        public string EFPCurrency { get; set; }
        public Nullable<decimal> EFPAmount { get; set; }
        public string ActualFareCurrencyPaid { get; set; }
        public Nullable<decimal> ActualFarePaid { get; set; }
        public Nullable<decimal> FareDifference { get; set; }
        public Nullable<decimal> BankersSellingRate { get; set; }
        public string EquivalentFareCurrency { get; set; }
        public Nullable<decimal> EquivalentFare { get; set; }
        public string OriginalTFCSurchargeCurrency { get; set; }
        public Nullable<decimal> OriginalTFCs { get; set; }
        public Nullable<decimal> OriginalSurcharges { get; set; }
        public Nullable<decimal> TotalOriginalTFCs_Surcharges { get; set; }
        public string ActualTFCsSurchargeCurrency { get; set; }
        public Nullable<decimal> ActualTFCs { get; set; }
        public Nullable<decimal> ActualSurcharges { get; set; }
        public Nullable<decimal> TotalActualTFCs_Surcharges { get; set; }
        public Nullable<decimal> TFCs_SurchargesDifference { get; set; }
        public string ServiceChargeCurrency { get; set; }
        public Nullable<decimal> ServiceCharges { get; set; }
        public string PenaltyFeesCurrency { get; set; }
        public Nullable<decimal> PenaltyFees { get; set; }
        public Nullable<decimal> CommissionRecalPercentage { get; set; }
        public string CommissionRecallCurrency { get; set; }
        public Nullable<decimal> CommissionRecalAmount { get; set; }
        public string OtherCurrency { get; set; }
        public Nullable<decimal> Other { get; set; }
        public string TotalAmountRetainedCurrency { get; set; }
        public Nullable<decimal> TotalAmountRetainedByIssuingAirline { get; set; }
        public string CurrencyToBeRefunded { get; set; }
        public Nullable<decimal> AmountToBeRefunded { get; set; }
        public Nullable<bool> OALInvolvedInRouting { get; set; }
        public string ResonForRefund { get; set; }
        public string RefundedBy { get; set; }
        public Nullable<bool> Authorised { get; set; }
        public Nullable<bool> Refunded { get; set; }
        public string RefundeProcessBy { get; set; }
        public string ProcessIP { get; set; }
        public string AuthorisedBy { get; set; }
        public string AuthorisedIP { get; set; }
        public string RefundedPerson { get; set; }
        public string RefundedPersonIP { get; set; }
    }
}
