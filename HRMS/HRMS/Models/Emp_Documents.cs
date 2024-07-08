using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models
{
    public class Emp_Documents
    {
        [Key]
        public long id { get; set; }
        public string Emp_Id { get; set; }
        public long Document_Type_id { get; set; }
        public string Name { get; set; }
        public string File { get; set; }
        public string Issue_Date { get; set; }

        public virtual Emp_Document_Type Emp_Document_Type { get; set; }
        public virtual Employee Employee { get; set; }
    }
}