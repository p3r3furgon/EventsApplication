using Microsoft.EntityFrameworkCore;
using Users.Application.Services;
using Users.Domain.Interfaces.Authentification;
using Users.Domain.Interfaces.Repositories;
using Users.Domain.Interfaces.Services;
using Users.Infrastructure;
using Users.Persistance;
using Users.Persistance.Repositories;
using Users.Infrastructure.Helper;
using Users.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Users.API.Authorization.RequirementsHandlers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
 

builder.Services.AddDbContext<UsersDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(UsersDbContext)));
});

builder.Services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IRefreshTokensRepository, RefreshTokenRepository>();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddApiAuthentification(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter foramt: Bearer ACCESS_TOKEN",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
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

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
