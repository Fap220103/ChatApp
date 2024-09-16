using ChatApp_Api.Entities;
using ChatApp_Api.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApp_Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateToken(AppUser user)
        {
            string signatureKey = _configuration["TokenBear:SignatureKey"];
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signatureKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(3),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDecriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
