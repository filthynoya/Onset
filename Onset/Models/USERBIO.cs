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
    
    public partial class USERBIO
    {
        public int USERBIO_ID { get; set; }
        public int USER_ID { get; set; }
        public string USER_BIO { get; set; }
    
        public virtual USER USER { get; set; }
    }
}
