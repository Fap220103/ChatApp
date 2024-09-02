using ChatApp_Api.Data;
using ChatApp_Api.Extensions;
using ChatApp_Api.Interfaces;
using ChatApp_Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatApp_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddApplicationServices(builder.Configuration);
         
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();

                    });
            });
            builder.Services.AddIdentityServices(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowSpecificOrigin");
            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}