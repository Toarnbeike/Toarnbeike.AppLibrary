namespace Toarnbeike.AppLibrary.Failures;

public class MethodUnavailableFailureTests
{
    [Test]
    public void MethodUnavailableFailure_CreatesInstanceWithMethodNameAndReason()
    {
        var methodName = "TestMethod";
        var reason = "This method is not implemented.";
        var failure = new MethodUnavailableFailure(methodName, reason);
        failure.ShouldNotBeNull();
        failure.MethodName.ShouldBe(methodName);
        failure.Reason.ShouldBe(reason);
        failure.Message.ShouldBe($"Method '{methodName}' is unavailable: {reason}");
    }

    [Test]
    public void MethodUnavailableFailure_CreatesInstanceWithMethodNameOnly()
    {
        var methodName = "TestMethod";
        var failure = new MethodUnavailableFailure(methodName);
        failure.ShouldNotBeNull();
        failure.MethodName.ShouldBe(methodName);
        failure.Reason.ShouldBeNull();
        failure.Message.ShouldBe($"Method '{methodName}' is unavailable");
    }
}
