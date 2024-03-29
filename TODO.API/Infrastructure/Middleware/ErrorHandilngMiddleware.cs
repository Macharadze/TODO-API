using Serilog;

namespace TODO.API.Infrastructure.Middleware
{
    public class ErrorHandilngMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandilngMiddleware(RequestDelegate request)
        {
            _next = request;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unhandled exception occurred");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new
                {
                    Title = "Internal Server Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response); ;
            }
        }
    }
}
