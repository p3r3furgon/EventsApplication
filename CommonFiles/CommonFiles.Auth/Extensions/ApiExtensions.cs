using CommonFiles.Auth.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CommonFiles.Auth.Extensions
{
    public static class ApiExtensions
    {
        public static void AddApiAuthentification(this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                    };
                    options.Events = new JwtBearerEvents
                    { 
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Headers["Authorization"];
                            return Task.CompletedTask;
                        }
                    };

                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.AddRequirements(new RoleRequirement("Admin")));
                options.AddPolicy("SuperAdmin", policy => policy.AddRequirements(new RoleRequirement("SuperAdmin")));
            });
        }
    }
}
