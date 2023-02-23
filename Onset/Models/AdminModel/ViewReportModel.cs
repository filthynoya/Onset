using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onset.Models.AdminModel
{
    public class ViewReportModel
    {
        public string NAME { get; set; }
        public REPORT RPT { get; set; }
        public string FROMNAME { get; set; }
        public string FROMEMAIL { get; set; }
        public string TONAME { get; set; }
        public string TOEMAIL { get; set; }
    }
}