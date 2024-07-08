using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.ViewModels
{
    public class AdminAttendanceViewModel
    {
        public List<Employee> Employees { get; set; }
        public List<Attendances> AttendanceRecords { get; set; }

        public List<Check> Checks { get; set; }
        public int? SelectedMonth { get; set; }
        public int? SelectedYear { get; set; }

        public DateTime? SelectedDate {  get; set; } 
    }
}