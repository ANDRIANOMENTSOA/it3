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
    
    public partial class CurrencyRounding
    {
        public string Currency { get; set; }
        public string Acceptability { get; set; }
        public double FareRelatedCharges { get; set; }
        public double TaxesFeesCHarges { get; set; }
        public byte DecimalUnits { get; set; }
        public string Notes { get; set; }
        public string RoundType { get; set; }
    
        public virtual Currency Currency1 { get; set; }
    }
}
