using Serilog;

namespace TODO.API.Infrastructure.Middleware
{
    public class RequestResponseMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestResponseMiddleware(RequestDelegate request)
        {
            _next = request;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await LogRequestAndResponse(context.Request, context.Response);
                await _next(context);
            }
            catch (Exception ex)
            {
                await LogError(context, ex);
            }
        }

        private async Task LogRequestAndResponse(HttpRequest request, HttpResponse response)
        {
            var toLog = $"{Environment.NewLine} Request Info {Environment.NewLine}" +
            $"IP = {request.HttpContext.Connection.RemoteIpAddress}{Environment.NewLine}" +
            $"Address = {request.Scheme}{Environment.NewLine}" +
            $"IsSescured = {request.IsHttps}{Environment.NewLine}" +
            $"Body = {request.Body}{Environment.NewLine}" +
            $"QueryString = {request.QueryString}{Environment.NewLine}" +
            $"Time = {DateTime.Now}{Environment.NewLine}" +
            $"{Environment.NewLine} Response Info {Environment.NewLine}" +
            $"StatusCode = {response.StatusCode}{Environment.NewLine}" +
            $"Body = {response.Body}{Environment.NewLine}";



            Log.Information(toLog);
            await Task.CompletedTask;
        }
        private  async Task LogError(HttpContext context,Exception ex)
        {
            var toLog = $"{Environment.NewLine} Request Info {Environment.NewLine}" +
                $"TracId {context.TraceIdentifier}" +
                $"Exception {ex.Message}" +
                $"instance {context.Request.Path}";

            Log.Error(toLog);
            await Task.CompletedTask;
        }
      
    }
}
