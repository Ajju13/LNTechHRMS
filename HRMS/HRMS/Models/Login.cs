using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models
{
    public class Login
    {
        [Key]
        public int Login_ID { get; set; }
        public string Emp_Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string VerificationToken { get; set; }
        public bool EmailConfirmed { get; set; }
        public string ResetPasswordCode { get; set; }
        public string ResetPasswordExpiry { get; set; }

        public virtual Employee Employee { get; set; }
    }
}