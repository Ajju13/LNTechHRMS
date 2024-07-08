using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.ViewModels
{
    public class AddDocumentViewModel
    {
        public List<Emp_Documents> Emp_Documents { get; set; }
        public List<Emp_Document_Type> DocumentTypes { get; set; }
        public Emp_Documents AddDocument { get; set; }
        public List<Employee> Employees { get; set; }
    }
}