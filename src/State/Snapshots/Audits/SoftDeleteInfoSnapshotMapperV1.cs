using Toarnbeike.AppLibrary.Entities.Audits;
using Toarnbeike.Optional.Extensions;

namespace Toarnbeike.AppLibrary.Snapshots.Audits;

/// <summary>
/// Snapshot mapper for the <see cref="SoftDeleteInfo"/> value object.
/// </summary>
internal sealed record SoftDeleteInfoSnapshotMapperV1 : ISnapshotMapper<SoftDeleteInfo, SoftDeleteInfoSnapshot>
{
    /// <inheritdoc />
    public SoftDeleteInfoSnapshot ToSnapshot(SoftDeleteInfo domainObject) => new(
        domainObject.IsDeleted,
        domainObject.DeletedBy.AsNullable(),
        domainObject.DeletedAt.AsNullableValue());

    /// <inheritdoc />
    public SoftDeleteInfo FromSnapshot(SoftDeleteInfoSnapshot snapshot) => new(
        snapshot.IsDeleted,
        snapshot.DeletedBy.AsOption(),
        snapshot.DeletedAt.AsOption());
}