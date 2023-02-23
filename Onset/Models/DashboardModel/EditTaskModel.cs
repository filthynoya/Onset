using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Onset.Models.DashboardModel
{
    public class EditTaskModel
    {
        public string NAME { get; set; }
        public string USERTYPE { get; set; }
        public string PROFILEPIC { get; set; }

        [Required]
        public string TASKNAME { get; set; }
        public string CATEGORY { get; set; }
        public List<SelectListItem> CATEGORIES { get; set; }
        [Required]
        public string DESCRIPTION { get; set; }
        public int EMPLOYEEID { get; set; }
        public List<SelectListItem> EMPLOYEES { get; set; }
        [Required]
        public System.DateTime DEADLINE { get; set; }

        public DashboardModel DASHINFO { get; set; }
    }
}