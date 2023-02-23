using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Onset.Models.FormModel
{
    public class RegistrationForm
    {
        [Required]
        [Display(Name = "First Name")]
        [DataType(DataType.Text)]
        public string FIRSTNAME { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [DataType(DataType.Text)]
        public string LASTNAME { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string EMAIL { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string PASSWORD { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("PASSWORD", ErrorMessage = "Password Does Not Match.")]
        public string CONFIRMPASSWORD { get; set; }

        [Required]
        [Display(Name = "Designation")]
        [DataType(DataType.Text)]
        public string DESIGNATION { get; set; }

        [Required]
        [Display(Name = "Organization")]
        [DataType(DataType.Text)]
        public string ORGANIZATION { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PHONE { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string GENDER { get; set; }

        [Required]
        [Display(Name = "User Type")]
        public string USERTYPE { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public System.DateTime DATEOFBIRTH { get; set; }

        [Required]
        [Display(Name = "Location")]
        [DataType(DataType.Text)]
        public string LOCATION { get; set; }
    }
}