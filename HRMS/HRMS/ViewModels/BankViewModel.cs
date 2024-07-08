using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.ViewModels
{
    public class BankViewModel
    {
        public List<Bank> Banks { get; set; }
        public Bank AddBank { get; set; }
        public List<Employee> Employees { get; set; }
    }
}