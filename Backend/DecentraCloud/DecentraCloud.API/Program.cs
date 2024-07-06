using DecentraCloud.API.Config;
using DecentraCloud.API.Data;
using DecentraCloud.API.Extensions;
using DecentraCloud.API.Mappers;
using DecentraCloud.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDecentraCloudServices(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddEndpointsApiExplorer(); // Ensure this line is present
builder.Services.AddSwaggerGen(); // Ensure this line is present

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<NodeAuthenticationMiddleware>(builder.Configuration["Jwt:Key"]);

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
