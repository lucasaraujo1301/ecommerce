using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerce.AuthService.Domain.Entities;
using ECommerce.AuthService.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.AuthService.Infrastructure.Services
{
    public class JwtService(IConfiguration configuration) : IJwtService
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>([
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.GetFullName()),
                new Claim(ClaimTypes.Email, user.Email)
            ]);

            var jwtConfig = _configuration.GetSection("Jwt");
            string? key = jwtConfig["Key"];

            if (jwtConfig == null || string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("JWT configuration is missing.");
            }

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(key));
            SigningCredentials creds = new(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new(
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}