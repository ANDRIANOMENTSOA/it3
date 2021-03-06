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
    
    public partial class Country
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Country()
        {
            this.Cities = new HashSet<City>();
            this.PMPAttachments = new HashSet<PMPAttachment>();
            this.TFCs = new HashSet<TFC>();
        }
    
        public string Code { get; set; }
        public string Name { get; set; }
        public string SubArea { get; set; }
        public string SubGroup { get; set; }
        public string Currency { get; set; }
        public string PostCode { get; set; }
        public byte Status { get; set; }
        public byte EuroMember { get; set; }
        public Nullable<decimal> Commission { get; set; }
        public Nullable<decimal> ComDomestic { get; set; }
        public Nullable<decimal> ComInternational { get; set; }
        public Nullable<decimal> ComPax { get; set; }
        public string CountryISO3L { get; set; }
        public Nullable<int> CountryISONum { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<City> Cities { get; set; }
        public virtual Currency Currency1 { get; set; }
        public virtual IataSubArea IataSubArea { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PMPAttachment> PMPAttachments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TFC> TFCs { get; set; }
    }
}
