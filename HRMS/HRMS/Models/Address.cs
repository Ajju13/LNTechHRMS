using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HRMS.Models
{
    public class Address
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        [ForeignKey("Check")]
        public int CheckInId { get; set; }
        public virtual Check Check { get; set; }

        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
}
