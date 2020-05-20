using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class OtherPayement
    {
        public String RelatedDocumentGuid { get; set; }
        public String SequenceNumber { get; set; }
        public String DocumentAmountType { get; set; }
        public String DateofIssue { get; set; }
        public String CurrencyType { get; set; }
        public String TransactionCode { get; set; }
        public String OtherAmountCode { get; set; }
        public String OtherAmount { get; set; }
        public String OtherAmountRate { get; set; }
        public String TicketDocumentAmount { get; set; }
        public String CommissionableAmount { get; set; }
        public String AmountEnteredbyAgent { get; set; }
        public String AmountPaidbyCustomer { get; set; }
        public String LateReportingPenalty { get; set; }
        public String NetFareAmount { get; set; }
        public String StatisticalCode { get; set; }
        public String HdrGuidRef { get; set; }
        public String DocumentNumber { get; set; }
        public String RelatedDocumentNumber { get; set; }
        public String FileSequence { get; set; }

    }
}