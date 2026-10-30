using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Toarnbeike.AppLibrary.DependencyInjection;

/// <summary>
/// Extensions on the IServiceCollection that allow explicit replacement of all existing registrations of a service with the newly provided implementation.
/// Performs a RemoveAll followed by an Add with the specified lifetime.
/// </summary>
public static class ReplaceAllExtensions
{
    /// <param name="services">The service collection to modify</param>
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Replaces all existing registrations of <typeparamref name="TService"/> with a registration of <typeparamref name="TImplementation"/> in the provided <paramref name="lifetime"/>.
        /// </summary>
        /// <typeparam name="TService">The service type to replace</typeparam>
        /// <typeparam name="TImplementation">The implementation type to register</typeparam>
        /// <param name="lifetime">The lifetime of the new registration</param>
        /// <returns>The service collection for chaining</returns>
        public IServiceCollection ReplaceAll<TService, TImplementation>(ServiceLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            services.RemoveAll<TService>();
            services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime));
            return services;
        }

        /// <summary>
        /// Replaces all existing registrations of <typeparamref name="TService"/> with a singleton registration of <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">The service type to replace</typeparam>
        /// <typeparam name="TImplementation">The implementation type to register</typeparam>
        /// <returns>The service collection for chaining</returns>
        public IServiceCollection ReplaceAllSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService =>
            services.ReplaceAll<TService, TImplementation>(ServiceLifetime.Singleton);

        /// <summary>
        /// Replaces all existing registrations of <typeparamref name="TService"/> with a scoped registration of <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">The service type to replace</typeparam>
        /// <typeparam name="TImplementation">The implementation type to register</typeparam>
        /// <returns>The service collection for chaining</returns>
        public IServiceCollection ReplaceAllScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService =>
            services.ReplaceAll<TService, TImplementation>(ServiceLifetime.Scoped);

        /// <summary>
        /// Replaces all existing registrations of <typeparamref name="TService"/> with a transient registration of <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">The service type to replace</typeparam>
        /// <typeparam name="TImplementation">The implementation type to register</typeparam>
        /// <returns>The service collection for chaining</returns>
        public IServiceCollection ReplaceAllTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService =>
            services.ReplaceAll<TService, TImplementation>(ServiceLifetime.Transient);
    }
}
