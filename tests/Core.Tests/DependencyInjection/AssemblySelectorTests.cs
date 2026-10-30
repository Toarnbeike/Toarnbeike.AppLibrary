namespace Toarnbeike.AppLibrary.DependencyInjection;

public class AssemblySelectorTests
{
    [Test]
    public void Assemblies_IsInitiallyEmpty()
    {
        var selector = new AssemblySelector();

        selector.Assemblies.ShouldBeEmpty();
    }

    [Test]
    public void FromAssembly_AddsAssemblyAndReturnsSelector()
    {
        var selector = new AssemblySelector();
        var assembly = typeof(string).Assembly;

        var result = selector.FromAssembly(assembly);

        result.ShouldBeSameAs(selector);
        selector.Assemblies.ShouldHaveSingleItem().ShouldBeSameAs(assembly);
    }

    [Test]
    public void FromAssemblies_AddsAllAssembliesAndReturnsSelector()
    {
        var selector = new AssemblySelector();
        var first = typeof(string).Assembly;
        var second = typeof(AssemblySelector).Assembly;

        var result = selector.FromAssemblies(first, second);

        result.ShouldBeSameAs(selector);
        selector.Assemblies.Count.ShouldBe(2);
        selector.Assemblies.ShouldContain(first);
        selector.Assemblies.ShouldContain(second);
    }

    [Test]
    public void FromAssemblies_WithNoArguments_DoesNotChangeSelection()
    {
        var selector = new AssemblySelector().FromAssembly(typeof(string).Assembly);

        var result = selector.FromAssemblies();

        result.ShouldBeSameAs(selector);
        selector.Assemblies.ShouldHaveSingleItem().ShouldBeSameAs(typeof(string).Assembly);
    }

    [Test]
    public void FromAssemblyContaining_AddsTypeAssemblyAndReturnsSelector()
    {
        var selector = new AssemblySelector();

        var result = selector.FromAssemblyContaining<string>();

        result.ShouldBeSameAs(selector);
        selector.Assemblies.ShouldHaveSingleItem().ShouldBeSameAs(typeof(string).Assembly);
    }

    [Test]
    public void FromExecutingAssembly_AddsSelectorAssemblyAndReturnsSelector()
    {
        var selector = new AssemblySelector();

        var result = selector.FromExecutingAssembly();

        result.ShouldBeSameAs(selector);
        selector.Assemblies.ShouldHaveSingleItem().ShouldBeSameAs(typeof(AssemblySelector).Assembly);
    }

    [Test]
    public void SelectionMethods_DoNotAddDuplicateAssemblies()
    {
        var selector = new AssemblySelector();
        var assembly = typeof(AssemblySelector).Assembly;

        selector.FromAssembly(assembly)
            .FromAssemblies(assembly, assembly)
            .FromAssemblyContaining<AssemblySelector>()
            .FromExecutingAssembly();

        selector.Assemblies.ShouldHaveSingleItem().ShouldBeSameAs(assembly);
    }
}
