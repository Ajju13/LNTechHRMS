using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.ViewModels
{
    public class AddRelativeViewModel
    {
        public List<Relative> Relative { get; set; }
        public Relative AddRelative { get; set; }
        public List<Employee> Employees { get; set; }
    }
}