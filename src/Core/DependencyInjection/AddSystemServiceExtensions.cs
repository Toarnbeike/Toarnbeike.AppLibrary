using Microsoft.Extensions.DependencyInjection;
using Toarnbeike.AppLibrary.Services;

namespace Toarnbeike.AppLibrary.DependencyInjection;

/// <summary>
/// Extension method to register the default system service providers with DI.
/// </summary>
public static class AddSystemServiceExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers implementations for date and time and current user providers based on the system environment and clock.
        /// </summary>
        public IServiceCollection AddSystemServices()
        {
            services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
            services.AddSingleton<ICurrentUserProvider, SystemUserProvider>();
            return services;
        }
    }
}
