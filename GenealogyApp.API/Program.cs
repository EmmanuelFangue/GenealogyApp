using GenealogyApp.Infrastructure.Services;
using GenealogyApp.Application.Interfaces;
using GenealogyApp.Application.Services;
using GenealogyApp.Infrastructure.Data;
using GenealogyApp.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using GenealogyApp.Application.Mappings;
using GenealogyApp.API.Middleware;

var builder = WebApplication.CreateBuilder(args);
// ClÃ© secrÃ©te (a stocker dans appsettings ou variables d'env en prod)
var secretKey = builder.Configuration["Jwt:Key"] ?? "supersecretkey";
var issuer = builder.Configuration["Jwt:Issuer"] ?? "GenealogyApp";
var audience = builder.Configuration["Jwt:Audience"] ?? "GenealogyAppClient";

// Connexion Ã  la base de donnÃ©es SQL Server

if (builder.Environment.EnvironmentName != "Testing")
{
    builder.Services.AddDbContext<GenealogyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GenealogyDbContext")));
}


// Authentification JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });



// CORS pour React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:5173") // ou le port de ton client React
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Injection des services mÃ©tiers
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFamilyService, FamilyService>();
builder.Services.AddScoped<ILinkService, LinkService>();
builder.Services.AddScoped<IAuditService, AuditService>();

// AutoMapper
builder.Services.AddAutoMapper(cfg => {cfg.AddProfile<MappingProfile>();});

// Swagger + sÃ©curitÃ©
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Entrez un token JWT valide",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();

builder.Services.AddScoped<IGenealogyService, GenealogyService>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

//
//app.UseMiddleware<JwtAuthMiddleware>();

app.MapControllers();

app.Run();

public partial class Program { }

