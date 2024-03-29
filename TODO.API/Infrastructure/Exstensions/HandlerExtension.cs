using TODO.API.Infrastructure.Middleware;

namespace TODO.API.Infrastructure.Exstensions
{
    public static class HandlerExtension
    {
        public static IApplicationBuilder UseHandlerMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ErrorHandilngMiddleware>();
            return builder;
        }
    }
}
