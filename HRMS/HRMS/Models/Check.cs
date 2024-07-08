using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HRMS.Models
{
    public class Check
    {

        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Employee")]
        public string Emp_id { get; set; }
        public virtual Employee Employee { get; set; }
        [Required]
        public DateTime CheckTime { get; set; }

        [Required]
        [StringLength(50)]
        public string CheckType { get; set; }
    }
}