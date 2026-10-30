using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Toarnbeike.AppLibrary.DependencyInjection;

public class AddImplementationsOffExtensionTests
{
    [Test]
    public void AddImplementationsOf_NullServices_Throws()
    {
        IServiceCollection services = null!;

        var exception = Should.Throw<ArgumentNullException>(() =>
            services.AddImplementationsOf(typeof(IService), new TestAssembly(), ServiceLifetime.Singleton));

        exception.ParamName.ShouldBe("services");
    }

    [Test]
    public void AddImplementationsOf_NullServiceType_Throws()
    {
        var services = new ServiceCollection();

        var exception = Should.Throw<ArgumentNullException>(() =>
            services.AddImplementationsOf(null!, new TestAssembly(), ServiceLifetime.Singleton));

        exception.ParamName.ShouldBe("serviceType");
    }

    [Test]
    public void AddImplementationsOf_NullAssembly_Throws()
    {
        var services = new ServiceCollection();

        var exception = Should.Throw<ArgumentNullException>(() =>
            services.AddImplementationsOf(typeof(IService), (Assembly)null!, ServiceLifetime.Singleton));

        exception.ParamName.ShouldBe("assembly");
    }

    [Test]
    public void AddImplementationsOf_NullAssemblySequence_Throws()
    {
        var services = new ServiceCollection();

        var exception = Should.Throw<ArgumentNullException>(() =>
            services.AddImplementationsOf(typeof(IService), (IEnumerable<Assembly>)null!, ServiceLifetime.Singleton));

        exception.ParamName.ShouldBe("assemblies");
    }

    [Test]
    public void AddImplementationsOf_NonGenericService_RegistersConcreteExactMatches()
    {
        var services = new ServiceCollection();
        var assembly = new TestAssembly(typeof(IService), typeof(AbstractService), typeof(Service), typeof(UnrelatedService));

        var result = services.AddImplementationsOf(typeof(IService), assembly, ServiceLifetime.Scoped);

        result.ShouldBeSameAs(services);
        var descriptor = services.ShouldHaveSingleItem();
        descriptor.ServiceType.ShouldBe(typeof(IService));
        descriptor.ImplementationType.ShouldBe(typeof(Service));
        descriptor.Lifetime.ShouldBe(ServiceLifetime.Scoped);
    }

    [Test]
    public void AddImplementationsOf_NonGenericService_WithAllowDerivedFalse_RegistersExactMatch()
    {
        var services = new ServiceCollection();

        var result = services.AddImplementationsOf<IService>(new TestAssembly(typeof(Service)), ServiceLifetime.Transient);

        result.ShouldBeSameAs(services);
        var descriptor = services.ShouldHaveSingleItem();
        descriptor.ServiceType.ShouldBe(typeof(IService));
        descriptor.ImplementationType.ShouldBe(typeof(Service));
    }

    [Test]
    public void AddImplementationsOf_NonGenericService_DoesNotRegisterBaseInterfaceMatch()
    {
        var services = new ServiceCollection();

        services.AddImplementationsOf(typeof(IDerivedService), new TestAssembly(typeof(Service)), ServiceLifetime.Singleton, allowDerived: true);

        services.ShouldBeEmpty();
    }

    [Test]
    public void AddImplementationsOf_DerivedInterface_DefaultRegistersOnlyRequestedInterface()
    {
        var services = new ServiceCollection();

        services.AddImplementationsOf(typeof(IService), new TestAssembly(typeof(DerivedService)), ServiceLifetime.Scoped);

        var descriptor = services.ShouldHaveSingleItem();
        descriptor.ServiceType.ShouldBe(typeof(IService));
        descriptor.ImplementationType.ShouldBe(typeof(DerivedService));
    }

    [Test]
    public void AddImplementationsOf_DerivedInterface_WhenEnabledRegistersBaseAndDerivedInterfaces()
    {
        var services = new ServiceCollection();

        services.AddImplementationsOf<IService>(new TestAssembly(typeof(DerivedService)), ServiceLifetime.Scoped, allowDerived: true);

        services.Count.ShouldBe(2);
        services.ShouldContain(descriptor => descriptor.ServiceType == typeof(IService) && descriptor.ImplementationType == typeof(DerivedService));
        services.ShouldContain(descriptor => descriptor.ServiceType == typeof(IDerivedService) && descriptor.ImplementationType == typeof(DerivedService));
        services.ShouldAllBe(descriptor => descriptor.Lifetime == ServiceLifetime.Scoped);
    }

    [Test]
    public void AddImplementationsOf_GenericOverload_WithAllowDerivedTrue_RegistersExactMatch()
    {
        var services = new ServiceCollection();

        services.AddImplementationsOf<IService>(new TestAssembly(typeof(Service)), ServiceLifetime.Transient, allowDerived: true);

        var descriptor = services.ShouldHaveSingleItem();
        descriptor.ServiceType.ShouldBe(typeof(IService));
        descriptor.ImplementationType.ShouldBe(typeof(Service));
        descriptor.Lifetime.ShouldBe(ServiceLifetime.Transient);
    }

    [Test]
    public void AddImplementationsOf_OpenGenericService_RegistersMatchingOpenAndClosedImplementations()
    {
        var services = new ServiceCollection();
        var assembly = new TestAssembly(
            typeof(IService), typeof(AbstractGenericService<>), typeof(Service), typeof(OtherGenericService<>),
            typeof(OpenGenericService<>), typeof(WrongArityService<,>), typeof(ClosedGenericService));

        var result = services.AddImplementationsOf(typeof(IGenericService<>), assembly, ServiceLifetime.Singleton, allowDerived: false);

        result.ShouldBeSameAs(services);
        services.Count.ShouldBe(2);
        var open = services.Single(descriptor => descriptor.ImplementationType == typeof(OpenGenericService<>));
        open.ServiceType.ShouldBe(typeof(IGenericService<>));
        open.Lifetime.ShouldBe(ServiceLifetime.Singleton);
        var closed = services.Single(descriptor => descriptor.ImplementationType == typeof(ClosedGenericService));
        closed.ServiceType.ShouldBe(typeof(IGenericService<int>));
        closed.Lifetime.ShouldBe(ServiceLifetime.Singleton);
    }

    [Test]
    public void AddImplementationsOf_OpenGenericService_RegistersEachClosedInterface()
    {
        var services = new ServiceCollection();

        services.AddImplementationsOf(typeof(IGenericService<>), new TestAssembly(typeof(MultipleGenericService)), ServiceLifetime.Scoped);

        services.Count.ShouldBe(2);
        services.ShouldContain(descriptor => descriptor.ServiceType == typeof(IGenericService<int>) && descriptor.ImplementationType == typeof(MultipleGenericService));
        services.ShouldContain(descriptor => descriptor.ServiceType == typeof(IGenericService<string>) && descriptor.ImplementationType == typeof(MultipleGenericService));
    }

    [Test]
    public void AddImplementationsOf_OpenGenericService_DefaultRegistersOnlyRequestedInterface()
    {
        var services = new ServiceCollection();

        services.AddImplementationsOf(typeof(IGenericService<>), new TestAssembly(typeof(DerivedGenericService<>)), ServiceLifetime.Scoped);

        var descriptor = services.ShouldHaveSingleItem();
        descriptor.ServiceType.ShouldBe(typeof(IGenericService<>));
        descriptor.ImplementationType.ShouldBe(typeof(DerivedGenericService<>));
    }

    [Test]
    public void AddImplementationsOf_OpenGenericService_WhenEnabledRegistersDerivedInterfaces()
    {
        var services = new ServiceCollection();

        services.AddImplementationsOf(typeof(IGenericService<>), new TestAssembly(typeof(DerivedGenericService<>), typeof(ClosedDerivedGenericService)), ServiceLifetime.Scoped, allowDerived: true);

        services.Count.ShouldBe(4);
        services.ShouldContain(descriptor => descriptor.ServiceType == typeof(IGenericService<>) && descriptor.ImplementationType == typeof(DerivedGenericService<>));
        services.ShouldContain(descriptor => descriptor.ServiceType == typeof(IDerivedGenericService<>) && descriptor.ImplementationType == typeof(DerivedGenericService<>));
        services.ShouldContain(descriptor => descriptor.ServiceType == typeof(IGenericService<int>) && descriptor.ImplementationType == typeof(ClosedDerivedGenericService));
        services.ShouldContain(descriptor => descriptor.ServiceType == typeof(IDerivedGenericService<int>) && descriptor.ImplementationType == typeof(ClosedDerivedGenericService));
    }

    [Test]
    public void AddImplementationsOf_ClosedGenericService_UsesNonGenericMatching()
    {
        var services = new ServiceCollection();

        services.AddImplementationsOf(typeof(IGenericService<int>), new TestAssembly(typeof(ClosedGenericService)), ServiceLifetime.Scoped);

        var descriptor = services.ShouldHaveSingleItem();
        descriptor.ServiceType.ShouldBe(typeof(IGenericService<int>));
        descriptor.ImplementationType.ShouldBe(typeof(ClosedGenericService));
        descriptor.Lifetime.ShouldBe(ServiceLifetime.Scoped);
    }

    [Test]
    public void AddImplementationsOf_AssemblySequence_RegistersEachAssemblyAndReturnsServices()
    {
        var services = new ServiceCollection();
        Assembly[] assemblies = [new TestAssembly(typeof(Service)), new TestAssembly(typeof(SecondService))];

        var result = services.AddImplementationsOf(typeof(IService), assemblies, ServiceLifetime.Singleton);

        result.ShouldBeSameAs(services);
        services.Count.ShouldBe(2);
        services.ShouldContain(descriptor => descriptor.ServiceType == typeof(IService) && descriptor.ImplementationType == typeof(Service) && descriptor.Lifetime == ServiceLifetime.Singleton);
        services.ShouldContain(descriptor => descriptor.ServiceType == typeof(IService) && descriptor.ImplementationType == typeof(SecondService) && descriptor.Lifetime == ServiceLifetime.Singleton);
    }

    [Test]
    public void AddImplementationsOf_GenericAssemblySequence_ForwardsAllowDerived()
    {
        var services = new ServiceCollection();
        Assembly[] assemblies = [new TestAssembly(typeof(Service)), new TestAssembly(typeof(SecondService))];

        var result = services.AddImplementationsOf<IService>(assemblies, ServiceLifetime.Scoped, allowDerived: true);

        result.ShouldBeSameAs(services);
        services.Count.ShouldBe(2);
        services.ShouldAllBe(descriptor => descriptor.ServiceType == typeof(IService) && descriptor.Lifetime == ServiceLifetime.Scoped);
    }

    [Test]
    public void AddImplementationsOf_GenericAssemblySequence_DefaultRegistersExactMatch()
    {
        var services = new ServiceCollection();

        services.AddImplementationsOf<IService>([new TestAssembly(typeof(Service))], ServiceLifetime.Scoped);

        services.ShouldHaveSingleItem().ImplementationType.ShouldBe(typeof(Service));
    }

    [Test]
    public void AddImplementationsOf_EmptyAssemblySequence_ValidatesServiceType()
    {
        var services = new ServiceCollection();

        var exception = Should.Throw<ArgumentNullException>(() =>
            services.AddImplementationsOf(null!, Array.Empty<Assembly>(), ServiceLifetime.Singleton));

        exception.ParamName.ShouldBe("serviceType");
    }

    [Test]
    public void AddImplementationsOf_EmptyAssemblySequence_ValidatesServices()
    {
        IServiceCollection services = null!;

        var exception = Should.Throw<ArgumentNullException>(() =>
            services.AddImplementationsOf(typeof(IService), Array.Empty<Assembly>(), ServiceLifetime.Singleton));

        exception.ParamName.ShouldBe("services");
    }

    [Test]
    public void AddImplementationsOf_NonInterfaceServiceType_Throws()
    {
        var services = new ServiceCollection();

        var exception = Should.Throw<ArgumentException>(() =>
            services.AddImplementationsOf(typeof(Service), new TestAssembly(), ServiceLifetime.Singleton));

        exception.ParamName.ShouldBe("serviceType");
    }

    [Test]
    public void AddImplementationsOf_InvalidLifetime_ThrowsEvenWhenAssemblyIsEmpty()
    {
        var services = new ServiceCollection();

        var exception = Should.Throw<ArgumentOutOfRangeException>(() =>
            services.AddImplementationsOf(typeof(IService), Array.Empty<Assembly>(), (ServiceLifetime)999));

        exception.ParamName.ShouldBe("lifetime");
    }

    [Test]
    public void AddImplementationsOf_OpenGenericService_SkipsUnresolvableParameterMappings()
    {
        var services = new ServiceCollection();
        var assembly = new TestAssembly(typeof(ResolutionService<>), typeof(NestedParameterService<>), typeof(FixedParameterService<>));

        services.AddImplementationsOf(typeof(IResolutionService<>), assembly, ServiceLifetime.Transient);

        var descriptor = services.ShouldHaveSingleItem();
        descriptor.ServiceType.ShouldBe(typeof(IResolutionService<>));
        descriptor.ImplementationType.ShouldBe(typeof(ResolutionService<>));
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });
        provider.GetRequiredService<IResolutionService<int>>().ShouldBeOfType<ResolutionService<int>>();
    }

    [Test]
    public void AddImplementationsOf_OpenGenericService_SkipsReorderedParameters()
    {
        var services = new ServiceCollection();
        var assembly = new TestAssembly(typeof(OrderedPairService<,>), typeof(ReorderedPairService<,>));

        services.AddImplementationsOf(typeof(IPairService<,>), assembly, ServiceLifetime.Transient);

        services.ShouldHaveSingleItem().ImplementationType.ShouldBe(typeof(OrderedPairService<,>));
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });
        provider.GetRequiredService<IPairService<int, string>>().ShouldBeOfType<OrderedPairService<int, string>>();
    }

    [Test]
    public void AddImplementationsOf_NonGenericService_SkipsOpenGenericImplementation()
    {
        var services = new ServiceCollection();

        services.AddImplementationsOf(typeof(IService), new TestAssembly(typeof(OpenNonGenericService<>)), ServiceLifetime.Singleton);

        services.ShouldBeEmpty();
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });
        provider.GetService<IService>().ShouldBeNull();
    }

    [Test]
    public void AddImplementationsOf_DerivedInterfaces_CanResolveBothServiceTypes()
    {
        var services = new ServiceCollection();
        var assembly = new TestAssembly(typeof(ResolvableDerivedService), typeof(ResolvableDerivedGenericService<>));
        services.AddImplementationsOf(typeof(IResolvableBase), assembly, ServiceLifetime.Singleton, allowDerived: true);
        services.AddImplementationsOf(typeof(IResolvableGenericBase<>), assembly, ServiceLifetime.Transient, allowDerived: true);
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });

        provider.GetRequiredService<IResolvableBase>().ShouldBeOfType<ResolvableDerivedService>();
        provider.GetRequiredService<IResolvableDerived>().ShouldBeOfType<ResolvableDerivedService>();
        provider.GetRequiredService<IResolvableGenericBase<int>>().ShouldBeOfType<ResolvableDerivedGenericService<int>>();
        provider.GetRequiredService<IResolvableGenericDerived<int>>().ShouldBeOfType<ResolvableDerivedGenericService<int>>();
    }

    [Test]
    public void AddImplementationsOf_AssemblySequenceContainingNull_ThrowsAfterEarlierRegistrations()
    {
        var services = new ServiceCollection();
        Assembly[] assemblies = [new TestAssembly(typeof(Service)), null!];

        var exception = Should.Throw<ArgumentNullException>(() =>
            services.AddImplementationsOf(typeof(IService), assemblies, ServiceLifetime.Singleton));

        exception.ParamName.ShouldBe("assembly");
        services.ShouldHaveSingleItem().ImplementationType.ShouldBe(typeof(Service));
    }

    [Test]
    public void AddImplementationsOf_RealAssembly_FindsImplementation()
    {
        var services = new ServiceCollection();

        services.AddImplementationsOf(typeof(IService), typeof(Service).Assembly, ServiceLifetime.Transient);

        services.ShouldContain(descriptor => descriptor.ServiceType == typeof(IService) && descriptor.ImplementationType == typeof(Service));
    }

    private sealed class TestAssembly(params Type[] types) : Assembly
    {
        public override Type[] GetTypes() => types;
    }

#pragma warning disable S2326
    private interface IService;

    private interface IDerivedService : IService;

    private interface IUnrelatedService;

    private interface IGenericService<T>;

    private interface IDerivedGenericService<T> : IGenericService<T>;

    private interface IOtherGenericService<T>;

    private abstract class AbstractService : IService;

    private abstract class AbstractGenericService<T> : IGenericService<T>;

    private sealed class Service : IService;

    private sealed class SecondService : IService;

    private sealed class DerivedService : IDerivedService;

    private sealed class UnrelatedService : IUnrelatedService;

    private sealed class OpenGenericService<T> : IGenericService<T>;

    private sealed class DerivedGenericService<T> : IDerivedGenericService<T>;

    private sealed class ClosedDerivedGenericService : IDerivedGenericService<int>;

    private sealed class WrongArityService<T, TOther> : IGenericService<T>;

    private sealed class OtherGenericService<T> : IOtherGenericService<T>;

    private sealed class ClosedGenericService : IGenericService<int>;

    private sealed class MultipleGenericService : IGenericService<int>, IGenericService<string>;

    private sealed class OpenNonGenericService<T> : IService;

    public interface IResolutionService<T>;

    public sealed class ResolutionService<T> : IResolutionService<T>;

    public sealed class NestedParameterService<T> : IResolutionService<List<T>>;

    public sealed class FixedParameterService<T> : IResolutionService<int>;

    public interface IPairService<TFirst, TSecond>;

    public sealed class OrderedPairService<TFirst, TSecond> : IPairService<TFirst, TSecond>;

    public sealed class ReorderedPairService<TFirst, TSecond> : IPairService<TSecond, TFirst>;

    public interface IResolvableBase;

    public interface IResolvableDerived : IResolvableBase;

    public sealed class ResolvableDerivedService : IResolvableDerived;

    public interface IResolvableGenericBase<T>;

    public interface IResolvableGenericDerived<T> : IResolvableGenericBase<T>;

    public sealed class ResolvableDerivedGenericService<T> : IResolvableGenericDerived<T>;
}
