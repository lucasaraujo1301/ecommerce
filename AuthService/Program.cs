using Microsoft.EntityFrameworkCore;

using ECommerce.AuthService.Infrastructure.Data;
using ECommerce.AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using ECommerce.AuthService.Domain.Interfaces;
using ECommerce.AuthService.Infrastructure.Repositories;
using ECommerce.AuthService.Application.UseCase;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ECommerce.AuthService.Infrastructure.Services;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ECommerceAuthServiceDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IUserUseCases, UserUseCases>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.Configure<RouteOptions>(options => {
    options.LowercaseUrls = true;
});

var jwtSettings = builder.Configuration.GetSection("Jwt")
    ?? throw new Exception("Missing Jwt configuration");

var jwtSettingsKey = jwtSettings["Key"]
    ?? throw new Exception("Missing JWT Key configuration");

var key = Encoding.UTF8.GetBytes(jwtSettingsKey);

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Make sure to include JWT authentication in the Swagger UI
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            c.RoutePrefix = "api/docs";  // This is optional, to serve Swagger at the root URL
        }
    );
}

app.UseAuthorization();
app.MapControllers();

app.Run();
