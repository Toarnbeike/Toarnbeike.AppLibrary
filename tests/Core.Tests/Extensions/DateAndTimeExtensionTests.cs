namespace Toarnbeike.AppLibrary.Extensions;

public class DateAndTimeExtensionTests
{
    [Test]
    public void Today_ReturnsCurrentDateInUtc()
    {
        var today = DateOnly.Today;
        var utcNow = DateTime.UtcNow;
        var expectedToday = new DateOnly(utcNow.Year, utcNow.Month, utcNow.Day);
        today.ShouldBe(expectedToday);
    }

    [Test]
    public void GetAge_CalculatesAge_WithSuppliedAsOfDate()
    {
        var birthDate = new DateOnly(1990, 1, 1);
        var asOfDate = new DateOnly(2023, 1, 7);
        var age = birthDate.GetAge(asOfDate);
        age.ShouldBe(33);
    }

    [Test]
    public void GetAge_CalculatesAge_WhenBirthdayHasNotOccurredYet()
    {
        var birthDate = new DateOnly(1990, 12, 31);
        var asOfDate = new DateOnly(2023, 1, 1);
        var age = birthDate.GetAge(asOfDate);
        age.ShouldBe(32);
    }

    [Test]
    public void GetAge_CalculatesAge_WhenBirthdayIsToday()
    {
        var birthDate = new DateOnly(1990, 1, 1);
        var asOfDate = new DateOnly(2023, 1, 1);
        var age = birthDate.GetAge(asOfDate);
        age.ShouldBe(33);
    }

    [Test]
    public void GetAge_CalculatesAge_WhenBirthdayIsOnLeapDay()
    {
        var birthDate = new DateOnly(1992, 2, 29);
        var asOfDate = new DateOnly(2023, 2, 28);
        var age = birthDate.GetAge(asOfDate);
        age.ShouldBe(30);
    }

    [Test]
    public void GetAge_CalculatesAge_WhenBirthdayIsOnLeapDayAndAsOfDateIsLeapYear()
    {
        var birthDate = new DateOnly(1992, 2, 29);
        var asOfDate = new DateOnly(2024, 2, 29);
        var age = birthDate.GetAge(asOfDate);
        age.ShouldBe(32);
    }

    [Test]
    public void GetAge_CalculatesAge_WhenTodayIsALeapDayAndBirthdayIsNot()
    {
        var birthDate = new DateOnly(1990, 3, 1);
        var asOfDate = new DateOnly(2024, 2, 29);
        var age = birthDate.GetAge(asOfDate);
        age.ShouldBe(33);
    }

    [Test]
    public void GetAge_CalculatesAge_AcceptsNullAsOfDate()
    {
        var birthDate = new DateOnly(1990, 1, 1);
        var result = birthDate.GetAge();
        result.ShouldBeGreaterThan(35);
    }

    [Test]
    public void ToFileNameFormat_ReturnsCorrectFormat_ForDateTime()
    {
        var dateTime = new DateTime(2023, 1, 7, 15, 30, 45, DateTimeKind.Local);
        var formatted = dateTime.ToFileNameFormat();
        formatted.ShouldBe("2023-01-07_15-30-45");
    }

    [Test]
    public void ToFileNameFormat_ReturnsCorrectFormat_ForDateTimeOffset()
    {
        var dateTimeOffset = new DateTimeOffset(2023, 1, 7, 15, 30, 45, TimeSpan.FromHours(-5));
        var formatted = dateTimeOffset.ToFileNameFormat();
        formatted.ShouldBe("2023-01-07_15-30-45");
    }
}
