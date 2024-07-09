using HRMS.Functions; // Adjust namespace as per your project structure
using HRMS.Models;
using System;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using System.Threading;
using Owin;
using System.Web.Http;
using Hangfire;
using Microsoft.Owin;
using NLog;
using SimpleInjector;


using HangfireConfig = Hangfire.GlobalConfiguration;
using WebApiConfig = System.Web.Http.GlobalConfiguration;
using SimpleInjector.Lifestyles;

namespace HRMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private Timer dailyTaskTimer;
        private AttendanceService attendanceService;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private Container container = new Container(); // Initialize the container

        protected void Application_Start()
        {
            // Disable database initializer (useful in production)
            Database.SetInitializer<ApplicationDbContext>(null);

            // Initialize ApplicationDbContext if needed
            var dbContext = new ApplicationDbContext(); // Replace with your actual initialization

            // Initialize attendanceService with the application context
            attendanceService = new AttendanceService(dbContext); // Pass dbContext to constructor

            // Register MVC areas, routes, and filters
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Register types with SimpleInjector container
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            container.Register<ApplicationDbContext>(() => dbContext, Lifestyle.Scoped);
            container.Register<AttendanceService>(Lifestyle.Scoped);

            // Verify the container configuration
            container.Verify();

            // Initialize Hangfire using custom bootstrapper
            HangfireBootstrapper.Start(container); // Pass the container to HangfireBootstrapper.Start()

            // Schedule daily task using Hangfire
            RecurringJob.AddOrUpdate("DailyAttendanceJob", () => attendanceService.CreateOrUpdateAttendanceForAllEmployees(DateTime.Today), Cron.Daily, TimeZoneInfo.Local);

            // Start timer for daily task (optional, if still needed)
            // DateTime now = DateTime.Now;
            // DateTime nextRun = now.Date.AddDays(1); // Tomorrow at midnight
            // TimeSpan timeUntilNextRun = nextRun - now;
            // dailyTaskTimer = new Timer(DoDailyTask, null, timeUntilNextRun, TimeSpan.FromHours(24));
        }

        private void DoDailyTask(object state)
        {
            try
            {
                // Run your daily task here
                attendanceService.CreateOrUpdateAttendanceForAllEmployees(DateTime.Today);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred in daily task execution");
                // Optionally handle or log the exception
            }
        }

        private void ConfigureOwin(IAppBuilder app)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            // Use the OWIN pipeline to configure Web API
            app.UseWebApi(config);
        }
    }
}
