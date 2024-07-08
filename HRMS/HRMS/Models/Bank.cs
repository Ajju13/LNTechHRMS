using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models
{
    public class Bank
    {
        [Key]
        public string Bank_id { get; set; }
        public string Emp_Id { get; set; }
        public string Bank_Name { get; set; }
        public string Branch_Name { get; set; }
        public string Branch_Code { get; set; }
        public string Account_Title { get; set; }
        public string Account_No { get; set; }
        public System.DateTime Effective_Date { get; set; }
        public string Description { get; set; }

        public virtual Employee Employee { get; set; }
    }
}