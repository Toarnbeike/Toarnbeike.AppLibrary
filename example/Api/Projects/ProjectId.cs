using Toarnbeike.AppLibrary.Entities;

namespace Toarnbeike.AppLibrary.TicketApp.Projects;

public readonly record struct ProjectId(Guid Value) : IEntityId
{
    public static ProjectId New() => new(Guid.CreateVersion7());
}
