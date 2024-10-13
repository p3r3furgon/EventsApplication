using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notifications.Application.Notifications.Consumers;
using Notifications.Domain.Interfaces;
using Notifications.Infrastructure.Helper;
using Notifications.Persistance;
using Notifications.Persistance.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<NotificationsDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(NotificationsDbContext)));
});

builder.Services.AddScoped<INotificationsRepository, NotificationsRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddMassTransit(m =>
{
    m.AddConsumer<EventsConsumer>();
    m.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
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
            ep.ConfigureConsumer<EventsConsumer>(provider);
        });
    }));
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
