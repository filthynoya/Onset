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
    
    public partial class TASK
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TASK()
        {
            this.TASKEMPLOYEES = new HashSet<TASKEMPLOYEE>();
            this.TASKFILES = new HashSet<TASKFILE>();
            this.TASKMANAGERS = new HashSet<TASKMANAGER>();
            this.TASKPROGRESSES = new HashSet<TASKPROGRESS>();
        }
    
        public int TASK_ID { get; set; }
        public string TASK_NAME { get; set; }
        public string TASK_CATEGORY { get; set; }
        public string TASK_DESCRIPTION { get; set; }
        public System.DateTime TASK_DEADLINE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TASKEMPLOYEE> TASKEMPLOYEES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TASKFILE> TASKFILES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TASKMANAGER> TASKMANAGERS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TASKPROGRESS> TASKPROGRESSES { get; set; }
    }
}
