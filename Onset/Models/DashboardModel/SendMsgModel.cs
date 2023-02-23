using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Onset.Models.DashboardModel
{
    public class SendMsgModel
    {
        public string NAME { get; set; }
        public string USERTYPE { get; set; }
        public string PROFILEPIC { get; set; }
        public DashboardModel DASHINFO { get; set; }

        public string MSG { get; set; }
        public int ROOMID { get; set; }
        public List<SelectListItem> ROOMS { get; set; }
    }
}