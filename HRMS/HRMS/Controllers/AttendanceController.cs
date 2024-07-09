using HRMS.Models;
using HRMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;


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

        public ActionResult PersonalAttendence()
        {
            // Check if user is authenticated
            var authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Decrypt the authentication ticket
            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
            if (ticket == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Get the username from the ticket
            string username = ticket.Name;

            // Retrieve employee info based on username
            var user = db.Logins.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Retrieve employee info based on Emp_Id from Login table
            var employeeInfo = db.EmployeeInfos.FirstOrDefault(e => e.Emp_Id == user.Emp_Id);
            if (employeeInfo == null)
            {
                ModelState.AddModelError("", "Employee information not found.");
                return View();
            }

            // Prepare data for the view model
            AdminAttendanceViewModel viewModel = new AdminAttendanceViewModel();

            // Default behavior: Display attendance for current month and year
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            // Ensure proper date format parsing for current month and year
            DateTime startDate = new DateTime(currentYear, currentMonth, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            // Fetch attendance records for the current user
            viewModel.AttendanceRecords = db.Attendances
                .Where(a => a.Date != null &&
                            a.Date >= startDate && a.Date <= endDate &&
                            a.Emp_id == user.Emp_Id)
                .ToList();

            var empIds = viewModel.AttendanceRecords.Select(ar => ar.Emp_id).ToList();

            // Fetch checks corresponding to attendance records
            viewModel.Checks = db.Check
                .Where(c => empIds.Contains(c.Emp_id))
                .ToList();

            ViewBag.SelectedEmployeeId = user.Emp_Id; // Set as per your requirement
            ViewBag.SelectedMonth = currentMonth;
            ViewBag.SelectedYear = currentYear;

            // Pass the selected date to the view model
            viewModel.SelectedDate = new DateTime(currentYear, currentMonth, 1);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult PersonalAttendence(string empId)
        {
            // Validate parameter
            if (string.IsNullOrEmpty(empId))
            {
                return HttpNotFound("EmpId is required.");
            }

            // Get the current date
            DateTime currentDate = DateTime.Today;

            // Get the last check for today
            var lastCheck = db.Check
                .Where(c => c.Emp_id == empId && DbFunctions.TruncateTime(c.CheckTime) == currentDate)
                .OrderByDescending(c => c.CheckTime)
                .FirstOrDefault();

            // Determine the next check type
            string nextCheckType = "IN"; // Default to CheckIn
            if (lastCheck != null && lastCheck.CheckType == "IN")
            {
                nextCheckType = "OUT";
            }

            // Get the user's IP address
            string ipAddress = Request.UserHostAddress;

            // Get location based on IP address
            Address location = GetLocationFromIP(ipAddress);

            // Create new Check record
            var newCheck = new Check
            {
                Emp_id = empId,
                CheckTime = DateTime.UtcNow,
                CheckType = nextCheckType,
                IPAddress = ipAddress
            };

            // Add Check to database
            db.Check.Add(newCheck);
            db.SaveChanges();

            // Save location information
            location.CheckInId = newCheck.Id;
            db.Address.Add(location);
            db.SaveChanges();

            // Return success response
            return Json(new { success = true, message = "Check added successfully." }, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private Address GetLocationFromIP(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new ArgumentNullException(nameof(ipAddress));
            }

            string url = $"http://ip-api.com/json/{ipAddress}";
            using (var client = new WebClient())
            {
                try
                {
                    string json = client.DownloadString(url);
                    if (!string.IsNullOrEmpty(json))
                    {
                        dynamic data = JsonConvert.DeserializeObject(json);

                        if (data != null)
                        {
                            var address = new Address
                            {
                                Country = data.country,
                                Region = data.regionName,
                                City = data.city,
                                Zip = data.zip,
                                Latitude = data.lat,
                                Longitude = data.lon
                            };

                            return address;
                        }
                        else
                        {
                            throw new Exception("Failed to deserialize JSON.");
                        }
                    }
                    else
                    {
                        throw new Exception("Received empty JSON response.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions as needed
                    throw new Exception("Error fetching location data: " + ex.Message);
                }
            }
        }
    }
}