using DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
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

        public HealthCheckController(IApiExceptionService apiExceptionService, ILogger<HealthCheckController> logger)
        {
            _apiExceptionService = apiExceptionService;
            _logger = logger;
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
    }
}
