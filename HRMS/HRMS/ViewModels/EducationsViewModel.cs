using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.ViewModels
{
    public class EducationsViewModel
    {
        public List<Education> Educations { get; set; }
        public Education AddEducation { get; set; }

        public List<Employee> Employees { get; set; }   
    }

    public class AddEducationViewModel
    {
        public List<Education> Educations { get; set; }
        public Education AddEducation { get; set; }
        public List<Employee> Employees { get; set; }
    }
}