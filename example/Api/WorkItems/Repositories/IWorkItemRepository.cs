using Toarnbeike.AppLibrary.Repositories;
using Toarnbeike.AppLibrary.TicketApp.Projects;
using Toarnbeike.Results;

namespace Toarnbeike.AppLibrary.TicketApp.WorkItems.Repositories;

public interface IWorkItemRepository : IRepository<WorkItem, WorkItemId>
{
    /// <summary>
    /// Retrieve all work items by foreign key projectId.
    /// </summary>
    /// <param name="projectId">The identifier of the project.</param>
    /// <returns>A sequence of work items associated with the specified project.</returns>
    IEnumerable<WorkItem> GetByProjectId(ProjectId projectId);

    /// <summary>
    /// Get all workitems of the specified type.
    /// </summary>
    /// <typeparam name="TWorkItem">The requested work item type, e.g. <see cref="Bug"/> or <see cref="Failure"/> or <see cref="Refactoring"/>.</typeparam>
    /// <returns>A sequence of work items of the specified type.</returns>
    IEnumerable<TWorkItem> GetAll<TWorkItem>() where TWorkItem : WorkItem;
}
