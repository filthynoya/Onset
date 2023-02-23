using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onset.Models.DashboardModel
{
    public class ViewRoomModel
    {
        public string NAME { get; set; }
        public string USERTYPE { get; set; }
        public string PROFILEPIC { get; set; }
        public DashboardModel DASHINFO { get; set; }

        public string ROOMNAME { get; set; }
        public List<int> RECEIVED { get; set; }
        public List<int> SENT { get; set; }
    }
}