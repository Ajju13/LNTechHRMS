using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models
{
    public class Employee_info
    {
        [Key]
        public long Emp_info_id { get; set; }
        public string Emp_Id { get; set; }
        public DateTime Joining_date { get; set; }
        public bool Probation { get; set; }
        public DateTime? Probation_Confirmation_Date { get; set; }
        public string Location { get; set; }
        public long Department_Id { get; set; }
        public long Branch_id { get; set; }
        public string Designation { get; set; }
        public string Job_Title { get; set; }
        public string Official_Email { get; set; }
        public string Employment_Type { get; set; }
        public string Machine_Code { get; set; }

        public virtual Employee Employee { get; set; }
    }
}