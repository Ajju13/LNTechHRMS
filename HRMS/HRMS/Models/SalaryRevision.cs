using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HRMS.Models
{
    public class SalaryRevision
    {
        [Key]
        public int SalReviseId { get; set; }
        [Required]
        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public double ReviseSalary { get; set; }

        public DateTime EffectiveDate { get; set; }

    }
}