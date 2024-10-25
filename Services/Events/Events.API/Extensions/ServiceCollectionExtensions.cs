using Events.Application.UseCases.Commands.CreateEvent;

namespace Events.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateEventCommandHandler>());
            return services;
        }
    }
}
