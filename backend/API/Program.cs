using API;
using API.Filters;
using Business;
using Business.Authorization;
using Business.Services;
using Database.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration);  // Reads the Serilog config from appsettings.json
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddBusinessService();



// At the top where your services are registered, add:
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthorizationHandler, ClubAuthorizationHandler>();

// Replace your current JWT authentication setup with:
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtOptions>(jwtSection);
var jwtOptions = jwtSection.Get<JwtOptions>();

if (jwtOptions == null)
{
    throw new InvalidOperationException("JWT configuration is missing in appsettings.json");
}

builder.Services.AddSingleton(jwtOptions);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
    };
});

// Add authorization after authentication
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ClubMember", policy =>
        policy.Requirements.Add(new ClubAuthorizationRequirement("", ClubRole.Member)));
    options.AddPolicy("ClubModerator", policy =>
        policy.Requirements.Add(new ClubAuthorizationRequirement("", ClubRole.Moderator)));
    options.AddPolicy("ClubOwner", policy =>
        policy.Requirements.Add(new ClubAuthorizationRequirement("", ClubRole.Owner)));
});

// In the middleware section (after app.UseHttpsRedirection()):

//
builder.Services.AddScoped<ClubMemberFilter>();
builder.Services.AddScoped<ClubOwnerFilter>();
builder.Services.AddScoped<ClubModeratorFilter>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors(static builder =>
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

app.MapControllers();

app.Run();
