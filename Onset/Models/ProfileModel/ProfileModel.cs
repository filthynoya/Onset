using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Onset.Models.ProfileModel
{
    public class ProfileModel
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string DESIGNATION { get; set; }
        public string ORGANIZATION { get; set; }
        public string LOCATION { get; set; }
        public System.DateTime DATEOFBIRTH { get; set; }
        public string PICTURE { get; set; }
        public string DESCRIPTION { get; set; }
        public System.DateTime REGTIME { get; set; }
        public bool OTHERPROFILE { get; set; }
        public int OTHERID { get; set; }
    }
}