using Toarnbeike.Optional.TestExtensions;

namespace Toarnbeike.AppLibrary.Entities.Audits;

public class SoftDeleteInfoTests
{
    [Test]
    public void Empty_SoftDeleteInfo_HasDefaultValues()
    {
        var emptySoftDeleteInfo = SoftDeleteInfo.Empty;
        emptySoftDeleteInfo.IsDeleted.ShouldBe(false);
        emptySoftDeleteInfo.DeletedBy.ShouldBeNone();
        emptySoftDeleteInfo.DeletedAt.ShouldBeNone();
    }
    [Test]
    public void NotDeleted_SoftDeleteInfo_HasDefaultValues()
    {
        var notDeletedSoftDeleteInfo = SoftDeleteInfo.NotDeleted;
        notDeletedSoftDeleteInfo.IsDeleted.ShouldBe(false);
        notDeletedSoftDeleteInfo.DeletedBy.ShouldBeNone();
        notDeletedSoftDeleteInfo.DeletedAt.ShouldBeNone();
    }
}