using DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Services.DTO;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [ApiController]
    [EnableCors("DefaultPolicy")]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly IApiExceptionService _apiExceptionService;
        private readonly ILogger<HealthCheckController> _logger;
        private readonly IEmailSender _emailSender;

        public HealthCheckController(IApiExceptionService apiExceptionService, ILogger<HealthCheckController> logger, IEmailSender emailSender)
        {
            _apiExceptionService = apiExceptionService;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Simple endpoint for testing CORS using http://test-cors.org
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public IActionResult Cors()
        {
            _logger.LogInformation("HealthCheckController.Cors called");
            return Ok("CORS");
        }

        /// <summary>
        /// Throw test exception for logging
        /// </summary>
        /// <param name="exception">Integer 1 to 3</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{exception}")]
        public IActionResult TryException(int exception)
        {
            var ex = (TestException)exception;
            _apiExceptionService.ThrowTestException(ex);
            _logger.LogInformation("HealthCheckController.TryException called, {e} thorwn!", ex.ToString());
            return Ok($"Exception {ex} thrown!");
        }

        /// <summary>
        /// Get All Logged Exceptions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Exceptions()
        {
            var exceptions = await _apiExceptionService.GetAll();
            _logger.LogInformation("HealthCheckController.Exceptions return {e} result", exceptions.Count);
            return Ok(exceptions);
        }

        /// <summary>
        /// Test Email Sender
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{email}")]
        public IActionResult SendTestEmail(string email)
        {
            _logger.LogInformation("Sending email to {email}..", email);
            _emailSender.Send(new EmailMessage()
            {
                To = email,
                Body = "Test Email",
                Subject = "Test Email"
            });
            _logger.LogInformation("Email sent");
            return Ok();
        }
    }
}
