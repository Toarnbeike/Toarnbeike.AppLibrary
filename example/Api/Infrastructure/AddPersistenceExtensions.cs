using Toarnbeike.AppLibrary.DependencyInjection;
using Toarnbeike.AppLibrary.TicketApp.Projects;
using Toarnbeike.AppLibrary.TicketApp.Projects.Persistence;
using Toarnbeike.AppLibrary.TicketApp.WorkItems;
using Toarnbeike.AppLibrary.TicketApp.WorkItems.Persistence;
using Toarnbeike.AppLibrary.TicketApp.WorkItems.Repositories;
using Toarnbeike.AppLibrary.TicketApp.WorkItemTasks;
using Toarnbeike.AppLibrary.TicketApp.WorkItemTasks.Persistence;

namespace Toarnbeike.AppLibrary.TicketApp.Infrastructure;

public static class AddPersistenceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence()
        {
            //services.AddStateManagement<TSnapshotStore>()
            services.AddSnapshot<Project, ProjectSnapshotV1, ProjectSnapshotMapperV1>("Projects");
            services.AddRepository<Project, ProjectId>();

            services.AddSnapshot<WorkItem, WorkItemSnapshotV1, WorkItemSnapshotMapperV1>("WorkItems");
            services.AddNonAggregateMapper<WorkItemTask, WorkItemTaskSnapshotV1, WorkItemTaskSnapshotMapperV1>();
            services.AddRepository<WorkItem, WorkItemId, IWorkItemRepository, WorkItemRepository>();

            return services;
        }
    }
}
