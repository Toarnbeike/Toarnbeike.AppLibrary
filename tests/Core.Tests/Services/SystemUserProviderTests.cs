namespace Toarnbeike.AppLibrary.Services;

public class SystemUserProviderTests
{
    [Test]
    public void UserName_ShouldReturnValueFromEnvironment()
    {
        ICurrentUserProvider userProvider = new SystemUserProvider();
        var currentUser = userProvider.UserName;
        var expected = Environment.UserName;
        currentUser.ShouldNotBeNull();
        currentUser.ShouldBe(expected);
    }
}
