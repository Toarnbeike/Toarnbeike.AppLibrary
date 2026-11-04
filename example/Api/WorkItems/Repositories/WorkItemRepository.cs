using Toarnbeike.AppLibrary.Repositories;
using Toarnbeike.AppLibrary.TicketApp.Projects;

namespace Toarnbeike.AppLibrary.TicketApp.WorkItems.Repositories;

/// <inheritdoc/>
internal sealed class WorkItemRepository(IAggregateStore dataStore) : Repository<WorkItem, WorkItemId>(dataStore), IWorkItemRepository
{
    /// <inheritdoc/>
    public IEnumerable<WorkItem> GetByProjectId(ProjectId projectId) =>
        GetAll().Where(workItem => workItem.ProjectId == projectId);

    /// <inheritdoc/>
    public IEnumerable<TWorkItem> GetAll<TWorkItem>() where TWorkItem : WorkItem =>
        GetAll().OfType<TWorkItem>();
}