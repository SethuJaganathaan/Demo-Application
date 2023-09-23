using Application.Repository.Context;
using Application.Repository.Core;
using Application.Repository.Interfaces;
using Application.Repository.Repositories;
using Application.Service.Interfaces;
using Application.Service.Models;
using Application.Service.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Application.API.Extensions
{
    [Obsolete]
    public static class ServiceExtension
    {
        public static void ConfigureScope(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton(configuration);

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

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        Log.Error($"Something went wrong {contextFeature.Error}");

                        await context.Response.WriteAsync(new Error
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal server error"
                        }.ToString());
                    }
                });
            });
        }
    }
}
