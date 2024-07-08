using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models
{
    public class CNIC
    {
        [Key]
        public long Document_id { get; set; }
        public string Emp_Id { get; set; }
        public string CNIC_Number { get; set; }
        public string Document_Attachment { get; set; }
        public DateTime Document_Expiry { get; set; }

        public virtual Employee Employee { get; set; }

    }
}