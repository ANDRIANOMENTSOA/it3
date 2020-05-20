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
    
    public partial class Invoice1
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice1()
        {
            this.InvoiceDetails = new HashSet<InvoiceDetail>();
        }
    
        public int InvoiceID { get; set; }
        public Nullable<int> InvoiceFrom { get; set; }
        public Nullable<int> InvoiceTo { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string Currency { get; set; }
        public Nullable<System.DateTime> PaymentDueDate { get; set; }
        public string TermsOfPayment { get; set; }
        public string Amount { get; set; }
        public string Details { get; set; }
    
        public virtual Contact Contact { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}