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
    
    public partial class PROGRESSFILE
    {
        public int PROGRESSFILE_ID { get; set; }
        public int PROGRESS_ID { get; set; }
        public string FILES { get; set; }
    
        public virtual PROGRESS PROGRESS { get; set; }
    }
}
