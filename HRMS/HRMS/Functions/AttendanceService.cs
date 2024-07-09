using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HRMS.Functions
{
    public class AttendanceService
    {
        private readonly ApplicationDbContext _context;

        public AttendanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void UpdateAttendanceForChecks(string empId, DateTime checkDate)
        {
            double workHours = 0;
            double breakHours = 0;
            DateTime? lastCheckIn = null;
            DateTime? lastCheckOut = null;

            // Fetch check records for the specified employee and date
            var checkRecords = _context.Check
                .Where(c => c.Emp_id == empId && DbFunctions.TruncateTime(c.CheckTime) == checkDate.Date)
                .OrderBy(c => c.CheckTime)
                .ToList();

            // Calculate work hours and break hours based on check records
            foreach (var check in checkRecords)
            {
                if (check.CheckType == "CheckIn")
                {
                    if (lastCheckIn.HasValue && lastCheckOut.HasValue)
                    {
                        breakHours += (check.CheckTime - lastCheckOut.Value).TotalHours;
                    }
                    lastCheckIn = check.CheckTime;
                }
                else if (check.CheckType == "CheckOut" && lastCheckIn.HasValue)
                {
                    workHours += (check.CheckTime - lastCheckIn.Value).TotalHours;
                    lastCheckOut = check.CheckTime;
                    lastCheckIn = null;
                }
            }

            // Fetch or create attendance record for the employee and date
            var attendance = _context.Attendances
                .FirstOrDefault(a => a.Emp_id == empId && DbFunctions.TruncateTime(a.Date) == checkDate.Date);

            if (attendance == null)
            {
                // Create new attendance record if it doesn't exist
                attendance = new Attendances
                {
                    Emp_id = empId,
                    Date = checkDate.Date,
                    Work_Hours = (float)workHours,
                    Break_Hours = (float)breakHours
                };
                _context.Attendances.Add(attendance);
            }
            else
            {
                // Update existing attendance record with calculated hours
                attendance.Work_Hours = (float)workHours;
                attendance.Break_Hours = (float)breakHours;
            }

            // Save changes to the database
            _context.SaveChanges();
        }

        public void UpdateAttendanceForChecks(List<Check> checks)
        {
            // Group checks by employee ID and date
            var groupedChecks = checks.GroupBy(c => new { c.Emp_id, Date = c.CheckTime.Date });

            // Process each group of checks
            foreach (var group in groupedChecks)
            {
                // Update attendance for each employee and date group
                UpdateAttendanceForChecks(group.Key.Emp_id, group.Key.Date);
            }
        }

        public void CreateOrUpdateAttendanceForAllEmployees(DateTime checkDate)
        {
            // Get all distinct employee IDs
            var allEmployeeIds = _context.Logins.Select(u => u.Emp_Id).Distinct().ToList();

            // Process each employee ID
            foreach (var empId in allEmployeeIds)
            {
                // Fetch or create attendance record for each employee and date
                var attendance = _context.Attendances
                    .FirstOrDefault(a => a.Emp_id == empId && DbFunctions.TruncateTime(a.Date) == checkDate.Date);

                if (attendance == null)
                {
                    // Create new attendance record if it doesn't exist
                    attendance = new Attendances
                    {
                        Emp_id = empId,
                        Date = checkDate.Date,
                        Work_Hours = 0,    // Initialize with zero hours
                        Break_Hours = 0    // Initialize with zero hours
                    };
                    _context.Attendances.Add(attendance);
                }
                else
                {
                    // Update existing attendance record (if any logic to update is needed)
                    // Currently, we assume it's already handled by UpdateAttendanceForChecks method
                }
            }

            // Save changes to the database
            _context.SaveChanges();
        }
    }
}