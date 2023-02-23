using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onset.Models.DashboardModel
{
    public class IndexModel
    {
        public string NAME { get; set; }
        public string USERTYPE { get; set; }
        public string PROFILEPIC { get; set; }
        public List<TASK> TASKS { get; set; }
        public List<EMPLOYEE> EMPS { get; set; }
        public List<MANAGER> MANS { get; set; }
        public List<String> PROPICS { get; set; }
        public List<float> PERCENTS { get; set; }
        public List<int> USERIDS { get; set; }

        public DashboardModel DASHINFO { get; set; }
    }
}