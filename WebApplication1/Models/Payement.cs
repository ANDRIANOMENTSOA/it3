using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Payement
    {
        public String RelatedDocumentGuid { get; set; }
        public String HdrGuid { get; set; }
        public String DocumentNumber { get; set; }
        public String CheckDigit { get; set; }
        public String IsConjunction { get; set; }
        public String IsReissue { get; set; }
        public String RelatedDocumentNumber { get; set; }
        public String RelatedTicketCheckDigit { get; set; }
        public String CouponIndicator { get; set; }
        public String TransactionCode { get; set; }
        public String TransmissionControlNumber { get; set; }
        public String SalesUploadGuid { get; set; }
        public String LiftUploadGuid { get; set; }
        public String SequenceNumber { get; set; }

        public String DateofIssue { get; set; }
        public String AccountNumber { get; set; }
        public String Amount { get; set; }
        public String Currency { get; set; }
        public String ApprovalCode { get; set; }
        public String ExtendedPaymentCode { get; set; }
        public String FormofPaymentType { get; set; }
        public String ExpiryDate { get; set; }
        public String InvoiceNumber { get; set; }
        public String InvoiceDate { get; set; }
        public String CustomerFileReference { get; set; }
        public String CreditCardCorporateContract { get; set; }
        public String AddressVerificationCode { get; set; }
        public String SourceofApprovalCode { get; set; }
        public String TransactionInformation { get; set; }
        public String AuthorisedAmount { get; set; }
        public String AccountNumber1 { get; set; }
        public String AuthorisedAmount1 { get; set; }
        public String CardVerificationValueResult { get; set; }
        public String CardVerificationValueResult1 { get; set; }
        public String SignedforAmount { get; set; }
        public String RemittanceAmount { get; set; }
        public String RemittanceCurrency { get; set; }

        public String HdrGuidRef { get; set; }

        public String TransactionCode2 { get; set; }


    }
}