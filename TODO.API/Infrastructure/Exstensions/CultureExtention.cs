using TODO.API.Infrastructure.Middleware;

namespace TODO.API.Infrastructure.Exstensions
{
    public static class CultureExtention
    {
        public static IApplicationBuilder UseCultureMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CultureMiddleware>();
            return app;
        }
    }
}
