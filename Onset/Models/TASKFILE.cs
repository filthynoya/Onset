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
    
    public partial class TASKFILE
    {
        public int TASKFILE_ID { get; set; }
        public int TASK_ID { get; set; }
        public string FILES { get; set; }
    
        public virtual TASK TASK { get; set; }
    }
}
