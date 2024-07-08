using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models
{
    public class Education
    {
        [Key]
        public long Education_id { get; set; }
        public string Emp_id { get; set; }
        public string University_College_Name { get; set; }
        public string Degree { get; set; }
        public System.DateTime From_Date { get; set; }
        public System.DateTime To_Date { get; set; }

        public virtual Employee Employee { get; set; }
    }
}