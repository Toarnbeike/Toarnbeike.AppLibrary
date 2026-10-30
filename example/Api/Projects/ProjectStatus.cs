namespace Toarnbeike.AppLibrary.TicketApp.Projects;

/// <summary>
/// The lifecycle state of a project.
/// </summary>
/// <remarks>Use to indicate whether a project is in active development, publicly released, or no longer maintained.</remarks>
public enum ProjectStatus
{
    Development,
    Release,
    Archived
}
