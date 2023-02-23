using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onset.Models.DashboardModel
{
    public class DashboardModel
    {
        public string NAME { get; set; }
        public string USERTYPE { get; set; }
        public string PROFILEPIC { get; set; }

        public List<TASK> TASKS { get; set; }
        public List<EVENT> EVENTS { get; set; }

        public List<EVENT> TODOLIST { get; set; }

        public int NOWDATE { get; set; }
    }
}