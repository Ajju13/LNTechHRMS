using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models
{
    public class Attendances
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [ForeignKey("Employee")]
        public string Emp_id { get; set; }

        public virtual Employee Employee { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public float Work_Hours { get; set; } = 0;

        public float Break_Hours { get; set; } = 0;
    }
}