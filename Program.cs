using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieManagementSystem.DataContext;
using MovieManagementSystem.Payloads.Converters;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;
using MovieManagementSystem.Services;
using MovieManagementSystem.Services.Implements;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem
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
            //builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(
                builder.Configuration.GetConnectionString("MyDB")
            ));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    // tự cấp token
                    ValidateAudience = false,
                    ValidateIssuer = false,

                    // ký vào token
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:SecretKey").Value!))
                };
            });
            builder.Services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Auth", new OpenApiSecurityScheme
                {
                    Description = "Làm theo mẫu này. Ví dụ: Bearer {Token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
            });

            // Call methods from new files
            ServiceRegistrar.RegisterServices(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}