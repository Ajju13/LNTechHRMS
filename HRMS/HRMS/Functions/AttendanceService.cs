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

            var checkRecords = _context.Check
                .Where(c => c.Emp_id == empId && DbFunctions.TruncateTime(c.CheckTime) == checkDate.Date)
                .OrderBy(c => c.CheckTime)
                .ToList();

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

            var attendance = _context.Attendances
                .FirstOrDefault(a => a.Emp_id == empId && DbFunctions.TruncateTime(a.Date) == checkDate.Date);

            if (attendance == null)
            {
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
                attendance.Work_Hours = (float)workHours;
                attendance.Break_Hours = (float)breakHours;
            }

            _context.SaveChanges();
        }

        public void UpdateAttendanceForChecks(List<Check> checks)
        {
            var groupedChecks = checks.GroupBy(c => new { c.Emp_id, Date = c.CheckTime.Date });

            foreach (var group in groupedChecks)
            {
                UpdateAttendanceForChecks(group.Key.Emp_id, group.Key.Date);
            }
        }
    }
}