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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.UserHistories = new HashSet<UserHistory>();
            this.UserSessions = new HashSet<UserSession>();
        }
    
        public string UsrId { get; set; }
        public string UsrHash { get; set; }
        public string UsrSalt { get; set; }
        public string UsrStatus { get; set; }
        public byte UsrSecurityLevel { get; set; }
        public System.DateTime UsrPwdExpiry { get; set; }
        public string UsrModifyBy { get; set; }
        public System.DateTime UsrModifyDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserHistory> UserHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSession> UserSessions { get; set; }
    }
}