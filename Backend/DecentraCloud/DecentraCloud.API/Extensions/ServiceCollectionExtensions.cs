using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Repositories;
using DecentraCloud.API.Services;

namespace DecentraCloud.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDecentraCloudServices(this IServiceCollection services)
        {
            // Add all the services and repositories to the dependency injection container
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        // Additional extension methods can be added here
    }
}
