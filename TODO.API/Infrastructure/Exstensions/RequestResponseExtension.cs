using TODO.API.Infrastructure.Middleware;

namespace TODO.API.Infrastructure.Exstensions
{
    public static class RequestResponseExtension
    {
        public static IApplicationBuilder UseRequestResponseMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestResponseMiddleware>();
            return app;
        }
    }
}
