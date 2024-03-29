using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TODO.API.Infrastructure.Exstensions
{
    public static class HealthCheckExtension
    {
        public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
        .AddSqlServer(configuration["ConnectionStrings:DefaultConnection"], healthQuery: "select 1", name: "TODO-API", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Feedback", "Database" });

            //services.AddHealthChecksUI();
           /* services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(10); //time in seconds between check    
                opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks    
                opt.SetApiMaxActiveRequests(1); //api requests concurrency    
                opt.AddHealthCheckEndpoint("feedback api", "/api/health"); //map health check api    

            });*/
        }
    }
}
