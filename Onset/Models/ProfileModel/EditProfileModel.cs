using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Onset.Models.ProfileModel
{
    public class EditProfileModel
    {
        [Required]
        public string FIRSTNAME { get; set; }
        [Required]
        public string LASTNAME { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EMAIL { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PHONE { get; set; }
        [Required]
        public string DESIGNATION { get; set; }
        [Required]
        public string ORGANIZATION { get; set; }
        [Required]
        public string LOCATION { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DATEOFBIRTH { get; set; }
        public string PICTUREPATH { get; set; }
        public HttpPostedFileBase PICTURE { get; set; }
        public string DESCRIPTION { get; set; }
    }
}