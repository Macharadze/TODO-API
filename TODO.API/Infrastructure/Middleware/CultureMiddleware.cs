using System.Globalization;
using System.Runtime.InteropServices;

namespace TODO.API.Infrastructure.Middleware
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }


        public async Task Invoke(HttpContext context)
        {
            string cultureName = context.Request.Headers["Accept-Language"].ToString();
            CultureInfo culture = new CultureInfo("ka-GE");
            if (!string.IsNullOrEmpty(cultureName))
            {
                try
                {
                    culture = new CultureInfo(cultureName);
                }
                catch (Exception ex)
                {

                }
            }
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            await _next(context);
        }
    }
}
