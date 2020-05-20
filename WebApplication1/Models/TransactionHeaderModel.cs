using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TransactionHeaderModel
    {
        public string firstDocNumber { get; set; } 
        public string secondDocNumber { get; set; } 
        public string CheckDigit { get; set; }
        public string TrueOriginDestinationCityCodes { get; set; }
        public string FareCalculationArea { get; set; }
        public string FareCurrency { get; set; }
        public string Fare { get; set; }
        public string BookingReference { get; set; }
        public string PassengerName { get; set; }
        public string DateofIssue { get; set; }
        public string AgentNumericCode { get; set; }
        public string BookingAgentIdentification { get; set; }
        public string ReportingSystemIdentifier { get; set; }
        public string VendorIdentification { get; set; }
        public string LegalName { get; set; }
        public string LocationAddress { get; set; }
        public string LocationCity { get; set; }
        public string LocationCountry { get; set; }
        public string Tax1Currency { get; set; }
        public string Tax2Currency { get; set; }
        public string Tax3Currency { get; set; }
        public string Tax1Code { get; set; }
        public string Tax2Code { get; set; }
        public string Tax3Code { get; set; }
        public string Tax1Amount { get; set; }
        public string Tax2Amount { get; set; }
        public string Tax3Amount { get; set; }
        public string TotalCurrency { get; set; }
        public string TotalAmount { get; set; }
        public string EquivalentFare { get; set; }
        public string FareCalculationModeIndicator { get; set; }
        public string EndosRestriction { get; set; }
        public string Discount { get; set; }
        public string VendorISOCountryCode { get; set; }
        public string CommercialAgreementReference { get; set; }
        public string TransactionCode { get; set; }
        public string AmountCollected { get; set; }
        public string OwnIsoCountry { get; set; }
        public string OwnAirline { get; set; }
        public string CommissionCollected { get; set; }
        public string CommissionCollectedCurrency { get; set; }

    }
    




}

