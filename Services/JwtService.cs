using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
                .AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));
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
                Expiration = expirationDate
            };
        }
    }
}
