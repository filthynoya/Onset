using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onset.Models.DashboardModel
{
    public class ViewTable
    {
        public string NAME { get; set; }
        public string USERTYPE { get; set; }
        public string PROFILEPIC { get; set; }

        public List<EMPLOYEE> EMPS { get; set; }
        public List<MANAGER> MANS { get; set; }

        public List<TASK> TASKS { get; set; }
        public List<string> TASKUSER { get; set; }
        public List<int> PERCENT { get; set; }

        public List<EVENT> EVENTS { get; set; }

        public List<int> USERID { get; set; }

        public DashboardModel DASHINFO { get; set; }
    }
}