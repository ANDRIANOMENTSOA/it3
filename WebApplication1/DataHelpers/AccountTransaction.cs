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
    
    public partial class AccountTransaction
    {
        public string Accounting_Year { get; set; }
        public string Accounting_Period { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public Nullable<decimal> Dr { get; set; }
        public Nullable<decimal> Cr { get; set; }
        public string Remarks { get; set; }
    }
}
