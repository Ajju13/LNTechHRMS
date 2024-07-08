using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models
{
    public class Relative
    {
        [Key]
        public long Relative_id { get; set; }
        public string Emp_Id { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string Address { get; set; }
        public string Mobile_No { get; set; }
        public string Contact_Type { get; set; }

        public virtual Employee Employee { get; set; }
    }
}