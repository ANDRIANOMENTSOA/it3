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
    
    public partial class CurrencyRate
    {
        public string Period { get; set; }
        public int CurrencyType { get; set; }
        public string Currency { get; set; }
        public decimal USDRate { get; set; }
        public decimal GBPRate { get; set; }
        public decimal EURRate { get; set; }
        public System.DateTime DateAdded { get; set; }
    
        public virtual Currency Currency1 { get; set; }
    }
}
