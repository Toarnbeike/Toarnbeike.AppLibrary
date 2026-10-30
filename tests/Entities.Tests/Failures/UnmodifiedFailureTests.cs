using Toarnbeike.Results;

namespace Toarnbeike.AppLibrary.Failures;

public class UnmodifiedFailureTests
{
    [Test]
    public void UnmodifiedFailure_CreatesInstanceWithPropertyName()
    {
        var propertyName = "TestProperty";
        var failure = new UnmodifiedFailure(propertyName);

        failure.ShouldNotBeNull();
        failure.Property.ShouldBe(propertyName);
        failure.Message.ShouldBe($"Property '{propertyName}' was not modified, value is unchanged");
        failure.Category.ShouldBe(FailureCategory.Business);
    }
}
