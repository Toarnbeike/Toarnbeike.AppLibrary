using Microsoft.Extensions.DependencyInjection;

namespace Toarnbeike.AppLibrary.DependencyInjection;

public class ReplaceAllExtensionTests
{
    [Test]
    public void ReplaceAll_ShouldReplaceAllRegistrations()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITestService, TestService1>();
        services.AddSingleton<ITestService, TestService2>();

        services.ReplaceAll<ITestService, TestService3>(ServiceLifetime.Singleton);
        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetRequiredService<ITestService>();

        service.ShouldBeOfType<TestService3>();
    }

    [Test]
    public void ReplaceAll_ShouldReplaceAllRegistrations_WhenRequestingACollection()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITestService, TestService1>();
        services.AddSingleton<ITestService, TestService2>();

        services.ReplaceAll<ITestService, TestService3>(ServiceLifetime.Singleton);
        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetRequiredService<IEnumerable<ITestService>>();

        service.ShouldHaveSingleItem().ShouldBeOfType<TestService3>();
    }

    [Test]
    public void ReplaceAll_ShouldReplaceAllRegistrations_WhenNoExistingRegistrations()
    {
        var services = new ServiceCollection();
        services.ReplaceAll<ITestService, TestService3>(ServiceLifetime.Singleton);
        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetRequiredService<ITestService>();
        service.ShouldBeOfType<TestService3>();
    }

    [Test]
    public void ReplaceAll_ShouldReplaceAllRegistrations_WhenExistingRegistrationsAreOfDifferentLifetime()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITestService, TestService1>();
        services.AddScoped<ITestService, TestService2>();

        services.ReplaceAll<ITestService, TestService3>(ServiceLifetime.Singleton);
        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetRequiredService<ITestService>();

        service.ShouldBeOfType<TestService3>();
    }

    [Test]
    public void ReplaceAllSingleton_ShouldReplaceAllRegistrations()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITestService, TestService1>();
        services.AddSingleton<ITestService, TestService2>();

        services.ReplaceAllSingleton<ITestService, TestService3>();
        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetRequiredService<ITestService>();

        service.ShouldBeOfType<TestService3>();
    }

    [Test]
    public void ReplaceAllScoped_ShouldReplaceAllRegistrations()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITestService, TestService1>();
        services.AddSingleton<ITestService, TestService2>();

        services.ReplaceAllScoped<ITestService, TestService3>();
        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetRequiredService<ITestService>();

        service.ShouldBeOfType<TestService3>();
    }

    [Test]
    public void ReplaceAllTransient_ShouldReplaceAllRegistrations()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITestService, TestService1>();
        services.AddSingleton<ITestService, TestService2>();

        services.ReplaceAllTransient<ITestService, TestService3>();
        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetRequiredService<ITestService>();

        service.ShouldBeOfType<TestService3>();
    }
}

file interface ITestService;

file class TestService1 : ITestService;

file class TestService2 : ITestService;

file class TestService3 : ITestService;
