using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CitiesManager.WebAPI.DTO;
using CitiesManager.WebAPI.Identity;
using CitiesManager.WebAPI.ServiceContracts;
using Microsoft.IdentityModel.Tokens;

namespace CitiesManager.WebAPI.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthenticationResponse CreateJwtToken(ApplicationUser user)
        {
            var expirationDate = DateTime
                .UtcNow
                .AddMinutes(Convert.ToInt32(_configuration["Jwt:EXPIRATION_MINUTES"]));
            var claims = new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject (user id)
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT unique Id
                new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), // Issued at (date and time of token generation)
                new(ClaimTypes.NameIdentifier, user.Id.ToString()), // Unique name identifier of the user (Email)
                new(ClaimTypes.Name, user.PersonName.ToString()), // Name of the user
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );
            var signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expirationDate,
                signingCredentials: signingCredentials
            );
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            return new AuthenticationResponse()
            {
                Token = token,
                Email = user.Email,
                PersonName = user.PersonName,
                Expiration = expirationDate,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpirationDateTime = DateTime
                    .UtcNow
                    .AddMilliseconds(
                        Convert.ToInt32(_configuration["RefreshToken:EXPIRATION_MINUTES"])
                    )
            };
        }

        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
