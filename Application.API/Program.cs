using Application.API.Extensions;
using Application.Repository.DTO.Common;
using Application.Repository.Enums;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json")
.Build();

builder.Services.ConfigureScope(configuration);
builder.Services.ConfigureServices(configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(CommonConstant.Policies.UserPolicy, policy => policy.RequireClaim(ClaimTypes.Role, CommonConstant.Role.User));
    options.AddPolicy(CommonConstant.Policies.AdminPolicy, policy => policy.RequireClaim(ClaimTypes.Role, CommonConstant.Role.Admin));
    options.AddPolicy(CommonConstant.Policies.UserAndAdminPolicy, policy =>
    {
        policy.RequireRole(CommonConstant.Role.Admin, CommonConstant.Role.User);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options.WithOrigins("http://localhost:3000/").
    AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
