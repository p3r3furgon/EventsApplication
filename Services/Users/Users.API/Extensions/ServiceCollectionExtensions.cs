using Users.Application.UseCases.UserUseCases.Commands.DeleteUser;

namespace user.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<DeleteUserCommandHandler>());
            return services;
        }
    }
}
