﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onset.Models.DashboardModel
{
    public class RoomsModel
    {
        public string NAME { get; set; }
        public string USERTYPE { get; set; }
        public string PROFILEPIC { get; set; }
        public DashboardModel DASHINFO { get; set; }

        public List<ROOM> ROOMS { get; set; }
    }
}