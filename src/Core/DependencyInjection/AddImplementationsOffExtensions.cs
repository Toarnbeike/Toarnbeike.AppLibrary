using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Toarnbeike.AppLibrary.DependencyInjection;

/// <summary>
/// Extensions on the <see cref="IServiceCollection"/> interface to auto register services from assemblies.
/// </summary>
/// <remarks>
/// Each matched interface is registered separately, so singleton instances are not shared across service types.
/// Open generic implementations are registered only when their type parameters map directly and in order to the service interface.
/// </remarks>
public static class AddImplementationsOffExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Register all concrete implementations of <typeparamref name="TService"/> interface from the specified assembly with the specified lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service interface</typeparam>
        /// <param name="assembly">The assembly to scan for implementations</param>
        /// <param name="lifetime">The required lifetime of the service when registered</param>
        /// <param name="allowDerived">Also register implemented interfaces derived from the requested interface.</param>
        /// <returns>The service collection for chaining</returns>
        public IServiceCollection AddImplementationsOf<TService>(Assembly assembly,
            ServiceLifetime lifetime,
            bool allowDerived = false)
            where TService : class =>
            services.AddImplementationsOf(typeof(TService), assembly, lifetime, allowDerived);

        /// <summary>
        /// Register all concrete implementations of <typeparamref name="TService"/> interface from the specified assemblies with the specified lifetime.
        /// </summary>
        /// <typeparam name="TService">The type of the service interface</typeparam>
        /// <param name="assemblies">The assemblies to scan for implementations</param>
        /// <param name="lifetime">The required lifetime of the service when registered</param>
        /// <param name="allowDerived">Also register implemented interfaces derived from the requested interface.</param>
        /// <returns>The service collection for chaining</returns>
        public IServiceCollection AddImplementationsOf<TService>(IEnumerable<Assembly> assemblies,
            ServiceLifetime lifetime,
            bool allowDerived = false)
            where TService : class =>
            services.AddImplementationsOf(typeof(TService), assemblies, lifetime, allowDerived);

        /// <summary>
        /// Register all implementations of <paramref name="serviceType"/> interface from the specified assemblies with the specified lifetime.
        /// </summary>
        /// <param name="serviceType">The service interface type, optionally an open generic interface definition.</param>
        /// <param name="assemblies">The assemblies to scan for implementations</param>
        /// <param name="lifetime">The required lifetime of the service when registered</param>
        /// <param name="allowDerived">Also register implemented interfaces derived from the requested interface.</param>
        /// <returns>The service collection for chaining</returns>
        public IServiceCollection AddImplementationsOf(Type serviceType,
            IEnumerable<Assembly> assemblies,
            ServiceLifetime lifetime,
            bool allowDerived = false)
        {
            ValidateArguments(services, serviceType, lifetime);
            ArgumentNullException.ThrowIfNull(assemblies);
            foreach (var assembly in assemblies)
            {
                services.AddImplementationsOf(serviceType, assembly, lifetime, allowDerived);
            }

            return services;
        }

        /// <summary>
        /// Register all implementations of <paramref name="serviceType"/> interface from the specified assembly with the specified lifetime.
        /// </summary>
        /// <param name="serviceType">The service interface type</param>
        /// <param name="assembly">The assembly to scan for implementations</param>
        /// <param name="lifetime">The required lifetime of the service when registered</param>
        /// <param name="allowDerived">Also register implemented interfaces derived from the requested interface.</param>
        /// <returns>The service collection for chaining</returns>
        public IServiceCollection AddImplementationsOf(Type serviceType,
            Assembly assembly,
            ServiceLifetime lifetime,
            bool allowDerived = false)
        {
            ValidateArguments(services, serviceType, lifetime);
            ArgumentNullException.ThrowIfNull(assembly);

            foreach (var implementationType in assembly.GetTypes())
            {
                if (!implementationType.IsClass || implementationType.IsAbstract)
                {
                    continue;
                }

                foreach (var implementedInterface in implementationType.GetInterfaces())
                {
                    services.AddMatchingImplementation(serviceType, lifetime, allowDerived, implementedInterface, implementationType);
                }
            }

            return services;
        }

        private void AddMatchingImplementation(Type serviceType, ServiceLifetime lifetime, bool allowDerived,
            Type implementedInterface, Type implementationType)
        {
            if (!MatchesServiceType(serviceType, implementedInterface) &&
                (!allowDerived || !implementedInterface.GetInterfaces().Any(baseInterface => MatchesServiceType(serviceType, baseInterface))))
            {
                return;
            }

            if (!implementationType.ContainsGenericParameters)
            {
                services.Add(new ServiceDescriptor(implementedInterface, implementationType, lifetime));
                return;
            }

            if (!implementationType.IsGenericTypeDefinition || !implementedInterface.IsGenericType ||
                !implementedInterface.GetGenericArguments().SequenceEqual(implementationType.GetGenericArguments()))
            {
                return;
            }

            services.Add(new ServiceDescriptor(implementedInterface.GetGenericTypeDefinition(), implementationType, lifetime));
        }
    }

    private static bool MatchesServiceType(Type serviceType, Type implementedInterface) =>
        serviceType.IsGenericTypeDefinition
            ? implementedInterface.IsGenericType && implementedInterface.GetGenericTypeDefinition() == serviceType
            : implementedInterface == serviceType;

    private static void ValidateArguments(IServiceCollection services, Type serviceType, ServiceLifetime lifetime)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceType);
        if (!serviceType.IsInterface || (serviceType.ContainsGenericParameters && !serviceType.IsGenericTypeDefinition))
        {
            throw new ArgumentException("The service type must be a closed interface or an open generic interface definition.", nameof(serviceType));
        }

        if (!Enum.IsDefined(lifetime))
        {
            throw new ArgumentOutOfRangeException(nameof(lifetime));
        }
    }
}
