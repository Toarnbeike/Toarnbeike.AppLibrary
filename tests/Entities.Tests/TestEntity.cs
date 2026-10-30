using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.AppLibrary.Entities.Audits;
using Toarnbeike.AppLibrary.Events;

namespace Toarnbeike.AppLibrary;

public readonly record struct TestEntityId(Guid Value) : IEntityId
{
    public static TestEntityId New() => new(Guid.NewGuid());
}

public record TestEntity(TestEntityId Id, string Name) : IAggregate<TestEntityId>, IAuditable<TestEntity>, ISoftDeletable<TestEntity>
{
    public AuditInfo AuditInfo { get; private init; } = AuditInfo.Empty;

    public SoftDeleteInfo SoftDeleteInfo { get; private init; } = SoftDeleteInfo.NotDeleted;

    public TestEntity WithAuditInfo(AuditInfo auditInfo) => 
        this with { AuditInfo = auditInfo };

    public TestEntity WithSoftDeleteInfo(SoftDeleteInfo softDeleteInfo) => 
        this with { SoftDeleteInfo = softDeleteInfo };
}

public sealed record TestEvent(TestEntityId Id) : IDomainEvent;

public sealed record DerivedTestEntity(TestEntityId Id, string Name, int Value) : TestEntity(Id, Name);