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
    
    public partial class RjTax
    {
        public System.Guid TaxGuid { get; set; }
        public Nullable<decimal> Billed { get; set; }
        public Nullable<decimal> Accepted { get; set; }
        public Nullable<decimal> Difference { get; set; }
        public string Code { get; set; }
        public string InvoiceNumber { get; set; }
        public int CouponNumber { get; set; }
        public string RjMemo { get; set; }
        public string AirportFrom { get; set; }
        public string AirportTo { get; set; }
        public string DocumentNumber { get; set; }
        public Nullable<System.Guid> RjGuid { get; set; }
    }
}