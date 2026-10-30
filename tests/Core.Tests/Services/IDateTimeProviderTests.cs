namespace Toarnbeike.AppLibrary.Services;

public class IDateTimeProviderTests
{
    [Test]
    public void Today_ShouldReturnCurrentDateInUtc()
    {
        IDateTimeProvider dateTimeProvider = new TestDateTimeProvider();
        var today = dateTimeProvider.Today;
        var expectedToday = new DateOnly(2025, 1, 1);
        today.ShouldBe(expectedToday);
    }
}

file class TestDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public DateTimeOffset Now => throw new NotImplementedException();
}