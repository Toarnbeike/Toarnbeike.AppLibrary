#pragma warning disable S2436
using Microsoft.Extensions.DependencyInjection;
using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.AppLibrary.Entities.Audits;
using Toarnbeike.AppLibrary.Mutations;
using Toarnbeike.AppLibrary.Repositories;
using Toarnbeike.AppLibrary.Snapshots;
using Toarnbeike.AppLibrary.Snapshots.Audits;
using Toarnbeike.AppLibrary.Snapshots.Implementation;
using Toarnbeike.AppLibrary.Snapshots.Implementation.Validations;
using Toarnbeike.AppLibrary.State;

namespace Toarnbeike.AppLibrary.DependencyInjection;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Add state management services to the dependency injection container, including the aggregate store, mutation store, and snapshot registry.
        /// </summary>
        /// <typeparam name="TSnapshotStore">The specific implementation of <see cref="ISnapshotStore"/> to use. Will depend on the persistence mechanism being used.</typeparam>
        /// <returns>The current service collection.</returns>
        public IServiceCollection AddStateManagement<TSnapshotStore>()
            where TSnapshotStore : class, ISnapshotStore
        {
            services.AddSingleton<IApplicationStateManager, ApplicationStateManager>();
            services.AddSingleton<IAggregateStore, InMemoryAggregateStore>();
            services.AddSingleton<IMutationStore, InMemoryMutationStore>();
            services.AddSingleton<ISnapshotStore, TSnapshotStore>();
            services.AddNonAggregateMapper<AuditInfo, AuditInfoSnapshot, AuditInfoSnapshotMapperV1>();
            services.AddNonAggregateMapper<SoftDeleteInfo, SoftDeleteInfoSnapshot, SoftDeleteInfoSnapshotMapperV1>();
            services.AddSnapshotRegistry();
            return services;
        }

        /// <summary>
        /// Registers <see cref="IRepository{TAggregate, TId}"/> with its default <see cref="Repository{TAggregate, TId}"/> implementation.
        /// </summary>
        /// <typeparam name="TAggregate">The aggregate root type handled by the repository.</typeparam>
        /// <typeparam name="TId">The identifier type of the aggregate root.</typeparam>
        /// <returns>The current service collection.</returns>
        public IServiceCollection AddRepository<TAggregate, TId>()
            where TAggregate : IAggregate<TId>
            where TId : IEntityId
        {
            services.AddScoped<IRepository<TAggregate, TId>, Repository<TAggregate, TId>>();
            return services;
        }

        /// <summary>
        /// Registers a specific interface <typeparamref name="TRepositoryInterface"/> as repository for the <typeparamref name="TAggregate"/>.
        /// Also registers <see cref="IRepository{TAggregate, TId}"/> with the provided specific implemenation <typeparamref name="TRepositoryImplementation"/> for the aggregate, 
        /// allowing for requesting the default interface where no specific interface methods are required.
        /// </summary>
        /// <typeparam name="TAggregate">The aggregate root type handled by the repository.</typeparam>
        /// <typeparam name="TId">The identifier type of the aggregate root.</typeparam>
        /// <typeparam name="TRepositoryInterface">The specific repository interface type.</typeparam>
        /// <typeparam name="TRepositoryImplementation">The specific repository implementation type.</typeparam>
        /// <returns>The current service collection.</returns>
        public IServiceCollection AddRepository<TAggregate, TId, TRepositoryInterface, TRepositoryImplementation>()
            where TAggregate : IAggregate<TId>
            where TId : IEntityId
            where TRepositoryInterface : class, IRepository<TAggregate, TId>
            where TRepositoryImplementation : class, TRepositoryInterface
        {
            services.AddScoped<TRepositoryInterface, TRepositoryImplementation>();
            services.AddScoped<IRepository<TAggregate, TId>>(sp => sp.GetRequiredService<TRepositoryInterface>());
            return services;
        }

        /// <summary>
        /// Registers snapshot services for an aggregate, including the snapshot mapper and snapshot definition.
        /// </summary>
        /// <typeparam name="TAggregate">The aggregate type associated with the snapshot.</typeparam>
        /// <typeparam name="TSnapshot">The snapshot type used to persist aggregate state.</typeparam>
        /// <typeparam name="TMapper">The mapper type that converts between the aggregate and its snapshot.</typeparam>
        /// <param name="resourceName">The resource name used by the snapshot definition.</param>
        /// <returns>The current service collection.</returns>
        public IServiceCollection AddSnapshot<TAggregate, TSnapshot, TMapper>(string resourceName)
            where TAggregate : IAggregate
            where TSnapshot : ISnapshot
            where TMapper : class, ISnapshotMapper<TAggregate, TSnapshot>
        {
            services.AddSingleton<ISnapshotMapper<TAggregate, TSnapshot>, TMapper>();
            services.AddSingleton<ISnapshotDefinition>(sp =>
            {
                var mapper = sp.GetRequiredService<ISnapshotMapper<TAggregate, TSnapshot>>();
                return new SnapshotDefinition<TAggregate, TSnapshot>(resourceName, mapper);
            });
            return services;
        }

        /// <summary>
        /// Registers a snapshot migration for converting from one snapshot type to another.
        /// </summary>
        /// <typeparam name="TSourceSnapshot">The source snapshot type.</typeparam>
        /// <typeparam name="TTargetSnapshot">The target snapshot type.</typeparam>
        /// <typeparam name="TMigrator">The migration implementation that converts from the source snapshot type to the target snapshot type.</typeparam>
        /// <returns>The current service collection.</returns>
        public IServiceCollection AddSnapshotMigration<TSourceSnapshot, TTargetSnapshot, TMigrator>()
            where TSourceSnapshot : ISnapshot
            where TTargetSnapshot : ISnapshot
            where TMigrator : class, ISnapshotMigration<TSourceSnapshot, TTargetSnapshot>
        {
            services.AddSingleton<ISnapshotMigration<TSourceSnapshot, TTargetSnapshot>, TMigrator>();
            services.AddSingleton<ISnapshotMigrator>(sp =>
            {
                var migrator = sp.GetRequiredService<ISnapshotMigration<TSourceSnapshot, TTargetSnapshot>>();
                return new SnapshotMigrator<TSourceSnapshot, TTargetSnapshot>(migrator);
            });
            return services;
        }

        /// <summary>
        /// Registers a singleton snapshot mapper for a non-aggregate domain object.
        /// Use this method to register snapshot mappers for domain objects that are not aggregates, such as value objects or other entities.
        /// For aggregates, use the <see cref="AddSnapshot{TAggregate, TSnapshot, TMapper}(IServiceCollection, string)"/> method instead, which also adds internal helpers for saving and loading all aggregate snapshots from the data store.
        /// </summary>
        /// <typeparam name="TDomainObject">The non-aggregate domain object type to map.</typeparam>
        /// <typeparam name="TSnapshot">The snapshot type associated with the domain object.</typeparam>
        /// <typeparam name="TMapper">The mapper implementation type that maps between the domain object and snapshot.</typeparam>
        /// <returns>The same service collection instance for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when <typeparamref name="TDomainObject"/> is an aggregate type.</exception>
        public IServiceCollection AddNonAggregateMapper<TDomainObject, TSnapshot, TMapper>()
            where TSnapshot : ISnapshot
            where TMapper : class, ISnapshotMapper<TDomainObject, TSnapshot>
        {
            if (typeof(TDomainObject).IsAssignableTo(typeof(IAggregate)))
            {
                throw new InvalidOperationException($"The type {typeof(TDomainObject).Name} is an aggregate and cannot be registered as a non-aggregate snapshot mapper.");
            }
            services.AddSingleton<ISnapshotMapper<TDomainObject, TSnapshot>, TMapper>();
            return services;
        }

        /// <summary>
        /// Registers the snapshot registry and all snapshot definitions and migrators in the dependency injection container.
        /// </summary>
        /// <remarks>All snapshot definitions and migrators must be registered in the dependency injection container before calling this method.</remarks>
        private void AddSnapshotRegistry()
        {
            services.AddSingleton<ISnapshotRegistryValidator, SnapshotDefinitionValidator>();
            services.AddSingleton<ISnapshotRegistryValidator, SnapshotMigratorValidator>();
            services.AddSingleton<ISnapshotRegistryValidator, SnapshotMigratorChainValidator>();
            services.AddSingleton(serviceProvider =>
            {
                var validators = serviceProvider.GetServices<ISnapshotRegistryValidator>();
                var definitions = serviceProvider.GetServices<ISnapshotDefinition>();
                var migrators = serviceProvider.GetServices<ISnapshotMigrator>();
                var builder = new SnapshotRegistryBuilder(validators, definitions, migrators);
                return builder.Build();
            });
        }
    }
}
#pragma warning restore S2436