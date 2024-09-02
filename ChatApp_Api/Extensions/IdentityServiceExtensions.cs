using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatApp_Api.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("TokenBear:SignatureKey").Value)),
                         ValidateIssuer = false,
                         ValidateAudience = false
                     };
                 });
            return services;
        }
    }
}
