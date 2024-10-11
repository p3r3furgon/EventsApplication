using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Users.API.Authorization.Requirements;
using Users.Infrastructure;

namespace Users.API.Extensions
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
                options.AddPolicy("User", policy => policy.AddRequirements(new RoleRequirement("User")));
            });
        }
    }
}
