using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Onset.Models.DashboardModel
{
    public class AddProgessModel
    {
        public string NAME { get; set; }
        public string USERTYPE { get; set; }
        public string PROFILEPIC { get; set; }

        public string DESCRIPTION { get; set; }
        public float PERCENT { get; set; }

        public int TASKID { get; set; }
        public List<SelectListItem> TASKS { get; set; }
        public string PROGESSFILEPATH { get; set; }
        public HttpPostedFileBase PROGRESSFILE { get; set; }

        public DashboardModel DASHINFO { get; set; }
    }
}