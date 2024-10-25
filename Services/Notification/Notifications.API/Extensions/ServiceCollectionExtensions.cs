using Notifications.Application.Notifications.UseCases.Commands.DeleteNotification;

namespace Notifications.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<DeleteNotificationCommand>());
            return services;
        }
    }
}
