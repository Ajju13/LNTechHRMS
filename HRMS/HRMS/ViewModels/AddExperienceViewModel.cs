using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.ViewModels
{
    public class AddExperienceViewModel
    {
        public List<Experience> Experience { get; set; }
        public Experience AddExperience { get; set; }
        public List<Employee> Employees { get; set; }
    }
}