using Hangfire;
using Hangfire.SqlServer;
using SimpleInjector;

namespace HRMS.Functions
{
    public static class HangfireBootstrapper
    {
        public static void Start(Container container)
        {
            // Optionally, configure Hangfire logging with NLog
            Hangfire.GlobalConfiguration.Configuration.UseNLogLogProvider();

            // Configure Hangfire to use SQL Server storage
            Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage("data source=PC-AZLAN\\SQLEXPRESS;initial catalog=11th;integrated security=True;trustservercertificate=True;", new SqlServerStorageOptions
            {
                PrepareSchemaIfNecessary = true
            });

            // Start Hangfire server
            using (var server = new BackgroundJobServer())
            {
                // No need to use app.UseHangfireServer() here since we are using BackgroundJobServer directly
            }
        }
    }
}
