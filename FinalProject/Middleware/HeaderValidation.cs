using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FinalProject.Middleware
{
    /// <summary>
    /// Static class to register this middleware into the application.
    /// </summary>
    public static class HeaderValidationExtension
    {
        public static IApplicationBuilder ApplyHeaderValidation(this IApplicationBuilder app)
        {
            app.UseMiddleware<HeaderValidation>();
            return app;
        }
    }

    /// <summary>
    /// Middleware class to implement header validation.
    /// </summary>
    public class HeaderValidation
    {
        private readonly RequestDelegate _next;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next"><see cref="RequestDelegate"/> automatically passed by the pipeline.</param>
        public HeaderValidation(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Method that is used by the pipeline to execute this middleware.
        /// </summary>
        /// <param name="context">Context automatically injected by the request pipeline.</param>
        /// <returns><see cref="Task"/></returns>
        /// <remarks>Accept-Charset is forbidden to be modified, hence it is not included in the validation.</remarks>
        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.Keys.Contains("x-api-version"))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("x-api-version header is missing.");
                return;
            }

            if (!context.Request.Headers.Keys.Contains("x-api-key"))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("x-api-key header is missing.");
                return;
            }

            await _next.Invoke(context);
        }
    }
}
