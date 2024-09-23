using ChatApp_Api.Data;
using ChatApp_Api.Entities;
using ChatApp_Api.Extensions;
using ChatApp_Api.Interfaces;
using ChatApp_Api.Middleware;
using ChatApp_Api.Services;
using ChatApp_Api.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatApp_Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
                 //.AddJsonOptions(options =>
                 //{
                 //    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                 //}); ;
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddApplicationServices(builder.Configuration);
         
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowCredentials()
                                .AllowAnyMethod();

                    });
            });
            builder.Services.AddSignalR();
            builder.Services.AddIdentityServices(builder.Configuration);

            var app = builder.Build();

            // Áp dụng các migration và seed dữ liệu khi ứng dụng khởi động.
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                    // Áp dụng migration (tạo database nếu chưa có và áp dụng thay đổi)
                    await context.Database.MigrateAsync();
                    // Gọi phương thức SeedUsers để seed dữ liệu người dùng
                    await Seed.SeedUsers(userManager,roleManager);
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi nếu xảy ra lỗi trong quá trình seed
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during migration or seeding.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCors("AllowSpecificOrigin");
            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            // Cấu hình endpoint cho Hub
            app.MapHub<PresenceHub>("hubs/presence");
            app.MapHub<MessageHub>("hubs/message");

            app.Run();
        }
    }
}