using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models
{
    public class Salary
    {
        [Key]
        public long Sal_id { get; set; }
        public string Emp_id { get; set; }
        public long Payroll_Type { get; set; }
        public string Frequency { get; set; }
        public string NTN_Number { get; set; }
        public string Payment_Method { get; set; }
        public double Probation_Salary { get; set; }
        public double After_Probation_Gross_Salary { get; set; }
        public bool Tax_Exempted_ { get; set; }
        public string Description { get; set; }

        public virtual Employee Employee { get; set; }
    }
}