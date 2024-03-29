using System.Security.Claims;
using CitiesManager.WebAPI.DTO;
using CitiesManager.WebAPI.Identity;
using CitiesManager.WebAPI.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class AccountController : CustomControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtService _jwtService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IJwtService jwtService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(
                    " | ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                );
                return Problem(errorMessage);
            }

            var user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.Email,
                PersonName = registerDTO.PersonName
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                var authResponse = _jwtService.CreateJwtToken(user);
                await GenerateRefreshToken(user, authResponse);

                return Ok(authResponse);
            }
            else
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
                return Problem(errorMessage);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> PostLogin(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(
                    " | ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                );
                return Problem(errorMessage);
            }
            var result = await _signInManager.PasswordSignInAsync(
                loginDTO.Email,
                loginDTO.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user is null)
                {
                    return NoContent();
                }
                var authResponse = _jwtService.CreateJwtToken(user);
                await GenerateRefreshToken(user, authResponse);
                return Ok(authResponse);
            }
            else
            {
                return Problem("Invalid email or password");
            }
        }

        [HttpGet("logout")]
        public async Task<IActionResult> GetLogout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> IsEmailNotRegistered(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        [HttpPost("generate-new-jwt-token")]
        public async Task<IActionResult> GenerateNewAccessToken(TokenModel tokenModel)
        {
            if (tokenModel == null)
            {
                return BadRequest("Invalid client request");
            }
            var jwtToken = tokenModel.Token;
            var refreshToken = tokenModel.RefreshToken;

            var principal = _jwtService.GetPrincipalFromJwtToken(jwtToken);
            if (principal == null)
            {
                return BadRequest("Invalid jwt access token");
            }

            var email = principal.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);
            if (
                user == null
                || user.RefreshToken != refreshToken
                || user.RefreshTokenExpirationDateTime <= DateTime.UtcNow
            )
            {
                return BadRequest("Invalid refresh token");
            }

            var authenticationResponse = _jwtService.CreateJwtToken(user);
            await GenerateRefreshToken(user, authenticationResponse);

            return Ok(authenticationResponse);
        }

        private async Task GenerateRefreshToken(
            ApplicationUser user,
            AuthenticationResponse authResponse
        )
        {
            user.RefreshToken = authResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authResponse.RefreshTokenExpirationDateTime;
            await _userManager.UpdateAsync(user);
        }
    }
}
