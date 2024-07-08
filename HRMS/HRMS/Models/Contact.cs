using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class Contact
    {
        [Key]
        public long Contact_id { get; set; }
        public string Emp_Id { get; set; }
        public string Email { get; set; }
        public string Land_Line { get; set; }
        public string Mobile_No { get; set; }

        public virtual Employee Employee { get; set; }
    }
}