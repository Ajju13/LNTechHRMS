using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models
{
    public class Experience
    {
        [Key]
        public long Exp_id { get; set; }
        public string Emp_Id { get; set; }
        public string Company_name { get; set; }
        public string Position { get; set; }
        public System.DateTime From_Date { get; set; }
        public System.DateTime To_Date { get; set; }

        public virtual Employee Employee { get; set; }
    }
}