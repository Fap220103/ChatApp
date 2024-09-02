using ChatApp_Api.Data;
using ChatApp_Api.Interfaces;
using ChatApp_Api.Services;
using Microsoft.EntityFrameworkCore;

namespace ChatApp_Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
