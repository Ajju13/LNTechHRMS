using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;

namespace HRMS.Functions
{
    public class HangfireJobs
    {
        private readonly Container _container;

        public HangfireJobs(Container container)
        {
            _container = container;
        }

        public void CreateOrUpdateAttendanceForAllEmployees()
        {
            using (AsyncScopedLifestyle.BeginScope(_container))
            {
                // Resolve AttendanceService within the scope
                var attendanceService = _container.GetInstance<AttendanceService>();

                // Call your service method
                attendanceService.CreateOrUpdateAttendanceForAllEmployees(DateTime.Today);
            }
        }
    }
}
