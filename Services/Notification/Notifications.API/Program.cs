using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notifications.Application.Notifications.Consumers;
using Notifications.Application.Notifications.Services;
using Notifications.Domain.Interfaces;
using Notifications.Infrastructure.Helper;
using Notifications.Persistance;
using Notifications.Persistance.Repositories;
using CommonFiles.Auth.Extensions;
using Microsoft.OpenApi.Models;
using Notifications.API.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<NotificationsDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(NotificationsDbContext)));
});

builder.Services.AddScoped<INotificationsRepository, NotificationsRepository>();
builder.Services.AddScoped<INotificationService, NotificationsService>();

builder.Services.AddApiAuthentification(builder.Configuration);

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

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddMassTransit(m =>
{
    m.AddConsumer<EventsConsumer>();

    m.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
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
