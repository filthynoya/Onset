using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onset.Models.AdminModel
{
    public class UsersModel
    {
        public string NAME { get; set; }
        public List<EMPLOYEE> EMPLIST { get; set; }
        public List<MANAGER> MANLIST { get; set; }
    }
}