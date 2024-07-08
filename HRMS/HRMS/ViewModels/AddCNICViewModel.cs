using HRMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.ViewModels
{
    public class AddCNICViewModel
    {
        public List<CNIC> CNIC { get; set; }
        public CNIC AddCNIC { get; set; }
        public List<Employee> Employees { get; set; }
    }
}