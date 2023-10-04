using Application.Repository.Context;
using Application.Repository.Core;
using Application.Repository.DTO.Common;
using Application.Repository.Interfaces;
using Application.Repository.Repositories;
using Application.Service.Interfaces;
using Application.Service.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Application.API.Extensions
{
    [Obsolete]
    public static class ServiceExtension
    {
        public static void ConfigureScope(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUserManagementRepository, UserManagementRepository>();
            services.AddScoped<IUserManagementService, UserManagementService>();

            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAdminService, AdminService>();

            services.AddScoped<IRegistrationRepository, RegistrationRepository>();
            services.AddScoped<IRegistrationService, RegistrationService>();
        }

        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();

            var _jwtsetting = configuration.GetSection("JWTSetting");
            services.Configure<JWTSetting>(_jwtsetting);
            var authkey = configuration.GetValue<string>("JWTSetting:SecurityKey");

            services.AddDbContext<ApplicationContext>(option => option
               .UseSqlServer(configuration.GetConnectionString("DataBaseConnectionString")));

            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            services.AddLogging(build =>
            {
                build.AddConsole();
            });

            services.AddFluentValidation(x =>
            {
                x.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });

            services.AddAuthentication(item =>
            {
                item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(item =>
            {
                item.RequireHttpsMetadata = true;
                item.SaveToken = true;
                item.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authkey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });


            services.AddSwaggerGen(c => c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Enter Your Token Here",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            }));

            services.AddSwaggerGen(w => w.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                {
                   new OpenApiSecurityScheme
                   {
                      Reference = new OpenApiReference
                      {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                      }
                   },
                   new string[] {}
                }
             }));

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }
}
