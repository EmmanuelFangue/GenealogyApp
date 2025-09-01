using System.Security.Claims;
using System.Text;
using GenealogyApp.Application.Interfaces;
using GenealogyApp.Infrastructure.Data;
using GenealogyApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using GenealogyApp.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<GenealogyDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("GenealogyDbContext")));

// Auth / JWT
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = !string.IsNullOrEmpty(jwtIssuer),
            ValidateAudience = !string.IsNullOrEmpty(jwtAudience),
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// IoC Application/Infrastructure
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFamilyService, FamilyService>();
builder.Services.AddScoped<ILinkService, LinkService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();

app.UseSwagger(); app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

// Map Minimal APIs
app.MapGroup("/auth").MapAuthEndpoints().WithTags("Auth");
app.MapGroup("/api").RequireAuthorization()
    .MapPeopleEndpoints()
    .MapRelationshipsEndpoints()
    .MapTreeEndpoints()
    .WithTags("Genealogy");

app.Run();
