using Toarnbeike.AppLibrary.Entities.Audits;
using Toarnbeike.Optional.Extensions;

namespace Toarnbeike.AppLibrary.Snapshots.Audits;

/// <summary>
/// Snapshot mapper for the <see cref="AuditInfo"/> value object.
/// </summary>
internal sealed class AuditInfoSnapshotMapperV1 : ISnapshotMapper<AuditInfo, AuditInfoSnapshot>
{
    /// <inheritdoc />
    public AuditInfoSnapshot ToSnapshot(AuditInfo domainObject) => new(
        domainObject.CreatedAt,
        domainObject.CreatedBy,
        domainObject.LastModifiedAt.AsNullableValue(),
        domainObject.LastModifiedBy.AsNullable());

    /// <inheritdoc />
    public AuditInfo FromSnapshot(AuditInfoSnapshot snapshot) => new(
        snapshot.CreatedAt,
        snapshot.CreatedBy,
        snapshot.LastModifiedAt.AsOption(),
        snapshot.LastModifiedBy.AsOption());
}
