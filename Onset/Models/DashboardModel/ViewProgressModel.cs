using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onset.Models.DashboardModel
{
    public class ViewProgressModel
    {
        public string NAME { get; set; }
        public string USERTYPE { get; set; }
        public string PROFILEPIC { get; set; }

        public List<string> DESCRIPTION { get; set; }
        public List<string> IMAGES { get; set; }

        public DashboardModel DASHINFO { get; set; }
    }
}