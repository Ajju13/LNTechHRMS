using HRMS.Functions;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRMS.Controllers
{
    public class CheckController : Controller
    {
        // GET: Check
        private readonly ApplicationDbContext _context;
        private readonly AttendanceService _attendanceService;

        public CheckController()
        {
            _context = new ApplicationDbContext();
            _attendanceService = new AttendanceService(_context);
        }

        // GET: Check/Add?empId=1&checkType=CheckIn
        public ActionResult Add(string empId, string checkType)
        {
            // Validate parameters
            if (string.IsNullOrEmpty(empId) || string.IsNullOrEmpty(checkType))
            {
                return HttpNotFound("EmpId and CheckType are required.");
            }

            // Convert checkType to uppercase for consistency
            checkType = checkType.ToUpper();

            // Create new Check record
            var newCheck = new Check
            {
                Emp_id = empId,
                CheckTime = DateTime.Now,
                CheckType = checkType
            };

            // Add Check to database
            _context.Check.Add(newCheck);
            _context.SaveChanges();

            // Update attendance based on new Check
            _attendanceService.UpdateAttendanceForChecks(empId, newCheck.CheckTime.Date);

            // Redirect to a success page or return a JSON response indicating success
            return Json(new { success = true, message = "Check added successfully." }, JsonRequestBehavior.AllowGet);
        }

        // GET: Check/TestUpdateAttendance
        public ActionResult TestUpdateAttendance()
        {
            // Fetch checks for Emp_id "1" (example)
            var checks = _context.Check.Where(c => c.Emp_id == "1").ToList();

            // Call the service method to update attendance
            _attendanceService.UpdateAttendanceForChecks(checks);

            // Optionally, return a view or JSON response to indicate success
            return Json(new { success = true, message = "Attendance updated successfully." }, JsonRequestBehavior.AllowGet);
        }
    }
}