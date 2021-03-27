using DomainModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinalProject.Middleware
{
    public static class ExceptionLoggerExtension
    {
        public static IApplicationBuilder ApplyExceptionLogger(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionLogger>();
            return app;
        }
    }

    public class ExceptionLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionLogger> _logger;

        public ExceptionLogger(RequestDelegate next, ILogger<ExceptionLogger> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var apiExceptionService = (IApiExceptionService)context.RequestServices.GetService(typeof(IApiExceptionService));
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                var type = e.GetType().ToString();

                ApiException apiException = new ApiException()
                {
                    Namespace = e.TargetSite.DeclaringType.Namespace,
                    Classname = e.TargetSite.DeclaringType.Name,
                    Method = e.TargetSite.Name,
                    Type = type,
                    Message = e.Message,
                    StackTrace = e.StackTrace,
                    CreatedOn = DateTime.UtcNow
                };

                await apiExceptionService.Create(apiException);
            }
        }
    }
}
