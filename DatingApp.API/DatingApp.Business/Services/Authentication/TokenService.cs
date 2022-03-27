using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingApp.Business.Services.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AuthenticationTokenKey"]));
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(5),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public int? GetCurrentUserId(ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal)); ;

            var loggedInUserId = principal.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType);

            if (loggedInUserId != null)
            {
                var userId = Convert.ToInt32(loggedInUserId.Value);

                return userId;
            }

            return null;
        }
    }
}
