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
    
    public partial class City
    {
        public string AirportCode { get; set; }
        public string CityCode { get; set; }
        public string AirportName { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
        public string CityIsoCode { get; set; }
        public byte Status { get; set; }
    
        public virtual Country Country1 { get; set; }
    }
}
