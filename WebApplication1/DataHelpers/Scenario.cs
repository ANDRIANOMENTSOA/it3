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
    
    public partial class Scenario
    {
        public int AccountingScenarioID { get; set; }
        public string AccountingContext { get; set; }
        public int Sequence { get; set; }
        public string AccountID { get; set; }
        public string Amount { get; set; }
        public bool DebitFlag { get; set; }
        public string DocType { get; set; }
        public string Criteria { get; set; }
        public string OtherInfo { get; set; }
    
        public virtual AccountMaster AccountMaster { get; set; }
        public virtual Charge Charge { get; set; }
    }
}