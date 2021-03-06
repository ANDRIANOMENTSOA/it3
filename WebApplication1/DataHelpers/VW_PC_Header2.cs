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
    
    public partial class VW_PC_Header2
    {
        public System.Guid HdrGuid { get; set; }
        public string DocumentNumber { get; set; }
        public string TransactionGroup { get; set; }
        public string TransactionCode { get; set; }
        public string PNR_NO { get; set; }
        public string AgentNumericCode { get; set; }
        public string DocumentType { get; set; }
        public string GSA_SR_NAME { get; set; }
        public string SR_NAME { get; set; }
        public string SR_CODE { get; set; }
        public string SR_GROUP1 { get; set; }
        public string SR_GROUP2 { get; set; }
        public string SR_GROUP3 { get; set; }
        public string SR_GROUP4 { get; set; }
        public string SR_TYPE { get; set; }
        public string SR_ISN { get; set; }
        public string LEGAL_NAME { get; set; }
        public string PaxSPC { get; set; }
        public Nullable<int> ADULT_COUNT { get; set; }
        public Nullable<int> CHILD_COUNT { get; set; }
        public Nullable<int> INFANT_COUNT { get; set; }
        public Nullable<decimal> VAT_DIVISOR { get; set; }
        public string SOURCE { get; set; }
        public Nullable<System.DateTime> DATE_OF_ACTION { get; set; }
        public Nullable<System.DateTime> DATE_OF_ACTION_TIME { get; set; }
        public Nullable<System.DateTime> DateofIssue { get; set; }
        public Nullable<decimal> Fare { get; set; }
        public Nullable<decimal> LocFare { get; set; }
        public Nullable<decimal> RepFare { get; set; }
        public Nullable<decimal> EquivalentFare { get; set; }
        public string FareCurrency { get; set; }
        public string TotalCurrency { get; set; }
        public string PayCurrency { get; set; }
        public string HostCurrency { get; set; }
        public string RepCurrency { get; set; }
        public Nullable<double> USDRateOfPayCurrency { get; set; }
        public Nullable<double> USDRateOfHostCurrency { get; set; }
        public string MiscItem { get; set; }
        public Nullable<int> MiscItemQty { get; set; }
        public string MiscConnectedDocument { get; set; }
        public string MiscItemGroup { get; set; }
        public Nullable<int> MiscIsCatering { get; set; }
        public string SERVICE_TYPE { get; set; }
        public string TrueOriginDestinationCityCodes { get; set; }
        public string DOM_INT { get; set; }
        public string AgencyDataAgentUse { get; set; }
        public string IS_GROUP { get; set; }
        public Nullable<System.DateTime> BillingAnalysisEndingDate { get; set; }
        public Nullable<long> Parent_SR_ISN { get; set; }
        public string BankName { get; set; }
        public string BNK_Branch_Name { get; set; }
        public string BankAccount { get; set; }
        public string CountryCode { get; set; }
        public string CITY_CODE { get; set; }
        public string PassengerName { get; set; }
        public string BspIdentifier { get; set; }
    }
}
