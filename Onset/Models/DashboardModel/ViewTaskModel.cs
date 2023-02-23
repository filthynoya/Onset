using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onset.Models.DashboardModel
{
    public class ViewTaskModel
    {
        public string NAME { get; set; }
        public string USERTYPE { get; set; }
        public string PROFILEPIC { get; set; }

        public int? TASKID { get; set; }
        public string TASKNAME { get; set; }
        public string CATEGORY { get; set; }
        public string DESCRIPTION { get; set; }
        public string EMPLOYEENAME { get; set; }
        public System.DateTime DEADLINE { get; set; }

        public List<int> PERCENT { get; set; }

        public List<string> FILES { get; set; }

        public DashboardModel DASHINFO { get; set; }
    }
}