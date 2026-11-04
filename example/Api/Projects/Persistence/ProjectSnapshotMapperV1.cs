using Toarnbeike.AppLibrary.Entities.Audits;
using Toarnbeike.AppLibrary.Snapshots;
using Toarnbeike.AppLibrary.Snapshots.Audits;

namespace Toarnbeike.AppLibrary.TicketApp.Projects.Persistence;

/// <summary>
/// Maps <see cref="Project"/> instances to <see cref="ProjectSnapshotV1"/> snapshots and vice versa.
/// </summary>
/// <remarks>
/// - Here also the version number is included in the class name to allow for future migrations and versioning of snapshot mappings.
/// - The auditInfo and softDeleteInfo data is stored as snapshots. 
///   This allows for easy mapping, but might be less suitable for sql table style persistence.
///   If a more relational approach is desired, consider storing audit and soft-delete info as flattened fields in the snapshot.
/// - The enum values for Status and Visibility are stored as strings in the snapshot.
///   This allows for future changes to the enum definitions without breaking existing snapshots.
///   An alternative approach with integer values could be used, but would require careful management of enum value changes.
/// </remarks>
/// <param name="auditInfoSnapshotMapper">Maps <see cref="AuditInfo"/> to <see cref="AuditInfoSnapshot"/>.</param>
/// <param name="softDeleteInfoSnapshotMapper">Maps <see cref="SoftDeleteInfo"/> to <see cref="SoftDeleteInfoSnapshot"/>.</param>
internal sealed class ProjectSnapshotMapperV1(
    ISnapshotMapper<AuditInfo, AuditInfoSnapshot> auditInfoSnapshotMapper,
    ISnapshotMapper<SoftDeleteInfo, SoftDeleteInfoSnapshot> softDeleteInfoSnapshotMapper)
    : ISnapshotMapper<Project, ProjectSnapshotV1>
{
    public ProjectSnapshotV1 ToSnapshot(Project domainObject) => new(
        Id: domainObject.Id.Value,
        Name: domainObject.Name,
        Description: domainObject.Description,
        Status: domainObject.Status.ToString(),
        Visibility: domainObject.Visibility.ToString(),
        AuditInfo: auditInfoSnapshotMapper.ToSnapshot(domainObject.AuditInfo),
        SoftDeleteInfo: softDeleteInfoSnapshotMapper.ToSnapshot(domainObject.SoftDeleteInfo));

    public Project FromSnapshot(ProjectSnapshotV1 snapshot) => new Project(
        Id: new ProjectId(snapshot.Id),
        Name: snapshot.Name,
        Description: snapshot.Description,
        Status: Enum.Parse<ProjectStatus>(snapshot.Status),
        Visibility: Enum.Parse<ProjectVisibility>(snapshot.Visibility))
        .WithAuditInfo(auditInfoSnapshotMapper.FromSnapshot(snapshot.AuditInfo))
        .WithSoftDeleteInfo(softDeleteInfoSnapshotMapper.FromSnapshot(snapshot.SoftDeleteInfo));
}