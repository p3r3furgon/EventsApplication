using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notifications.Application.Notifications.Consumers;
using Notifications.Domain.Interfaces;
using Notifications.Persistance;
using Notifications.Persistance.Repositories;
using CommonFiles.Auth.Extensions;
using Microsoft.OpenApi.Models;
using Notifications.API.Middleware;
using CommonFiles.Messaging;
using Microsoft.Extensions.Options;
using CommonFiles.Auth.RequirementsHandlers;
using Microsoft.AspNetCore.Authorization;
using Notifications.Persistance.Repositories.UnitOfWork;
using CommonFiles.Interfaces;
using Notifications.Application.Notifications.UseCases.Commands.DeleteNotification;
using Notifications.Application.Notifications.UseCases.CommonMappers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<NotificationsDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(NotificationsDbContext)));
});

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<INotificationsRepository, NotificationsRepository>();

builder.Services.AddApiAuthentification(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<DeleteNotificationCommand>());
builder.Services.AddAutoMapper(typeof(NotificationsMapper));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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


builder.Services.AddMassTransit(m =>
{
    m.AddConsumer<EventsConsumer>();

    m.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqSettings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
        cfg.Host(new Uri(rabbitMqSettings.Host), h =>
        {
            h.Username(rabbitMqSettings.Username);
            h.Password(rabbitMqSettings.Password);
        });

        cfg.ReceiveEndpoint("events", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<EventsConsumer>(context);
        });
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userDbContext = services.GetRequiredService<NotificationsDbContext>();
        userDbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
