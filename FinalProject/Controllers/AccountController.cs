using FinalProject.ApiModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly JwtOptions _jwtOptions;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<JwtOptions> jwtOptions,
            ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerDTO)
        {
            _logger.LogInformation("UserController.Register called with email = {email}", registerDTO.Email);
            var user = new IdentityUser { UserName = registerDTO.Email, Email = registerDTO.Email };
            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("UserController.Register succesfully added user with email = {email}", registerDTO.Email);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return Ok(code.ToString());
            }
            _logger.LogWarning("UserController.Register failed to register user with email = {email}", registerDTO.Email);
            return BadRequest("Failed to register user!");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailModel confirmEmailDTO)
        {
            _logger.LogInformation("UserController.ConfirmEmail failed to register user with email = {email}", confirmEmailDTO.Email);
            var user = await _userManager.FindByEmailAsync(confirmEmailDTO.Email);
            if (user == null)
            {
                _logger.LogWarning("UserController.ConfirmEmail Unable to load user with email = {email}", confirmEmailDTO.Email);
                return NotFound($"Unable to load user with Email '{confirmEmailDTO.Email}'.");
            }

            var code = confirmEmailDTO.Code;
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                _logger.LogInformation("UserController.ConfirmEmail succesfully registered user with email = {email}", confirmEmailDTO.Email);
                return Ok("Success!");
            }

            _logger.LogWarning("UserController.ConfirmEmail failed to confirm email, bad request, with email = {email}", confirmEmailDTO.Email);

            return BadRequest("Confirm Email Failed!");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginDTO)
        {
            _logger.LogInformation("UserController.Login called using email = {email}", loginDTO.Email);
            IActionResult actionResult;
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                _logger.LogWarning("UserController.Login use with email = {email} not found", loginDTO.Email);

                actionResult = NotFound(new { errors = new[] { $"User with email '{loginDTO.Email}' is not found" } });
            }
            else if (await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                if (!user.EmailConfirmed)
                {
                    _logger.LogWarning("UserController.Login Email = {email} is not confirmed!", loginDTO.Email);

                    actionResult = BadRequest(new { errors = new[] { "Email is not confirmed. Please, go to your email account" } });
                }
                else
                {
                    _logger.LogInformation("UserController.Login user with email = {email} found, generating JWT token.", loginDTO.Email);

                    var token = GenerateToken(user);

                    _logger.LogInformation("UserController.Login user with email = {email} token generated, token = {token}.", loginDTO.Email, token);
                    actionResult = Ok(token);
                }
            }
            else
            {
                _logger.LogWarning("UserController.Login user with email = {email} password not valid.", loginDTO.Email);
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
