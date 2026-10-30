using Toarnbeike.Results.Failures;

namespace Toarnbeike.AppLibrary.Failures;

/// <summary>
/// Represents a failure when a property was expected to be modified but remained unchanged.
/// </summary>
/// <param name="Property">The name of the property that was expected to be modified.</param>
public sealed record UnmodifiedFailure(string Property) 
    : DomainFailure($"Property '{Property}' was not modified, value is unchanged");