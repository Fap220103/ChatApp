using ChatApp_Api.Data;
using ChatApp_Api.Helpers;
using ChatApp_Api.Interfaces;
using ChatApp_Api.Services;
using ChatApp_Api.SignalR;
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
            services.AddSingleton<PresenceTracker>();
            services.AddScoped<ITokenService, TokenService>();
         
            services.AddScoped<IPhotoService, PhotoService>();
       
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<LogUserActivity>();
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            return services;
        }
    }
}
