using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PaxModel
    {
        public string RowNum { get; set; }
        public string DocumentNumber { get; set; }
        public string CheckDigit { get; set; }
        public string Tax1Amount { get; set; }
        public string Tax2Amount { get; set; }
        public string Tax3Amount { get; set; }
        public string AgentNumericCode { get; set; }
        public string TradingName { get; set; }
        public string DateofIssue { get; set; }
        public string SaleDate { get; set; }
        public string DomesticInternational { get; set; }
        public string PassengerName { get; set; }
        public string PassengerSpecificData { get; set; }
        public string FareCalculationArea { get; set; }
        public string EndosRestriction { get; set; }
        public string Fare { get; set; }
        public string FareCurrency { get; set; }
        public string ComputedFare { get; set; }
        public string EquivalentFare { get; set; }
        public string TotalAmount { get; set; }
        public string TotalCurrency { get; set; }
        public string CommRate { get; set; }
        public string Comm { get; set; }
        public string BookingAgentIdentification { get; set; }
        public string BookingEntityOutletType { get; set; }
        public string FareCalculationPricingIndicator { get; set; }
        public string TransactionCode { get; set; }
        public string FareCalculationModeIndicator { get; set; }
        public string AmountCollectedCurrency { get; set; }
        public string AmountCollected { get; set; }
        public string TaxOnCommissionCollected { get; set; }
    }
}