using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace PersonalFinance.Infrastructure.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly IOptions<JwtOptions> _options;

        public JwtTokenService(IOptions<JwtOptions> options)
        {
            _options = options;
        }
        public string CreateToken(string userId, string role, string name)
        {
            
            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Name, name)

            };

            var jwtKey = _options.Value.Key;
            var jwtIssuer = _options.Value.Issuer;
            var jwtAudience = _options.Value.Audience;
            var jwtExpirationMinutes = _options.Value.ExpirationMinutes;

            var key =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentionals = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expirationTime = DateTime.UtcNow.AddMinutes(jwtExpirationMinutes);

            var token = new JwtSecurityToken(jwtIssuer, jwtAudience, claims, null, expirationTime, credentionals);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
