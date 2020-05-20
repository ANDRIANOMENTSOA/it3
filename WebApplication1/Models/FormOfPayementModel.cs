using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FormOfPayementModel
    {
        public string FOP { get; set; }
        public string CUR { get; set; }
        public string AmountCollected { get; set; }
        public string RemitanceAmount { get; set; }
        public string TransactionCode { get; set; }
        public string IssueDate { get; set; }
        public string remitCur { get; set; }
    }
}