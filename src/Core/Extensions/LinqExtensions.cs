namespace Toarnbeike.AppLibrary.Extensions;

/// <summary>
/// Extensions on IEnumerable{T}
/// </summary>
public static class LinqExtensions
{
    /// <summary>
    /// Apply an action to each element of the source collection.
    /// </summary>
    /// <param name="source">The collection to iterate over.</param>
    /// <param name="action">The action to apply to each element.</param>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in source)
        {
            action(item);
        }
    }
}
