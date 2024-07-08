using HRMS.Models;
using HRMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HRMS.Controllers
{
    public class AttendanceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Attendance(string searchValue, int? month, int? year)
        {
            AdminAttendanceViewModel viewModel = new AdminAttendanceViewModel();

            if (!string.IsNullOrEmpty(searchValue))
            {
                int currentMonth = DateTime.Now.Month;
                int currentYear = DateTime.Now.Year;

                if (month.HasValue)
                {
                    currentMonth = month.Value;
                }

                if (year.HasValue)
                {
                    currentYear = year.Value;
                }

                // Ensure proper date format parsing for current month and year
                DateTime startDate = new DateTime(currentYear, currentMonth, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                viewModel.Employees = db.Employees.Where(e => e.Emp_id == searchValue).ToList();

                viewModel.AttendanceRecords = db.Attendances
                    .Where(a => a.Emp_id == searchValue && a.Date != null &&
                                a.Date >= startDate && a.Date <= endDate)
                    .ToList();

                // Fetch checks corresponding to attendance records
                viewModel.Checks = (from c in db.Check
                                    join a in db.Attendances on new { c.Emp_id, CheckDate = DbFunctions.TruncateTime(c.CheckTime) } equals new { a.Emp_id, CheckDate = DbFunctions.TruncateTime(a.Date) }
                                    where a.Emp_id == searchValue && DbFunctions.TruncateTime(a.Date) >= startDate.Date && DbFunctions.TruncateTime(a.Date) <= endDate.Date
                                    select c)
                                    .ToList();

                ViewBag.SelectedEmployeeId = searchValue;
                ViewBag.SelectedMonth = currentMonth;
                ViewBag.SelectedYear = currentYear;
            }
            else if (month.HasValue)
            {
                int currentYear = DateTime.Now.Year;

                if (year.HasValue)
                {
                    currentYear = year.Value;
                }

                // Ensure proper date format parsing for selected month and current year
                DateTime startDate = new DateTime(currentYear, month.Value, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                viewModel.Employees = db.Employees.ToList();

                viewModel.AttendanceRecords = db.Attendances
                    .Where(a => a.Date != null &&
                                a.Date >= startDate && a.Date <= endDate)
                    .ToList();

                // Fetch checks corresponding to attendance records
                viewModel.Checks = (from c in db.Check
                                    join a in db.Attendances on new { c.Emp_id, CheckDate = DbFunctions.TruncateTime(c.CheckTime) } equals new { a.Emp_id, CheckDate = DbFunctions.TruncateTime(a.Date) }
                                    where DbFunctions.TruncateTime(a.Date) >= startDate.Date && DbFunctions.TruncateTime(a.Date) <= endDate.Date
                                    select c)
                                    .ToList();

                ViewBag.SelectedEmployeeId = searchValue;
                ViewBag.SelectedMonth = month;
                ViewBag.SelectedYear = currentYear;
            }
            else
            {
                // Default behavior: Display attendance for all employees for current month and year
                int currentMonth = DateTime.Now.Month;
                int currentYear = DateTime.Now.Year;

                viewModel.Employees = db.Employees.ToList();

                // Ensure proper date format parsing for current month and year
                DateTime startDate = new DateTime(currentYear, currentMonth, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                viewModel.AttendanceRecords = db.Attendances
                    .Where(a => a.Date != null &&
                                a.Date >= startDate && a.Date <= endDate)
                    .ToList();

                // Fetch checks corresponding to att    endance records
                viewModel.Checks = (from c in db.Check
                                    join a in db.Attendances on new { c.Emp_id, CheckDate = DbFunctions.TruncateTime(c.CheckTime) } equals new { a.Emp_id, CheckDate = DbFunctions.TruncateTime(a.Date) }
                                    where DbFunctions.TruncateTime(a.Date) >= startDate.Date && DbFunctions.TruncateTime(a.Date) <= endDate.Date
                                    select c)
                                    .ToList();

                ViewBag.SelectedEmployeeId = searchValue;
                ViewBag.SelectedMonth = currentMonth;
                ViewBag.SelectedYear = currentYear;
            }

            // Pass the selected date to the view model
            viewModel.SelectedDate = new DateTime(ViewBag.SelectedYear, ViewBag.SelectedMonth, 1);

            return View(viewModel);
        }
    }
}