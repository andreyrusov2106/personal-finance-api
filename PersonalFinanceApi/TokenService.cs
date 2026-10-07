using PersonalFinance.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalFinanceApi
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtExpirationMinutes;

        public TokenService(IConfiguration configuration)
        {
            _jwtSecret = configuration.GetValue<string>("Jwt:Key");
            _jwtIssuer = configuration.GetValue<string>("Jwt:Issuer");
            _jwtAudience = configuration.GetValue<string>("Jwt:Audience");
            _jwtExpirationMinutes = configuration.GetValue<int>("Jwt:ExpirationMinutes");
        }

        public string CreateToken(string userId, string role, string name)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Name, name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expirationTime = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes);

            var token = new JwtSecurityToken(_jwtIssuer, _jwtAudience, claims, null, expirationTime, credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
