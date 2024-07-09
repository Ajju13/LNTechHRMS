using HRMS.Functions; // Adjust namespace as per your project structure
using HRMS.Models;
using System.Web.Http;
using Owin;
using Hangfire;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

namespace HRMS
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Create the container
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            // Register your types, for instance:
            container.Register<ApplicationDbContext>(Lifestyle.Scoped);
            container.Register<AttendanceService>(Lifestyle.Scoped);

            // Register the Web API controllers.
            container.RegisterWebApiControllers(System.Web.Http.GlobalConfiguration.Configuration);

            // Verify the container configuration
            container.Verify();

            // Configure Hangfire to use SimpleInjector
            Hangfire.GlobalConfiguration.Configuration
                .UseSqlServerStorage("data source=PC-AZLAN\\SQLEXPRESS;initial catalog=11th;integrated security=True;trustservercertificate=True;")
                .UseActivator(new SimpleInjectorJobActivator(container));

            // Enable Hangfire dashboard and server
            app.UseHangfireDashboard("/hangfire");
            app.UseHangfireServer();

            // Configure Web API to use SimpleInjector
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

            // Register Web API routes
            WebApiConfig.Register(System.Web.Http.GlobalConfiguration.Configuration);
        }
    }
}
