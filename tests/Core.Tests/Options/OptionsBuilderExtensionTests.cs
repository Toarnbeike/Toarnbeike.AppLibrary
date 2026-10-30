using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Toarnbeike.AppLibrary.Options;

public class OptionsBuilderExtensionTests
{
    [Test]
    public void AddNestedOptions_ThrowsWhenBuilderIsNull()
    {
        OptionsBuilder<RootOptions> builder = null!;

        var exception = Should.Throw<ArgumentNullException>(() => builder.AddNestedOptions(root => root.Nested));

        exception.ParamName.ShouldBe("builder");
    }

    [Test]
    public void AddNestedOptions_ThrowsWhenSelectorIsNull()
    {
        var builder = new ServiceCollection().AddOptions<RootOptions>();
        Func<RootOptions, NestedOptions> selector = null!;

        var exception = Should.Throw<ArgumentNullException>(() => builder.AddNestedOptions(selector));

        exception.ParamName.ShouldBe("selector");
    }

    [Test]
    public void AddNestedOptions_ReturnsTheOriginalBuilder()
    {
        var builder = new ServiceCollection().AddOptions<RootOptions>();

        var result = builder.AddNestedOptions(root => root.Nested);

        result.ShouldBeSameAs(builder);
    }

    [Test]
    public void AddNestedOptions_ResolvesSelectedNestedOptionsAsSingleton()
    {
        var services = new ServiceCollection();
        var nested = new NestedOptions { Value = "configured" };
        var selectorCalls = 0;
        services.AddOptions<RootOptions>()
            .Configure(root => root.Nested = nested)
            .AddNestedOptions(root =>
            {
                selectorCalls++;
                return root.Nested;
            });
        using var provider = services.BuildServiceProvider();

        selectorCalls.ShouldBe(0);
        var first = provider.GetRequiredService<IOptions<NestedOptions>>();
        var second = provider.GetRequiredService<IOptions<NestedOptions>>();

        first.ShouldBeSameAs(second);
        first.Value.ShouldBeSameAs(nested);
        first.Value.Value.ShouldBe("configured");
        selectorCalls.ShouldBe(1);
    }

    [Test]
    public void AddNestedOptions_RegistersAnnotationValidatorByDefault()
    {
        var services = new ServiceCollection();
        services.AddOptions<RootOptions>().AddNestedOptions(root => root.Nested);
        using var provider = services.BuildServiceProvider();

        var validators = provider.GetServices<IValidateOptions<NestedOptions>>().ToArray();

        validators.Length.ShouldBe(1);
        validators[0].GetType().Name.ShouldBe("DataAnnotationsOptionsValidator`1");
    }

    [Test]
    public void AddNestedOptions_DoesNotRegisterAnnotationValidatorWhenDisabled()
    {
        var services = new ServiceCollection();
        services.AddOptions<RootOptions>().AddNestedOptions(root => root.Nested, withAnnotations: false);
        using var provider = services.BuildServiceProvider();

        provider.GetServices<IValidateOptions<NestedOptions>>().ShouldBeEmpty();
        provider.GetRequiredService<IOptions<NestedOptions>>().Value.ShouldNotBeNull();
    }

    public class RootOptions
    {
        public NestedOptions Nested { get; set; } = new();
    }

    public class NestedOptions
    {
        public string? Value { get; set; }
    }
}
