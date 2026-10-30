using Microsoft.Extensions.DependencyInjection;
using Toarnbeike.AppLibrary.Services;

namespace Toarnbeike.AppLibrary.DependencyInjection;

public class AddSystemServiceExtensionTests
{
    [Test]
    public void AddSystemService_ShouldRegisterIDateTimeProvider()
    {
        var services = new ServiceCollection();
        
        services.AddSystemServices();
        var provider = services.BuildServiceProvider();
        var dateTimeProvider = provider.GetService<IDateTimeProvider>();
        
        dateTimeProvider.ShouldNotBeNull();
        dateTimeProvider.ShouldBeOfType<SystemDateTimeProvider>();
    }

    [Test]
    public void AddSystemService_ShouldRegisterICurrentUserProvider()
    {
        var services = new ServiceCollection();

        services.AddSystemServices();
        var provider = services.BuildServiceProvider();
        var currentUserProvider = provider.GetService<ICurrentUserProvider>();

        currentUserProvider.ShouldNotBeNull();
        currentUserProvider.ShouldBeOfType<SystemUserProvider>();
    }
}
