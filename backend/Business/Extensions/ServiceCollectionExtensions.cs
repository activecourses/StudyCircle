using API;
using Business.Authorization;
using Business.Services;
using Database.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Business.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClubAuthorization(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ClubMember", policy =>
                    policy.Requirements.Add(new ClubAuthorizationRequirement("", ClubRole.Member)));
                options.AddPolicy("ClubModerator", policy =>
                    policy.Requirements.Add(new ClubAuthorizationRequirement("", ClubRole.Moderator)));
                options.AddPolicy("ClubOwner", policy =>
                    policy.Requirements.Add(new ClubAuthorizationRequirement("", ClubRole.Owner)));
            });

            services.AddScoped<IAuthorizationHandler, ClubAuthorizationHandler>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}