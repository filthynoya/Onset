//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Onset.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EVENT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EVENT()
        {
            this.EVENTUSERS = new HashSet<EVENTUSER>();
        }
    
        public int EVENT_ID { get; set; }
        public string EVENT_NAME { get; set; }
        public string EVENT_CATEGORY { get; set; }
        public string EVENT_DESCRIPTION { get; set; }
        public System.DateTime EVENT_DEADLINE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EVENTUSER> EVENTUSERS { get; set; }
    }
}
