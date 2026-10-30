using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Toarnbeike.AppLibrary.DependencyInjection;

/// <summary>
/// Select assemblies to scan for implementations of services and open generic services.
/// </summary>
/// <remarks>
/// Expose on an extension of <see cref="IServiceCollection"/> to allow for fluent configuration of assemblies to scan for service implementations.
/// <code>
/// AddValidation(this IServiceCollection services, Action&lt;AssemblySelector&gt; options) {}
/// </code>
/// </remarks>
public class AssemblySelector
{
    private readonly HashSet<Assembly> _assemblies = [];

    /// <summary>
    /// Add a specific assembly to the list of assemblies to scan for implementations of services.
    /// </summary>
    /// <returns>The selector for fluent chaining.</returns>
    public AssemblySelector FromAssembly(Assembly assembly)
    {
        _assemblies.Add(assembly);
        return this;
    }

    /// <summary>
    /// Add a set of assemblies to the list of assemblies to scan for implementations of services.
    /// </summary>
    /// <returns>The selector for fluent chaining.</returns>
    public AssemblySelector FromAssemblies(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            _assemblies.Add(assembly);
        }
        return this;
    }

    /// <summary>
    /// Add the assembly of the specified type to the list of assemblies to scan for implementations of services.
    /// </summary>
    /// <returns>The selector for fluent chaining.</returns>
    public AssemblySelector FromAssemblyContaining<TType>() where TType : class
    {
        _assemblies.Add(typeof(TType).Assembly);
        return this;
    }

    /// <summary>
    /// Add the execution assembly (as defined by <c>Assembly.GetExecutingAssembly()</c>) to the list of assemblies to scan for implementations of services.
    /// </summary>
    /// <returns>The selector for fluent chaining.</returns>
    public AssemblySelector FromExecutingAssembly()
    {
        _assemblies.Add(System.Reflection.Assembly.GetExecutingAssembly());
        return this;
    }

    /// <summary>
    /// Gets the collection of assemblies that have been selected for scanning.
    /// </summary>
    public IReadOnlyCollection<Assembly> Assemblies => _assemblies;
}
