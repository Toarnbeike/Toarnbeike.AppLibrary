namespace Toarnbeike.AppLibrary.Services;

public class SystemDateTimeProviderTests
{
    [Test]
    public void UtcNow_ShouldReturnCurrentUtcDateTime()
    {
        IDateTimeProvider dateTimeProvider = new SystemDateTimeProvider();
        var utcNow = dateTimeProvider.UtcNow;
        var expectedUtcNow = DateTime.UtcNow;
        utcNow.ShouldBe(expectedUtcNow, TimeSpan.FromMilliseconds(10));
        utcNow.Kind.ShouldBe(DateTimeKind.Utc);
    }

    [Test]
    public void Now_ShouldReturnCurrentLocalDateTimeOffset()
    {
        IDateTimeProvider dateTimeProvider = new SystemDateTimeProvider();
        var now = dateTimeProvider.Now;
        var expectedNow = DateTimeOffset.Now;
        now.ShouldBe(expectedNow, TimeSpan.FromMilliseconds(10));
        now.Offset.ShouldBe(expectedNow.Offset);
    }
}
