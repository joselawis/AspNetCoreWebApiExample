using CitiesManager.WebAPI.DTO;
using CitiesManager.WebAPI.Identity;
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

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
                return Ok(user);
            }
            else
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
                return Problem(errorMessage);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApplicationUser>> PostLogin(LoginDTO loginDTO)
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
                return Ok(new { personName = user.PersonName, email = user.Email });
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
    }
}
