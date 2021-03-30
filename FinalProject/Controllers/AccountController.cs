using FinalProject.ApiModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace FinalProject.Controllers
{
    [ApiController]
    [EnableCors("DefaultPolicy")]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly JwtOptions _jwtOptions;
        private readonly SmtpConfig _smtpConfig;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<JwtOptions> jwtOptions,
            ILogger<AccountController> logger,
            IEmailSender emailSender,
            IOptions<SmtpConfig> options)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _jwtOptions = jwtOptions.Value;
            _smtpConfig = options.Value;
        }

        /// <summary>
        /// Endpoint for New User Registration
        /// </summary>
        /// <param name="registerDTO">Provide New User's email address, Password and Password Confirmation</param>
        /// <returns>
        /// The Generated Email Confirmation Token that will be used in /account/confirmEmail
        /// </returns>
        /// <response code="200">Return the generated email confirmation token to be used in /account/confirmEmail </response>
        /// <response code="400">When request is invalid due to invalid email address, email address already taken, password not match or password requirements not met,</response>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerDTO)
        {
            _logger.LogInformation("AccountController.Register called with email = {email}", registerDTO.Email);
            var user = new IdentityUser { UserName = registerDTO.Email, Email = registerDTO.Email };
            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("AccountController.Register succesfully added user with email = {email}", registerDTO.Email);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                SendEmailConfirmationCode(registerDTO.Email, code);
                return Ok(code.ToString());
            }
            _logger.LogWarning("AccountController.Register failed to register user with email = {email}", registerDTO.Email);
            return BadRequest(result.Errors);
        }

        private void SendEmailConfirmationCode(string email, string code)
        {
            var encoded = code.Replace('+', '-').Replace('/', '_');
            var message = new EmailMessage()
            {
                To = email,
                Subject = "Confirm Email",
                Body = $"<a href='{_smtpConfig.ConfirmEmailBaseUrl}account/confirmEmail/{email}/{encoded}'>Click here to confirm email.</a>"
            };
            _emailSender.Send(message);
        }

        /*[HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailModel confirmEmailDTO)
        {
            _logger.LogInformation("AccountController.ConfirmEmail failed to register user with email = {email}", confirmEmailDTO.Email);
            var user = await _userManager.FindByEmailAsync(confirmEmailDTO.Email);
            if (user == null)
            {
                _logger.LogWarning("AccountController.ConfirmEmail Unable to load user with email = {email}", confirmEmailDTO.Email);
                return NotFound($"Unable to load user with Email '{confirmEmailDTO.Email}'.");
            }

            var code = confirmEmailDTO.Code;
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                _logger.LogInformation("AccountController.ConfirmEmail succesfully registered user with email = {email}", confirmEmailDTO.Email);
                return Ok("Success!");
            }

            _logger.LogWarning("AccountController.ConfirmEmail failed to confirm email, bad request, with email = {email}", confirmEmailDTO.Email);

            return BadRequest("Confirm Email Failed!");
        }*/

        /// <summary>
        /// For Confirming User's email address after registration
        /// </summary>
        /// <param name="confirmEmailDTO">Provide the User's email address and Returns Confirmation Token</param>
        /// <returns></returns>
        /// <response code="200">The confirmation is successful</response>
        /// <response code="400">Invalid confirmation token provided</response>
        /// <response code="404">Unable to load user with the provided email address</response>
        [HttpGet]
        [Route("[action]/{email}/{code}")]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            var urlDecodedCode = code.Replace('_', '/').Replace('-', '+');
            _logger.LogInformation("AccountController.ConfirmEmail failed to register user with email = {email}", email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("AccountController.ConfirmEmail Unable to load user with email = {email}", email);
                return NotFound($"Unable to load user with Email '{email}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, urlDecodedCode);
            if (result.Succeeded)
            {
                _logger.LogInformation("AccountController.ConfirmEmail succesfully registered user with email = {email}", email);
                return Ok("Success!");
            }

            _logger.LogWarning("AccountController.ConfirmEmail failed to confirm email, bad request, with email = {email}", email);

            return BadRequest("Confirm Email Failed!");
        }

        /// <summary>
        /// Use this endpoint to generate JWT token
        /// </summary>
        /// <param name="loginDTO">Provide valid email and password</param>
        /// <returns></returns>
        /// <response code="200">Request is succesfull, returns generated JWT Token</response>
        /// <response code="400">Request failed due to invalid credentials or unconfirmed email address provided.</response>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Token([FromBody] LoginModel loginDTO)
        {
            _logger.LogInformation("AccountController.Login called using email = {email}", loginDTO.Email);
            IActionResult actionResult;
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                _logger.LogWarning("AccountController.Login use with email = {email} not found", loginDTO.Email);

                actionResult = NotFound(new { errors = new[] { $"User with email '{loginDTO.Email}' is not found" } });
            }
            else if (await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                if (!user.EmailConfirmed)
                {
                    _logger.LogWarning("AccountController.Login Email = {email} is not confirmed!", loginDTO.Email);

                    actionResult = BadRequest(new { errors = new[] { "Email is not confirmed. Please, go to your email account" } });
                }
                else
                {
                    _logger.LogInformation("AccountController.Login user with email = {email} found, generating JWT token.", loginDTO.Email);

                    var token = GenerateToken(user);

                    _logger.LogInformation("AccountController.Login user with email = {email} token generated, token = {token}.", loginDTO.Email, token);
                    actionResult = Ok(token);
                }
            }
            else
            {
                _logger.LogWarning("AccountController.Login user with email = {email} password not valid.", loginDTO.Email);
                actionResult = BadRequest(new { errors = new[] { "User password is not valid" } });
            }

            return actionResult;
        }

        private string GenerateToken(IdentityUser user)
        {
            IList<Claim> userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
              claims: userClaims,
              expires: DateTime.UtcNow.AddMonths(1),
              signingCredentials: new SigningCredentials(_jwtOptions.SecurityKey, SecurityAlgorithms.HmacSha256)
            ));
        }
    }
}
