namespace Toarnbeike.AppLibrary.Entities.Audits;

public class SoftDeleteInfoTests
{
    [Test]
    public void Empty_SoftDeleteInfo_HasDefaultValues()
    {
        var emptySoftDeleteInfo = SoftDeleteInfo.Empty;
        emptySoftDeleteInfo.IsDeleted.ShouldBe(false);
        emptySoftDeleteInfo.DeletedBy.ShouldBeNull();
        emptySoftDeleteInfo.Deleted.ShouldBeNull();
    }
    [Test]
    public void NotDeleted_SoftDeleteInfo_HasDefaultValues()
    {
        var notDeletedSoftDeleteInfo = SoftDeleteInfo.NotDeleted;
        notDeletedSoftDeleteInfo.IsDeleted.ShouldBe(false);
        notDeletedSoftDeleteInfo.DeletedBy.ShouldBeNull();
        notDeletedSoftDeleteInfo.Deleted.ShouldBeNull();
    }
}