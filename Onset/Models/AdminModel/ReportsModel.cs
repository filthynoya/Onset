using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onset.Models.AdminModel
{
    public class ReportsModel
    {
        public string NAME { get; set; }
        public List<string> FROM { get; set; }
        public List<string> TO { get; set; }
        public List<int> ID { get; set; }
    }
}